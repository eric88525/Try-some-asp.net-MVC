
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
        
        
        [HttpPost]
        public ActionResult Village(string id)
        {
            MyDataBase db = new MyDataBase();

            var list = db.GetVillageList(id);
            string result = "";
            if (list == null)
            {
                return Json(result);
            }
            else
            {
                result = JsonConvert.SerializeObject(list);
                return Json(result);
            }

        }



        public class MyDataBase
        {
            string connString = "server=127.0.0.1;port=3306;user id=mvcuser;password=mvcpassword;database=mvc;charset=utf8;";

            MySqlConnection conn = new MySqlConnection();

            public MyDataBase()
            {
            }

            

            public List<City> GetCityList()
            {
                conn.ConnectionString = connString;
                string sql = @" SELECT `id`, `city` FROM `city`";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

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

                return list;
            }

            public List<Village> GetVillageList(string id)
            {
                string sql = @" SELECT `VillageId`, `VillageName` FROM `Village` WHERE `CityId`=" + id;
                try
                {
                    conn.ConnectionString = connString;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                   
                    
                    
                    Debug.Write(sql);

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    List<Village> list = new List<Village>();

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Village data = new Village();
                            data.VillageId = dr["VillageId"].ToString();
                            data.VillageName = dr["VillageName"].ToString();
                            list.Add(data);
                        }
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                    return null;
                }
                finally
                {
                    Debug.Write(sql);
                    conn.Close();
                }
            }
        }

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

        public ActionResult UserLogin()
        {
            
            var db = new MyDataBase();

            ViewBag.CityList = db.GetCityList() ;
            return View();
        }


        public ActionResult Citys()
        {
            MyDataBase db = new MyDataBase();
            var cityList = db.GetCityList();

            ViewBag.List = cityList;
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