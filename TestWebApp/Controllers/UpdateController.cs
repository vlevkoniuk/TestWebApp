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
    public class UpdateController : Controller
    {
        private readonly ILogger<UpdateController> _logger;
        private IRepository<Employee> _repo;

        public UpdateController(ILogger<UpdateController> logger, EmployeeRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpPost("id/{id}")]
        public IActionResult EmployeeUpdate(int id, XMLUpdEmployee employee)
        {
            if (UpdateHelpers.IsNull(employee))
            {
                ModelState.AddModelError("Error", "The model body cannot be empty or null");
                return BadRequest(ModelState);
            }
            var emp = _repo.GetAll().Where(x => x.ID == id);
            if (emp.Count() < 1)
                return BadRequest("Cant find matching ID");
            var newemp = UpdateHelpers.XmlEmployeeToEmployee(employee, emp.First());
            newemp.ID = id;

            _repo.Update(newemp);

            return Ok(newemp);
        }
    }
}
