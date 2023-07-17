using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TaskApplication.Models
{
    public class User
    {

        public int userId { get; set; }
        [Required(ErrorMessage = "No Number nor Special Character")]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        public string fname { get; set; }
        [Required(ErrorMessage = "No Number nor Special Character")]
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        public string lname { get; set; }
        [Required]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string phoneNo { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string emailNo { get; set; }
        [Required]
        [Display(Name = "City [(Enter 1 or 2) 1 => Dhaka; 2 => Kolkata]")]
        public string userCity { get; set; }
        [ForeignKey("userCity")]
        [Required]
        [Display(Name = "Image")]
        public string userImg { get; set; }
        
        [Required]
        [Display(Name = "CV Upload")]
        public string userCV { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name = "Date Of Birth [Format (yyyy-MM-dd)]")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dob { get; set; }

        public City City { get; set; }

    }
}