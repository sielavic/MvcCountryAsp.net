using MvcCountry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcСountry.Models
{
    public class Regions
    {
        /**
        * Класс модели региона
        * Class Regions
        * 
        */
        public int Id { get; set; }
        public string Region { get; set; }
        public Country Country { get; set; }
    }
}
