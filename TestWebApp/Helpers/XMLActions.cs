using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TestWebApp.Domain.Models;
using TestWebApp.Models;
using TestWebApp.Models.Interfaces;

namespace TestWebApp.Helpers
{
    public static class  XMLActions
    {
        public static XDocument ReadXML()
        {
            XDocument xdoc = XDocument.Load("./Data/Data[1].xml");
            return xdoc;
        }

        public static bool UpdateXML(XDocument document)
        {
            bool result = true;
            try
            {
                document.Save("./Data/Data[1].xml");
            }
            catch 
            {
                result = false;
            }

            return result;
        }

        public static List<Employee> ToEmployee(IEnumerable<XMLEmployee> items)
        {
            List<Employee> employees = new List<Employee>();

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberDecimalSeparator = ",";
            int empid = 1;

            foreach (var item in items)
            {
                var employee = new Employee();
                employee.Name = item.Name;
                employee.Position = item.Position;
                employee.Specialization = item.Specialization;
                double salary = 0;
                if (!Double.TryParse(item.Salary.ToString().Replace(" ", ""), NumberStyles.Currency, nfi, out salary))
                {
                    nfi.NumberDecimalSeparator = ".";
                    Double.TryParse(item.Salary.ToString().Replace(" ", ""), NumberStyles.Currency, nfi, out salary);
                }
                employee.Salary = salary;
                double experience = 0;
                if (!Double.TryParse(item.Experience.ToString().Replace(" ", ""), NumberStyles.Currency, nfi, out experience))
                {
                    nfi.NumberDecimalSeparator = ".";
                    Double.TryParse(item.Experience.ToString().Replace(" ", ""), NumberStyles.Currency, nfi, out experience);
                }
                employee.Experience = experience;
                int id = 0;
                if (item.ID == "" || item.ID == "null" || item.ID == null)
                {
                    //id is empty so set it
                    employee.ID = empid;
                }
                else
                    employee.ID = int.Parse(item.ID);

                employees.Add(employee);
                empid++;


            }
            return employees;

        }

        public static Employee ToEmployee(IModelEmployee item)
        {
            List<Employee> employees = new List<Employee>();

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberDecimalSeparator = ",";
            int empid = 1;
            var employee = new Employee();
            employee.Name = item.Name;
            employee.Position = item.Position;
            employee.Specialization = item.Specialization;
            double salary = 0;
            try
            {
                if (!Double.TryParse(item.Salary.ToString().Replace(" ", ""), NumberStyles.Currency, nfi, out salary))
                {
                    nfi.NumberDecimalSeparator = ".";
                    Double.TryParse(item.Salary.ToString().Replace(" ", ""), NumberStyles.Currency, nfi, out salary);
                }
            }
            catch
            {
                //means error parsing double
                salary = -1;
            }
            employee.Salary = salary;
            double experience = 0;
            try
            {
                if (!Double.TryParse(item.Experience.ToString().Replace(" ", ""), NumberStyles.Currency, nfi, out experience))
                {
                    nfi.NumberDecimalSeparator = ".";
                    Double.TryParse(item.Experience.ToString().Replace(" ", ""), NumberStyles.Currency, nfi, out experience);
                }
            }
            catch
            {
                //means error parsing double
                experience = -1;
            }
            employee.Experience = experience;
            employees.Add(employee);

            return employee;

        }

        public static void CorrectXMLIDs()
        {
            XDocument xdoc = ReadXML();
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
            int largestID = 0;
            try
            {
                largestID = items.Where(x => x.ID != null).Select(x => int.Parse(x.ID)).OrderBy(x => x).Last();
            }
            catch
            {
                largestID = 1;
            }
            
            //int counter = 1;
            var xitems = from item in xdoc.Descendants("Employee")
                        select item;
            foreach (XElement item in xitems)
            {
                if ((string)item.Element("ID") == null)
                {
                    item.SetElementValue("ID", largestID.ToString());
                }
                largestID++;
            }
            XMLActions.UpdateXML(xdoc);
        }
    }
}
