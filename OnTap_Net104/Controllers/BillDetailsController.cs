using Microsoft.AspNetCore.Mvc;
using OnTap_Net104.Models;

namespace OnTap_Net104.Controllers
{
    public class BillDetailsController : Controller
    {
        AppDbContext _db;
        public BillDetailsController()
        {
            _db = new AppDbContext();
        }
        public IActionResult Index()
        {
            var billDetails = _db.BillDetails.ToList();
            return View(billDetails);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(BillDetail billDetail)
        {
            try
            {
                billDetail.Id = Guid.NewGuid();
                billDetail.BillId = HttpContext.Session.GetString("currentBill");

                _db.BillDetails.Add(billDetail);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
        public IActionResult Details(Guid id)
        {
            var billDetail = _db.BillDetails.Find(id);
            return View(billDetail);
        }
        public IActionResult Delete(Guid id)
        {
            try
            {
                var billDetail = _db.BillDetails.Find(id);
                _db.BillDetails.Remove(billDetail);
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
