using Microsoft.AspNetCore.Mvc;
using OnTap_Net104.Models;

namespace OnTap_Net104.Controllers
{
    public class ProductController : Controller
    {
        AppDbContext _db;
        public ProductController()
        {
            _db = new AppDbContext();
        }
        public IActionResult Index()
        {
            var products = _db.Products.ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            try
            {
                product.Id = Guid.NewGuid();
                product.Status = 0;
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
        public IActionResult Details(Guid id)
        {
            var product = _db.Products.Find(id);
            return View(product);
        }
        public IActionResult Edit (Guid id)
        {
            var product = _db.Products.Find(id);
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            try
            {
                var product_db = _db.Products.Find(product.Id);

                product_db.Name = product.Name;
                product_db.Price = product.Price;
                product_db.Description = product.Description;
                product_db.Status = product.Status;

                _db.Products.Update(product_db);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
        public IActionResult Delete(Guid id)
        {
            try
            {
                var product = _db.Products.Find(id);
                _db.Products.Remove(product);
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
