using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.Data.CityDropdownlist;

namespace TracyShop.ViewModels
{
    public class ChangeAddressModel
    {
        [Required]
        public int Id { set; get; }

        [Required]
        public string SpecificAddress { set; get; }

        [Required]
        public string SelectDistrict { set; get; }

        [Required]
        public string SelectCity { set; get; }

        [Required]
        public List<District> Districts { set; get; }

        [Required]
        public List<City> Cities { set; get; }
    }
}
