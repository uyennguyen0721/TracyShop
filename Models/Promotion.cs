using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Promotion
    {
        [Key]
        public int Id { set; get; }
        public float percent { set; get; }
        public DateTime StartedDate { set; get; }
        public DateTime EndDate { set; get; }

        public virtual ICollection<Product> Products { set; get; }
    }
}
