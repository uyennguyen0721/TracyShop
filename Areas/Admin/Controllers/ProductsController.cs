using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.ViewModels;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: Admin/Products
        public async Task<IActionResult> Index(int id)
        {
            if(id <= 0)
            {
                return View(await _context.Product.ToListAsync());
            }
            else
            {
                return View(await _context.Product.Where(p => p.CategoryId == id).ToListAsync());
            }
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ProductManageModel productManage = new ProductManageModel();
            productManage.Categories = _context.Category.ToList();
            productManage.Promotions = _context.Promotion.ToList();
            return View(productManage);
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductManageModel productManage)
        {
            var product = new Product();
            if (ModelState.IsValid)
            {
                product.Name = productManage.Name;
                product.Description = productManage.Description;
                product.Price = productManage.Price;
                product.Year_SX = productManage.Year_SX;
                product.Origin = productManage.Origin;
                product.Trandemark = productManage.Trandemark;
                product.Active = productManage.Active;
                product.CategoryId = productManage.SelectedCate;
                product.PromotionId = productManage.SelectedPromo;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productManage);
        }

        // GET: Admin/Products/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            var productManage = new ProductManageModel();
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                productManage.Id = product.Id;
                productManage.Name = product.Name;
                productManage.Description = product.Description;
                productManage.Price = product.Price;
                productManage.Year_SX = product.Year_SX;
                productManage.Origin = product.Origin;
                productManage.Trandemark = product.Trandemark;
                productManage.Active = product.Active;
                productManage.Categories = _context.Category.ToList();
                productManage.SelectedCate = product.CategoryId;
                productManage.Promotions = _context.Promotion.ToList();
                productManage.SelectedPromo = product.PromotionId;
                return View(productManage);
            }
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductManageModel productManage)
        {
            if (id != productManage.Id)
            {
                return NotFound();
            }

            var product = _context.Product.Where(p => p.Id == productManage.Id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    product.Name = productManage.Name;
                    product.Description = productManage.Description;
                    product.Price = productManage.Price;
                    product.Year_SX = productManage.Year_SX;
                    product.Origin = productManage.Origin;
                    product.Trandemark = productManage.Trandemark;
                    product.Active = productManage.Active;
                    product.CategoryId = productManage.SelectedCate;
                    product.PromotionId = productManage.SelectedPromo;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, Employee")]
        public IActionResult ProductSizeDetail(Product product)
        {
            if (product == null)
            {
                return NotFound();
            }

            var productSize =  _context.ProductSize.Where(p => p.ProductId == product.Id).ToList();
            if (productSize == null)
            {
                return NotFound();
            }
            return View(productSize);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
