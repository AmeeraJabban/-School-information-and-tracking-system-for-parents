using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolWebApp.Models;

namespace WebApplication1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Sex { get; set; }
        public string Subject { get; set; }
        public string Salary { get;  set; }
        public string Age { get;  set; }
        public string FullName { get;  set; }
        public string image { get; set; }
        public string ParentName { get; set; }
        public string address { get; set; }
        public int GradeId { get; set; }
        public string SectionId { get; set; }







        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<SchoolWebApp.Models.NewsTypes> NewsTypes { get; set; }

        public System.Data.Entity.DbSet<SchoolWebApp.Models.News> News { get; set; }

        public System.Data.Entity.DbSet<SchoolWebApp.Models.TimeTablesTypes> TimeTablesTypes { get; set; }

        public System.Data.Entity.DbSet<SchoolWebApp.Models.TimeTables> TimeTables { get; set; }

        public System.Data.Entity.DbSet<SchoolWebApp.Models.Sections> Sections { get; set; }

        public System.Data.Entity.DbSet<SchoolWebApp.Models.Grades> Grades { get; set; }

        public System.Data.Entity.DbSet<SchoolWebApp.Models.Photos> Photos { get; set; }

        public System.Data.Entity.DbSet<SchoolWebApp.Models.Albums> Albums { get; set; }

        public System.Data.Entity.DbSet<SchoolWebApp.Models.GradeTimeTable> GradeTimeTables { get; set; }

        public System.Data.Entity.DbSet<SchoolWebApp.Models.Notes> Notes { get; set; }

        public System.Data.Entity.DbSet<SchoolWebApp.Models.Attends> Attends { get; set; }

    }
}