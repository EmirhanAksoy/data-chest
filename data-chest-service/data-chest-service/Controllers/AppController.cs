using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Diagnostics;

namespace data_chest_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AppController : ControllerBase
    {

        private readonly ILogger<AppController> _logger;

        public AppController(ILogger<AppController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [Route("version")]
        public IActionResult GetVersion()
        {
            _logger.LogInformation("Version checked.");

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string? version = fileVersionInfo?.ProductVersion;
            return Ok(version);
        }
    }
}
