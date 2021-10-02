using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TracyShop.Models;
using TracyShop.ViewModels;
using TracyShop.Repository;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using TracyShop.Data;

namespace TracyShop.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;

        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ILogger<LoginController> logger;
        private readonly AppDbContext context;

        public LoginController(ILoginRepository loginRepository,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<LoginController> logger,
            AppDbContext context)
        {
            _loginRepository = loginRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.context = context;
        }

        [Route("register")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl)
        {
            RegisterModel model = new RegisterModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel userModel)
        {
            if (ModelState.IsValid)
            {
                // write your code
                var result = await _loginRepository.CreateUserAsync(userModel);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }

                    return View(userModel);
                }

                ModelState.Clear();
                return RedirectToAction("ConfirmEmail", new { email = userModel.Email });
            }

            return View(userModel);
        }


        [Route("login")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginModel model = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel signInModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _loginRepository.PasswordSignInAsync(signInModel);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Không được phép đăng nhập");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Tài khoản bị khóa. Hãy thử sau một thời gian.");
                }
                else
                {
                    ModelState.AddModelError("", "Thông tin không hợp lệ");
                }

            }

            return View(signInModel);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _loginRepository.SignOutAsync();
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            EmailConfirmModel model = new EmailConfirmModel
            {
                Email = email
            };

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _loginRepository.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    model.EmailVerified = true;
                }
            }

            return View(model);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {
            var user = await _loginRepository.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    model.EmailVerified = true;
                    return View(model);
                }

                await _loginRepository.GenerateEmailConfirmationTokenAsync(user);
                model.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi.");
            }
            return View(model);
        }

        [AllowAnonymous, HttpGet("fotgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous, HttpPost("fotgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // code here
                var user = await _loginRepository.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    await _loginRepository.GenerateForgotPasswordTokenAsync(user);
                }

                ModelState.Clear();
                model.EmailSent = true;
            }
            return View(model);
        }

        [AllowAnonymous, HttpGet("reset-password")]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel
            {
                Token = token,
                UserId = uid
            };
            return View(resetPasswordModel);
        }

        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await _loginRepository.ResetPasswordAsync(model);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    model.IsSuccess = true;
                    return View(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Login",
                                    new { ReturnUrl = returnUrl });

            var properties =
                signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginModel loginModel = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty,
                    $"Error from external provider: {remoteError}");

                return View("Login", loginModel);
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty,
                    "Error loading external login information.");

                return View("Login", loginModel);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            AppUser user = null;

            if (email != null)
            {
                user = await userManager.FindByEmailAsync(email);

                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View("Login", loginModel);
                }
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(
                                        info.LoginProvider, info.ProviderKey,
                                        isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                if (email != null)
                {
                    if (user == null)
                    {
                        user = new AppUser
                        {
                            Name = info.Principal.FindFirstValue(ClaimTypes.Name),
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            EmailConfirmed = true,
                            Avatar = info.Principal.FindFirstValue("image"),
                            Gender = info.Principal.FindFirstValue(ClaimTypes.Gender)
                        };


                        await userManager.CreateAsync(user);
                        Task.Delay(1000).Wait();

                        //var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                        //var confirmationLink = Url.Action("ConfirmEmail", "Login",
                        //                new { userId = user.Id, token = token }, Request.Scheme);

                        

                        await userManager.AddLoginAsync(user, info);
                        await signInManager.SignInAsync(user, isPersistent: false);

                        Task.Delay(2000).Wait();
                        var users = context.Users.ToList().OrderByDescending(u => u.Joined_date).First();
                        var role = context.Roles.Where(r => r.Name.Contains("Customer")).First();

                        context.UserRoles.Add(new IdentityUserRole<string>
                        {
                            RoleId = role.Id,
                            UserId = user.Id
                        });
                        await context.SaveChangesAsync();
                        Task.Delay(1000).Wait();

                        return LocalRedirect(returnUrl);

                        //logger.Log(LogLevel.Warning, confirmationLink);

                        //ViewBag.ErrorTitle = "Registration successful";
                        //ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                        //    "email, by clicking on the confirmation link we have emailed you";
                        //return View("Error");
                    }

                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                else
                {
                    ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                    ViewBag.ErrorMessage = "Please contact support on TracyShop@tracyshop.com";

                    return View("Error");
                }
            }
        }
    }
}
