using AppData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        OnTapC4Context _db;
        public AccountController()
        {
            _db = new OnTapC4Context();
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_db.Accounts.ToList());
        }

        [HttpGet("get-by-id")]
        public IActionResult GetByID(string id)
        {
            return Ok(_db.Accounts.Find(id));
        }

        [HttpPost("create")]
        public IActionResult Create(Account account)
        {
            try
            {
                _db.Accounts.Add(account);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut("update")]
        public IActionResult Update(Account account)
        {
            try
            {
                var acc = _db.Accounts.Find(account.Username);
                acc.Password = account.Password;
                acc.Address = account.Address;
                acc.Phone = account.Phone;
                _db.Accounts.Update(acc);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpDelete("delete")]
        public IActionResult Delete(string username)
        {
            try
            {
                var acc = _db.Accounts.Find(username);
                _db.Accounts.Remove(acc);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                return BadRequest();
            }
        }
        [HttpPost("Sign-up")]
        public IActionResult SignUp(Account account)
        {
            try
            {
                var gh = new Cart()
                {
                    Username = account.Username,
                    Status = 0,
                    Description = "",
                };
                _db.Carts.Add(gh);
                
                _db.Accounts.Add(account);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        // POST api/<AccountController>
        [HttpGet("Sign-in")]
        public IActionResult SignIn(string username, string password)
        {
            try
            {
                _db.Accounts.FirstOrDefault(a => a.Username == username && a.Password == password);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        //PUT api/<AccountController>/5
        [HttpGet("Log-Out")]
        public IActionResult LogOut()
        {
            try
            {
                HttpContext.Session.Clear();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
