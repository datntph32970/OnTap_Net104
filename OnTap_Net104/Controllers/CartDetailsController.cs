using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("currentUsername");
            var cartDetails = _db.CartDetails.Where(a => a.CartID == username).ToList();
            return View(cartDetails);
        }
        [HttpPost]
        public IActionResult Create(Guid ProductID, int Quatity)
        {
            try
            {
                var username = HttpContext.Session.GetString("currentUsername");
                if(username == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    var existCartDetail = _db.CartDetails.Where(a => a.CartID == username && a.ProductId == ProductID).FirstOrDefault();
                    if(existCartDetail != null)
                    {
                        try
                        {
                            existCartDetail.Quantity += Quatity;
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
        public IActionResult Edit(Guid id,int Quantity,bool Status)
        {
            try
            {
                var cartDetail_db = _db.CartDetails.Find(id);

                cartDetail_db.Quantity = Quantity;
                cartDetail_db.Status = Status;

                _db.CartDetails.Update(cartDetail_db);
                _db.SaveChanges();
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
