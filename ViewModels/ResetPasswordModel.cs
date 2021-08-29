using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.ViewModels
{
    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Xác nhận lại mật khẩu")]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        public bool IsSuccess { get; set; }
    }
}
