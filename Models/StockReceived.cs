using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class StockReceived
    {
        public int Id { set; get; }
        public DateTime date { set; get; }

        public virtual AppUser User { set; get; }

        public virtual ICollection<StockReceivedDetail> StockReceivedDetails { set; get; }
    }
}
