using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnTap_Net104.Models;

namespace OnTap_Net104.Controllers
{
    public class BillDetailsController : Controller
    {
        HttpClient client;
        public BillDetailsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7011/api/");
        }
        public IActionResult Index(string id)
        {
            if (HttpContext.Session.GetString("currentUsername") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                    var billDetails = client.GetStringAsync("APIBillDetail/get-all").Result;
                if (id == null)
                {
                    var data = JsonConvert.DeserializeObject<List<BillDetail>>(billDetails);
                    return View(data);
                }
                else
                {
                    var data = JsonConvert.DeserializeObject<List<BillDetail>>(billDetails).Where(a => a.BillId == id);

                    return View(data);
                }
            }
        }
        [HttpPost]
        public IActionResult Create(string billID)
        {
            try
            {
                var listProduct_Cart = client.GetStringAsync("CartDetails/get-all").Result;
                var listProduct_Cart_True = JsonConvert.DeserializeObject<List<CartDetail>>(listProduct_Cart).Where(a => a.Status == true && a.CartID == (HttpContext.Session.GetString("currentUsername"))).ToList();
                if (listProduct_Cart_True.Count == 0)
                {
                    return Json("Không có sản phẩm nào được chọn");
                }
                foreach (var item in listProduct_Cart_True)
                {
                        var product = client.GetStringAsync("SanPham/get-by-id?id=" + item.ProductId).Result;
                        var jsonProduct = JsonConvert.DeserializeObject<Product>(product);

                    var newBillDetail = new BillDetail()
                    {
                        Id = Guid.NewGuid(),
                        BillId = billID,
                        ProductId = item.ProductId,
                        ProductPrice = jsonProduct.Price,
                        Quantity = item.Quantity,
                        Status = 0
                    };

                    var addBillDetail = client.PostAsJsonAsync("BillDetail/create", newBillDetail).Result;
                    if (addBillDetail.IsSuccessStatusCode)
                    {
                        var productUpdate = client.GetStringAsync("SanPham/get-by-id?id=" + item.ProductId).Result;
                        var jsonProductUpdate = JsonConvert.DeserializeObject<Product>(productUpdate);
                        jsonProductUpdate.Quantity -= item.Quantity;

                        var respone = client.PutAsJsonAsync("SanPham/Edit", jsonProductUpdate);
                        if (respone.Result.IsSuccessStatusCode)
                        {
                            var updateTotalBill = client.GetStringAsync("Bill/get-by-id?id=" + billID).Result;
                            var jsonUpdateTotalBill = JsonConvert.DeserializeObject<Bill>(updateTotalBill);

                            jsonUpdateTotalBill.TotalBill += newBillDetail.ProductPrice * newBillDetail.Quantity;
                            client.PutAsJsonAsync("APIBill/update", jsonUpdateTotalBill);

                            var removeCartDetail = client.DeleteAsync("CartDetails/delete?id=" + item.Id);
                        }
                    }
                }
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
            var billDetail = client.GetStringAsync("APIBillDetail/get-by-id?id=" + id).Result;
            var data = JsonConvert.DeserializeObject<BillDetail>(billDetail);

            return View(data);
        }
        public IActionResult Delete(Guid id)
        {
            try
            {
                var billDetail = client.DeleteAsync("APIBillDetail/delete?id=" + id).Result;
                if (billDetail.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                return BadRequest();

            }
        }
    }
}
