using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        OnTapC4Context _context;
        public SanPhamController()
        {
            _context = new OnTapC4Context();
        }
        // GET: api/<SanPhamController>
        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            return Ok(_context.Products.ToList());
        }

        // GET api/<SanPhamController>/5
        [HttpGet("Details")]
        public IActionResult Get(Guid id)
        {
            return Ok(_context.Products.Find(id));
        }

        // POST api/<SanPhamController>
        [HttpPost("Create")]
        public IActionResult Post(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(400);
            }
        }

        // PUT api/<SanPhamController>/5
        [HttpPut("Edit")]
        public IActionResult Put(Product product)
        {
            try
            {
                var editItiems = _context.Products.Find(product.Id);
                editItiems.Name = product.Name;
                editItiems.Description = product.Description;
                editItiems.Price = product.Price;
                editItiems.Status = product.Status;
                editItiems.Quantity = product.Quantity;
                _context.Update(editItiems);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(400);
            }
        }

        // DELETE api/<SanPhamController>/5
        [HttpDelete("Delete")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var deleteItiems = _context.Products.Find(id);
                _context.Products.Remove(deleteItiems);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(400);
            }
        }
    }
}
