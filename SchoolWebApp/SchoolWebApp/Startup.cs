using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplication1.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }
        public void CreateDefaultRolesAndUsers()
        {
            var rolemanger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var usermanger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();


            if (!(rolemanger.RoleExists("Admins")))
            {

                role.Name = ("Admins");
                rolemanger.Create(role);
                ApplicationUser user = new ApplicationUser();
                user.Email = "ameera_45149@yahoo.com";
                user.UserName = "ameera_45149@yahoo.com";
                var check = usermanger.Create(user, ")99Ameera");
                if (check.Succeeded)
                {
                    usermanger.AddToRole(user.Id, "Admins");
                }
            }
            if (!(rolemanger.RoleExists("Teachers")))
            {

                role.Name = ("Teachers");
                rolemanger.Create(role);
            }
            if (!(rolemanger.RoleExists("Students")))
            {

                role.Name = ("Students");
                rolemanger.Create(role);
            }
            if (!(rolemanger.RoleExists("Parents")))
            {

                role.Name = ("Parents");
                rolemanger.Create(role);
            }

        }
    }
}

