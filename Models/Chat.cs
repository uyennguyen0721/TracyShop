using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Models
{
    public class Chat
    {
        public int Id { set; get; }
        public DateTime CreatedAt { set; get; } = DateTime.Now;
        public string Request { set; get; }
        public string UserId { set; get; }
        public int MessageId { set; get; }
        public bool IsSeen { set; get; } = false;

        public virtual AppUser User { set; get; }
        public virtual Message Message { set; get; }

        public virtual ICollection<ResponseMessage> ResponseMessages { set; get; }
    }
}
