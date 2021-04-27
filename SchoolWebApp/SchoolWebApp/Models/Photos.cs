using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolWebApp.Models
{
    public class Photos
    {
        public int Id { get; set; }


        public string PhotoUrl { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }


        [Required]
        [DisplayName("نوع الخبر")]
        public int AlbumsID { get; set; }          //Foergin Key
        public virtual Albums Album { get; set; }
    }
}