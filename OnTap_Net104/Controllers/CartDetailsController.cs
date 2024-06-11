using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnTap_Net104.Models;
using System.Text;

namespace OnTap_Net104.Controllers
{
    public class CartDetailsController : Controller
    {
        HttpClient _client;
        AppDbContext _db;
        public CartDetailsController()
        {
            _client = new HttpClient();
            _db = new AppDbContext();
        }
        public IActionResult Index(string id)
        {
            if (HttpContext.Session.GetString("currentUsername") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var requesURL = $@"https://localhost:7011/api/CartDetails/get-all";
                var cartDetails = _client.GetStringAsync(requesURL).Result;
                if (id == null)
                {
                    var data = JsonConvert.DeserializeObject<List<CartDetail>>(cartDetails);
                    return View(data);
                }
                else
                {
                    var data = JsonConvert.DeserializeObject<List<CartDetail>>(cartDetails).Where(a => a.CartID == id).ToList();
                    return View(data);
                }
            }
        }

        //[HttpPost]
        //public IActionResult Create(Guid ProductID, int Quantity)
        //{
        //    try
        //    {
        //        var username = HttpContext.Session.GetString("currentUsername");
        //        if (username == null)
        //        {
        //            return Json("Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng!");
        //        }

        //        var requesURL = $@"https://localhost:7011/api/CartDetail/get-all";
        //        var cart = _client.GetStringAsync(requesURL).Result;
        //        var cart_dt = JsonConvert.DeserializeObject<List<CartDetail>>(cart).Where(a => a.ProductId == ProductID &&  a.CartID == (HttpContext.Session.GetString("currentUsername"))).ToList();

        //        var productURL = $@"https://localhost:7011/api/Product/get-by-id?id={ProductID}";
        //        var productRes = _client.GetStringAsync(productURL).Result;
        //        var products = JsonConvert.DeserializeObject<Product>(productRes);
        //        if (cart_dt != null)
        //        {
        //            try
        //            {


        //                foreach (var item in cart_dt)
        //                {
        //                    item.Quantity += Quantity;
        //                    if (Quantity < 1 || item.Quantity > products.Quantity)
        //                    {
        //                        return Json("Số lượng trong giỏ hàng không thể quá số lượng trong kho!");
        //                    }
        //                    var updateURL = $@"https://localhost:7011/api/CartDetail/update";
        //                    var updateResponse = _client.PutAsJsonAsync(updateURL, cart_dt).Result;
        //                    return RedirectToAction("Index");
        //                }

        //            }
        //            catch (Exception)
        //            {
        //                return Json("Có lỗi xảy ra khi cập nhật giỏ hàng!");
        //            }
        //        }
        //        else
        //        {
        //            if ((Quantity < 1) || Quantity > products.Quantity)
        //            {
        //                return Json("Số lượng vượt quá số lượng trong kho");
        //            }
        //            var cartDetail = new CartDetail();
        //            cartDetail.Id = Guid.NewGuid();
        //            cartDetail.CartID = username;
        //            cartDetail.ProductId = ProductID;
        //            cartDetail.Quantity = Quantity;
        //            cartDetail.Status = false;

        //            var createUrl = $@"https://localhost:7011/api/CartDetail/create";
        //            var createContent = new StringContent(JsonConvert.SerializeObject(cartDetail), Encoding.UTF8, "application/json");
        //            var createResponse = _client.PostAsync(createUrl, createContent);
        //            return RedirectToAction("Index");
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception e)
        //    {
        //        // Log the error properly
        //        // Consider using a logging library such as Serilog, NLog, etc.
        //        // Log.Error(e, "Error in Create method");

        //        return StatusCode(500, "Đã có lỗi xảy ra, vui lòng thử lại sau.");
        //    }
        //}
        [HttpPost]
        public IActionResult Create(Guid ProductID, int Quatity)
        {
            try
            {
                var username = HttpContext.Session.GetString("currentUsername");
                if (username == null)
                {
                    return Json("Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng!");
                }
                else
                {

                    var existCartDetail = _db.CartDetails.Where(a => a.CartID == username && a.ProductId == ProductID).FirstOrDefault();
                    if (existCartDetail != null)
                    {
                        try
                        {
                            existCartDetail.Quantity += Quatity;
                            if ((Quatity < 1) || (existCartDetail.Quantity > _db.Products.Find(ProductID).Quantity))
                            {
                                return Json("Số lượng trong giỏ hàng không thể quá số lượng trong kho!");
                            }
                            _db.CartDetails.Update(existCartDetail);
                            _db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.InnerException.Message, e.Message);
                            throw;
                        }
                    }
                    else
                    {
                        if ((Quatity < 1) || (Quatity > _db.Products.Find(ProductID).Quantity))
                        {
                            return Json("Số lượng vượt quá số lượng trong kho!");
                        }
                        var cartDetail = new CartDetail();
                        cartDetail.Id = Guid.NewGuid();
                        cartDetail.CartID = username;
                        cartDetail.ProductId = ProductID;
                        cartDetail.Quantity = Quatity;
                        cartDetail.Status = false;

                        _db.CartDetails.Add(cartDetail);
                        _db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
        [HttpPost]
        public IActionResult MuaLai(string idBill)
        {
            string requestURL = $"https://localhost:7011/api/Bill/Mua-lai?id={idBill}";
            var respone = _client.GetAsync(requestURL);
            if (respone.Result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest();
            }

        }
        public IActionResult View_MuaLai()
        {
            var listProductMuaLai = JsonConvert.DeserializeObject<List<CartDetail>>(HttpContext.Session.GetString("listProductMuaLai"));

            return View(listProductMuaLai);
        }
        public IActionResult Details(Guid id)
        {
            var requetURL = $@"https://localhost:7011/api/CartDetail/Details?id={id}";
            var reponse = _client.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<CartDetail>(reponse);
            return View(data);
        }
        public IActionResult Delete(Guid id, int Quatity)
        {
            var requetURL = $@"https://localhost:7011/api/CartDetail/delete?id={id}";
            var reponse = _client.DeleteAsync(requetURL).Result;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Guid id, int Quantity, bool Status)
        {
            try
            {
                var cartDetail_id = $@"https://localhost:7011/api/CartDetails/get-by-id?id={id}";
                var response = _client.GetStringAsync(cartDetail_id).Result;
                var json = JsonConvert.DeserializeObject<CartDetail>(response);

                json.Quantity = Quantity;
                json.Status = Status;
                var apiUpdate = $@"https://localhost:7011/api/CartDetails/update";
                var client = _client.PutAsJsonAsync(apiUpdate, json).Result;
                if (client.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
    }
}
