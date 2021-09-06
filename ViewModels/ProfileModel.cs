using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.ViewModels
{
    public class ProfileModel
    {
        public string Name { set; get; }

        public string UserName { set; get; }

        public IFormFile AvatarPath { set; get; }
        public DateTime? Birthday { set; get; }

        public string Email { set; get; }

        public string PhoneNumber { set; get; }

        public string Gender { set; get; }
    }
}
