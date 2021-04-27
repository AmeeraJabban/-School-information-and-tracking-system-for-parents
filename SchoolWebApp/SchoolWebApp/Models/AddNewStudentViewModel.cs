using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolWebApp.Models
{
    public class AddNewStudentViewModel
    {
        public string Image { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ParentName { get; set; }
        public string FullName { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public IEnumerable<SelectListItem> Grades { get; set; }
        public int GradeId { get; set; }
        public IEnumerable<SelectListItem> Sections { get; set; }
        public int SectionId { get; set; }

    }
}