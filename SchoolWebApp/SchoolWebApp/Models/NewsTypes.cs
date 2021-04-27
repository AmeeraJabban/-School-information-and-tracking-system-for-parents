using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolWebApp.Models
{
    public class NewsTypes
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "صنف الخبر")]
        public string NewsTypeName { get; set; }


        [Required]
        [Display(Name = "تعريف الصنف")]
        public string NewsTypeDescription { get; set; }


        // many News to one type
        public virtual ICollection<News> News { get; set; }

    }
}