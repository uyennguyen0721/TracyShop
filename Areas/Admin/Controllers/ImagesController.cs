using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.ViewModels;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ImagesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImagesController(AppDbContext context , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Images
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Index(int id)
        {
            return View(await _context.Image.Where(p => p.ProductId == id).ToListAsync());
        }

        // GET: Images/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            UploadImagesOfProduct uploadImages = new UploadImagesOfProduct();
            uploadImages.Product = _context.Product.ToList();
            return View(uploadImages);
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UploadImagesOfProduct uploadImages)
        {
            foreach (var item in uploadImages.Images)
            {
                string fileName = UploadFile(item);
                var productImage = new Image
                {
                    Path = "/img/products/" + fileName,
                    ProductId = uploadImages.Selected
                };
                _context.Image.Add(productImage);
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

        private string UploadFile(IFormFile file)
        {
            string fileName = null;
            if (file != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string filePath = Path.Combine(wwwRootPath + @"/img/products/", fileName);
                //string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, @"img\products");
                //fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                //string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return fileName;


        }


        // GET: Admin/Images/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Image.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }

        // POST: Admin/Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Image.FindAsync(id);
            _context.Image.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return _context.Image.Any(e => e.Id == id);
        }
    }
}
