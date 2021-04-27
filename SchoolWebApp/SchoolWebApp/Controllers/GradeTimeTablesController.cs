using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using SchoolWebApp.Models;
using WebApplication1.Models;

namespace SchoolWebApp.Controllers
{
    public class GradeTimeTablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GradeTimeTables
        public ActionResult Index(int? page)
        {
            var gradeTimeTables = db.GradeTimeTables.Include(g => g.Grade).Include(g => g.Section).Include(g => g.TimeTable);
            return View(gradeTimeTables.ToList().ToPagedList(page ?? 1, 10));
        }

        // GET: GradeTimeTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradeTimeTable gradeTimeTable = db.GradeTimeTables.Find(id);
            if (gradeTimeTable == null)
            {
                return HttpNotFound();
            }
            return View(gradeTimeTable);
        }

        // GET: GradeTimeTables/Create
        public ActionResult Create()
        {
            ViewBag.GradesId = new SelectList(db.Grades, "Id", "Id");
            ViewBag.SectionsId = new SelectList(db.Sections, "Id", "SectionName");
            ViewBag.TimeTablesId = new SelectList(db.TimeTables, "Id", "TableTitle");
            return View();
        }

        // POST: GradeTimeTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GradesId,TimeTablesId,SectionsId")] GradeTimeTable gradeTimeTable)
        {
            if (ModelState.IsValid)
            {
                db.GradeTimeTables.Add(gradeTimeTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GradesId = new SelectList(db.Grades, "Id", "Id", gradeTimeTable.GradesId);
            ViewBag.SectionsId = new SelectList(db.Sections, "Id", "SectionName", gradeTimeTable.SectionsId);
            ViewBag.TimeTablesId = new SelectList(db.TimeTables, "Id", "TableTitle", gradeTimeTable.TimeTablesId);
            return View(gradeTimeTable);
        }

        // GET: GradeTimeTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradeTimeTable gradeTimeTable = db.GradeTimeTables.Find(id);
            if (gradeTimeTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.GradesId = new SelectList(db.Grades, "Id", "Id", gradeTimeTable.GradesId);
            ViewBag.SectionsId = new SelectList(db.Sections, "Id", "SectionName", gradeTimeTable.SectionsId);
            ViewBag.TimeTablesId = new SelectList(db.TimeTables, "Id", "TableTitle", gradeTimeTable.TimeTablesId);
            return View(gradeTimeTable);
        }

        // POST: GradeTimeTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GradesId,TimeTablesId,SectionsId")] GradeTimeTable gradeTimeTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gradeTimeTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GradesId = new SelectList(db.Grades, "Id", "Id", gradeTimeTable.GradesId);
            ViewBag.SectionsId = new SelectList(db.Sections, "Id", "SectionName", gradeTimeTable.SectionsId);
            ViewBag.TimeTablesId = new SelectList(db.TimeTables, "Id", "TableTitle", gradeTimeTable.TimeTablesId);
            return View(gradeTimeTable);
        }

        // GET: GradeTimeTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradeTimeTable gradeTimeTable = db.GradeTimeTables.Find(id);
            if (gradeTimeTable == null)
            {
                return HttpNotFound();
            }
            return View(gradeTimeTable);
        }

        // POST: GradeTimeTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GradeTimeTable gradeTimeTable = db.GradeTimeTables.Find(id);
            db.GradeTimeTables.Remove(gradeTimeTable);
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
