using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.ViewModels;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StockController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public StockController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Index()
        {
            var productsInStock = new List<StockViewModel>();
            var products = _context.Product.ToList();
            foreach (var product in products)
            {
                var productInStock = new StockViewModel();
                productInStock.ProductId = product.Id;
                productInStock.ProductName = product.Name;
                var productSizes = _context.ProductSize.Where(p => p.ProductId == product.Id).ToList();
                var total = 0;
                foreach (var item in productSizes)
                {
                    total += item.Quantity;
                }
                productInStock.TotalQuantity = total;
                productsInStock.Add(productInStock);
            }
            
            return View(productsInStock);
        }

        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Details(int id)
        {
            var productSize = _context.ProductSize.Where(p => p.ProductId == id).ToList();
            return View(productSize);
        }

        [Authorize(Roles = "Admin, Employee")]
        [Route("/admin/stock-received-info", Name = "stock-received-info")]
        public IActionResult StockReceivedInfo()
        {
            List<StockReceived> stockReceived = _context.StockReceived.ToList();
            return View(stockReceived);
        }

        [Authorize(Roles = "Admin, Employee")]
        [Route("/admin/stock-received-item-info", Name= "stock-received-item-info")]
        public IActionResult StockReceivedItemInfo(int id)
        {
            var stockReceivedItem = _context.StockReceivedDetail.Where(s => s.StockReceivedId == id).ToList();
            return View(stockReceivedItem);
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpGet]
        [Route("/admin/stock-received", Name ="stock-received")]
        public IActionResult StockReceived()
        {
            ProductStockItem productStock = new ProductStockItem();
            productStock.Products = _context.Product.ToList();
            productStock.Sizes = _context.Sizes.ToList();
            return View(productStock);
        }

        [Authorize(Roles = "Admin, Employee")]
        [Route("/admin/stock-received", Name = "stock-received")]
        [HttpPost]
        public async Task<IActionResult> StockReceived(ProductStockItem productStock)
        {
            ProductSize product = new ProductSize();
            StockReceived stockReceived = new StockReceived();
            StockReceivedDetail receivedDetail = new StockReceivedDetail();
            if (ModelState.IsValid)
            {
                product = _context.ProductSize.FirstOrDefault(p => p.ProductId == productStock.SelectedPro && p.SizeId == productStock.SelectedSize);

                if(product == default)
                {
                    // Thêm mới số lượng vào ProductSize
                    var productSize = new ProductSize
                    {
                        ProductId = productStock.SelectedPro,
                        SizeId = productStock.SelectedSize,
                        Quantity = productStock.Quantity
                    };
                    _context.ProductSize.Add(productSize);

                }
                else
                {
                    // Cập nhật số lượng vào ProductSize
                    product.Quantity += productStock.Quantity;
                    _context.Update(product);
                }

                if(_context.StockReceivedDetail.ToList().Count != 0)
                {
                    var stockDetail = _context.StockReceivedDetail.OrderBy(s => s.Id).Last();
                    var stock = _context.StockReceived.Where(s => s.Id == stockDetail.StockReceivedId).First();

                    if (productStock.SelectedPro == stockDetail.ProductId && (stock.Date.Date == DateTime.Now.Date))
                    {
                        stockDetail.Quantity += productStock.Quantity;
                        _context.Update(stockDetail);
                    }
                    else
                    {
                        // Thêm nhập kho
                        stockReceived.UserId = _userManager.GetUserId(HttpContext.User);
                        _context.Add(stockReceived);

                        // Cập nhật CSDL
                        await _context.SaveChangesAsync();
                        Task.Delay(500).Wait();

                        receivedDetail.ProductId = productStock.SelectedPro;
                        receivedDetail.StockReceivedId = _context.StockReceived.OrderBy(s => s.Id).Last().Id;
                        receivedDetail.Quantity = productStock.Quantity;
                        receivedDetail.Unit_price = productStock.UnitPrice;
                        _context.Add(receivedDetail);
                    }
                    await _context.SaveChangesAsync();
                    Task.Delay(100).Wait();
                }
                else
                {
                    // Thêm nhập kho
                    stockReceived.UserId = _userManager.GetUserId(HttpContext.User);
                    _context.Add(stockReceived);

                    // Cập nhật CSDL
                    await _context.SaveChangesAsync();
                    Task.Delay(500).Wait();

                    receivedDetail.ProductId = productStock.SelectedPro;
                    receivedDetail.StockReceivedId = _context.StockReceived.OrderBy(s => s.Id).Last().Id;
                    receivedDetail.Quantity = productStock.Quantity;
                    receivedDetail.Unit_price = productStock.UnitPrice;
                    _context.Add(receivedDetail);

                    await _context.SaveChangesAsync();
                    Task.Delay(100).Wait();
                }
            }
            return RedirectToAction("Index", "Stock");
        }
    }
}
