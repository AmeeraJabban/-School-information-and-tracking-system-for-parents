using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using SchoolWebApp.Models;
using WebApplication1.Models;

namespace SchoolWebApp.Controllers
{
    [Authorize(Roles = "Admins")]
    public class NewsTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NewsTypes
        public ActionResult Index(int? page)
        {
            return View(db.NewsTypes.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: NewsTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsTypes newsTypes = db.NewsTypes.Find(id);
            if (newsTypes == null)
            {
                return HttpNotFound();
            }
            return View(newsTypes);
        }

        // GET: NewsTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewsTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NewsTypeName,NewsTypeDescription")] NewsTypes newsTypes)
        {
            if (ModelState.IsValid)
            {
                db.NewsTypes.Add(newsTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newsTypes);
        }

        // GET: NewsTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsTypes newsTypes = db.NewsTypes.Find(id);
            if (newsTypes == null)
            {
                return HttpNotFound();
            }
            return View(newsTypes);
        }

        // POST: NewsTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NewsTypeName,NewsTypeDescription")] NewsTypes newsTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newsTypes);
        }

        // GET: NewsTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsTypes newsTypes = db.NewsTypes.Find(id);
            if (newsTypes == null)
            {
                return HttpNotFound();
            }
            return View(newsTypes);
        }

        // POST: NewsTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsTypes newsTypes = db.NewsTypes.Find(id);
            db.NewsTypes.Remove(newsTypes);
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
