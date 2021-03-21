using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApp.Domain.Models;
using TestWebApp.Helpers;
using TestWebApp.Models;
using TestWebApp.Repository;

namespace TestWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddController : Controller
    {
        private readonly ILogger<AddController> _logger;
        private IRepository<Employee> _repo;

        public AddController(ILogger<AddController> logger, EmployeeRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpPost]
        public IActionResult EmployeeAdd(XMLEmployee employee)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var lempxml = new List<XMLEmployee>();
            lempxml.Add(employee);

            var lemp = XMLActions.ToEmployee(lempxml);
            var emp = lemp.First();

            //Check if we have already the same object with criteria Matching Name
            var emps = _repo.GetAll().Where(x => x.Name.Replace(" ", "").Replace(",","").Replace(".","").Contains(emp.Name.Replace(" ", "").Replace(",", "").Replace(".", "")));
            if (emps.Count() > 0)
            {
                ModelState.AddModelError("Error", "Person with the same name exists. You may want to use Update functionality to update this person");
                return BadRequest(ModelState);
            }

             _repo.Create(emp);
            return Ok(emp);
        }
    }
}
