using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace itWeb.Models
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
}