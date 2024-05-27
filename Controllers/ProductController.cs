using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.ViewModels;

namespace TracyShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static double GetTicks(DateTime dateTime)
        {
            return dateTime.Subtract(Epoch).TotalMilliseconds;
        }

        public ProductController(ILogger<ProductController> logger, AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("/product", Name = "product")]
        public IActionResult Product()
        {
            List<ProductsListViewModel> products = GetProducts(_context.Product.Where(p => p.Active).OrderBy(p => p.Price).ToList());
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.Products = products;
            return View();
        }

        public IActionResult Category(int? id)
        {
            List<ProductsListViewModel> products = new();

            if(id != null)
            {
                products = GetProducts(_context.Product.Where(p => p.Active && p.CategoryId == id).ToList());
            }

            ViewBag.Categories = _context.Category.ToList();
            ViewBag.Products = products;

            return View();
        }

        [HttpGet]
        public IActionResult Search(string search)
        {
            List<ProductsListViewModel> products = new();

            if (!string.IsNullOrEmpty(search))
            {
                products = GetProducts(_context.Product.Where(p => p.Active == true)
                                                       .Where(p => p.Name.ToLower().Contains(search) ||
                                                                   p.Category.Name.ToLower().Contains(search) ||
                                                                   p.Trandemark.ToLower().Contains(search) ||
                                                                   p.Origin.Contains(search)).ToList());
            }

            ViewBag.Categories = _context.Category.ToList();
            ViewBag.Products = products;
            ViewData["GetProduct"] = search;
            return View();
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            // Lấy sản phẩm
            var query = _context.Product.Where(p => p.Id == id).FirstOrDefault();

            // Lấy danh sách ảnh
            List<Image> images = _context.Image.Where(i => i.ProductId == id).ToList();

            // Lấy danh sách sizes và descriptionSize
            List<Size> sizes = _context.Sizes.Where(s => _context.ProductSize.Where(p => p.ProductId == id && p.Quantity > 0).Select(ps => ps.SizeId).Contains(s.Id)).ToList();

            // Lấy tổng số lượng sản phẩm
            var count = _context.ProductSize.Where(p => p.ProductId == query.Id && sizes.Select(x => x.Id).Contains(p.SizeId)).Sum(q => q.Quantity);

            ViewBag.Bit = count > 0;

            // Lấy khuyến mãi
            Promotion promotion = _context.Promotion.Where(p => p.StartedDate <= DateTime.Now && p.EndDate >= DateTime.Now && p.Id == query.PromotionId).FirstOrDefault();

            // Lấy comment và rating
            var reviews = _context.Reviews.Where(r => r.ProductId == id).ToList();

            ViewBag.Review = reviews.Count == 0 ? 0 : 1;

            //Hiển thị lên view
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.CountImages = images.Count;
            ProductsListViewModel product = new()
            {
                Id = query.Id,
                Name = query.Name,
                Price = query.Price,
                Promotion = promotion != null ? (int)(promotion.percent * 100) : 0,
                Year_SX = query.Year_SX,
                PriceDiscounted = promotion != null ? query.Price * (1 - promotion.percent) : query.Price,
                Images = images,
                Sizes = sizes,
                Origin = query.Origin,
                Trandemark = query.Trandemark,
                Description = query.Description,
                Count = count,
                Reviews = reviews
            };

            return View(product);
        }

        [Authorize]
        public async Task<ActionResult> AddCart(int id)
        {
            if(UpdateCart(id) == 1)
            {
                await _context.SaveChangesAsync();

                ViewBag.News = true;
                ViewBag.Class = "alert alert-success";
                ViewBag.Message = "Thêm sản phẩm vào giỏ hàng thành công!";
            }

            return View();
        }

        [Authorize]
        public async Task<ActionResult> BuyNow(int id)
        {
            if (UpdateCart(id) == 1)
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Cart", "Cart");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Helper

        private List<ProductsListViewModel> GetProducts(List<Product> lstProduct)
        {
            List<ProductsListViewModel> products = new();

            foreach (var p in lstProduct)
            {
                var imgs = _context.Image.Where(img => img.ProductId == p.Id);

                // Kiểm tra số lượng sản phẩm trong kho

                ProductsListViewModel product = new()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Count = _context.ProductSize.Where(ps => ps.ProductId == p.Id).Sum(ps => ps.Quantity),
                    Price = p.Price,
                    ImageDefault = imgs.Any() ? imgs.First().Path : ""
                };

                products.Add(product);
            }

            return products;
        }

        private int UpdateCart(int id)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            var product = _context.Product.Where(p => p.Id == id).FirstOrDefault();

            // Kiểm tra số lượng sản phẩm trong kho
            if (_context.ProductSize.Where(p => p.ProductId == id).Sum(p => p.Quantity) > 0)
            {
                // Lấy khuyến mãi 
                Promotion promotion = _context.Promotion.Where(p => p.StartedDate <= DateTime.Now && p.EndDate >= DateTime.Now && p.Id == product.PromotionId).FirstOrDefault();

                // Lấy size
                var sizeId = _context.ProductSize.Where(p => p.ProductId == id && p.Quantity > 0).FirstOrDefault().SizeId;

                // Lấy ảnh
                var image = _context.Image.Where(i => i.ProductId == id).FirstOrDefault();

                // Kiểm tra sp đã có trong giỏ hàng chưa, nếu có thì tăng số lượng sp trong giỏ, nếu chưa thì thêm sp vào giỏ
                var carts = _context.Carts.Where(c => c.UserId == userid && c.IsBuy == false && c.ProductId == id && c.SelectedSize == sizeId);

                if (carts.ToList().Count == 0 && ModelState.IsValid)
                {
                    _context.Carts.Add(new Cart()
                    {
                        Quantity = 1,
                        ProductId = id,
                        UnitPrice = product.Price,
                        Promotion = promotion != null ? promotion.percent : 0,
                        SelectedSize = sizeId,
                        Image = image != null ? image.Path : "",
                        UserId = userid
                    });
                }
                else
                {
                    var cart = carts.FirstOrDefault();
                    cart.Promotion = promotion != null ? promotion.percent : 0;
                    cart.Quantity++;
                    _context.Update(cart);
                }

                return 1;
            }
            else
            {
                ViewBag.News = false;
                ViewBag.Class = "alert alert-danger";
                ViewBag.Message = "Hiện tại sản phẩm này không còn trong kho. Bạn chọn sản phẩm khác giúp shop nhé!";

                return 2;
            }
        }

        #endregion
    }
}