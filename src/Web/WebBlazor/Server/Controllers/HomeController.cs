using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebBlazor.Shared;

namespace WebBlazor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;

        public HomeController(IOptionsSnapshot<AppSettings> settings) =>
            _settings = settings;

        [HttpGet("config")]
        public AppSettings Get() =>
            _settings.Value;
    }
}
