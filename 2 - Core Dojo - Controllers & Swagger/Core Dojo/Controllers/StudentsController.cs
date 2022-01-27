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

        [HttpGet("")]
        public async Task<IActionResult> GetStudent()
        {
            return Ok("This is a student");
        }

    }
}
