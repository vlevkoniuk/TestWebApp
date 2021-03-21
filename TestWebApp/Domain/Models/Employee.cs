using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApp.Domain.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string Specialization { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        public double Experience { get; set; }
    }
}
