using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        [HttpGet("")]
        public ActionResult GetAll()
        {
            var student = new Student()
            {
                Id = 1,
                FirstName = "John",
                MiddleName = "Doe",
                LastName = "Smith",
                DateOfBirth = new DateTime(1999, 1, 1),
                Department = new Department()
                {
                    Id = 2,
                    Name = "Computer Science"
                }

            };

            var response = student.Adapt<StudenResponse>();
            return Ok(response);
        }
    }
}
