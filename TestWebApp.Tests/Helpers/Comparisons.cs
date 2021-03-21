using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using TestWebApp.Helpers;
using TestWebApp.Models;

namespace TestWebApp.Tests.Helpers
{
    public static class Comparisons
    {
        /// <summary>
        /// Verify that the Dynamic object(Json) contains the same objects as is stored in XML with correct data
        /// </summary>
        /// <param name="JSONresp"></param>
        /// <param name="xdoc"></param>
        /// <returns></returns>
        public static bool ContainsMany(dynamic JSONresp, XDocument xdoc)
        {
            bool status = false;

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
            var emps = XMLActions.ToEmployee(items);

            for (int i = 0; i < emps.Count; i++)
            {
                if (JSONresp[i].ID == emps.ElementAt(i).ID.ToString()
                    && JSONresp[i].Name == emps.ElementAt(i).Name
                    && JSONresp[i].Experience == emps.ElementAt(i).Experience
                    && JSONresp[i].Position == emps.ElementAt(i).Position
                    && JSONresp[i].Specialization == emps.ElementAt(i).Specialization
                    && JSONresp[i].Salary == emps.ElementAt(i).Salary.ToString())
                {
                    status = true;
                }
                else
                {
                    return false;
                }
            }
            return status;
        }

        /// <summary>
        /// Verify that the Dynamic object(Json) contains the same object as is stored in XML with correct data
        /// </summary>
        /// <param name="JSONresp"></param>
        /// <param name="xdoc"></param>
        /// <returns></returns>
        public static bool ContainsCorrectObject(dynamic JSONresp, XDocument xdoc)
        {
            bool status = false;

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
            var emps = XMLActions.ToEmployee(items.Where(x => x.ID.ToString() == JSONresp.ID.ToString()));


            if (JSONresp.ID == emps.ElementAt(0).ID.ToString()
                    && JSONresp.Name == emps.ElementAt(0).Name
                    && JSONresp.Experience == emps.ElementAt(0).Experience
                    && JSONresp.Position == emps.ElementAt(0).Position
                    && JSONresp.Specialization == emps.ElementAt(0).Specialization
                    && JSONresp.Salary == emps.ElementAt(0).Salary.ToString())
            {
                status = true;
            }
            else
            {
                return false;
            }
            return status;
        }

        public static Dictionary<string, string> CompareXMLsForChanges(XDocument before, XDocument after, string id)
        {
            bool status = false;

            var itemsBefore = (from xe in before.Descendants("Employee")
                         select new XMLEmployee
                         {
                             Name = xe.Element("Name").Value,
                             Experience = xe.Element("Experience").Value,
                             Position = xe.Element("Position").Value,
                             Salary = xe.Element("Salary").Value,
                             Specialization = xe.Element("Specialization").Value,
                             ID = (string)xe.Element("ID")
                         }).ToList().Where(x => x.ID.ToString().Contains(id)).First() ;

            var itemsAfter = (from xe in after.Descendants("Employee")
                               select new XMLEmployee
                               {
                                   Name = xe.Element("Name").Value,
                                   Experience = xe.Element("Experience").Value,
                                   Position = xe.Element("Position").Value,
                                   Salary = xe.Element("Salary").Value,
                                   Specialization = xe.Element("Specialization").Value,
                                   ID = (string)xe.Element("ID")
                               }).ToList().Where(x => x.ID.ToString().Contains(id)).First();

            //compare 2 XMLEmployee objects and create Dict with Values only for changed values
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (itemsBefore.Experience != itemsAfter.Experience)
                dict.Add("Experience", itemsAfter.Experience);
            if (itemsBefore.Name != itemsAfter.Name)
                dict.Add("Name", itemsAfter.Name);
            if (itemsBefore.Position != itemsAfter.Position)
                dict.Add("Position", itemsAfter.Position);
            if (itemsBefore.Salary != itemsAfter.Salary)
                dict.Add("Salary", itemsAfter.Salary);
            if (itemsBefore.Specialization != itemsAfter.Specialization)
                dict.Add("Specialization", itemsAfter.Specialization);

            return dict;
        }

        /// <summary>
        /// Verify that the Dynamic object(Json) contains the at least one object that is stored in XML with correct data with the specified criteria
        /// </summary>
        /// <param name="JSONresp"></param>
        /// <param name="xdoc"></param>
        /// /// <param name="position"></param>
        /// <returns></returns>
        public static bool ContainsAtLeastOne(dynamic JSONresp, XDocument xdoc, string position)
        {
            bool status = false;

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
            var emps = XMLActions.ToEmployee(items.Where(x => x.Position.ToLower() == position.ToLower()));

            //now based on whether we have JArray or just single JProperty verify the match
            var type = JSONresp.Type;
            var cnt = ((JArray)JSONresp).Count;
            if ( type == JTokenType.Array && emps.Count != cnt)
                return false;
            else if (type == JTokenType.Array || emps.Count > 1)
            {
                for (int i = 0; i < emps.Count; i++)
                {
                    if (JSONresp[i].ID == emps.ElementAt(i).ID.ToString()
                        && JSONresp[i].Name == emps.ElementAt(i).Name
                        && JSONresp[i].Experience == emps.ElementAt(i).Experience
                        && JSONresp[i].Position == emps.ElementAt(i).Position
                        && JSONresp[i].Specialization == emps.ElementAt(i).Specialization
                        && JSONresp[i].Salary == emps.ElementAt(i).Salary.ToString())
                    {
                        status = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (JSONresp.ID == emps.ElementAt(0).ID.ToString()
                    && JSONresp.Name == emps.ElementAt(0).Name
                    && JSONresp.Experience == emps.ElementAt(0).Experience
                    && JSONresp.Position == emps.ElementAt(0).Position
                    && JSONresp.Specialization == emps.ElementAt(0).Specialization
                    && JSONresp.Salary == emps.ElementAt(0).Salary.ToString())
                {
                    status = true;
                }
                else
                {
                    return false;
                }
            }
            
            return status;
        }
    }
}
