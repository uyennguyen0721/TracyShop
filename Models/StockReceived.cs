using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class StockReceived
    {
        [Key]
        public int Id { set; get; }

        public DateTime Date { set; get; } = DateTime.Now;
        public string UserId { set; get; }

        public virtual AppUser User { set; get; }

        public virtual ICollection<StockReceivedDetail> StockReceivedDetails { set; get; }
    }
}
