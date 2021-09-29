using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.ViewModels
{
    public class PaymentModel
    {
        [Required, EmailAddress, Display(Name = "Địa chỉ email đã đăng ký")]
        public string Email { get; set; }
        public bool IsPay { get; set; }
        public bool EmailSent { get; set; }
    }
}
