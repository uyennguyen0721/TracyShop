using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Promotion
    {
        public int Id { set; get; }
        public float percent { set; get; }

        public virtual ICollection<Product> Products { set; get; }
    }
}
