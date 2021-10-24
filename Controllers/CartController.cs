using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Helpers;
using TracyShop.Models;

namespace TracyShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ILogger<CartController> logger, AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: CartController
        [Authorize]
        [Route("/cart")]
        public IActionResult Cart()
        {
            var cart = _context.Carts.Where(c => (c.UserId == _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value && c.IsBuy == false)).ToList();
            if(cart.Count == 0)
            {
                ViewBag.Message = "Giỏ hàng của bạn còn trống.";
                return View();
            }
            else
            {
                ViewBag.Message = "";
                float total = 0;
                int count = 0;
                foreach(var item in cart)
                {
                    total += item.UnitPrice * (1 - item.Promotion) * item.Quantity;
                    count += item.Quantity;
                }
                ViewBag.Count = count;
                ViewBag.Total = total;
                return View(cart);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult UpdateCart(int id)
        {
            var cart = _context.Carts.Where(c => c.Id == id).First();
            List<Size> sizes = new List<Size>();
            // Lấy danh sách sizes và descriptionSize
            var qr1 = _context.Sizes.ToList();
            var qr3 = _context.ProductSize.Where(p => p.ProductId == cart.ProductId && p.Quantity > 0).ToList();
            foreach (var pr in qr3)
            {
                sizes.Add(qr1.Where(d => d.Id == pr.SizeId).First());
            }
            ViewBag.Sizes = new SelectList(sizes, "Id", "Name");
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateCart(int id, Cart cart)
        {
            if(id != cart.Id)
            {
                return NotFound();
            }
            var carts = await _context.Carts.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    carts.Quantity = cart.Quantity;
                    carts.SelectedSize = cart.SelectedSize;
                    _context.Update(carts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Cart", "Cart");
            }
            return View(carts);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Cart));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
