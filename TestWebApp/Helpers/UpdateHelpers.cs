using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApp.Domain.Models;
using TestWebApp.Models;
using TestWebApp.Models.Interfaces;

namespace TestWebApp.Helpers
{
    public static class UpdateHelpers
    {
        public static bool IsNull(IModelEmployee emp)
        {
            if (emp.Experience == null && emp.Name == null && emp.Position == null && emp.Salary == null && emp.Specialization == null)
                return true;
            return false;
        }

        public static Employee XmlEmployeeToEmployee(IModelEmployee xemployee, Employee employee)
        {
            Employee emp = XMLActions.ToEmployee(xemployee);
            if (emp.Experience == -1)
                emp.Experience = employee.Experience;
            if (emp.Name == null)
                emp.Name = employee.Name;
            if (emp.Position == null)
                emp.Position = employee.Position;
            if (emp.Specialization == null)
                emp.Specialization = employee.Specialization;
            if (emp.Salary == -1)
                emp.Salary = employee.Salary;

            return emp;
        }
    }
}
