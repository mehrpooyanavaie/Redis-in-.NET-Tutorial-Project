using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNewwRedis.Models;
using StackExchange.Redis;
using System.Text;

namespace MyNewwRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageCenterController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;
        public MessageCenterController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishMessage(PublishDto dto)
        {
            var pub = _redis.GetSubscriber();
            var message = Encoding.UTF8.GetBytes(dto.Message);
            await pub.PublishAsync("codecell-channel", message);
            return Ok();
        }
    }
}
