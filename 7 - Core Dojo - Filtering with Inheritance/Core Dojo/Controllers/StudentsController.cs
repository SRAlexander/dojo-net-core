using Common.RequestModels;
using Common.Queries;
using Core_Dojo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core_Dojo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {

        private readonly IStudentsService _studentsService;

        //Constructor
        public StudentsController(IStudentsService studentsServide) {
            _studentsService = studentsServide;
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var response = await _studentsService.GetStudent(id);
            return Ok(response);
        }

        [HttpPost("")]
        public async Task<IActionResult> GetStudents(StudentFilterQuery query)
        {
            var response = await _studentsService.GetStudents(query);
            return Ok(response);
        }

        [HttpPut("")]
        public async Task<IActionResult> SaveStudent([FromBody] StudentAddRequest request)
        {
            // We will fail fast or return a success response
            await _studentsService.AddStudent(request);
            return Ok();
        }

        [HttpDelete("/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            // We will fail fast or return a success response
            await _studentsService.DeleteStudent(id);
            return Ok();
        }

    }
}
