using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskApplication.Models
{
    public class City
    {
        public int cityId { get; set; }
        public string cityName { get; set; }
        public int countryId { get; set; }
        [ForeignKey("countryId")]
        public Country Country { get; set; }
    }
}