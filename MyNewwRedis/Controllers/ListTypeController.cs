using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNewwRedis.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace MyNewwRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListTypeController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;
        private const string keyPrefix = "myList";
        public ListTypeController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpPost("AddMemberToListAsync")]
        public async Task<ActionResult<Guid>> AddMemberToListAsync(RedisModel model, ListDirectionTypes direction = ListDirectionTypes.Left)
        {
            var db = _redis.GetDatabase();

            model.Id = Guid.NewGuid();

            if (direction == ListDirectionTypes.Left)
                await db.ListLeftPushAsync(keyPrefix, JsonSerializer.Serialize(model));
            else
                await db.ListRightPushAsync(keyPrefix, JsonSerializer.Serialize(model));
            return Ok(model.Id);
        }

        [HttpGet("GetByIdOfListAsync/{id}")]
        public async Task<ActionResult<RedisModel>> GetByIdOfListAsync(Guid id)
        {
            var db = _redis.GetDatabase();
            var data = await db.ListRangeAsync(keyPrefix, 0, -1);
            if (data.Length <= 0)
            {
                return NotFound();
            }
            List<RedisModel> redisList = data.Select(x => JsonSerializer.Deserialize<RedisModel>(x)).ToList();

            if (!redisList.Any(x => x.Id == id))
            {
                return NotFound();
            }
            var myRedisModel = redisList.FirstOrDefault(x => x.Id == id);
            return Ok(myRedisModel);
        }

        [HttpGet("GetAllOfListAsync")]
        public async Task<ActionResult<List<RedisModel>>> GetAllOfListAsync()
        {
            var db = _redis.GetDatabase();
            var data = await db.ListRangeAsync(keyPrefix, 0, -1);
            if (data.Length <= 0)
            {
                return NotFound();
            }
            List<RedisModel> redisList = data.Select(x => JsonSerializer.Deserialize<RedisModel>(x.ToString())).ToList();
            return Ok(redisList);
        }

        [HttpPut("UpdateOneMemberOfList")]
        public async Task<ActionResult<Guid>> UpdateOneMemberOfList(RedisModel model)
        {
            var db = _redis.GetDatabase();
            var data = await db.ListRangeAsync(keyPrefix, 0, -1);
            if (data.Length <= 0)
            {
                return NotFound();
            }
            List<RedisModel> redisList = data.Select(x => JsonSerializer.Deserialize<RedisModel>(x.ToString())).ToList();
            if (!redisList.Any(x => x.Id == model.Id))
            {
                return BadRequest();
            }
            redisList = redisList.Where(x => x.Id != model.Id).ToList();
            redisList.Add(model);
            await db.KeyDeleteAsync(keyPrefix);
            RedisValue[] newItems = redisList.Select(x => new RedisValue(JsonSerializer.Serialize(x))).ToArray();
            await db.ListLeftPushAsync(keyPrefix, newItems);

            return Ok(model.Id);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(RedisModel model)
        {
            var db = _redis.GetDatabase();
            var result = await db.ListRemoveAsync(keyPrefix, JsonSerializer.Serialize(model));
            if (result > 0)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("GetByRangeInListAsync")]
        public async Task<ActionResult<List<RedisModel>>> GetByRangeInListAsync(int startIndex, int stopIndex)
        {
            var db = _redis.GetDatabase();
            var data = await db.ListRangeAsync(keyPrefix, startIndex, stopIndex);
            if (data.Length <= 0)
            {
                return NotFound();
            }
            List<RedisModel> redisList = data.Select(x => JsonSerializer.Deserialize<RedisModel>(x.ToString())).ToList();

            return Ok(redisList);
        }

        [HttpDelete("PopStartAsync")]
        public async Task<IActionResult> PopStartAsync()
        {
            var db = _redis.GetDatabase();
            var data = await db.ListLeftPopAsync(keyPrefix);
            if (data.HasValue)
            {
                return Ok(JsonSerializer.Deserialize<RedisModel>(data.ToString()));
            }
            return NotFound();
        }

        [HttpDelete("PopEndAsync")]
        public async Task<IActionResult> PopEndAsync()
        {
            var db = _redis.GetDatabase();
            var data = await db.ListRightPopAsync(keyPrefix);
            if (data.HasValue)
            {
                return Ok(JsonSerializer.Deserialize<RedisModel>(data.ToString()));
            }
            return NotFound();
        }
    }
}
