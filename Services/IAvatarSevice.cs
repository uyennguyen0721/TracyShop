using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Services
{
    public interface IAvatarSevice
    {
        public void UploadAvatar(IFormFile file);
    }
}
