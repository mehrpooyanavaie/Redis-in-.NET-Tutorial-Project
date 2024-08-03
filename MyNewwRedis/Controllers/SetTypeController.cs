using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNewwRedis.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace MyNewwRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetTypeController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;
        private const string key = "mySet";
        public SetTypeController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpPost("AddMemberToSetAsync")]
        public async Task<ActionResult<Guid>> AddMemberToSetAsync(RedisModel model)
        {
            var db = _redis.GetDatabase();
            model.Id = Guid.NewGuid();
            await db.SetAddAsync(key, JsonSerializer.Serialize(model));
            return Ok();
        }

        [HttpGet("GetByIdInSetAsync/{id}")]
        public async Task<ActionResult> GetByIdInSetAsync(Guid id)
        {
            var db = _redis.GetDatabase();
            if (await db.SetLengthAsync(key) == 0)
            {
                return NotFound();
            }
            var data = await db.SetMembersAsync(key);
            List<RedisModel> redisList = data.Select(x => JsonSerializer.Deserialize<RedisModel>(x)).ToList();
            //db.SetContainsAsync(key, JsonSerializer.Serialize(model)); 
            if (!redisList.Any(x => x.Id == id))
            {
                return NotFound();
            }
            return Ok(redisList.FirstOrDefault(x => x.Id == id));
        }

        [HttpGet("GetAllOfSetAsync")]
        public async Task<IActionResult> GetAllOfSetAsync()
        {
            var db = _redis.GetDatabase();
            if (await db.SetLengthAsync(key) == 0)
            {
                return Ok(new List<RedisModel>());
            }
            var data = await db.SetMembersAsync(key);
            List<RedisModel> redisList = data.Select(x => JsonSerializer.Deserialize<RedisModel>(x)).ToList();

            return Ok(redisList);
        }

        [HttpPut("UpdateOneMemberOfSetAsync")]
        public async Task<ActionResult<Guid>> UpdateOneMemberOfSetAsync(RedisModel model)
        {
            var db = _redis.GetDatabase();
            if (await db.SetLengthAsync(key) == 0)
            {
                return BadRequest();
            }
            var data = await db.SetMembersAsync(key);
            List<RedisModel> redisList = data.Select(x => JsonSerializer.Deserialize<RedisModel>(x)).ToList();
            if (!redisList.Any(x => x.Id == model.Id))
            {
                return BadRequest();
            }
            redisList = redisList.Where(x => x.Id != model.Id).ToList();
            redisList.Add(model);
            await db.KeyDeleteAsync(key);
            RedisValue[] newItems = redisList.Select(x => new RedisValue(JsonSerializer.Serialize(x))).ToArray();
            await db.SetAddAsync(key, newItems);
            return Ok(model.Id);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOneMemberOfSet(RedisModel model)
        {
            var db = _redis.GetDatabase();
            var result = await db.SetRemoveAsync(key, JsonSerializer.Serialize(model));
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
