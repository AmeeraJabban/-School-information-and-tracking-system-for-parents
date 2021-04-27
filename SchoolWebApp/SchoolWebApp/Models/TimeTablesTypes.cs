using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


// ameera_45149
namespace SchoolWebApp.Models
{
    public class TimeTablesTypes
    {

        public int Id { get; set; }


        [Required]
        [Display(Name = "نوع الجدول")]
        public string TimeTablesTypeName { get; set; }


        [Required]
        [Display(Name = "تعريف الجدول")]
        public string TimeTablesTypeDescription { get; set; }


        // many TimeTables to one type
        public virtual ICollection<TimeTables> TimeTable { get; set; }

    }
}