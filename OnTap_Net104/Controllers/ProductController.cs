using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnTap_Net104.Models;
using System.Net.WebSockets;
namespace OnTap_Net104.Controllers
{
    public class ProductController : Controller
    {
      
        HttpClient _clitent;

        public ProductController()
        {
            _clitent = new HttpClient();
             
        }
        public IActionResult Index()
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/GetAll";
            var reponse = _clitent.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<List<Product>>(reponse);
            return View(data);
        }
        public IActionResult Create()
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Giay A",
                Price = 20000,
                Description = "Hay quas",
                Status = 1,
                Quantity = 20
            };
            return View(product);
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/Create";
            var reponse = _clitent.PostAsJsonAsync(requetURL, product).Result;
            return RedirectToAction("Index");
        }

        public IActionResult Details(Guid id)
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/Details?id={id}";
            var reponse = _clitent.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<Product>(reponse);
            return View(data);
        }
        public IActionResult Edit(Guid id)
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/Details?id={id}";
            var reponse = _clitent.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<Product>(reponse);
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/Edit";
            var reponse = _clitent.PutAsJsonAsync(requetURL, product).Result;
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/Delete?id={id}";
            var reponse = _clitent.DeleteAsync(requetURL).Result;
            return RedirectToAction("Index");
        }
        public IActionResult AddToCart(Guid id, int quantity)
        {
           
            return RedirectToAction("Index", "Product");
        }
    }
}
