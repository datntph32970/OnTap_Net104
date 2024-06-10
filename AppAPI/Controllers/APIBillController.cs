using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIBillController : ControllerBase
    {
        OnTapC4Context _db;
        public APIBillController()
        {
            _db = new OnTapC4Context();
        }
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_db.Bills.ToList());
        }
        [HttpGet("get-by-id")]
        public IActionResult GetById(string id)
        {
            return Ok(_db.Bills.Find(id));
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
                var billUpdate = _db.Bills.Find(bill.Id);
                billUpdate.Username = bill.Username;
                billUpdate.TotalBill = bill.TotalBill;
                billUpdate.Status = bill.Status;
                billUpdate.CreateDate = bill.CreateDate;

                _db.Bills.Update(billUpdate);
                _db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message,e.Message);
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
