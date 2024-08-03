using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyNewwRedis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RateLimitedController : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        [Route("limited")]
        public async Task<IActionResult> Limited()
        {
            return new JsonResult(new { Limited = false });
        }

        [HttpGet]
        [HttpPost]
        [Route("indirectly-limited")]
        public async Task<IActionResult> IndirectlyLimited()
        {
            return new JsonResult(new { NeverLimited = true });
        }
    }
}

