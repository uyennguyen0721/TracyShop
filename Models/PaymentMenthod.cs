using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class PaymentMenthod
    {
        [Key]
        public int Id { set; get; }

        [MaxLength(50)]
        public string Name { set; get; }

        public virtual ICollection<Order> Orders { set; get; }
    }
}
