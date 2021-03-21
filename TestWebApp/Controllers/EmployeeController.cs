using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApp.Domain.Models;
using TestWebApp.Models;
using TestWebApp.Repository;

namespace TestWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private IRepository<Employee> _repo;

        public EmployeeController(ILogger<EmployeeController> logger, EmployeeRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = _repo.GetAll();
            return Ok(JsonConvert.SerializeObject(employees));
        }


        [HttpGet("id/{id}")]
        public IActionResult GetAll(int id)
        {
            var employee = _repo.Get(id);
            if (employee == null)
                return NotFound();
            return Ok(JsonConvert.SerializeObject(employee));
        }

        [HttpGet("position/{position}")]
        public IActionResult GetAll(string position)
        {
            var employee = _repo.GetAll().Where(x => x.Position.ToLower().Contains(position.ToLower()));
            if (employee.Count() == 0)
                return NotFound();
            return Ok(JsonConvert.SerializeObject(employee));
        }

        

    }
}
