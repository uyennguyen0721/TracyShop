using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public ProductController(ILogger<ProductController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
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
                //var query = _context.Product.Where(p => p.CategoryId == id);
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
            }
            return View(await query.AsNoTracking().ToListAsync());
        }

        // GET: ProductController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}