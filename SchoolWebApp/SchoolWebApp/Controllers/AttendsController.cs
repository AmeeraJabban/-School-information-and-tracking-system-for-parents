using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using SchoolWebApp.Models;
using WebApplication1.Models;

namespace SchoolWebApp.Controllers
{
    public class AttendsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Attends
        public ActionResult Index(int? page)
        {
            var UserID = User.Identity.GetUserId();
            var attends = db.Attends.Include(a => a.Grade).Include(a => a.Section).Include(a => a.Student).Include(a => a.Teacher);
            return View(attends.ToList().Where(a => a.TeacherID == UserID).Where(a=> a.Attend== "Absent").Where(a => a.AttendDate.ToString().Substring(0,10) == DateTime.Now.ToString().Substring(0,10)).ToPagedList(page ?? 1, 10));
        }

        public ActionResult ChosseClass()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChosseClass(AddNewStudentViewModel addNewStduentViewmodel)
        {

            if (ModelState.IsValid)
            {
                Session["GradeId"] = addNewStduentViewmodel.GradeId.ToString();
                Session["SectionId"] = addNewStduentViewmodel.SectionId.ToString();
                return RedirectToAction("GetStudents");
            }

            return View(addNewStduentViewmodel);
        }


        public ActionResult GetStudents(int? page)
        {
            var G = Convert.ToInt64(Session["GradeId"]);
            var S = Convert.ToInt64(Session["SectionId"]);

            var roleId = db.Roles.Where(m => m.Name == "Students").Select(m => m.Id).SingleOrDefault();
            var SectionId = db.Sections.FirstOrDefault(section => section.Id == S).Id;
            var SectionName = db.Sections.FirstOrDefault(section => section.Id == S).SectionName;
            var GradeName = db.Grades.FirstOrDefault(grade => grade.Id == G).ClassGrade.ToString();



            var users = db.Users.Where(user => user.Roles.Any(userRole => userRole.RoleId == roleId))
                                .OrderBy(user => user.Id).Where(grade => grade.GradeId == G).Where(a => a.SectionId == SectionId.ToString())
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
                                    GradeName = GradeName,
                                    SectionName = SectionName,
                                }).ToPagedList(page ?? 1, 10);

            return View(users);





        }


        public ActionResult Absent(Attends attends,string Id)
        {
            var StudentId = db.Users.Where(m => m.Id == Id).Select(a => a.Id).SingleOrDefault();
            var StudentName = db.Users.Where(m => m.Id == Id).Select(a => a.FullName).SingleOrDefault();
            var UserID = User.Identity.GetUserId();
            var GradesId = db.Users.Where(m => m.Id == Id).Select(a => a.GradeId).SingleOrDefault();
            var SectionsId = db.Users.Where(m => m.Id == Id).Select(a => a.SectionId).SingleOrDefault();
            Session["GradesId"] = Convert.ToInt32(GradesId.ToString());

            attends.AttendDate = DateTime.Now;
            attends.StudentID = StudentId;
            attends.TeacherID = UserID;
            attends.SectionsId = Convert.ToInt32(SectionsId);
            attends.GradesId = GradesId;
            attends.Attend = "Absent";

            db.Attends.Add(attends);
            db.SaveChanges();
            return View();
        }
        public ActionResult Attends(Attends attends, string Id)
        {
            var StudentId = db.Users.Where(m => m.Id == Id).Select(a => a.Id).SingleOrDefault();
            var StudentName = db.Users.Where(m => m.Id == Id).Select(a => a.FullName).SingleOrDefault();
            var UserID = User.Identity.GetUserId();
            var GradesId = db.Users.Where(m => m.Id == Id).Select(a => a.GradeId).SingleOrDefault();
            var SectionsId = db.Users.Where(m => m.Id == Id).Select(a => a.SectionId).SingleOrDefault();
            Session["GradesId"] = Convert.ToInt32(GradesId.ToString());

            attends.AttendDate = DateTime.Now;
            attends.StudentID = StudentId;
            attends.TeacherID = UserID;
            attends.SectionsId = Convert.ToInt32(SectionsId);
            attends.GradesId = GradesId;
            attends.Attend = "Attends";

            db.Attends.Add(attends);
            db.SaveChanges();
            return View();
        }



        public ActionResult GEtAttends(int? page)
        {
            var UserID = User.Identity.GetUserId();
            var attends = db.Attends.Include(a => a.Grade).Include(a => a.Section).Include(a => a.Student).Include(a => a.Teacher);
            return View(attends.ToList().Where(a => a.StudentID == UserID).Where(a => a.Attend == "Absent").Where(a => a.AttendDate.ToString().Substring(0, 10) == DateTime.Now.ToString().Substring(0, 10)).ToPagedList(page ?? 1, 10));
        }



        // GET: Attends/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attends attends = db.Attends.Find(id);
            if (attends == null)
            {
                return HttpNotFound();
            }
            return View(attends);
        }

      
        // GET: Attends/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attends attends = db.Attends.Find(id);
            if (attends == null)
            {
                return HttpNotFound();
            }
            return View(attends);
        }

        // POST: Attends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attends attends = db.Attends.Find(id);
            db.Attends.Remove(attends);
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
    }
}
