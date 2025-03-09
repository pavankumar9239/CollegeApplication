using CollegeApplication.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        //1. Strongly/tightly coupled
        //2. Loosely coupled
        private readonly ILogger _logger;

        public DemoController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            _logger.LogInformation("Index method started");
            return Ok();
        }
    }
}
