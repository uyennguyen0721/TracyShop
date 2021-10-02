using Microsoft.AspNetCore.Authorization;
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

        public StockController(AppDbContext context)
        {
            _context = context;
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
    }
}
