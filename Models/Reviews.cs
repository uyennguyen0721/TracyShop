using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Reviews
    {
        public int Id { set; get; }
        public int Rate { set; get; }
        public string Content { set; get; }
        public string Image { set; get; }

        public virtual Product Product { set; get; }
        public virtual AppUser User { set; get; }
    }
}
