using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNewwRedis.Models;
using StackExchange.Redis;

namespace MyNewwRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashTypeController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IConfiguration _configuration;
        private const string key = "myHash";
        public HashTypeController(IConnectionMultiplexer redis, IConfiguration configuration)
        {
            _redis = redis;
            _configuration = configuration;
        }

        [HttpPost("CreateHashAsync")]
        public async Task<ActionResult<Guid>> CreateHashAsync(RedisModel model)
        {
            var db = _redis.GetDatabase();
            model.Id = Guid.NewGuid();
            var hashFields = new HashEntry[]
            {
                new HashEntry("Id",model.Id.ToString()),
                new HashEntry("Name",model.Name),
                new HashEntry("CreationDate",model.CreationDate.Ticks.ToString())
            };
            await db.HashSetAsync($"{key}:{model.Id}", hashFields);
            return Ok(model.Id);
        }

        [HttpGet("GetHashByIdAsync/{id}")]
        public async Task<ActionResult<RedisModel>> GetHashByIdAsync(Guid id)
        {
            var db = _redis.GetDatabase();
            if (!await db.HashExistsAsync($"{key}:{id}", new RedisValue("Id")))
            {
                return NotFound();
            }
            var hash = await db.HashGetAllAsync($"{key}:{id}");
            var myRedisModel = ConvertHashToModel(hash);
            return Ok(myRedisModel);
        }

        [HttpGet("GetAllHashAsync")]
        public async Task<ActionResult<List<RedisModel>>> GetAllHashAsync()
        {
            var host = _configuration.GetValue<string>("RedisOption:Host");
            var port = _configuration.GetValue<int>("RedisOption:Port");
            var db = _redis.GetDatabase();

            var redisKeys = _redis.GetServer(host, port).Keys(pattern: $"{key}*").AsQueryable().Select(x => x.ToString()).ToList();
            List<RedisModel> result = new();
            foreach (var key in redisKeys)
            {
                var hash = await db.HashGetAllAsync(key);
                if (hash is not null && hash.Any())
                {
                    result.Add(ConvertHashToModel(hash));
                }
            }
            return Ok(result);
        }

        [HttpPut("UpdateHashAsync")]
        public async Task<ActionResult<Guid>> UpdateHashAsync(RedisModel model)
        {
            var db = _redis.GetDatabase();
            if (!await db.HashExistsAsync($"{key}:{model.Id}", new RedisValue("Id")))
            {
                return NotFound();
            }
            var hash = await db.HashGetAllAsync($"{key}:{model.Id}");
            var newValues = new HashEntry[]
               {
                     new HashEntry("Name",model.Name),
                     new HashEntry("CreationDate",$"{model.CreationDate.Ticks}")
               };
            await db.HashSetAsync($"{key}:{model.Id}", newValues);
            return Ok(model.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHashByIdAsync(Guid id)
        {
            var db = _redis.GetDatabase();
            if (!await db.HashExistsAsync($"{key}:{id}", new RedisValue("Id")))
            {
                return NotFound();
            }
            var fields = new RedisValue[]
           {
                new RedisValue("Id"),
                new RedisValue("Name"),
                new RedisValue("CreationDate"),
           };
            var result = await db.HashDeleteAsync($"{key}:{id}", fields);
            if (result == 3)
            {
                return Ok();
            }
            return BadRequest();
        }

        private RedisModel ConvertHashToModel(HashEntry[] hash)
        {
            var model = new RedisModel
            {
                Id = Guid.Parse(hash.FirstOrDefault(d => d.Name == "Id").Value),
                Name = hash.FirstOrDefault(d => d.Name == "Name").Value
            };
            var tickString = hash.FirstOrDefault(d => d.Name == "CreationDate").Value.ToString();
            long ticks;
            if (long.TryParse(tickString, out ticks))
            {
                var date = new DateTime(ticks);
                model.CreationDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            }
            return model;
        }
    }
}
