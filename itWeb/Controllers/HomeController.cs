
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
        MyDataBase db = new MyDataBase();
        
        [HttpPost]
        public ActionResult Village(string id)
        {
            

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


        #region DB
        public class MyDataBase
        {
            string connString = "server=127.0.0.1;port=3306;user id=mvcuser;password=mvcpassword;database=mvc;charset=utf8;";

            MySqlConnection conn = new MySqlConnection();

            public MyDataBase()
            {
            }


            public bool  AddUserData(UserData data)
            {
                try
                {
                    conn.ConnectionString = connString;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    string id = Guid.NewGuid().ToString();
                    string sql = @"INSERT INTO userdata VALUES(@id,@account,@password,@city,@village,@address)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
                    cmd.Parameters.Add("@account", MySqlDbType.VarChar).Value = data.account;
                    cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = data.password1;
                    cmd.Parameters.Add("@city", MySqlDbType.VarChar).Value = data.city;
                    cmd.Parameters.Add("@village", MySqlDbType.VarChar).Value = data.village;
                    cmd.Parameters.Add("@address", MySqlDbType.VarChar).Value = data.address;
                    cmd.ExecuteNonQuery();
                    return true;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
                finally
                {

                    conn.Close();

                }

                

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
        

        #endregion
       
        
        public ActionResult Index(FormCollection post)
        {

            DateTime date = DateTime.Now;
            Student data = new Student("", "", 0);

            List<Student> list = new List<Student>();
            list.Add(new Student("1", "ERIC", 80));
            list.Add(new Student("2", "JOHN", 30));
            list.Add(new Student("3", "LUCKY", 40));
            list.Add(new Student("4", "JOHN", 20));
            list.Add(new Student("5", "TRACY", 10));

            ViewBag.Date = date;
            ViewBag.Student = data;
            ViewBag.List = list;


            return View(data);
        }

        [HttpPost]
        public ActionResult CheckRegister(UserData data)
        {
            

            if (string.IsNullOrWhiteSpace(data.password1) || data.password1 != data.password2)
            {
                
                var citylist = db.GetCityList();
                ViewBag.CityList = citylist;
                ViewBag.VillageList = new List<Village>();
                ViewBag.Msg = "密碼錯誤";

                data.password1 = "";
                data.password2 = "";

                return View("UserRegister",data);
            }
            else
            {

                if (db.AddUserData(data))
                {
                    Response.Redirect("~/Home/Login");
                    return new EmptyResult();
                }
                else
                {

                    data.password1 = "";
                    data.password2 = "";
                    ViewBag.Msg = "註冊失敗...";
                    return View("UserRegister", data);
                }
            }
        }
        [HttpPost]
        public ActionResult CheckLogin(UserData data)
        {
            return View();

        }


        public ActionResult Login()
        {

            return View();
        }

        public ActionResult UserRegister()
        {
            
            var db = new MyDataBase();

            ViewBag.CityList = db.GetCityList() ;
            ViewBag.VillageList = new List<Village>();

            return View(new UserData());
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