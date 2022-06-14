using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.Repository;
using TracyShop.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Collections.Generic;

namespace TracyShop.Controllers
{
    public class ProfileController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly ILoginRepository _loginRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public ProfileController(ILoginRepository loginRepository, UserManager<AppUser> userManager, AppDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _loginRepository = loginRepository;
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Authorize]
        [Route("profile", Name = "profile")]
        public IActionResult Profile()
        {
            var userid = _userManager.GetUserId(HttpContext.User);

            if(userid == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                AppUser user = _userManager.FindByIdAsync(userid).Result;
                ProfileModel profile = new ProfileModel();
                profile.Name = user.Name;
                profile.UserName = user.UserName;
                profile.Email = user.Email;
                profile.Birthday = user.Birthday;
                profile.PhoneNumber = user.PhoneNumber;
                profile.Gender = user.Gender;
                profile.AvatarPath = null;
                return View(profile);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("profile", Name = "profile")]
        public async Task<IActionResult> Profile(ProfileModel userdetails)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            AppUser user = _userManager.FindByIdAsync(userid).Result;

            string fileName = "";
            if(userdetails.AvatarPath != null)
            {
                string wwwRootPath = _hostingEnvironment.WebRootPath;
                fileName = Path.GetFileNameWithoutExtension(userdetails.AvatarPath.FileName);
                string extension = Path.GetExtension(userdetails.AvatarPath.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string filePath = Path.Combine(wwwRootPath + "/img/avatar/", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await userdetails.AvatarPath.CopyToAsync(fileStream);
                }
            }
            else
            {
                fileName = "";
            }
            
            user.Name = userdetails.Name;
            user.PhoneNumber = userdetails.PhoneNumber;
            user.Gender = userdetails.Gender;
            user.Birthday = userdetails.Birthday;
            
            if(fileName == "")
            {
                user.Avatar = fileName;
            }
            else
            {
                user.Avatar = "/img/avatar/" + fileName;
            }
            
            userdetails.Email = user.Email;
            userdetails.UserName = user.UserName;
            IdentityResult x = await _userManager.UpdateAsync(user);
            if (x.Succeeded)
            {
                ViewBag.Message = $"Update user information successed.";
                return View("profile", userdetails);
            }
            else
            {
                ViewBag.Message = $"Update user information failed.";
                return View(userdetails);
            }
        }


        [Authorize]
        [HttpGet]
        [Route("profile/change-address", Name = "change-address")]
        public IActionResult ChangeAddress()
        {
            var qr = _context.Address.ToList();
            var address = qr.Where(d => d.UserId == _userManager.GetUserId(HttpContext.User)).ToList();
            if (address.Count() == 0)
            {
                return View();
            }
            else
            {
                var model = address.First();
                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("profile/change-address")]
        public async Task<IActionResult> ChangeAddress(Address address)
        {
            var qr = _context.Address.ToList();
            var model = qr.Where(a => a.UserId == _userManager.GetUserId(HttpContext.User)).ToList();
            if(model.Count() == 0)
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine(address);
                    address.UserId = _userManager.GetUserId(HttpContext.User);
                    _context.Add(address);
                    await _context.SaveChangesAsync();
                    ViewBag.Message = true;
                    return View(address);
                }
                else
                {
                    ViewBag.Message = false;
                    return View(address);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var q = model.First();
                    q.City = address.City;
                    q.District = address.District;
                    q.SpecificAddress = address.SpecificAddress;
                    _context.Update(q);
                    await _context.SaveChangesAsync();
                    ViewBag.Message = true;
                    return View(address);
                }
                else
                {
                    ViewBag.Message = false;
                    return View(address);
                }
            }
        }


        [Authorize]
        [Route("profile/change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost("profile/change-password")]   
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _loginRepository.ChangePasswordAsync(model);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        [Authorize]
        [Route("/purchase-histrory")]
        public IActionResult PurchaseHistory()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var histories = new List<PurchaseHistoryViewModel>();
            var order = _context.Orders.Where(o => o.UserId == userId).ToList();
            if(order.Count == 0)
            {
                ViewBag.News = "Bạn chưa có đơn hàng nào.";
            }
            else
            {
                ViewBag.News = "";
                foreach (var item in order)
                {
                    var history = new PurchaseHistoryViewModel();
                    float total = 0;
                    var orderDetail = _context.OrderDetail.Where(o => o.OrderId == item.Id).ToList();
                    foreach (var detail in orderDetail)
                    {
                        total += detail.Price * detail.Quantity;
                    }
                    history.OrderId = item.Id;
                    history.OrderDate = item.Created_date;
                    history.OrderDetails = orderDetail;
                    history.TotalPrice = total;
                    history.Status = item.Status;
                    histories.Add(history);
                }
                ViewBag.WaitingForConfirmation = histories.Where(p => p.Status == 0).ToList().Count;
                ViewBag.WaitingForGetting = histories.Where(p => p.Status == 1).ToList().Count;
                ViewBag.Delivering = histories.Where(p => p.Status == 2).ToList().Count;
                ViewBag.Received = histories.Where(p => p.Status == 3).ToList().Count;
                ViewBag.Detroyed = histories.Where(p => p.Status == 4).ToList().Count;
            }
            return View(histories);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = _context.Orders.Where(p => p.Id == orderId && p.Status == 0).FirstOrDefault();
            order.Status = 4;
            order.Is_check = true;
            _context.Update(order);

            // cập nhật số lương sản phẩm trong vào kho
            var orderDetails = _context.OrderDetail.Where(p => p.OrderId == orderId).ToList();
            foreach(var item in orderDetails)
            {
                var proSize = _context.ProductSize.FirstOrDefault(p => p.ProductId == item.ProductId && p.SizeId == item.SelectedSize);
                proSize.Quantity += item.Quantity;
                _context.Update(proSize);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("PurchaseHistory", "Profile");
        }
    }
}
