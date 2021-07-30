using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Image
    {
        public int Id { set; get; }
        public string Path { set; get; }

        public virtual Product Product { set; get; }
    }
}
