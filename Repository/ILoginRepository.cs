using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;
using TracyShop.ViewModels;

namespace TracyShop.Repository
{
    public interface ILoginRepository
    {
        Task<AppUser> GetUserByEmailAsync(string email);

        Task<IdentityResult> CreateUserAsync(RegisterModel userModel);

        Task<SignInResult> PasswordSignInAsync(LoginModel signInModel);

        Task SignOutAsync();

        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);

        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);

        Task GenerateEmailConfirmationTokenAsync(AppUser user);

        Task GenerateForgotPasswordTokenAsync(AppUser user);

        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);
    }
}
