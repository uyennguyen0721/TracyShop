using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.ViewModels
{
    public class ResponseViewModel
    {
        public int Id { set; get; }
        public string Response { set; get; }
        public int ChatId { set; get; }
        public string UserId { set; get; }
    }
}
