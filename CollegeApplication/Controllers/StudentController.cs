using CollegeApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDto>> getStudents()
        {
            //List<StudentDto> students = new List<StudentDto>();
            //foreach (var student in CollegeRepository.Students)
            //{
            //    StudentDto studentDto = new StudentDto()
            //    {
            //        Id = student.Id,
            //        Name = student.Name,
            //        Address = student.Address,
            //        Email = student.Email
            //    };
            //    students.Add(studentDto);
            //}

            //using LINQ
            var students = CollegeRepository.Students.Select(s => new StudentDto()
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Address = s.Address
            });

            //Ok - 200 - Success
            return Ok(students);
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(200, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult getStudentById(int id)
        {
            //BadRequest - 400 - Client Error
            if (id <= 0)
                return BadRequest();

            var student = CollegeRepository.Students.Where(x => x.Id == id).FirstOrDefault();
            //NotFound - 404 - Client Error
            if (student == null)
                return NotFound($"Student with id {id} not found.");

            var studentDto = new StudentDto()
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Address = student.Address
            };

            //Ok - 200 - Success
            return Ok(studentDto);
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> deleteStudentById(int id)
        {
            //BadRequest - 400 - Client Error
            if (id <= 0)
                return BadRequest();

            var student = CollegeRepository.Students.Where(x => x.Id == id).FirstOrDefault();
            //NotFound - 404 - Client Error
            if (student == null)
                return NotFound($"Student with id {id} not found.");

            //Ok - 200 - Success
            return Ok(CollegeRepository.Students.Remove(student));
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDto> getStudentByName(string name)
        {
            //BadRequest - 400 - Client Error
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var student = CollegeRepository.Students.Where(x => x.Name == name).FirstOrDefault();
            //NotFound - 404 - Client Error
            if (student == null)
                return NotFound($"Student with name {name} not found.");

            var studentDto = new StudentDto()
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Address = student.Address
            };

            //Ok - 200 - Success
            return Ok(studentDto);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDto> createStudent([FromBody] StudentDto studentDto)
        {
            //By default, ApiController attribute should check for attribute validation. So below lines arenot required if ApiController attribute is present for class.
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            if (studentDto == null)
                return BadRequest();

            var newId = CollegeRepository.Students.LastOrDefault().Id + 1;
            Student student = new Student()
            {
                Id = newId,
                Name = studentDto.Name,
                Email = studentDto.Email,
                Address = studentDto.Address
            };
            CollegeRepository.Students.Add(student);

            studentDto.Id = newId;

            //Returns
            // status - 201 
            //https://localhost:5204/api/Student/3
            //New StudentDto details
            return CreatedAtRoute("GetStudentById", new { id = newId }, studentDto);
            //return Ok(studentDto);
        }
    }
}
