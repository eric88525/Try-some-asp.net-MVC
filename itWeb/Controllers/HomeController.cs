
using itWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
// db
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using System.EnterpriseServices;
using Newtonsoft.Json;

namespace itWeb.Controllers
{
    public class HomeController : Controller
    {
        string connString = "server=127.0.0.1;port=3306;user id=mvcuser;password=mvcpassword;database=mvc;charset=utf8;";
        MySqlConnection conn = new MySqlConnection();
        
        /*public ActionResult Create()
        {
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string sql = @"INSERT INTO `City` (`Id`, `City`) VALUES
                           ('0', '基隆市'),
                           ('1', '臺北市'),
                           ('2', '新北市'),
                           ('3', '桃園市'),
                           ('4', '新竹市'),
                           ('5', '新竹縣'),
                           ('6', '宜蘭縣'),
                           ('7', '苗栗縣'),
                           ('8', '臺中市'),
                           ('9', '彰化縣'),
                           ('A', '南投縣'),
                           ('B', '雲林縣'),
                           ('C', '嘉義市'),
                           ('D', '嘉義縣'),
                           ('E', '臺南市'),
                           ('F', '高雄市'),
                           ('G', '屏東縣'),
                           ('H', '澎湖縣'),
                           ('I', '花蓮縣'),
                           ('J', '臺東縣'),
                           ('K', '金門縣'),
                           ('L', '連江縣');";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int index = cmd.ExecuteNonQuery();
            bool success = false;
            if (index > 0)
                success = true;
            else
                success = false;
            ViewBag.Success = success;
            conn.Close();
            return Content(success.ToString());

        }*/


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

        public ActionResult Citys()
        {
            conn.ConnectionString = connString;
            string sql = @" SELECT `id`, `city` FROM `city`";
            MySqlCommand cmd = new MySqlCommand(sql,conn);
            
            List<City> list = new List<City>();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    City city = new City();
                    city.CityId = dr["id"].ToString();
                    city.CityName = dr["city"].ToString();
                    list.Add(city);
                }
            }
            if (conn.State != ConnectionState.Closed)
                conn.Close();

            ViewBag.List = list;
            return View();

        }

        public ActionResult Cityjson()
        {
            List<Student> list = new List<Student>()
            {
                new Student() {id = "1", name = "eric", score = 80},
                new Student() {id = "2", name = "tom", score = 90}
            };
            string jsonData = JsonConvert.SerializeObject(list);

            var dejson = JsonConvert.DeserializeObject<List<Student>>(jsonData);
            foreach (var s in dejson)
            {
                Debug.WriteLine(string.Format("id {0} name {1} score {2}", s.id, s.name, s.score));
            }
          

            return Content(jsonData);


        }


        public ActionResult Transcripts(string id,string name , int score )
        {
            Student data = new Student(id, name, score);


            return View(data);
        }

        /* this is post by FormCollection
        [HttpPost]
        public ActionResult Transcripts(FormCollection post)
        {
            string id = post["id"];
            string name = post["name"];
            int score = Convert.ToInt32(post["score"]);
            Student data = new Student(id,name,score);
            return View(data);
        }
        */

        [HttpPost]
        public ActionResult Transcripts(Student model)
        {
            string id = model.id;
            string name = model.name;
            int score = model.score;
            Student data = new Student(id,name,score);
            return View(data);
        }


    }
}