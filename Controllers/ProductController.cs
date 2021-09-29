﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
            List<ProductsListViewModel> products = new List<ProductsListViewModel>();
            List<Image> images = new List<Image>();
            var qr = _context.Image.ToList();
            foreach(var img in qr)
            {
                images.Add(img);
            }

            foreach (var p in _context.Product)
            {
                if(p.Active == true)
                {
                    var pro = new ProductsListViewModel();
                    if (images.Where(img => img.ProductId == p.Id) == null)
                    {
                        pro.ImageDefault = "";
                    }
                    else
                    {
                        pro.ImageDefault = images.Where(img => img.ProductId == p.Id).First().Path;
                    }
                    pro.Id = p.Id;
                    pro.Name = p.Name;
                    pro.Price = p.Price;
                    products.Add(pro);

                }
                else
                {
                    continue;
                }    
            }
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.Products = products;
            return View();
        }

        public async Task<IActionResult> Category(int? id)
        {
            List<ProductsListViewModel> products = new List<ProductsListViewModel>();
            List<Image> images = new List<Image>();
            var qr = _context.Image.ToList();
            foreach (var img in qr)
            {
                images.Add(img);
            }
            ViewBag.Categories = _context.Category.ToList();
            var query = from p in _context.Product select p;
            if (id != null)
            {
                query = query.Where(p => p.CategoryId == id);
                foreach (var pro in query)
                {
                    if(pro.Active == true)
                    {
                        var product = new ProductsListViewModel();
                        var img = images.Where(i => i.ProductId == pro.Id).First();
                        if(img == null)
                        {
                            product.ImageDefault = "";
                        }
                        else
                        {
                            product.ImageDefault = img.Path;
                        }
                        product.Name = pro.Name;
                        product.Price = pro.Price;
                        products.Add(product);
                    }
                }
            }
            ViewBag.Products = products;
            return View(await query.AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search)
        {
            List<ProductsListViewModel> products = new List<ProductsListViewModel>();
            List<Image> images = new List<Image>();
            var qr = _context.Image.ToList();
            foreach (var img in qr)
            {
                images.Add(img);
            }
            ViewBag.Categories = _context.Category.ToList();
            ViewData["GetProduct"] = search;
            var query = from x in _context.Product select x;
            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                p.Name.ToLower().Contains(search) ||
                p.Category.Name.ToLower().Contains(search) ||
                p.Trandemark.ToLower().Contains(search) ||
                p.Origin.Contains(search));

                foreach(var pro in query)
                {
                    if(pro.Active == true)
                    {
                        var product = new ProductsListViewModel();
                        var img = images.Where(i => i.ProductId == pro.Id).First();
                        if (img == null)
                        {
                            product.ImageDefault = "";
                        }
                        else
                        {
                            product.ImageDefault = img.Path;
                        }
                        product.Name = pro.Name;
                        product.Price = pro.Price;
                        products.Add(product);
                    }
                }
            }
            ViewBag.Products = products;
            return View(await query.AsNoTracking().ToListAsync());
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            ProductsListViewModel product = new ProductsListViewModel();
            List<Size> sizes = new List<Size>();
            List<Image> images = new List<Image>();
            // Lấy sản phẩm
            var query = _context.Product.Where(p => p.Id == id).First();

            // Lấy danh sách ảnh
            var qr = _context.Image.ToList();
            images.AddRange(qr.Where(i => i.ProductId == id).ToList());

            // Lấy danh sách sizes và descriptionSize
            var qr1 = _context.Sizes.ToList();
            var qr3 = _context.ProductSize.Where(p => p.ProductId == id).ToList();
            foreach (var pr in qr3)
            {
                sizes.Add(qr1.Where(d => d.Id == pr.SizeId).First());
            }

            // Lấy tổng số lượng sản phẩm
            var count = 0;
            foreach (var sz in sizes)
            {
                var q = _context.ProductSize.Where(p => p.ProductId == query.Id && p.SizeId == sz.Id).First();
                count += q.Quantity;
            }

            // Lấy khuyến mãi
            var promotions = _context.Promotion.Where(p => p.StartedDate <= DateTime.Now && p.EndDate >= DateTime.Now).ToList();
            Promotion promotion = new Promotion();
            if (promotions.Count == 0)
            {
                promotion = null;
            }
            else
            {
                foreach (var item in promotions)
                {
                    if (query.PromotionId == item.Id)
                    {
                        promotion = item;
                        break;
                    }
                }
            }

            // Lấy comment và rating
            var reviews = _context.Reviews.Where(r => r.ProductId == id).ToList();

            //Hiển thị lên view
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.CountImages = images.Count;
            product.Id = query.Id;
            product.Name = query.Name;
            product.Price = query.Price;
            if (promotion != null)
            {
                product.Promotion = (int)(promotion.percent * 100);
            }
            else
            {
                product.Promotion = 0;
            }
            
            product.Year_SX = query.Year_SX;
            if (promotion != null)
            {
                product.PriceDiscounted = query.Price * (1 - promotion.percent);
            }
            else
            {
                product.PriceDiscounted = query.Price;
            }
            
            product.Images = images;
            product.Sizes = sizes;
            product.Origin = query.Origin;
            product.Trandemark = query.Trandemark;
            product.Description = query.Description;
            product.Count = count;
            product.Reviews = reviews;

            return View(product);
        }

        [Authorize]
        public async Task<ActionResult> AddCart(int id)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            AppUser user = _userManager.FindByIdAsync(userid).Result;
            var product = _context.Product.Where(p => p.Id == id).First();
            // Lấy khuyến mãi
            var promotions = _context.Promotion.Where(p => p.StartedDate <= DateTime.Now && p.EndDate >= DateTime.Now).ToList();
            Promotion promotion = new Promotion();
            if (promotions.Count == 0)
            {
                promotion = null;
            }
            else
            {
                foreach (var item in promotions)
                {
                    if (product.PromotionId == item.Id)
                    {
                        promotion = item;
                        break;
                    }
                }
            }

            // Lấy size
            var qr1 = _context.Sizes.ToList();
            var qr3 = _context.ProductSize.Where(p => p.ProductId == id).First();
            var size = qr1.Where(s => s.Id == qr3.SizeId).First();

            // Lấy ảnh
            var qr2 = _context.Image.ToList();
            var image = qr2.Where(i => i.ProductId == product.Id).First();

            // Kiểm tra sp đã có trong giỏ hàng chưa, nếu có thì tăng số lượng sp trong giỏ, nếu chưa thì thêm sp vào giỏ
            var carts = _context.Carts.Where(c => c.UserId == userid && c.IsBuy == false).ToList();
            if (carts.Where(c => c.ProductId == id).ToList().Count == 0)
            {
                var cart = new Cart();
                if (ModelState.IsValid)
                {
                    cart.Quantity = 1;
                    cart.ProductId = product.Id;
                    cart.UnitPrice = product.Price;
                    if (promotion != null)
                    {
                        cart.Promotion = promotion.percent;
                    }
                    else
                    {
                        cart.Promotion = 0;
                    }
                    cart.SelectedSize = size.Id;
                    cart.Image = image.Path;
                    cart.UserId = userid;
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                var query = carts.Where(c => c.ProductId == id).First();
                query.Promotion = promotion.percent;
                query.Quantity = query.Quantity + 1;
                _context.Update(query);
                await _context.SaveChangesAsync();
            }
            ViewBag.News = true;
            ViewBag.Class = "alert alert-success";
            ViewBag.Message = "Thêm sản phẩm vào giỏ hàng thành công!";
            return View();
            
        }

        [Authorize]
        public async Task<ActionResult> BuyNow(int id)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            AppUser user = _userManager.FindByIdAsync(userid).Result;
            var product = _context.Product.Where(p => p.Id == id).First();
            // Lấy khuyến mãi
            var promotions = _context.Promotion.Where(p => p.StartedDate <= DateTime.Now && p.EndDate >= DateTime.Now).ToList();
            Promotion promotion = new Promotion();
            if (promotions.Count == 0)
            {
                promotion = null;
            }
            else
            {
                foreach (var item in promotions)
                {
                    if (product.PromotionId == item.Id)
                    {
                        promotion = item;
                        break;
                    }
                }
            }

            // Lấy size
            var qr1 = _context.Sizes.ToList();
            var qr3 = _context.ProductSize.Where(p => p.ProductId == id).First();
            var size = qr1.Where(s => s.Id == qr3.SizeId).First();

            // Lấy ảnh
            var qr2 = _context.Image.ToList();
            var image = qr2.Where(i => i.ProductId == product.Id).First();

            // Kiểm tra sp đã có trong giỏ hàng chưa, nếu có thì tăng số lượng sp trong giỏ, nếu chưa thì thêm sp vào giỏ
            var carts = _context.Carts.Where(c => c.UserId == userid && c.IsBuy == false).ToList();
            if (carts.Where(c => c.ProductId == id).ToList().Count == 0)
            {
                var cart = new Cart();
                if (ModelState.IsValid)
                {
                    cart.Quantity = 1;
                    cart.ProductId = product.Id;
                    cart.UnitPrice = product.Price;
                    if (promotion != null)
                    {
                        cart.Promotion = promotion.percent;
                    }
                    else
                    {
                        cart.Promotion = 0;
                    }
                    cart.SelectedSize = size.Id;
                    cart.Image = image.Path;
                    cart.UserId = userid;
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                var query = carts.Where(c => c.ProductId == id).First();
                query.Promotion = promotion.percent;
                query.Quantity = query.Quantity + 1;
                _context.Update(query);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Cart", "Cart");

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}