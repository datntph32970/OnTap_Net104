using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OnTap_Net104.Controllers
{
    public class CartController : Controller
    {
        HttpClient _client;
        public CartController()
        {
            _client = new HttpClient();
        }
        public IActionResult Index()
        {
            var requetURL = $@"https://localhost:7011/api/Cart/get-all";
            var reponse = _client.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<List<Cart>>(reponse);
            return View(data);
        }

        public IActionResult Edit(string username)
        {
            var requetURL = $@"https://localhost:7011/api/Cart/get-by-id?id={username}";
            var reponse = _client.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<Cart>(reponse);
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Cart cart)
        {
            var requetURL = $@"https://localhost:7011/api/Cart/Edit";
            var reponse = _client.PutAsJsonAsync(requetURL, cart).Result;
            return RedirectToAction("Index");

        }
        public IActionResult Details(string username)
        {
            var requetURL = $@"https://localhost:7011/api/Cart/Details?id={username}";
            var reponse = _client.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<Product>(reponse);
            return View(data);
        }
    }
}
