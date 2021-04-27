using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolWebApp.Models;
using WebApplication1.Models;

namespace SchoolWebApp.Controllers
{
    [Authorize(Roles = "Admins")]
    public class Photo1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Photo1
        public ActionResult Index(int? page)
        {
            var photos = db.Photos.Include(p => p.Album);
            return View(photos.ToList().ToPagedList(page ?? 1, 10));
        }

        // GET: Photo1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photos photos = db.Photos.Find(id);
            if (photos == null)
            {
                return HttpNotFound();
            }
            return View(photos);
        }

        // GET: Photo1/Create
        public ActionResult Create()
        {
            ViewBag.AlbumsID = new SelectList(db.Albums, "Id", "AlbumTitle");
            return View();
        }

        // POST: Photo1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PhotoUrl,AlbumsID")] Photos photos)
        {
            if (ModelState.IsValid)
            {
                db.Photos.Add(photos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumsID = new SelectList(db.Albums, "Id", "AlbumTitle", photos.AlbumsID);
            return View(photos);
        }

        // GET: Photo1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photos photos = db.Photos.Find(id);
            if (photos == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumsID = new SelectList(db.Albums, "Id", "AlbumTitle", photos.AlbumsID);
            return View(photos);
        }

        // POST: Photo1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Photos photos, HttpPostedFileBase[] files)
        {
            //Ensure model state is valid  
            if (ModelState.IsValid)
            {   //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    string old_image = Path.Combine(Server.MapPath("~/UploadedFiles/") + photos.PhotoUrl); 
                    //Checking file is available to save.  
                    if (file != null)
                    {
                       System.IO.File.Delete(old_image);
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
                        photos.PhotoUrl = InputFileName;
                        file.SaveAs(ServerSavePath);

                    }
                    db.Entry(photos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
                    ViewBag.AlbumsID = new SelectList(db.Albums, "Id", "AlbumTitle", photos.AlbumsID);
                    return View(photos);
                
            
        }
        // GET: Photo1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photos photos = db.Photos.Find(id);
            if (photos == null)
            {
                return HttpNotFound();
            }
            return View(photos);
        }

        // POST: Photo1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photos photos = db.Photos.Find(id);
            string old_image = Path.Combine(Server.MapPath("~/UploadedFiles/") + photos.PhotoUrl);
                System.IO.File.Delete(old_image);
                db.Photos.Remove(photos);
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


        public ActionResult UploadFiles()
        {
            ViewBag.AlbumsID = new SelectList(db.Albums, "Id", "AlbumTitle");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFiles(Photos photos, HttpPostedFileBase[] files)
        {


            //Ensure model state is valid  
            if (ModelState.IsValid)
            {   //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {

                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
                        photos.PhotoUrl = InputFileName;
                        db.Photos.Add(photos);
                        db.SaveChanges();
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.  
                        ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                    }

                }
            }
            ViewBag.AlbumsID = new SelectList(db.Albums, "Id", "AlbumTitle", photos.AlbumsID);
            return View(photos);
        }

    }
}
