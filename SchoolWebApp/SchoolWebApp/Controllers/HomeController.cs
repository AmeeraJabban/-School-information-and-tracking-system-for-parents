using System;
using System.Collections.Generic;
using System.Linq;
using SchoolWebApp.Models;
using WebApplication1.Models;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;

namespace SchoolWebApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Home
        public ActionResult Index()
        {
            var roleId = db.Roles.Where(m => m.Name == "Students").Select(m => m.Id).SingleOrDefault();
            var count = db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId)).Count();
            ViewBag.Students = count;
            var roleId2 = db.Roles.Where(m => m.Name == "Teachers").Select(m => m.Id).SingleOrDefault();
            var count2 = db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId)).Count();
            ViewBag.Teachers = count2;
            var subject = db.Users.Select(a => a.Subject).Distinct().Count();
                ViewBag.subject = subject;
            var UserID = User.Identity.GetUserId();
           // var userParentName = db.Users.Where(a => a.Id == UserID).Select(a => a.ParentName).FirstOrDefault().ToString();
           // ViewBag.userParentName = userParentName;

            return View();
        }
        public ActionResult About()
        {
            return View();
        }
       
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(string Email,string Name ,string Subject,string Message)
        {
            var mail = new MailMessage();
            //sender info
            var logininfo = new NetworkCredential("ameeraamoora29@gmail.com", ")99Ameer@");
            mail.From = new MailAddress(Email);
            mail.To.Add(new MailAddress("ameeraamoora29@gmail.com"));
            mail.Subject = Subject;
            string body = "اسم المرسل" + Name + "<br>" +
                "بريد المرسل:" + Email + "<br>" +
                "عنوان الرسالة:" + Subject + "<br>" +
                "نص الرسالة:" + Message;
            mail.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = logininfo
            };
            smtpClient.Send(mail);

            return RedirectToAction("Index");
        }
        public ActionResult Gallary(int? page)
        {
            var ALBUMS = db.Albums.ToList();
             return View(ALBUMS.ToPagedList(page ?? 1, 5));
        }



        public ActionResult Photos(int Id)
        {


            var album = db.Albums.Find(Id);
            ViewBag.album = album.AlbumTitle;
            var Photo = db.Photos.ToList().Where(a => a.AlbumsID == Id);
            ViewBag.Photo = Photo;
           
            return View(Photo);
        }
        public ActionResult Services()
        {
            return View();
        }

        public ActionResult News(int? page)
        {
            var News = db.News.ToList().OrderByDescending(a => a.NewsDate);

            return View(News.ToList().ToPagedList(page ?? 1, 12));
        }
        public ActionResult Details(int Id)
        {
            var news = db.News.Find(Id);
            if (news == null)
            {
                return HttpNotFound();
            }
            Session["newsid"] = Id;
            Session["newstype"] = news.NewsType.NewsTypeName;
            return View(news);
        }

       
    }
}