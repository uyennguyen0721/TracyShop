using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;

namespace TracyShop.ViewModels
{
    public class UploadImagesOfProduct
    {
        public List<IFormFile> Images { set; get; }
        public List<Product> Product { set; get; }
        public int Selected { set; get; }
    }
}
