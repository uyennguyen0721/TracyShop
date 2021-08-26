using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracyShop.Services
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
    }
}
