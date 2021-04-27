using System;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using SchoolWebApp.Models;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace SchoolWebApp.Controllers
{
    public class SmsController : Controller
    {
        // GET: Sms
        public ActionResult Index()
        {
            return View();
        }
            [HttpGet]
            public ActionResult SendMessage()
            {
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
            return RedirectToAction("../home/Index");
        }





    }
}
