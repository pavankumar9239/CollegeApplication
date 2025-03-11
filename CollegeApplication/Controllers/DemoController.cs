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
        private readonly ILogger<DemoController> _logger;

        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            _logger.LogTrace("Index method started Trace");
            _logger.LogDebug("Index method started Debug");
            _logger.LogInformation("Index method started Information");
            _logger.LogWarning("Index method started Warning");
            _logger.LogError("Index method started Error");
            _logger.LogCritical("Index method started Critical");
            return Ok();
        }
    }
}
