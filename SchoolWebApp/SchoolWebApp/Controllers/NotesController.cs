using Microsoft.AspNet.Identity;
using PagedList;
using SchoolWebApp.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using WebApplication1.Models;

namespace SchoolWebApp.Controllers
{

    public class NotesController : Controller
    {
        private const string ContentType = "application/pdf";
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Notes
        public ActionResult Index(int? page)
        {

            var UserID = User.Identity.GetUserId();
            var notes = db.Notes.Include(n => n.Grade).Include(n => n.Section).Include(n => n.Student).Include(n => n.Teacher);
            return View(notes.ToList().Where(a => a.TeacherID == UserID).OrderByDescending(a => a.NoteDate).ToPagedList(page ?? 1, 10));
        }

        public ActionResult ContactTeachers(int? page)
        {
            var roleId = db.Roles.Where(m => m.Name == "Teachers").Select(m => m.Id).SingleOrDefault();
            return View(db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId)).ToList().ToPagedList(page ?? 1, 10));
        }

        public ActionResult GEtMyNotes(int? page)
        {

            var UserID = User.Identity.GetUserId();
            var notes = db.Notes.Include(n => n.Grade).Include(n => n.Section).Include(n => n.Student).Include(n => n.Teacher);
            return View(notes.ToList().Where(a => a.StudentID == UserID).OrderByDescending(a => a.NoteDate).ToPagedList(page ?? 1, 10));
        }
        public ActionResult GEtMyTables(int? page)
        {

            var UserID = User.Identity.GetUserId();
            var userSection = Convert.ToInt32(db.Users.Where(a => a.Id == UserID).Select(a => a.SectionId).SingleOrDefault());
           // var userParentName = Convert.ToInt32(db.Users.Where(a => a.Id == UserID).Select(a => a.ParentName).SingleOrDefault());
          //  ViewBag.userParentName = userParentName;
            var userGrade = Convert.ToInt32(db.Users.Where(a => a.Id == UserID).Select(a => a.GradeId).SingleOrDefault());
            var gradeTimeTables = db.GradeTimeTables.Where(g=>g.TimeTable.TimeTablesType.TimeTablesTypeName == "امتحان").Where(g => g.SectionsId== userSection).Where(g => g.GradesId== userGrade).Include(g => g.TimeTable);
            return View(gradeTimeTables.ToList().ToPagedList(page ?? 1, 10));
            }


        public ActionResult GEtMyHomeWorks(int? page)
        {

            var UserID = User.Identity.GetUserId();
            var userSection = Convert.ToInt32(db.Users.Where(a => a.Id == UserID).Select(a => a.SectionId).SingleOrDefault());
            var userGrade = Convert.ToInt32(db.Users.Where(a => a.Id == UserID).Select(a => a.GradeId).SingleOrDefault());
            var gradeHomeworks = db.GradeTimeTables.Where(g => g.TimeTable.TimeTablesType.TimeTablesTypeName == "نشاط").Where(g => g.SectionsId == userSection).Where(g => g.GradesId == userGrade).Include(g => g.TimeTable).ToList().ToPagedList(page ?? 1, 10);      
            return View(gradeHomeworks.ToPagedList(page ?? 1, 10));
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
               Session["GradeId"]= addNewStduentViewmodel.GradeId.ToString();
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
            var SectionName = db.Sections.FirstOrDefault(section => section.Id ==S).SectionName;
            var GradeName = db.Grades.FirstOrDefault(grade => grade.Id == G).ClassGrade.ToString();



            var users = db.Users.Where(user => user.Roles.Any(userRole => userRole.RoleId == roleId))
                                .OrderBy(user => user.Id).Where(grade => grade.GradeId == G).Where(a=> a.SectionId == SectionId.ToString())
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

        // GET: Notes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notes notes = db.Notes.Find(id);
            if (notes == null)
            {
                return HttpNotFound();
            }
            return View(notes);
        }

        // GET: Notes/Create
        public ActionResult Create(string Id)
        {
            var StudentName = db.Users.Where(m => m.Id == Id).Select(a => a.FullName).SingleOrDefault();
            var Studentid = db.Users.Where(m => m.Id == Id).Select(a => a.Id).SingleOrDefault();
            ViewBag.StudentName = StudentName;
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Note,NoteDate,GradesId,SectionsId,TeacherID,StudentID")] Notes notes,string Id)
        {
           
            var StudentId = db.Users.Where(m => m.Id == Id).Select(a => a.Id).SingleOrDefault();
            var StudentName = db.Users.Where(m => m.Id == Id).Select(a => a.FullName).SingleOrDefault();
            var UserID = User.Identity.GetUserId();
            var GradesId = db.Users.Where(m => m.Id == Id).Select(a => a.GradeId).SingleOrDefault();
            var SectionsId = db.Users.Where(m => m.Id == Id).Select(a => a.SectionId).SingleOrDefault();
            Session["GradesId"] = Convert.ToInt32(GradesId.ToString());
            ViewBag.StudentName = StudentName;

            //if (ModelState.IsValid)
            //{
                 notes.NoteDate = DateTime.Now;
                notes.StudentID = StudentId;
                notes.TeacherID = UserID;
                notes.SectionsId = Convert.ToInt32(SectionsId);
                notes.GradesId = GradesId;
              
                db.Notes.Add(notes);
                db.SaveChanges();
                return RedirectToAction("Index");
           // }

           // return View(notes);

                  
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(int? id)

        {
            var GradeId = db.Notes.FirstOrDefault(Id => Id.Id == id).GradesId.ToString();
            var SectionId = db.Notes.FirstOrDefault(Id => Id.Id == id).SectionsId.ToString();
            var Studentid = db.Notes.FirstOrDefault(Id => Id.Id == id).StudentID.ToString();
            var Studentname = db.Notes.FirstOrDefault(Id => Id.Id == id).Student.FullName.ToString();

            Session["GID"] = GradeId;
            Session["SID"] = SectionId;
            Session["Studentid"] = Studentid;
            Session["Studentname"] = Studentname;

            var stud_id = Session["Studentid"].ToString();
            var stud_name= Session["Studentname"].ToString();

            ViewBag.name = stud_name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notes notes = db.Notes.Find(id);
            if (notes == null)
            {
                return HttpNotFound();
            }
           return View(notes);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Notes notes)
        {
           
            if (ModelState.IsValid)
            {
                notes.TeacherID = User.Identity.GetUserId();
                notes.StudentID = Session["Studentid"].ToString();
                notes.GradesId = Convert.ToInt32(Session["GID"]);
                notes.SectionsId = Convert.ToInt32(Session["SID"]);
                notes.NoteDate = DateTime.Now;
                 db.Entry(notes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notes);
        }

        // GET: Notes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notes notes = db.Notes.Find(id);
            if (notes == null)
            {
                return HttpNotFound();
            }
            return View(notes);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notes notes = db.Notes.Find(id);
            db.Notes.Remove(notes);
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



        [HttpGet]
        public ActionResult SendMessage(string id)
        {
            var phone = db.Users.Where(m => m.Id == id).Select(a => a.PhoneNumber).SingleOrDefault();
            ViewBag.phone = phone;
            return View();
        }

        public ActionResult SendMessage(Sms messages)
        {
            String result;
            string apiKey = "ofObqZGN+ts-L7ifzvR0HbuglfpTcJ9zUVXUoeAlws";
            string numbers = messages.To; // in a comma seperated list
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
            return RedirectToAction("Index");
        }


        public ActionResult GetTables(int? page)
        {
            var gradeTimeTables = db.GradeTimeTables.Include(g => g.Grade).Include(g => g.Section).Include(g => g.TimeTable);
            return View(gradeTimeTables.ToList().ToPagedList(page ?? 1, 10));
        }
        public FilePathResult DownloadExampleFiles(string fileName)
        {


            string contentType = string.Empty;

            if (fileName.Contains(".pdf"))
            {
                contentType = "application/pdf";
            }

            else if (fileName.Contains(".docx"))
            {
                contentType = "application/docx";
            }
             return new FilePathResult(string.Format(@"~\uploads\{0}", fileName), contentType);






        }

    }
}
