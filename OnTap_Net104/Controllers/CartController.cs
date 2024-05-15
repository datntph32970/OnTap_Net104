using Microsoft.AspNetCore.Mvc;
using OnTap_Net104.Models;

namespace OnTap_Net104.Controllers
{
    public class CartController : Controller
    {
        AppDbContext _db;
        public CartController()
        {
            _db = new AppDbContext();
        }
        public IActionResult Index()
        {
            var carts = _db.Carts.ToList();
            return View(carts);
        }

        public IActionResult Edit(string username)
        {
            var cart = _db.Carts.Find(username);
            return View(cart);
        }
        [HttpPost]
        public IActionResult Edit(Cart cart)
        {
            try
            {
                var cart_db = _db.Carts.Find(cart.Username);

                cart_db.Status = cart.Status;
                cart_db.Description = cart.Description;
                
                _db.Carts.Update(cart_db);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
        public IActionResult Details(string username)
        {
            var cart = _db.Carts.Find(username);
            return View(cart);
        }
    }
}
