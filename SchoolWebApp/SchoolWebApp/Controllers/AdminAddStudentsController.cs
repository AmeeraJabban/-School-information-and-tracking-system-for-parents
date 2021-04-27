using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using PagedList;

using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace SchoolWebApp.Controllers
{
    [Authorize(Roles = "Admins")]
    public class AdminAddStudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: استعراض جميع الطلاب
        public ActionResult Index(int? page)
        {

            var roleId = db.Roles.Where(m => m.Name == "Students").Select(m => m.Id).SingleOrDefault();
            var count = db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId)).Count();
            ViewBag.count = count;
            var users = db.Users.Where(user => user.Roles.Any(userRole => userRole.RoleId == roleId))
                                .OrderBy(user => user.Id)
                                .Select(user => new AdminAddStudentsIndex()
                                                {
                                                    Id = user.Id,
                                                    Email = user.Email,
                                                    PhoneNumber = user.PhoneNumber,
                                                    Sex = user.Sex,
                                                    Age = user.Age,
                                                    FullName = user.FullName,
                                                    Image = user.image,
                                                    ParentName = user.ParentName,
                                                    Address = user.address,
                                    GradeName = db.Grades.FirstOrDefault(grade => grade.Id == user.GradeId).ClassGrade.ToString(),
                                    SectionName = db.Sections.FirstOrDefault(section => section.Id.ToString() ==user.SectionId).SectionName,
                                }).ToPagedList(page ?? 1, 10);

            return View(users);
        }

        // GET:استعراض المعلومات التفصيلية للطالب
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

        // GET: اضافة طالب جديد
        public ActionResult Create()
        {
            var entityViewModel = new AddNewStudentViewModel()
            {
                Grades = db.Grades.Select(grade => new SelectListItem()
                {
                    Text = grade.ClassGrade.ToString(),
                    Value = grade.Id.ToString()
                }),
                Sections = db.Sections.Select(section => new SelectListItem()
                {
                    Text = section.SectionName,
                    Value = section.Id.ToString()
                })
            };

            return View(entityViewModel);
        }

        // POST:   اضافة طالب جديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddNewStudentViewModel addNewStduentViewmodel, HttpPostedFileBase upload)
        {

            var rolemanger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var usermanger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();

            if (ModelState.IsValid)
            {
                ApplicationUser user2 = new ApplicationUser();
                user2.Email = addNewStduentViewmodel.Email;
                user2.PhoneNumber = addNewStduentViewmodel.PhoneNumber;
                user2.Sex = addNewStduentViewmodel.Sex;
                user2.address = addNewStduentViewmodel.Address;
                user2.ParentName = addNewStduentViewmodel.ParentName;
                user2.image = addNewStduentViewmodel.Image;
                user2.Age = addNewStduentViewmodel.Age.ToString();
                user2.FullName = addNewStduentViewmodel.FullName;
                user2.UserName = addNewStduentViewmodel.Email;

                user2.GradeId = addNewStduentViewmodel.GradeId;
                user2.SectionId = addNewStduentViewmodel.SectionId.ToString();




                string pic = System.IO.Path.GetFileName(upload.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/uploads"), pic);
                upload.SaveAs(path);
                user2.image = pic;

                var check2 = usermanger.Create(user2, addNewStduentViewmodel.Email);
                if (check2.Succeeded)
                {
                    usermanger.AddToRole(user2.Id, "Students");
                }

                db.Users.Add(user2);

                return RedirectToAction("Index");
            }

         
            return View(addNewStduentViewmodel);
        }






        // GET:تعديل بيانات طالب
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
            ViewBag.Grade = new SelectList(db.Grades, "Id", "ClassGrade", applicationUser.GradeId);
            ViewBag.Section = new SelectList(db.Sections, "Id", "SectionName", applicationUser.SectionId);
            return View(applicationUser);
        }

        // POST:تعديل بيانات طالب
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser applicationUser, HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {
                string old_image = Path.Combine(Server.MapPath("~/uploads/") + applicationUser.image);

                if (upload != null)
                {

                    System.IO.File.Delete(old_image);
                    string pic = System.IO.Path.GetFileName(upload.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/uploads"), pic);
                    upload.SaveAs(path);
                    applicationUser.image = pic;
                }


                try
                {
                    db.Entry(applicationUser).State = EntityState.Modified;
                    db.SaveChanges();

                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }


                return RedirectToAction("Index");
            }

            ViewBag.Grade = new SelectList(db.Grades, "Id", "ClassGrade", applicationUser.GradeId);
            ViewBag.Section = new SelectList(db.Sections, "Id", "SectionName", applicationUser.SectionId);
            return View(applicationUser);
        }

        // GET: حذف طالب
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

        // POST: حذف طالب
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            string old_image = Path.Combine(Server.MapPath("~/uploads/") + applicationUser.image);
            System.IO.File.Delete(old_image);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // لولي امر الطالب (SmS) ارسال
        [HttpGet]
        public ActionResult SendMessage(string id)
        {
            var phone = db.Users.Where(m => m.Id == id).Select(a => a.PhoneNumber).SingleOrDefault();
            ViewBag.phone = phone;
            Session["phone"] = phone;
            return View();
        }

        // لولي امر الطالب (SmS) ارسال
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
            //return result;
            return RedirectToAction("Index");
        }


        // لولي امر الطالب (E-mail) ارسال

        [HttpGet]
        public ActionResult sendmsg(string id)
        {
            var email = db.Users.Where(m => m.Id == id).Select(a => a.Email).SingleOrDefault();
            ViewBag.email = email;
            return View();
        }


    }
}
