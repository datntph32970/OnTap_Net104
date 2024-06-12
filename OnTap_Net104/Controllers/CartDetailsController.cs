using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnTap_Net104.Models;
using System.Net.Http.Headers;
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
                        cartDetail.TransportFee = 0;
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
            string requestURL = $"https://localhost:7011/api/APIBill/Mua-lai?id={idBill}";
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
        public IActionResult Delete(Guid id)
        {
            var requetURL = $"https://localhost:7011/api/CartDetails/delete?id={id}";
            var reponse = _client.DeleteAsync(requetURL).Result;
            if (reponse.IsSuccessStatusCode)
            {

            return RedirectToAction("Index");
            }
            return BadRequest();
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


        public async Task<decimal> GetTotalTransportFee(int serviceId, decimal insuranceValue, int toDistrictId, string toWardCode)
        {
            try
            {
                var INTinsuranceValue = Convert.ToInt32(insuranceValue);
                string baseUrl = $"https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee?service_id={serviceId}&insurance_value={INTinsuranceValue}&from_district_id=3440&to_district_id={toDistrictId}&to_ward_code={toWardCode}&weight=1000&length=10&width=10&height=10";
               

                var a = _client.GetAsync(baseUrl).Result;
                var b = a.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<dynamic>(b);

                decimal totalTransportFee = result.data.total;

                return totalTransportFee;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
        public IActionResult UpdateTransportFeeForAllCartDetails(int serviceId, int toDistrictId, string toWardCode)
        {
            try
            {
                var cartDetails = _client.GetStringAsync("https://localhost:7011/api/CartDetails/get-all").Result;
                var datacartDetails = JsonConvert.DeserializeObject<List<CartDetail>>(cartDetails);

                _client.DefaultRequestHeaders.Add("token", "c5a07264-26a0-11ef-ad6a-e6aec6d1ae72");
                _client.DefaultRequestHeaders.Add("shop_id", "5123377");

                foreach (var cartDetail in datacartDetails)
                {
                    var product = _client.GetStringAsync($"https://localhost:7011/api/SanPham/get-by-id?id={cartDetail.ProductId}").Result;
                    var productData = JsonConvert.DeserializeObject<Product>(product);
                    var insuranceValue = productData.Price * cartDetail.Quantity;
                    var transportFee = GetTotalTransportFee(serviceId, insuranceValue, toDistrictId, toWardCode).Result;
                    cartDetail.TransportFee = transportFee;
                    _db.CartDetails.Update(cartDetail);
                    Thread.Sleep(500);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
