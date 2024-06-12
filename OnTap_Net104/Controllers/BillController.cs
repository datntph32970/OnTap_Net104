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
            var listBill = client.GetStringAsync("APIBill/get-all").Result;
            var data = JsonConvert.DeserializeObject<List<Bill>>(listBill);
            return View(data.OrderByDescending(a=>a.CreateDate));
            
        }
        public IActionResult Index_ViewKhachHang(string username)
        {
            if(HttpContext.Session.GetString("currentUsername") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var listBill = client.GetStringAsync("APIBill/get-all").Result;
            var data = JsonConvert.DeserializeObject<List<Bill>>(listBill).Where(a => a.Username == HttpContext.Session.GetString("currentUsername"));
            return View(data.OrderByDescending(a => a.CreateDate));
        }
        [HttpPost]
        public IActionResult Create()
        {
            try
            {
                var isHaveProductStatusTrue = client.GetStringAsync("CartDetails/get-all").Result;
                var dataIsHaveProductStatusTrue = JsonConvert.DeserializeObject<List<CartDetail>>(isHaveProductStatusTrue).Where(a => a.Status == true && a.CartID == HttpContext.Session.GetString("currentUsername")).ToList();

                if (dataIsHaveProductStatusTrue.Count == 0)
                {
                    return Json("Không có sản phẩm nào được chọn");
                }
                else
                {
                    foreach (var item in dataIsHaveProductStatusTrue)
                    {
                        var product = client.GetStringAsync($"SanPham/get-by-id?id={item.ProductId}").Result;
                        var dataProduct = JsonConvert.DeserializeObject<Product>(product);
                        if(item.Quantity > dataProduct.Quantity)
                        {
                            return Json("Có sản phẩm vượt quá số lượng vui lòng thử lại!");
                        }
                        else if(item.TransportFee <= 0)
                        {
                            return Json("Yêu cầu cần có địa điểm giao hàng");
                        }
                    }
                }

                var bill = new Bill();
                bill.Id = Guid.NewGuid().ToString();
                bill.Username = HttpContext.Session.GetString("currentUsername");
                bill.TotalBill = 0;
                bill.Status = 0;
                bill.CreateDate = DateTime.Now;

                var json = JsonConvert.SerializeObject(bill);

                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var result = client.PostAsync("APIBill/create", data).Result;
                if (result.IsSuccessStatusCode)
                {
                    return Json(bill.Id);
                };
                return BadRequest();
            }
          
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message, e.Message);
                return BadRequest();
            }
        }
        public IActionResult Details(string id)
        {
            var bill = client.GetAsync($"APIBill/get-by-id?id={id}").Result.Content.ReadAsStringAsync().Result;
            return View(bill);
        }
        [HttpPost]
        public IActionResult Update(string id, int status)
        {
            try
            {
                if (status == 2)
                {
                    var listBillDetails = client.GetStringAsync($"APIBillDetail/get-all").Result;
                    var datalistBillDetails = JsonConvert.DeserializeObject<List<BillDetail>>(listBillDetails);

                    foreach (var item in datalistBillDetails.Where(a => a.BillId == id).ToList())
                    {
                        var product = client.GetStringAsync($"SanPham/get-by-id?id={item.ProductId}").Result;
                        var dataProduct = JsonConvert.DeserializeObject<Product>(product);

                        dataProduct.Quantity += item.Quantity;
                        
                       var respone =  client.PutAsync("SanPham/Edit", new StringContent(JsonConvert.SerializeObject(dataProduct), Encoding.UTF8, "application/json"));
                       
                    }
                }

                var bill = client.GetStringAsync($"APIBill/get-by-id?id={id}").Result;
                var dataBill = JsonConvert.DeserializeObject<Bill>(bill);

                dataBill.Status = status;
                
                var result = client.PutAsJsonAsync("APIBill/update", dataBill).Result;
                
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
