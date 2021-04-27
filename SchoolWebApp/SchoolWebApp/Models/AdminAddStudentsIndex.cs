using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolWebApp.Models
{
    public class AdminAddStudentsIndex
    {
        public string Id { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }

        public string ParentName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string GradeName { get; set; }
        public string SectionName { get; set; }
    }
}