using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;

namespace TracyShop.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Vui lòng điền họ và tên")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng điền số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [Display(Name = "Địa chỉ email")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mạnh")]
        [Compare("ConfirmPassword", ErrorMessage = "Mật khẩu không hợp lệ")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [Display(Name = "Nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public int UserRoleId { set; get; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
