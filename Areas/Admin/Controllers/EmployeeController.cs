using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data;
using TracyShop.Models;
using TracyShop.Repository;

namespace TracyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILoginRepository _loginRepository;

        public EmployeeController(AppDbContext context, IHostingEnvironment hostingEnvironment, UserManager<AppUser> userManager, ILoginRepository loginRepository)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _loginRepository = loginRepository;
        }

        [Authorize(Roles = "Admin")]
        // GET: Admin/Employee
        public async Task<IActionResult> Index()
        {
            var userRole = _context.UserRoles.Where(u => u.RoleId.Contains("2")).ToList();
            List<AppUser> users = new List<AppUser>();
            foreach (var item in userRole)
            {
                var user = await _context.Users.Where(u => u.Id.Contains(item.UserId)).FirstAsync();
                users.Add(user);
            }
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        // GET: Admin/Employee/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id.Contains(id));
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // GET: Admin/Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (AppUser user)
        {
            if (ModelState.IsValid)
            {
                user.UserName = user.Email;
                user.EmailConfirmed = true;
                await _userManager.CreateAsync(user, user.PasswordHash);
                Task.Delay(200).Wait();
                var role = _context.Roles.Where(r => r.Id.Contains("2")).First();

                _context.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id
                });
                await _context.SaveChangesAsync();
                Task.Delay(100).Wait();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("/admin/add-address", Name ="add-address")]
        public IActionResult AddAddress(string? id)
        {
            ViewBag.UserId = id;
            var qr = _context.Address.ToList();
            var address = qr.Where(d => d.UserId.Contains(id)).ToList();
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/admin/add-address", Name = "add-address")]
        public async Task<IActionResult> AddAddress(string id, Address address)
        {
            var model = _context.Address.ToList().Where(a => a.UserId.Contains(id)).ToList();
            if (model.Count() == 0)
            {
                address.UserId = id;
                _context.Add(address);
                await _context.SaveChangesAsync();
                ViewBag.Message = true;
                return View(address);
            }
            else
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
        }


        // GET: Admin/Employee/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Users.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Admin/Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AppUser updateUserData)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.Name = string.IsNullOrEmpty(updateUserData.Name) ? user.Name : updateUserData.Name;
                    var result = await _userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    if (!EmployeeExists(updateUserData.Id))
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
            return View(user);
        }

        // POST: Admin/Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var employee = await _context.Users.FindAsync(id);
            employee.Is_active = false;
            _context.Update(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(string id)
        {
            return _context.Users.Any(u => u.Id == id);
        }
    }
}
