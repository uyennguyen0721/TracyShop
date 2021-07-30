using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Order
    {
        public int Id { set; get; }
        public DateTime Created_date { set; get; }
        public bool Is_check { set; get; }
        public bool Is_pay { set; get; }

        public virtual PaymentMenthod PaymentMenthod { set; get; }

        public virtual ICollection<OrderDetail> OrderDetails { set; get; }
    }
}
