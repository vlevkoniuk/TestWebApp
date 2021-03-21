using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestWebApp.Models.Interfaces;

namespace TestWebApp.Models
{
    public class XMLEmployee : IModelEmployee
    {
        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only positive digits are allowed")]
        public string ID { get; set; }
        [Required]
        [RegularExpression(@"(?=^$|.*\S+.*)[a-zA-Z0-9\s\.\,\-]*$", ErrorMessage = "Only characters are allowed.")]
        public string Specialization { get; set; }
        [Required]
        [RegularExpression(@"(?=^$|.*\S+.*)[a-zA-Z0-9\s\.\,\-]*$", ErrorMessage = "Only characters are allowed.")]
        public string Position { get; set; }
        [Required]
        [RegularExpression(@"(?=^$|.*\S+.*)[a-zA-Z0-9\s\.\,\-]*$", ErrorMessage = "Only Characters are allowed.")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[+-]?([0-9]{1,3}(,[0-9]{3})*(\.[0-9]+)?|\d*\.\d+|\d+)$", ErrorMessage = "only digits, `.` amd `,` allowed")]
        public string Salary { get; set; }
        [Required]
        [RegularExpression(@"^[+-]?([0-9]{1,3}(,[0-9]{3})*(\.[0-9]+)?|\d*\.\d+|\d+)$", ErrorMessage = "only digits, `.` amd `,` allowed")]
        public string Experience { get; set; }

        public XMLEmployee()
        { }

        public XMLEmployee(string id, string specialization, string position,  string name, string salary, string experience)
        {

            ID = id;
            Specialization = specialization;
            Position = position;
            Name = name;
            Salary = salary;
            Experience = experience;
        }
    }
}
