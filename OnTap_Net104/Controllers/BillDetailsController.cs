using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public IActionResult Index(string id)
        {
            if(HttpContext.Session.GetString("currentUsername") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

            if (id == null)
            {
                var billDetails = _db.BillDetails.ToList();
                return View(billDetails);
            }
            else
            {
                var billDetails = _db.BillDetails.Where(a => a.BillId == id).ToList();
                return View(billDetails);
            }
            }
        }
        [HttpPost]
        public IActionResult Create(string billID, bool muaLai)
        {
            try
            {
                if (muaLai)
                {
                    var listProduct_MuaLai = JsonConvert.DeserializeObject<List<CartDetail>>(HttpContext.Session.GetString("listProductMuaLai"));
                    foreach (var item in listProduct_MuaLai)
                    {
                        if (item.Status == true)
                        {

                        var newBillDetail = new BillDetail()
                        {
                            Id = Guid.NewGuid(),
                            BillId = billID,
                            ProductId = item.ProductId,
                            ProductPrice = _db.Products.Find(item.ProductId).Price,
                            Quantity = item.Quantity,
                            Status = 0
                        };

                        _db.BillDetails.Add(newBillDetail);

                        var updateProduct = _db.Products.Find(item.ProductId);
                        updateProduct.Quantity -= item.Quantity;
                        _db.Products.Update(updateProduct);

                        var updateTotalBill = _db.Bills.Find(billID);
                        updateTotalBill.TotalBill += newBillDetail.ProductPrice * newBillDetail.Quantity;
                        _db.Bills.Update(updateTotalBill);

                        _db.SaveChanges();
                        }
                    }
                    listProduct_MuaLai.Clear();
                    return RedirectToAction("Index");
                }
                else
                {

                    var listProduct_Cart_True = _db.CartDetails.Where(a => a.CartID == HttpContext.Session.GetString("currentUsername") && a.Status == true).ToList();
                    if(listProduct_Cart_True.Count == 0)
                    {
                        return RedirectToAction("Index");
                    }
                    foreach (var item in listProduct_Cart_True)
                    {
                        var newBillDetail = new BillDetail()
                        {
                            Id = Guid.NewGuid(),
                            BillId = billID,
                            ProductId = item.ProductId,
                            ProductPrice = _db.Products.Find(item.ProductId).Price,
                            Quantity = item.Quantity,
                            Status = 0
                        };

                        _db.BillDetails.Add(newBillDetail);
                        var updateProduct = _db.Products.Find(item.ProductId);
                        updateProduct.Quantity -= item.Quantity;
                        _db.Products.Update(updateProduct);

                        var updateTotalBill = _db.Bills.Find(billID);
                        updateTotalBill.TotalBill += newBillDetail.ProductPrice * newBillDetail.Quantity;
                        _db.Bills.Update(updateTotalBill);

                        _db.CartDetails.Remove(item);

                        _db.SaveChanges();
                    }
                    return RedirectToAction("Index");

                }
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
