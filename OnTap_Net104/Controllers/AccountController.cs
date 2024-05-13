using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnTap_Net104.Models;

namespace OnTap_Net104.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext _db;

        public AccountController()
        {
            _db = new AppDbContext();
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
                throw;
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
      
        public IActionResult Index()
        {
            var accounts = _db.Accounts.ToList();
            return View(accounts);
        }
        public IActionResult Edit(string username)
        {
            try 
            {
                var account = _db.Accounts.Find(username);
                return View(account);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }

        }
        [HttpPost]
        public IActionResult Edit(Account account)
        {
            try
            {
                var oldAccount = _db.Accounts.Find(account.Username);
                if (oldAccount != null)
                {
                    oldAccount.Password = account.Password;
                    oldAccount.Phone = account.Phone;
                    oldAccount.Address = account.Address;
                    _db.Accounts.Update(oldAccount);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Content("Không tìm thấy tài khoản");
                    return View();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
        public IActionResult Delete(string username)
        {
            try
            {
                var account = _db.Accounts.Find(username);
                if (account != null)
                {
                    _db.Accounts.Remove(account);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Content("Không tìm thấy tài khoản");
                    return View();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
        public IActionResult Details(string username)
        {
            try
            {
                var account = _db.Accounts.Find(username);
                return View(account);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
    }
}
