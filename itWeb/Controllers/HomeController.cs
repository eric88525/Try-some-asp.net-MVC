
using itWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;


namespace itWeb.Controllers
{
    public class HomeController : Controller
    {
        

        public ActionResult Index()
        {
            DateTime date = DateTime.Now;
            Student data = new Student("", "", 0);
            
            List<Student> list = new List<Student>();
            list.Add(new Student("1","ERIC",80));
            list.Add(new Student("2", "JOHN", 30));
            list.Add(new Student("3", "LUCKY", 40));
            list.Add(new Student("4", "JOHN", 20));
            list.Add(new Student("5", "TRACY", 10));
            
            ViewBag.Date = date;
            ViewBag.Student = data;
            ViewBag.List = list;


            return View(data);
        }
        public ActionResult Transcripts(string id,string name , int score )
        {
            Student data = new Student(id, name, score);


            return View(data);
        }

        [HttpPost]
        public ActionResult Transcripts(FormCollection post)
        {
            string id = post["id"];
            string name = post["name"];
            int score = Convert.ToInt32(post["score"]);
            Student data = new Student(id,name,score);
            return View(data);


        }



    }
}