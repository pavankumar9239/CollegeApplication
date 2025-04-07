using AutoMapper;
using Azure;
using CollegeApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repository.Contracts;
using Repository.Models;
using System.Net;
using System.Threading.Tasks;

namespace CollegeApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "LoginForLocal", Roles = "SuperAdmin, Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Role> _roleRepository;
        private ApiResponseDto _apiresponse;
        public RoleController(IMapper mapper, ICollegeRepository<Role> rolerepository)
        {
            _mapper = mapper;
            _roleRepository = rolerepository;
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
        public async Task<ActionResult<ApiResponseDto>> CreateRoleAsync(RoleDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid Role data");
                    return BadRequest(_apiresponse);
                }

                Role role = _mapper.Map<Role>(dto);

                role.IsDeleted = false;
                role.CreatedDate = DateTime.Now;
                role.ModifiedDate = DateTime.Now;

                var result = await _roleRepository.CreateAsync(role);

                dto.Id = result.Id;

                _apiresponse.StatusCode = HttpStatusCode.OK;
                _apiresponse.Status = true;
                _apiresponse.Data = dto;

                return CreatedAtRoute("GetRoleById", new { id = dto.Id }, _apiresponse);
                //return Ok(_apiresponse);
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
        [Route("GetAllRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> GetRoles()
        {
            try
            {
                var roles = await _roleRepository.GetAllAsync();

                _apiresponse.Data = _mapper.Map<List<RoleDto>>(roles);
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
        [Route("{id:int}", Name = "GetRoleById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> GetRoleById(int id)
        {
            try
            {
                if(id <= 0 || id == null)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid Role data");
                    return BadRequest(_apiresponse);
                }

                var role = await _roleRepository.GetAsync(x => x.Id == id);
                if (role == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.Error.Add($@"Role with provided id {id} not found.");
                    return NotFound(_apiresponse);
                }

                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;
                _apiresponse.Data = _mapper.Map<RoleDto>(role);
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
        [Route("{name:alpha}", Name = "GetRoleByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> GetRoleByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid Role data");
                    return BadRequest(_apiresponse);
                }

                var role = await _roleRepository.GetAsync(x => x.RoleName.Equals(name));
                if (role == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.Error.Add($@"Role with provided name {name} not found.");
                    return NotFound(_apiresponse);
                }

                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;
                _apiresponse.Data = _mapper.Map<RoleDto>(role);
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
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> UpdateRoleAsync([FromBody] RoleDto roleDto)
        {
            try
            {
                if(roleDto == null || roleDto.Id <= 0)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid Role data");
                    return BadRequest(_apiresponse);
                }

                Role role = await _roleRepository.GetAsync(role => role.Id == roleDto.Id, true);

                if(role == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.Error.Add($@"Role with provided id {roleDto.Id} not found.");
                    return NotFound(_apiresponse);
                }

                var newRole = _mapper.Map<Role>(roleDto);
                newRole.CreatedDate = role.CreatedDate;
                newRole.ModifiedDate = DateTime.Now;

                await _roleRepository.UpdateAsync(newRole);

                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;
                _apiresponse.Data = _mapper.Map<RoleDto>(newRole);

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

        [HttpPatch]
        [Route("UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> UpdatePartial(int id, [FromBody] JsonPatchDocument<RoleDto> roleDto)
        {
            try
            {
                if(roleDto == null || id <= 0)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid Role data");
                    return BadRequest(_apiresponse);
                }

                var role = await _roleRepository.GetAsync(x => x.Id == id, true);

                var createdDate = role.CreatedDate;

                if (role == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.Error.Add($@"Role with provided id {id} not found.");
                    return NotFound(_apiresponse);
                }

                var newRoleDto = _mapper.Map<RoleDto>(role);

                roleDto.ApplyTo(newRoleDto, ModelState);

                if(!ModelState.IsValid)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid Role data");
                    return BadRequest(_apiresponse);
                }

                role = _mapper.Map<Role>(newRoleDto);

                role.CreatedDate = createdDate;
                role.ModifiedDate = DateTime.Now;

                await _roleRepository.UpdateAsync(role);

                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;
                _apiresponse.Data = newRoleDto;

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
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> DeleteByIdAsync(int id)
        {
            if (id <= 0)
            {
                _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                _apiresponse.Status = false;
                _apiresponse.Error.Add("Please provide valid Role data");
                return BadRequest(_apiresponse);
            }

            Role role = await _roleRepository.GetAsync(role => role.Id == id);

            if (role == null)
            {
                _apiresponse.Status = false;
                _apiresponse.StatusCode = HttpStatusCode.NotFound;
                _apiresponse.Error.Add($@"Role with provided id {id} not found.");
                return NotFound(_apiresponse);
            }

            await _roleRepository.DeleteAsync(role);

            _apiresponse.Status = true;
            _apiresponse.StatusCode = HttpStatusCode.OK;

            return Ok(_apiresponse);
        }
    }
}
