﻿using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIBillDetailController : ControllerBase
    {
        OnTapC4Context _db;
        public APIBillDetailController()
        {
            _db = new OnTapC4Context();
        }
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_db.BillDetails.ToList());
        }
        [HttpGet("get-by-id")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_db.BillDetails.Find(id));
        }
        [HttpPost("create")]
        public IActionResult Create(BillDetail billDetail)
        {
            try
            {
                _db.BillDetails.Add(billDetail);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
            
        }
        [HttpPut("update")]
        public IActionResult Update(BillDetail billDetail)
        {
            try
            {
                _db.BillDetails.Update(billDetail);
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
                var billDetail = _db.BillDetails.Find(id);
                _db.BillDetails.Remove(billDetail);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(  e.InnerException.Message,e.Message);
                return BadRequest();
            }
            
        }

       
    }
}
