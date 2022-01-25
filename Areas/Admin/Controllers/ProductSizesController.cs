using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.ViewModels;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductSizesController : Controller
    {
        private readonly AppDbContext _context;

        public ProductSizesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductSizes
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Index(int id)
        {
            var appDbContext = _context.ProductSize.Include(p => p.Product).Include(p => p.Size);
            return View(await appDbContext.Where(p => p.ProductId == id).ToListAsync());
        }

        // GET: Admin/ProductSizes/Details/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSize
                .Include(p => p.Product)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        // GET: Admin/ProductSizes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ProductSizeModel productSize = new ProductSizeModel();
            productSize.Products = _context.Product.ToList();
            productSize.Sizes = _context.Sizes.ToList();
            return View(productSize);
        }

        // POST: Admin/ProductSizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductSizeModel productSize)
        {
            ProductSize product = new ProductSize();
            if (ModelState.IsValid)
            {
                product.ProductId = productSize.SelectedPro;
                product.SizeId = productSize.SelectedSize;
                product.Quantity = 0;
                var pro = _context.ProductSize.Where(p => p.ProductId == product.ProductId && p.SizeId == product.SizeId).FirstOrDefault();

                if(pro == null){
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    ViewBag.Message = $"Thêm không thành công vì size của sản phẩm này đã tồn tại. Vui lòng kiểm tra lại.";
                    return View(product);
                }
            }
            return View(productSize);
        }

        // POST: Admin/ProductSizes/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productSize = await _context.ProductSize.FindAsync(id);
            _context.ProductSize.Remove(productSize);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSizeExists(int id)
        {
            return _context.ProductSize.Any(e => e.ProductId == id);
        }
    }
}
