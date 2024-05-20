using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnTap_Net104.Models;

namespace OnTap_Net104.Controllers
{
    public class CartDetailsController : Controller
    {
        AppDbContext _db;
        public CartDetailsController()
        {
            _db = new AppDbContext();
        }
        public IActionResult Index(string id)
        {
            

            if (id == null)
            {
                var username = HttpContext.Session.GetString("currentUsername");
                var cartDetails = _db.CartDetails.Where(a => a.CartID == username).ToList();
                return View(cartDetails);
            }
            else
            {
                var cartDetails = _db.CartDetails.Where(a => a.CartID == id).ToList();
                return View(cartDetails);
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
            try
            {
                var listBillDetailMuaLai = _db.BillDetails.Where(a => a.BillId == idBill).ToList();

                List<CartDetail> listProductMuaLai = new List<CartDetail>();


                foreach (var item in listBillDetailMuaLai)
                {
                    var cartDetail = new CartDetail();
                    cartDetail.Id = Guid.NewGuid();
                    cartDetail.CartID = HttpContext.Session.GetString("currentUsername");
                    cartDetail.ProductId = item.ProductId;
                    cartDetail.Quantity = item.Quantity;

                    cartDetail.Status = false;

                    listProductMuaLai.Add(cartDetail);
                }

                var a = JsonConvert.SerializeObject(listProductMuaLai);
                HttpContext.Session.SetString("listProductMuaLai", a);
                return RedirectToAction("View_MuaLai");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }

        }
        public IActionResult View_MuaLai()
        {
            var listProductMuaLai = JsonConvert.DeserializeObject<List<CartDetail>>(HttpContext.Session.GetString("listProductMuaLai"));

            return View(listProductMuaLai);
        }
        public IActionResult Details(Guid id)
        {
            var cartDetail = _db.CartDetails.Find(id);
            return View(cartDetail);
        }
        public IActionResult Delete(Guid id, int Quatity)
        {
            try
            {
                var cartDetail = _db.CartDetails.Find(id);
                _db.CartDetails.Remove(cartDetail);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Edit(Guid id, int Quantity, bool Status)
        {
            try
            {
                var cartDetail_db = _db.CartDetails.Find(id);
                if (cartDetail_db == null)
                {
                    var listProductMuaLai = JsonConvert.DeserializeObject<List<CartDetail>>(HttpContext.Session.GetString("listProductMuaLai"));
                    cartDetail_db = listProductMuaLai.FirstOrDefault(a => a.Id == id);
                    cartDetail_db.Quantity = Quantity;
                    cartDetail_db.Status = Status;

                    HttpContext.Session.SetString("listProductMuaLai", JsonConvert.SerializeObject(listProductMuaLai));
                }
                else
                {
                    cartDetail_db.Quantity = Quantity;
                    cartDetail_db.Status = Status;

                    _db.CartDetails.Update(cartDetail_db);
                    _db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
    }
}
