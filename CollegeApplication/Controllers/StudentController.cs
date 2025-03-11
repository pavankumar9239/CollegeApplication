using CollegeApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repository.DBContext;
using Repository.Models;

namespace CollegeApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly CollegeDBContext _dbContext;

        public StudentController(ILogger<StudentController> logger, CollegeDBContext collegeDBContext)
        {
            _logger = logger;
            _dbContext = collegeDBContext;
        }


        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDto>> getStudents()
        {
            _logger.LogInformation("Get Student method");
            //when we want api to return in xml format as well, then we need to pass the response to OK as List and not Enumerable.
            List<StudentDto> students = new List<StudentDto>();
            foreach (var student in _dbContext.Students)
            {
                StudentDto studentDto = new StudentDto()
                {
                    Id = student.Id,
                    Name = student.Name,
                    Address = student.Address,
                    Email = student.Email,
                    DOB = student.DOB
                };
                students.Add(studentDto);
            }

            //using LINQ
            //var students = CollegeRepository.Students.Select(s => new StudentDto()
            //{
            //    Id = s.Id,
            //    Name = s.Name,
            //    Email = s.Email,
            //    Address = s.Address
            //});

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
            _logger.LogInformation("GetStudentsById method");
            //BadRequest - 400 - Client Error
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }
                

            var student = _dbContext.Students.Where(x => x.Id == id).FirstOrDefault();
            //NotFound - 404 - Client Error
            if (student == null)
            {
                _logger.LogError("Student with provided id not found.");
                return NotFound($"Student with id {id} not found.");
            }

            var studentDto = new StudentDto()
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Address = student.Address,
                DOB = student.DOB
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

            var student = _dbContext.Students.Where(x => x.Id == id).FirstOrDefault();
            //NotFound - 404 - Client Error
            if (student == null)
                return NotFound($"Student with id {id} not found.");

            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();

            //Ok - 200 - Success
            return Ok(true);
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

            var student = _dbContext.Students.Where(x => x.Name == name).FirstOrDefault();
            //NotFound - 404 - Client Error
            if (student == null)
                return NotFound($"Student with name {name} not found.");

            var studentDto = new StudentDto()
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Address = student.Address,
                DOB = student.DOB
            };

            //Ok - 200 - Success
            return Ok(studentDto);
        }

        [HttpPost]
        [Route("create")]
        ///api/Student/create
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

            //if(studentDto.AdmissionDate < DateTime.Now)
            //{
            //    //1. Directly adding error to modelstate
            //    //2. To create custom model validator
            //    ModelState.AddModelError("AdmissionDate Error", "Admission date should be greater than current date.");
            //    return BadRequest(ModelState);
            //}

            //var newId = CollegeRepository.Students.LastOrDefault().Id + 1;
            Student student = new Student()
            {
                //Id = newId,
                Name = studentDto.Name,
                Email = studentDto.Email,
                Address = studentDto.Address,
                DOB = studentDto.DOB
            };
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();
            studentDto.Id = student.Id;

            //Returns
            // status - 201 
            //https://localhost:5204/api/Student/3
            //New StudentDto details
            return CreatedAtRoute("GetStudentById", new { id = student.Id }, studentDto);
            //return Ok(studentDto);
        }

        //Drawback of HttpPut - It will update all the fields. If we want to update only few fields, then we need to use HttpPatch
        [HttpPut]
        [Route("update")]
        //api/Student/update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult updateStudent([FromBody]StudentDto studentDto)
        {
            if(studentDto == null || studentDto.Id <= 0)
                return BadRequest("Please enter valid student details.");

            var student = _dbContext.Students.Where(x => x.Id == studentDto.Id).FirstOrDefault();
            if (student == null)
                return NotFound($"Student with id {studentDto.Id} not found.");

            student.Name = studentDto.Name;
            student.Email = studentDto.Email;
            student.Address = studentDto.Address;
            student.DOB = studentDto.DOB;

            _dbContext.SaveChanges();

            //NoContent when nothing to be returned - 204 - Success
            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/updatePartial")]
        //api/Student/updatepartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult updatePartialStudent(int id, [FromBody]JsonPatchDocument<StudentDto> studentDto)
        {
            if (studentDto == null || id <= 0)
                return BadRequest("Please enter valid student details.");

            var existingStudent = _dbContext.Students.Where(x => x.Id == id).FirstOrDefault();
            if (existingStudent == null)
                return NotFound($"Student with id {id} not found.");

            StudentDto studentDto1 = new StudentDto()
            {
                Id = existingStudent.Id,
                Name = existingStudent.Name,
                Email = existingStudent.Email,
                Address = existingStudent.Address,
                DOB = existingStudent.DOB
            };

            studentDto.ApplyTo(studentDto1, ModelState);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);


            existingStudent.Name = studentDto1.Name;
            existingStudent.Email = studentDto1.Email;
            existingStudent.Address = studentDto1.Address;
            existingStudent.DOB = studentDto1.DOB;

            _dbContext.SaveChanges();

            //NoContent when nothing to be returned - 204 - Success
            return NoContent();
        }
    }
}
