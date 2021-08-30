using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.ViewModels
{
    public class UpdateUserInfoModel
    {
        public string Id { set; get; }

        [Required]
        public string Name { set; get; }

        [Required]
        public string UserName { set; get; }

        [Required]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        [Phone]
        public string PhoneNumber { set; get; }

        [Required]
        public string Avatar { set; get; }

        [Required]
        public bool Is_active { set; get; }

        [Required]
        public DateTime Joined_date { set; get; }

        [Required]
        public string Gender { set; get; }

        [Required]
        public DateTime? Birthday { set; get; }
    }
}
