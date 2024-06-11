using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using System.Net.WebSockets;
namespace OnTap_Net104.Controllers
{
    public class ProductController : Controller
    {
        OnTapC4Context _context;
        HttpClient _clitent;

        public ProductController()
        {
            _clitent = new HttpClient();
            _context = new OnTapC4Context();
        }
        public IActionResult Index()
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/GetAll";
            var reponse = _clitent.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<List<Product>>(reponse);
            return View(data);
        }
        public IActionResult Create()
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Giay A",
                Price = 20000,
                Description = "Hay quas",
                Status = 1,
                Quantity = 20
            };
            return View(product);
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/Create";
            var reponse = _clitent.PostAsJsonAsync(requetURL, product).Result;
            return RedirectToAction("Index");
        }

        public IActionResult Details(Guid id)
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/Details?id={id}";
            var reponse = _clitent.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<Product>(reponse);
            return View(data);
        }
        public IActionResult Edit(Guid id)
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/Details?id={id}";
            var reponse = _clitent.GetStringAsync(requetURL).Result;
            var data = JsonConvert.DeserializeObject<Product>(reponse);
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/Edit";
            var reponse = _clitent.PutAsJsonAsync(requetURL, product).Result;
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            var requetURL = $@"https://localhost:7011/api/SanPham/Delete?id={id}";
            var reponse = _clitent.DeleteAsync(requetURL).Result;
            return RedirectToAction("Index");
        }
        public IActionResult AddToCart(Guid id, int quantity)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa, nếu chưa thì chuyển hướng đến trang đăng nhập
            var check = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(check))
            {
                return RedirectToAction("Login", "Account"); // Chuyển hướng về trang Login
            }
            else
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);
                if (quantity <= 0)
                {
                    TempData[$"Error_{id}"] = "Số lượng sản phẩm không hợp lệ.";
                    return RedirectToAction("Index");
                }
                if (product.Quantity < quantity)
                {
                    TempData[$"Error_{id}"] = "Số lượng sản phẩm không đủ để thêm vào giỏ hàng.";
                    return RedirectToAction("Index");
                }

                // Lấy ra từ danh sách cartDetails của user đang đăng nhập xem có sản phẩm nào trùng id không?
                var allCartItem = _context.CartDetails.FirstOrDefault(p => p.CartID == check && p.ProductId == id);

                // Nếu sản phẩm chưa tồn tại trong giỏ hàng, tạo mới một CartDetails
                if (allCartItem == null)
                {


                    CartDetail details = new CartDetail()
                    {
                        Id = Guid.NewGuid(),
                        Quantity = quantity,
                        ProductId = id,
                        CartID = check,
                        Status = true
                    };
                    _context.CartDetails.Add(details);
                    _context.SaveChanges();
                }
                else
                {
                    // Nếu sản phẩm đã tồn tại trong giỏ hàng, cập nhật số lượng
                    allCartItem.Quantity = allCartItem.Quantity + quantity;
                    _context.CartDetails.Update(allCartItem);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Product");
        }
    }
}
