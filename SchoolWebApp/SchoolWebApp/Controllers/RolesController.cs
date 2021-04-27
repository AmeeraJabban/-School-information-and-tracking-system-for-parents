using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace SchoolWebApp.Controllers
{
    [Authorize(Roles = "Admins")]
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Roles
        public ActionResult Index()
        {
            var Roles = db.Roles;

            return View(Roles.ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(string id)
        {
            var Roles = db.Roles.Find(id);
            if (Roles==null)
            {
                return HttpNotFound();
            }

            return View(Roles);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            
                if (ModelState.IsValid)
                {
                    db.Roles.Add(role);
                    db.SaveChanges();
                    return View(db.Roles.ToList());
            }
                return RedirectToAction("Index");
            
           
            }

        // GET: Roles/Edit/5
        public ActionResult Edit(string id)
        {
            var Roles = db.Roles.Find(id);
            if (Roles == null)
            {
                return HttpNotFound();
            }

            return View(Roles);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name")]IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(string id)
        {
            var Roles = db.Roles.Find(id);
            if (Roles == null)
            {
                return HttpNotFound();
            }

            return View(Roles);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(IdentityRole role)
        {
            
            try
            {
                var Roles = db.Roles.Find(role.Id);
                db.Roles.Remove(Roles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
