using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using SchoolWebApp.Models;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;

namespace SchoolWebApp.Controllers
{
        public class TimeTablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TimeTables
        public ActionResult Index(int? page)
        {
            var UserID = User.Identity.GetUserId();
            var timeTables = db.TimeTables.Include(a => a.TimeTablesType).Include(a => a.User.Email);
             return View(db.TimeTables.ToList().Where(a => a.UserID == UserID).OrderByDescending(a => a.TableTitleDate).ToPagedList(page ?? 1, 10));
  }

        // GET: TimeTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeTables timeTables = db.TimeTables.Find(id);
            if (timeTables == null)
            {
                return HttpNotFound();
            }
            return View(timeTables);
        }

        // GET: TimeTables/Create
        public ActionResult Create()
        {
            ViewBag.TimeTablesTypesId = new SelectList(db.TimeTablesTypes, "Id", "TimeTablesTypeName");
            return View();
        }

        // POST: TimeTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TimeTables timeTables, HttpPostedFileBase file)
        {
            var UserID = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                timeTables.UserID = UserID;
                timeTables.TableTitleDate = DateTime.Now;
                string upload = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/uploads"), upload);
                file.SaveAs(path);
                timeTables.TableTitleURL = upload;
                db.TimeTables.Add(timeTables);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeTablesTypesId = new SelectList(db.TimeTablesTypes, "Id", "TimeTablesTypeName", timeTables.TimeTablesTypesId);
            return View(timeTables);
        }

        // GET: TimeTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeTables timeTables = db.TimeTables.Find(id);
            if (timeTables == null)
            {
                return HttpNotFound();
            }
            ViewBag.TimeTablesTypesId = new SelectList(db.TimeTablesTypes, "Id", "TimeTablesTypeName", timeTables.TimeTablesTypesId);
            return View(timeTables);
        }

        // POST: TimeTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TimeTables timeTables, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                timeTables.TableTitleDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    string old_image = Path.Combine(Server.MapPath("~/uploads/") + timeTables.TableTitleURL);

                    if (file != null)
                    {
                       // System.IO.File.Delete(old_image);
                        string pic = System.IO.Path.GetFileName(file.FileName);
                        string path = System.IO.Path.Combine(Server.MapPath("~/uploads"), pic);
                        file.SaveAs(path);
                        timeTables.TableTitleURL = pic;
                        file.SaveAs(path);
                    }
                    var UserID = User.Identity.GetUserId();
                    timeTables.UserID = UserID;
                    db.Entry(timeTables).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
                ViewBag.TimeTablesTypesId = new SelectList(db.TimeTablesTypes, "Id", "TimeTablesTypeName", timeTables.TimeTablesTypesId);
                return View(timeTables);


            
        }

        // GET: TimeTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeTables timeTables = db.TimeTables.Find(id);
            if (timeTables == null)
            {
                return HttpNotFound();
            }
            return View(timeTables);
        }

        // POST: TimeTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimeTables timeTables = db.TimeTables.Find(id);
            db.TimeTables.Remove(timeTables);
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
