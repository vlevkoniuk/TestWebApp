using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApp.Models.Interfaces
{
    public interface IModelEmployee
    {
        public string Specialization { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        public string Salary { get; set; }
        public string Experience { get; set; }
    }
}
