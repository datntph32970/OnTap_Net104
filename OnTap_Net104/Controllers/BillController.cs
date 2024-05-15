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
            var bills = _db.Bills.OrderByDescending(bill => bill.CreateDate).ToList();
            return View(bills);
        }
        public IActionResult Index_ViewKhachHang(string username)
        {
            var bills = _db.Bills.Where(bill => bill.Username == username).OrderByDescending(bill => bill.CreateDate).ToList();
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
        public IActionResult Update (string id,int status)
        {
            try
            {
                if(status == 2)
                {
                    var listProduct = _db.BillDetails.Where(x => x.BillId == id).ToList();
                    foreach (var item in listProduct)
                    {
                        var product = _db.Products.Find(item.ProductId);
                        product.Quantity += item.Quantity;
                        _db.Products.Update(product);
                    }
                }
                var bill = _db.Bills.Find(id);
                bill.Status = status;
                _db.Bills.Update(bill);
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
