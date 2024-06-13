using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnTap_Net104.Models;

namespace OnTap_Net104.Controllers
{
    public class AccountViewController : Controller
    {
        AppDbContext _db;
        HttpClient _client;
        public AccountViewController()
        {
            _db = new AppDbContext();
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _client = new HttpClient(handler);

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("currentUsername");
            return RedirectToAction("Login");
        }
        public IActionResult Login(string username, string password)
        {
            string requestURL = $"https://localhost:7011/api/Account/Sign-in?username={username}&password={password}";
            var respone = _client.GetAsync(requestURL);
            if (respone.Result.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("currentUsername", username);
                return RedirectToAction("Index", "Product");
            }
            else
            {
                return View();
            }
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(Account account)
        {
            try
            {
                string requestURL = $"https://localhost:7011/api/Account/Sign-up";
                var respone = _client.PostAsJsonAsync(requestURL, account);
                if (respone.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        [HttpGet]
        public IActionResult Index()
        {
            var requetURL = $@"https://localhost:7011/api/Account/get-all";
            var reponse = _client.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<List<Account>>(reponse);
            return View(data);
        }
        public IActionResult Create()
        {
            Account account = new Account()
            {
                Username = "thanhdx1234",
                Password = "thanhdx1234",
                Phone = "0796000234",
                Address = "Ha Noi"
            };
            return View(account);
        }
        [HttpPost]
        public IActionResult Create(Account account)
        {
            var acc = JsonConvert.SerializeObject(account);
            var requetURL = $@"https://localhost:7011/api/Account/create";
            var reponse = _client.PostAsJsonAsync(requetURL, acc).Result;
            if (reponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult Edit(string username)
        {
            var requetURL = $@"https://localhost:7011/api/Account/get-by-id?id={username}";
            var reponse = _client.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<Account>(reponse);
            return View(data);

        }
        [HttpPost]
        public IActionResult Edit(Account account)
        {
            try
            {
                var requetURL = "https://localhost:7011/api/Account/update";
                var reponse = _client.PutAsJsonAsync(requetURL, account).Result;
                if (reponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                return BadRequest();
            }

        }


        public IActionResult Delete(string username)
        {
            try
            {
                var requetURL = $@"https://localhost:7011/api/Account/delete?username={username}";
                var reponse = _client.DeleteAsync(requetURL).Result;
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                return BadRequest();
            }
        }

        public IActionResult Details(string username)
        {
            var requetURL = $@"https://localhost:7011/api/Account/get-by-id?id={username}";
            var reponse = _client.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<Account>(reponse);
            return View(data);
        }
    }
}
