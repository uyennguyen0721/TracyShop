using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class ResponseMessage
    {
        [Key]
        public int Id { set; get; }
        public DateTime CreatedAt { set; get; } = DateTime.Now;
        public string Response { set; get; }
        public int ChatId { set; get; }

        public virtual Chat Chat { set; get; }
    }
}
