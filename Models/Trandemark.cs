using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Trandemark
    {
        [Key]
        public int Id { set; get; }

        [MaxLength(100)]
        public string Name { set; get; }

        [MaxLength(100)]
        public string Origin { set; get; }

        public virtual ICollection<Product> Products { set; get; }
    }
}
