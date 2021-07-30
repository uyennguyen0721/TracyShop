using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Size
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }

        public virtual ICollection<Product> Products { set; get; }
    }
}
