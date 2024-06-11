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
        public IActionResult Update(CartDetail cartdt)
        {
            try
            {
                var cartdtUpdate = _db.CartDetails.Find(cartdt.Id);
                cartdtUpdate.CartID = cartdt.CartID;
                cartdtUpdate.Quantity = cartdt.Quantity;
                cartdtUpdate.Status = cartdt.Status;

                _db.CartDetails.Update(cartdtUpdate);
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
