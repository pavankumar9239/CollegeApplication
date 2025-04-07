using AutoMapper;
using CollegeApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repository.Contracts;
using Repository.Models;
using System.Net;

namespace CollegeApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(PolicyName = "AllowLocalHost")]
    [Authorize(AuthenticationSchemes = "LoginForLocal", Roles = "SuperAdmin, Admin")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        //private readonly ICollegeRepository<Student> _studentRepository;
        private readonly IStudentRepository _studentRepository;
        private ApiResponseDto _apiResponse;

        public StudentController(ILogger<StudentController> logger, IMapper mapper, IStudentRepository studentRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _studentRepository = studentRepository;
            _apiResponse = new();
            _apiResponse.Error = new();
        }


        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[DisableCors]
        //[AllowAnonymous]
        public async Task<ActionResult<ApiResponseDto>> getStudents()
        {
            try
            {
                _logger.LogInformation("Get Student method");

                //To fetch all data in Students table
                var students = await _studentRepository.GetAllAsync();

                //To fetch as customlist
                //var students = await _dbContext.Students.Select(s => new StudentDto()
                //{
                //    Id = s.Id,
                //    Name = s.Name,
                //    Address = s.Address,
                //    Email = s.Email,
                //    DOB = s.DOB
                //}).ToListAsync();

                //Field mapping can be done usinf automapper
                var studentDtoData = _mapper.Map<List<StudentDto>>(students);

                //when we want api to return in xml format as well, then we need to pass the response to OK as List and not Enumerable.
                //List<StudentDto> students = new List<StudentDto>();
                //foreach (var student in _dbContext.Students)
                //{
                //    StudentDto studentDto = new StudentDto()
                //    {
                //        Id = student.Id,
                //        Name = student.Name,
                //        Address = student.Address,
                //        Email = student.Email,
                //        DOB = student.DOB
                //    };
                //    students.Add(studentDto);
                //}

                //using LINQ
                //var students = CollegeRepository.Students.Select(s => new StudentDto()
                //{
                //    Id = s.Id,
                //    Name = s.Name,
                //    Email = s.Email,
                //    Address = s.Address
                //});

                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = studentDtoData;

                //Ok - 200 - Success
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(200, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> getStudentById(int id)
        {
            try
            {
                _logger.LogInformation("GetStudentsById method");
                //BadRequest - 400 - Client Error
                if (id <= 0)
                {
                    _logger.LogWarning("Bad Request"); 
                    _apiResponse.Status = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }


                var student = await _studentRepository.GetAsync(student => student.Id == id);
                //NotFound - 404 - Client Error
                if (student == null)
                {
                    _logger.LogError("Student with provided id not found.");
                    _apiResponse.Status = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Error.Add($@"Student with provided id {id} not found.");
                    return NotFound(_apiResponse);
                }

                //var studentDto = new StudentDto()
                //{
                //    Id = student.Id,
                //    Name = student.Name,
                //    Email = student.Email,
                //    Address = student.Address,
                //    DOB = student.DOB
                //};

                var studentDtoData = _mapper.Map<StudentDto>(student);

                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = studentDtoData;

                //Ok - 200 - Success
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error.Add(ex.Message);
                return _apiResponse;
            }            
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> getStudentByName(string name)
        {
            try
            {
                //BadRequest - 400 - Client Error
                if (string.IsNullOrEmpty(name))
                {
                    _apiResponse.Status = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var student = await _studentRepository.GetAsync(student => student.Name.ToLower().Contains(name.ToLower()));
                //NotFound - 404 - Client Error
                if (student == null)
                {
                    _apiResponse.Status = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Error.Add($"Student with name {name} not found.");
                    return NotFound(_apiResponse);
                }

                //var studentDto = new StudentDto()
                //{
                //    Id = student.Id,
                //    Name = student.Name,
                //    Email = student.Email,
                //    Address = student.Address,
                //    DOB = student.DOB
                //};

                var studentDto = _mapper.Map<StudentDto>(student);

                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = studentDto;

                //Ok - 200 - Success
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error.Add(ex.Message);
                return _apiResponse;
            }            
        }

        [HttpPost]
        [Route("create")]
        ///api/Student/create
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> createStudent([FromBody] StudentDto studentDto)
        {
            try
            {
                //By default, ApiController attribute should check for attribute validation. So below lines arenot required if ApiController attribute is present for class.
                //if (!ModelState.IsValid)
                //    return BadRequest(ModelState);

                if (studentDto == null)
                {
                    _apiResponse.Status = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                //if(studentDto.AdmissionDate < DateTime.Now)
                //{
                //    //1. Directly adding error to modelstate
                //    //2. To create custom model validator
                //    ModelState.AddModelError("AdmissionDate Error", "Admission date should be greater than current date.");
                //    return BadRequest(ModelState);
                //}

                //var newId = CollegeRepository.Students.LastOrDefault().Id + 1;
                //Student student = new Student()
                //{
                //    //Id = newId,
                //    Name = studentDto.Name,
                //    Email = studentDto.Email,
                //    Address = studentDto.Address,
                //    DOB = studentDto.DOB
                //};

                var student = _mapper.Map<Student>(studentDto);

                student = await _studentRepository.CreateAsync(student);
                studentDto.Id = student.Id;

                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Data = studentDto;

                //Returns
                // status - 201 
                //https://localhost:5204/api/Student/3
                //New StudentDto details
                return CreatedAtRoute("GetStudentById", new { id = student.Id }, _apiResponse);
                //return Ok(studentDto);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error.Add(ex.Message);
                return _apiResponse;
            }            
        }

        //Drawback of HttpPut - It will update all the fields. If we want to update only few fields, then we need to use HttpPatch
        [HttpPut]
        [Route("update")]
        //api/Student/update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> updateStudent([FromBody]StudentDto studentDto)
        {
            if(studentDto == null || studentDto.Id <= 0)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Error.Add("Please enter valid student details.");
                return BadRequest(_apiResponse);
            }

            //If we fetch the record with no tracking, then we can create a new object for entity class and directly update the record and need not have to map all columns as we did earlier.
            var student = await _studentRepository.GetAsync(student => student.Id == studentDto.Id, true);
            if (student == null)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Error.Add($"Student with id {studentDto.Id} not found.");
                return NotFound(_apiResponse);
            }

            //var newStudent = new Student()
            //{
            //    Id = student.Id,
            //    Name = studentDto.Name,
            //    Address = studentDto.Address,
            //    Email = studentDto.Email,
            //    DOB = studentDto.DOB
            //};

            var newStudent = _mapper.Map<Student>(studentDto);

            await _studentRepository.UpdateAsync(newStudent);

            //Earlier approach
            //student.Name = studentDto.Name;
            //student.Email = studentDto.Email;
            //student.Address = studentDto.Address;
            //student.DOB = studentDto.DOB;

            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.NoContent;

            //NoContent when nothing to be returned - 204 - Success
            return Ok(_apiResponse);
        }

        [HttpPatch]
        [Route("{id:int}/updatePartial")]
        //api/Student/updatepartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> updatePartialStudent(int id, [FromBody]JsonPatchDocument<StudentDto> studentDto)
        {   
            if (studentDto == null || id <= 0)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Error.Add("Please enter valid student details.");
                return BadRequest(_apiResponse);
            }

            var existingStudent = await _studentRepository.GetAsync(student => student.Id == id, true);
            if (existingStudent == null)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.Error.Add($"Student with id {id} not found.");
                return NotFound(_apiResponse);
            }

            //StudentDto studentDto1 = new StudentDto()
            //{
            //    Id = existingStudent.Id,
            //    Name = existingStudent.Name,
            //    Email = existingStudent.Email,
            //    Address = existingStudent.Address,
            //    DOB = existingStudent.DOB
            //};

            var studentDto1 = _mapper.Map<StudentDto>(existingStudent);

            studentDto.ApplyTo(studentDto1, ModelState);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);


            //existingStudent.Name = studentDto1.Name;
            //existingStudent.Email = studentDto1.Email;
            //existingStudent.Address = studentDto1.Address;
            //existingStudent.DOB = studentDto1.DOB;

            existingStudent = _mapper.Map<Student>(studentDto1);

            await _studentRepository.UpdateAsync(existingStudent);

            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.NoContent;

            //NoContent when nothing to be returned - 204 - Success
            return Ok(_apiResponse);
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> deleteStudentById(int id)
        {
            try
            {
                //BadRequest - 400 - Client Error
                if (id <= 0)
                {
                    _apiResponse.Status = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Error.Add("Please enter valid student details.");
                    return BadRequest(_apiResponse);
                }

                var student = await _studentRepository.GetAsync(student => student.Id == id);
                //NotFound - 404 - Client Error
                if (student == null)
                {
                    _apiResponse.Status = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Error.Add($"Student with id {id} not found.");
                    return NotFound(_apiResponse);
                }

                await _studentRepository.DeleteAsync(student);

                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = true;

                //Ok - 200 - Success
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Error.Add(ex.Message);
                return _apiResponse;
            }            
        }
    }
}
