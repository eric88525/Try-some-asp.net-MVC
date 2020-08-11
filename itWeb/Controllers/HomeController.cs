using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace itWeb.Controllers
{
    public class HomeController : Controller
    {
        public class Student
        {
            public string id { get; set; }
            public string name { get; set; }
            public int score { get; set; }

            public Student()
            {
                id = string.Empty;
                name = string.Empty;
                score = 0;
            }

            public Student(string _id, string _name, int _score)
            {
                id = _id;
                name = _name;
                score = _score;
            }

            public override string ToString()
            {
                return $"學號: {id} ,Name: {name} , score:  {score}";
            }

        }




        public ActionResult Index()
        {
            DateTime date = DateTime.Now;
            Student data = new Student();
            List<Student> list = new List<Student>();
            list.Add(new Student("1","ERIC",80));
            list.Add(new Student("2", "JOHN", 30));
            list.Add(new Student("3", "LUCKY", 40));
            list.Add(new Student("4", "JOHN", 20));
            list.Add(new Student("5", "TRACY", 10));

            ViewBag.Date = date;
            ViewBag.Student = data;
            ViewBag.List = list;


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}