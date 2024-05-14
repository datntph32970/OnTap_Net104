using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnTap_Net104.Models;

namespace OnTap_Net104.Controllers
{
    public class BillController : Controller
    {
        AppDbContext _db;
        public BillController()
        {
            _db = new AppDbContext();
        }
        public IActionResult Index()
        {
            var bills = _db.Bills.ToList();
            return View(bills);
        }
        [HttpPost]
        public IActionResult Create()
        {
            try
            {
                var bill = new Bill();

                bill.Id = Guid.NewGuid().ToString(); 
                bill.Username = HttpContext.Session.GetString("currentUsername");
                bill.CreateDate = DateTime.Now;
                bill.TotalBill = 0;
                bill.Status = 0;

                HttpContext.Session.SetString("currentBill", JsonConvert.SerializeObject(bill.Id));

                _db.Bills.Add(bill);
                _db.SaveChanges();
                return Json(bill.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
        public IActionResult Details(string id)
        {
            var bill = _db.Bills.Find(id);
            return View(bill);
        }
        public IActionResult Delete (string id)
        {
            try
            {
                var bill = _db.Bills.Find(id);
                _db.Bills.Remove(bill);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
            
        }
    }
}
