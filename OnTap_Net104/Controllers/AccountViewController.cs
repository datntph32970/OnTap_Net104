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
            try
            {
                if (username == null && password == null) 
                {
                    return View();
                }
                else
                {
                    var user = _db.Accounts.Find(username);
                    if (user != null && user.Password == password)
                    {
                        HttpContext.Session.SetString("currentUsername", user.Username);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu";
                        return View();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
               return BadRequest();
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
                var oldAccount = _db.Accounts.Find(account.Username);
                if (oldAccount == null)
                {
                    _db.Accounts.Add(account);
                    _db.Carts.Add(new Cart { Username = account.Username });
                    _db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại");
                    return View();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
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
                Address = "Ha Loi"
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
