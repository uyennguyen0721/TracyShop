using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Size
    {
        [Key]
        public int Id { set; get; }

        [MaxLength(50)]
        public string Name { set; get; }

        [MaxLength(255)]
        public string Description { set; get; }

        public virtual ICollection<ProductSize> ProductSizes { set; get; }
    }
}
