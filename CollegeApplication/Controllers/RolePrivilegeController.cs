using AutoMapper;
using CollegeApplication.Models;
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
    public class RolePrivilegeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<RolePrivilege> _rolePrivilegeRepository;
        private ApiResponseDto _apiresponse;

        public RolePrivilegeController(IMapper mapper, ICollegeRepository<RolePrivilege> rolePrivilegeRepository)
        {
            _mapper = mapper;
            _rolePrivilegeRepository = rolePrivilegeRepository;
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
        public async Task<ActionResult<ApiResponseDto>> CreateRolePrivilegeAsync(RolePrivilegeDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid RolePrivilege data");
                    return BadRequest(_apiresponse);
                }

                RolePrivilege rolePrivilege = _mapper.Map<RolePrivilege>(dto);

                rolePrivilege.IsDeleted = false;
                rolePrivilege.CreatedDate = DateTime.Now;
                rolePrivilege.ModifiedDate = DateTime.Now;

                var result = await _rolePrivilegeRepository.CreateAsync(rolePrivilege);

                dto.Id = result.Id;

                _apiresponse.StatusCode = HttpStatusCode.OK;
                _apiresponse.Status = true;
                _apiresponse.Data = dto;

                return CreatedAtRoute("GetRolePrivilegeById", new { id = dto.Id }, _apiresponse);
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
        [Route("GetAllRolePrivileges")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> GetRolePrivileges()
        {
            try
            {
                var rolePrivileges = await _rolePrivilegeRepository.GetAllAsync();

                _apiresponse.Data = _mapper.Map<List<RolePrivilegeDto>>(rolePrivileges);
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
        [Route("GetRolePrivilegesByRoleId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> GetRolePrivilegesByRoleId(int roleId)
        {
            try
            {
                if (roleId <= 0 || roleId == null)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid RolePrivilege data");
                    return BadRequest(_apiresponse);
                }

                var rolePrivileges = await _rolePrivilegeRepository.GetAllByColumnAsync(x => x.RoleId == roleId);
                if (rolePrivileges.Count == 0)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.Error.Add($@"RolePrivilege with provided roleId {roleId} not found.");
                    return NotFound(_apiresponse);
                }
                

                _apiresponse.Data = _mapper.Map<List<RolePrivilegeDto>>(rolePrivileges);
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
        [Route("{id:int}", Name = "GetRolePrivilegeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> GetRolePrivilegeById(int id)
        {
            try
            {
                if (id <= 0 || id == null)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid RolePrivilege data");
                    return BadRequest(_apiresponse);
                }

                var role = await _rolePrivilegeRepository.GetAsync(x => x.Id == id);
                if (role == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.Error.Add($@"RolePrivilege with provided id {id} not found.");
                    return NotFound(_apiresponse);
                }

                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;
                _apiresponse.Data = _mapper.Map<RolePrivilegeDto>(role);
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
        [Route("{name:alpha}", Name = "GetRolePrivilegeByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> GetRolePrivilegeByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid RolePrivilege data");
                    return BadRequest(_apiresponse);
                }

                var role = await _rolePrivilegeRepository.GetAsync(x => x.RolePrivilegeName.Contains(name));
                if (role == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.Error.Add($@"RolePrivilege with provided name {name} not found.");
                    return NotFound(_apiresponse);
                }

                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;
                _apiresponse.Data = _mapper.Map<RolePrivilegeDto>(role);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> UpdateRolePrivilegeAsync([FromBody] RolePrivilegeDto rolePrivilegeDto)
        {
            try
            {
                if (rolePrivilegeDto == null || rolePrivilegeDto.Id <= 0)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid RolePrivilege data");
                    return BadRequest(_apiresponse);
                }

                RolePrivilege rolePrivilege = await _rolePrivilegeRepository.GetAsync(rolePrivilege => rolePrivilege.Id == rolePrivilegeDto.Id, true);

                if (rolePrivilege == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.Error.Add($@"RolePrivilege with provided id {rolePrivilegeDto.Id} not found.");
                    return NotFound(_apiresponse);
                }

                var newRolePrivilege = _mapper.Map<RolePrivilege>(rolePrivilegeDto);
                newRolePrivilege.CreatedDate = rolePrivilege.CreatedDate;
                newRolePrivilege.ModifiedDate = DateTime.Now;

                await _rolePrivilegeRepository.UpdateAsync(newRolePrivilege);

                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;
                _apiresponse.Data = _mapper.Map<RolePrivilegeDto>(newRolePrivilege);

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseDto>> UpdatePartial(int id, [FromBody] JsonPatchDocument<RolePrivilegeDto> rolePrivilegeDto)
        {
            try
            {
                if (rolePrivilegeDto == null || id <= 0)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid RolePrivilege data");
                    return BadRequest(_apiresponse);
                }

                var rolePrivilege = await _rolePrivilegeRepository.GetAsync(x => x.Id == id, true);

                var createdDate = rolePrivilege.CreatedDate;

                if (rolePrivilege == null)
                {
                    _apiresponse.Status = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.Error.Add($@"RolePrivilege with provided id {id} not found.");
                    return NotFound(_apiresponse);
                }

                var newRolePrivilegeDto = _mapper.Map<RolePrivilegeDto>(rolePrivilege);

                rolePrivilegeDto.ApplyTo(newRolePrivilegeDto, ModelState);

                if (!ModelState.IsValid)
                {
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.Status = false;
                    _apiresponse.Error.Add("Please provide valid RolePrivilege data");
                    return BadRequest(_apiresponse);
                }

                rolePrivilege = _mapper.Map<RolePrivilege>(newRolePrivilegeDto);

                rolePrivilege.CreatedDate = createdDate;
                rolePrivilege.ModifiedDate = DateTime.Now;

                await _rolePrivilegeRepository.UpdateAsync(rolePrivilege);

                _apiresponse.Status = true;
                _apiresponse.StatusCode = HttpStatusCode.OK;
                _apiresponse.Data = newRolePrivilegeDto;

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     
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
                _apiresponse.Error.Add("Please provide valid RolePrivilege data");
                return BadRequest(_apiresponse);
            }

            RolePrivilege rolePrivilege = await _rolePrivilegeRepository.GetAsync(rolePrivilege => rolePrivilege.Id == id);

            if (rolePrivilege == null)
            {
                _apiresponse.Status = false;
                _apiresponse.StatusCode = HttpStatusCode.NotFound;
                _apiresponse.Error.Add($@"Role with provided id {id} not found.");
                return NotFound(_apiresponse);
            }

            await _rolePrivilegeRepository.DeleteAsync(rolePrivilege);

            _apiresponse.Status = true;
            _apiresponse.StatusCode = HttpStatusCode.OK;

            return Ok(_apiresponse);
        }
    }
}
