using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Models;

namespace TracyShop.ViewModels
{
    public class ChangeAddressModel
    {
        public int Id { set; get; }
        public string SpecificAddress { set; get; }
        public int SelectDistrictId { set; get; }
        public int SelectCityId { set; get; }
    }
}
