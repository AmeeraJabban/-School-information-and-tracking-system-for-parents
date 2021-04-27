using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolWebApp.Models
{
    public class Albums
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("عنوان الألبوم")]
        public string AlbumTitle { get; set; }


        [Required]
        [DisplayName("تفاصيل الألبوم")]
        public string AlbumDetails { get; set; }


        [Required]
        [DisplayName("تاريخ الألبوم")]
        public DateTime AlbumDate { get; set; }


        public virtual ICollection<Photos> Photos { get; set; }


    }
}