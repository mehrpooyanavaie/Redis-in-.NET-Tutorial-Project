using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.RegularExpressions;
using MyNewwRedis.Rules;
namespace MyNewwRedis.MiddleWares
{

    public class SlidingWindowRateLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _redis;
        private readonly string _rateLimiterScript;

        public SlidingWindowRateLimiterMiddleware(RequestDelegate next, IConfiguration configuration, IConnectionMultiplexer redis)
        {
            _next = next;
            _configuration = configuration;
            _redis = redis;
            _rateLimiterScript = File.ReadAllText("Scripts/rate_limiter.lua");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var rules = _configuration.GetSection("RedisRateLimits").Get<RateLimitRule[]>();
            var redisDb = _redis.GetDatabase();
            var path = context.Request.Path.ToString();

            foreach (var rule in rules)
            {
                if (path == rule.Path || (rule.PathRegex != null && Regex.IsMatch(path, rule.PathRegex)))
                {
                    var windowInSeconds = rule.GetWindowInSeconds();
                    var key = $"ratelimit:{path}:{context.Connection.RemoteIpAddress}";
                    var limit = rule.MaxRequests;

                    var result = (int)await redisDb.ScriptEvaluateAsync(_rateLimiterScript, new RedisKey[] { key }, new RedisValue[] { limit, windowInSeconds });

                    if (result == 0)
                    {
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
    }
