using AppData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        OnTapC4Context _db;

        public CartController()
        {
            _db = new OnTapC4Context();
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_db.Carts.ToList());
        }

        [HttpGet("get-by-id")]
        public IActionResult GetByID(string username)
        {
            return Ok(_db.Carts.Find(username));
        }

        [HttpPost("create")]
        public IActionResult Create(Cart cart)
        {
            try
            {

                _db.Carts.Add(cart);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut("update")]
        public IActionResult Update(Cart cart)
        {
            try
            {
                var cartUpdate = _db.Carts.Find(cart.Username);
                cartUpdate.Status = cart.Status;
                cartUpdate.Description = cart.Description;
                _db.Carts.Update(cartUpdate);
                _db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                return BadRequest();
            }
        }

        [HttpDelete("delete")]
        public IActionResult Delete(string username)
        {
            try
            {
                var cart = _db.Carts.Find(username);
                _db.Carts.Remove(cart);
                _db.SaveChanges();
                return Ok();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        [HttpGet("Details")]
        public IActionResult Get(string username)
        {
            return Ok(_db.Carts.Find(username));
        }
    }
}
