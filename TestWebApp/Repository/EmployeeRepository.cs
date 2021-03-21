using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TestWebApp.Domain.Models;
using TestWebApp.Helpers;
using TestWebApp.Models;

namespace TestWebApp.Repository
{
    public class EmployeeRepository : IRepository<Employee>
    {
        public bool Create(Employee employee)
        {
            XDocument xdoc = XMLActions.ReadXML();

            //need to assign new ID, because the ID that may come from the user may be busy already
            var items = (from xe in xdoc.Descendants("Employee")
                         select new 
                         {
                             ID = (string)xe.Element("ID")
                         }).ToList();
            int newId;
            //Avoid overflow of int
            if (items.Count() < int.MaxValue)
                newId = items.Count() + 1;
            else
                return false;

            var tmpId = items.Where(x => x.ID != null).Where(x => x.ID.Contains(newId.ToString())).Select(x => x.ID);
            if (tmpId.Count() < 1)
            {
                employee.ID = newId;
            }
            else 
            {
                int ID;
                var LastUsedId = tmpId.OrderBy(x => x).Last();
                //Avoid overflow of int
                if (int.Parse(LastUsedId) < int.MaxValue)
                    ID = int.Parse(LastUsedId) + 1;
                else
                    //We may want to implement logic to find not used ID to correctly handle this situation where we are close to the maximum int value
                    // But as for now we are OK with that for test porposes
                    return false;
                
                employee.ID = ID;
            }

            XElement root = xdoc.Element("EmployeeList");

            //Add new employee to XML tree
            root.Add(new XElement("Employee",
                        new XElement("ID", employee.ID),
                        new XElement("Name", employee.Name),
                        new XElement("Experience", employee.Experience),
                        new XElement("Position", employee.Position),
                        new XElement("Salary", employee.Salary),
                        new XElement("Specialization", employee.Specialization)
                        ));

            return XMLActions.UpdateXML(xdoc);
        }

        public bool Delete(Employee employee)
        {
            throw new NotImplementedException();
        }

        public Employee Get(int Id)
        {
            XDocument xdoc = XMLActions.ReadXML();
            var items = (from xe in xdoc.Descendants("Employee")
                         select new XMLEmployee
                         {
                             Name = xe.Element("Name").Value,
                             Experience = xe.Element("Experience").Value,
                             Position = xe.Element("Position").Value,
                             Salary = xe.Element("Salary").Value,
                             Specialization = xe.Element("Specialization").Value,
                             ID = (string)xe.Element("ID")
                         }).ToList();

            //Now convert to the Employe format
            //also If the ID field is empty - assign new ID to it
            var emp = XMLActions.ToEmployee(items);
            Employee employee;
            try
            {
                employee = emp.Where(x => x.ID == Id).First();
            }
            catch
            {
                employee = null;
            }
            return employee;

        }

        public List<Employee> GetAll()
        {
            XDocument xdoc = XMLActions.ReadXML();
    
            var items = (from xe in xdoc.Descendants("Employee")
                select new XMLEmployee
                {
                    Name = xe.Element("Name").Value,
                    Experience = xe.Element("Experience").Value,
                    Position = xe.Element("Position").Value,
                    Salary = xe.Element("Salary").Value,
                    Specialization = xe.Element("Specialization").Value,
                    ID = (string)xe.Element("ID")
                }).ToList();

            //Now convert to the Employe format
            //also If the ID field is empty - assign new ID to it
            var emp = XMLActions.ToEmployee(items);

            return emp;
        }

        public bool Update(Employee employee)
        {
            XDocument xdoc = XMLActions.ReadXML();
            //XElement root = xdoc.Element("EmployeeList");

            var items = from item in xdoc.Descendants("Employee")
                        where int.Parse(item.Element("ID").Value) == employee.ID
                        select item;
            foreach (XElement itemElement in items)
            {
                itemElement.SetElementValue("Name", employee.Name);
                itemElement.SetElementValue("Experience", employee.Experience);
                itemElement.SetElementValue("Position", employee.Position);
                itemElement.SetElementValue("Salary", employee.Salary);
                itemElement.SetElementValue("Specialization", employee.Specialization);
            }

            return XMLActions.UpdateXML(xdoc);
        }
    }
}
