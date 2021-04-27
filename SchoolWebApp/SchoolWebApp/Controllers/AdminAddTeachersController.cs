using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Data.Entity;
using System.Linq;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using SchoolWebApp.Models;
using WebApplication1.Models;
using System.Net.Mail;

namespace SchoolWebApp.Controllers
{
    [Authorize(Roles = "Admins")]
    public class AdminAddTeachersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin_ADD_Teachers_Panel
        public ActionResult Index(int? page)
        {
            var roleId = db.Roles.Where(m => m.Name == "Teachers").Select(m => m.Id).SingleOrDefault();
            var count = db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId)).Count();
             ViewBag.count = count;
            return View(db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId)).ToList().ToPagedList(page ?? 1, 10));
        }

        // GET: Admin_ADD_Teachers_Panel/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Admin_ADD_Teachers_Panel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin_ADD_Teachers_Panel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,PhoneNumber,Age,Sex,Subject,Salary,FullName,UserName")]ApplicationUser applicationUser)
        {
            var rolemanger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var usermanger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();

            if (ModelState.IsValid)
            {
                ApplicationUser user2 = new ApplicationUser();
                user2.Email = applicationUser.Email;
                user2.PhoneNumber = applicationUser.PhoneNumber;
                user2.Age = applicationUser.Age;
                user2.Sex = applicationUser.Sex;
                user2.Subject = applicationUser.Subject;
                user2.Salary = applicationUser.Salary;
                user2.FullName = applicationUser.FullName;
                user2.UserName = applicationUser.Email;

                var check2 = usermanger.Create(user2, applicationUser.Email);
                if (check2.Succeeded)
                {
                    usermanger.AddToRole(user2.Id, "Teachers");
                }

                db.Users.Add(user2);
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET:Admin_ADD_Teachers_Panel/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Admin_ADD_Teachers_Panel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Admin_ADD_Teachers_Panel/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Admin_ADD_Teachers_Panel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SendMessage(string id)
        {
            var phone = db.Users.Where(m => m.Id == id).Select(a => a.PhoneNumber).SingleOrDefault();
            ViewBag.phone = phone;
            Session["phone"] = phone;
            return View();
        }

        public ActionResult SendMessage(Sms messages)
        {
            String result;
            string apiKey = "ofObqZGN+ts-pnhr1atRdT6BuAeHS8QltYQXbhTk9d";
            string numbers = Session["phone"].ToString(); // in a comma seperated list
            string message = messages.ContentMsg;
            string sender = "ameera jabban";

            String url = "https://api.txtlocal.com/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
            //refer to parameters to complete correct url string

            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(url);
            }
            catch (Exception e)
            {
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                // Close and clean up the StreamReader
                sr.Close();
            }
           // return result;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult sendmsg(string id)
        {
            var email = db.Users.Where(m => m.Id == id).Select(a => a.Email).SingleOrDefault();
            ViewBag.email = email;
            return View();
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
