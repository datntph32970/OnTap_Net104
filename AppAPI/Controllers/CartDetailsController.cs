using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartDetailsController:ControllerBase
    {
        OnTapC4Context _db;
        public CartDetailsController()
        {
            _db = new OnTapC4Context();
        }
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_db.CartDetails.ToList());
        }
        [HttpGet("get-by-id")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_db.CartDetails.Find(id));
        }
        [HttpPost("create")]
        public IActionResult Create(CartDetail cartDetail)
        {
            try
            {
                _db.CartDetails.Add(cartDetail);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("update")]
        public IActionResult Update(CartDetail cartDetail)
        {
            try
            {
                _db.CartDetails.Update(cartDetail);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _db.CartDetails.Remove(_db.CartDetails.Find(id));
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
