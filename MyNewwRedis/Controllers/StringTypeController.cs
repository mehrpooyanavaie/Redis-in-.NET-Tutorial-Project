using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNewwRedis.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace MyNewwRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StringTypeController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IConfiguration _configuration;
        private const string keyPrefix = "myString_";
        public StringTypeController(IConnectionMultiplexer redis, IConfiguration configuration)
        {
            _redis = redis;
            _configuration = configuration;
        }

        [HttpPost("CreateStringAsync")]
        public async Task<ActionResult<Guid>> CreateStringAsync(RedisModel model, int expireInMinute = 0)
        {
            var db = _redis.GetDatabase();
            model.Id = Guid.NewGuid();
            var key = new RedisKey($"{keyPrefix}{model.Id}");
            await db.StringSetAsync(key, JsonSerializer.Serialize(model));
            if (expireInMinute > 0)
            {
                await db.KeyExpireAsync(key, TimeSpan.FromMinutes(expireInMinute));
            }
            return Ok(model.Id);
        }

        [HttpGet("GetStringByIdAsync/{id}")]
        public async Task<ActionResult<RedisModel>> GetStringByIdAsync(Guid id)
        {
            var db = _redis.GetDatabase();
            var data = await db.StringGetAsync($"{keyPrefix}{id}");
            if (!data.HasValue)
            {
                return NotFound();
            }
            var returnString = JsonSerializer.Deserialize<RedisModel>(data.ToString());
            return Ok(returnString);
        }

        [HttpGet("GetAllStringsAsync")]
        public async Task<ActionResult<List<RedisModel>>> GetAllStringsAsync()
        {
            var host = _configuration.GetValue<string>("RedisOption:Host");
            var port = _configuration.GetValue<int>("RedisOption:Port");
            var db = _redis.GetDatabase();

            var redisKeys = _redis.GetServer(host, port).Keys(pattern: $"{keyPrefix}*").AsQueryable().Select(x => x.ToString()).ToList();
            List<RedisModel> result = new();
            foreach (var key in redisKeys)
            {
                var data = await db.StringGetAsync(key);
                result.Add(JsonSerializer.Deserialize<RedisModel>(data.ToString()));
            }

            return Ok(result);

        }

        [HttpPut("UpdateStringAsync/{expireInMinute}")]
        public async Task<ActionResult<Guid>> UpdateStringAsync(RedisModel model, int expireInMinute)
        {
            var key = new RedisKey($"{keyPrefix}{model.Id}");
            var db = _redis.GetDatabase();
            var data = await db.StringGetAsync(key);
            if (!data.HasValue)
            {
                return NotFound();
            }
            await db.StringSetAsync(key, JsonSerializer.Serialize(model));
            if (expireInMinute > 0)
            {
                await db.KeyExpireAsync(key, TimeSpan.FromMinutes(expireInMinute));
            }
            return Ok(model.Id);
        }

        [HttpDelete("DeleteStringAsync/{id}")]
        public async Task<IActionResult> DeleteStringAsync(Guid id)
        {
            var key = new RedisKey($"{keyPrefix}{id}");
            var db = _redis.GetDatabase();
            var data = await db.StringGetAsync(key);
            if (!data.HasValue)
            {
                return NotFound();
            }

            await db.KeyDeleteAsync(key);
            return Ok();
        }
    }
}
