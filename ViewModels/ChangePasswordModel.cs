using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.ViewModels
{
    public class ChangePasswordModel
    {
        [Required, DataType(DataType.Password), Display(Name = "Mật khẩu hiện tại")]
        public string CurrentPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Xác nhận lại mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không trùng")]
        public string ConfirmNewPassword { get; set; }
    }
}
