using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using MvcСountry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCountry.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Country
    {
        /**
        * Класс модели страны
        * Class Country
        * 
        */
        [Key]
  
        public int CountryId { get; set; }
        [Display(Name = "Name")]
        [Required]
      
        public string Name { get; set; }
        [Display(Name = "Сapital")]
        public string capital { get; set; }
        public string Alpha2Code { get; set; }
        public int CityForeignKey { get; set; }
        public City City { get; set; }
        public Regions Regions { get; set; }
        public int RegionForeignKey { get; set; }
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double? Area { get;  set; }
        public int Population { get; set; }
        public string Region { get; set; }
    }
    

}