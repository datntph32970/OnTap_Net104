using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnTap_Net104.Models;
using System.Text;

namespace OnTap_Net104.Controllers
{
    public class BillController : Controller
    {
        HttpClient client;
        public BillController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7011/api/");
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("currentUsername") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var listBill = client.GetStringAsync("Bill/get-all").Result;
            var data = JsonConvert.DeserializeObject<List<Bill>>(listBill);
            return View(data);
            
        }
        public IActionResult Index_ViewKhachHang(string username)
        {
            if(HttpContext.Session.GetString("currentUsername") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var bills = client.GetAsync($"Bill/get-by-id?id={username}").Result.Content.ReadAsStringAsync().Result;
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
                bill.TotalBill = 0;
                bill.Status = 0;
                bill.CreateDate = new DateTime();

                var json = JsonConvert.SerializeObject(bill);

                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var result = client.PostAsync("Bill/create", data).Result;
                if (result.IsSuccessStatusCode)
                {
                    return Json(bill.Id);
                };
                return BadRequest   ();
            }
          
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                throw;
            }
        }
        public IActionResult Details(string id)
        {
            var bill = client.GetAsync($"Bill/get-by-id?id={id}").Result.Content.ReadAsStringAsync().Result;
            return View(bill);
        }
        [HttpPost]
        public IActionResult Update(string id, int status)
        {
            try
            {
                if (status == 2)
                {
                    var listBillDetails = client.GetStringAsync($"BillDetail/get-all").Result;
                    var datalistBillDetails = JsonConvert.DeserializeObject<List<BillDetail>>(listBillDetails);

                    foreach (var item in datalistBillDetails.Where(a => a.BillId == id).ToList())
                    {
                        var product = client.GetStringAsync($"SanPham/get-by-id?id={item.ProductId}").Result;
                        var dataProduct = JsonConvert.DeserializeObject<Product>(product);

                        dataProduct.Quantity += item.Quantity;
                        
                       var respone =  client.PutAsync("SanPham/Edit", new StringContent(JsonConvert.SerializeObject(dataProduct), Encoding.UTF8, "application/json"));
                       
                    }
                }

                var bill = client.GetStringAsync($"Bill/get-by-id?id={id}").Result;
                var dataBill = JsonConvert.DeserializeObject<Bill>(bill);

                dataBill.Status = status;
                
                var result = client.PutAsJsonAsync("Bill/update", dataBill).Result;
                
                    return RedirectToAction("Index");
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                return BadRequest();
            }

        }
    }
}
