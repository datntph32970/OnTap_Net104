using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController : ControllerBase
    {
        OnTapC4Context _db;
        public BillController()
        {
            _db = new OnTapC4Context();
        }
        [HttpGet("get-all")]
        public List<Bill> GetAll()
        {
            return _db.Bills.ToList();
        }
        [HttpGet("get-by-id")]
        public Bill GetById(Guid id)
        {
            return _db.Bills.Find(id);
        }
        [HttpPost("create")]
        public IActionResult Create(Bill bill)
        {
            try
            {
                _db.Bills.Add(bill);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpPut("update")]
        public IActionResult Update(Bill bill)
        {
            try
            {
                _db.Bills.Update(bill);
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
                var bill = _db.Bills.Find(id);
                _db.Bills.Remove(bill);
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
