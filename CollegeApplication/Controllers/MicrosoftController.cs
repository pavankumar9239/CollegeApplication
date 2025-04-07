using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors(PolicyName = "AllowOnlyMicrosoft")]
    [Authorize(AuthenticationSchemes = "LoginForMicrosoft", Roles = "Admin")]
    public class MicrosoftController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Get()
        {
            return Ok("This is from Microsoft Controller");
        }
    }
}
