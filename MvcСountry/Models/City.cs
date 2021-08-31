using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcCountry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcСountry.Models
{
    
    public class City
    {
        
        //public City() => Country = new Country();


        public Country Country { get; set; }
        //public City() => Country = new Country ();
        //public int CountryId { get; set; }
        [Key]
        public int CityId { get; set; }
       
        public string capital { get; set; }
        
    }
   

}
