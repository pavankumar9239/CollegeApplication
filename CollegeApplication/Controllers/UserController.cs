using CollegeApplication.Models;
using CollegeApplication.Services.UserService.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;
        public ApiResponseDto _apiresponse;

        public UserController(IUserService userService)
        {
            _userService = userService;
            _apiresponse = new();
            _apiresponse.Error = new();
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> CreateUser(UserDto dto)
        {
            try
            {
                var userCreated = await _userService.CreateUserAsync(dto);

                _apiresponse.Status = userCreated;
                _apiresponse.StatusCode = HttpStatusCode.OK;
                _apiresponse.Data = userCreated;

                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiresponse.Status = false;
                _apiresponse.Error.Add(ex.Message);
                return _apiresponse;
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> GetAllUsers()
        {
            try
            {
                _apiresponse.Data = await _userService.GetUsersAsync();
                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiresponse.Status = false;
                _apiresponse.Error.Add(ex.Message);
                return _apiresponse;
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> GetUserById(int id)
        {
            try
            {
                if (id <= 0 || id == null)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid user data");
                    return BadRequest(_apiresponse);
                }

                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.Error.Add($@"user with provided id {id} not found.");
                    return NotFound(_apiresponse);
                }

                _apiresponse.Data = user;
                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiresponse.Status = false;
                _apiresponse.Error.Add(ex.Message);

                return _apiresponse;
            }
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetUserByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> GetUserByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid user data");
                    return BadRequest(_apiresponse);
                }

                var user = await _userService.GetUserByName(name);
                if (user == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.Error.Add($@"user with provided name {name} not found.");
                    return NotFound(_apiresponse);
                }

                _apiresponse.Data = user;
                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiresponse.Status = false;
                _apiresponse.Error.Add(ex.Message);
                return _apiresponse;
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> UpdateUser(UserReadOnlyDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid user data");
                    return BadRequest(_apiresponse);
                }

                var result = await _userService.UpdateUser(dto);

                _apiresponse.Data = result;
                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiresponse.Status = false;
                _apiresponse.Error.Add(ex.Message);
                return _apiresponse;
            }
        }

        [HttpDelete]
        [Route("DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> Deleteuser(int id)
        {
            try
            {
                if (id <= 0 || id == null)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid user data");
                    return BadRequest(_apiresponse);
                }

                var result = await _userService.DeleteUser(id);

                _apiresponse.Data = result;
                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiresponse);
            }
            catch (Exception ex)
            {
                _apiresponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiresponse.Status = false;
                _apiresponse.Error.Add(ex.Message);
                return _apiresponse;
            }
        }
    }
}
