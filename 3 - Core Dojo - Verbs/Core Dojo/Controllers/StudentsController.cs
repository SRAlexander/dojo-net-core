using Core_Dojo.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Dojo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {

        //Constructor
        public StudentsController() { }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            return Ok("This is student " + id.ToString());
        }

        [HttpPost("")]
        public async Task<IActionResult> GetStudents()
        {
            var response = new List<string>();
            response.Add("This is student 1");
            response.Add("This is student 2");
            response.Add("This is student 3");
            return Ok(response);
        }

        [HttpPut("")]
        public async Task<IActionResult> SaveStudent([FromBody] StudentAddRequest request)
        {
            return Ok("This is a student, the student's name is " + request.FirstName);
        }

        [HttpDelete("/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            return Ok("Student " + id.ToString() + " has been deleted");
        }

    }
}
