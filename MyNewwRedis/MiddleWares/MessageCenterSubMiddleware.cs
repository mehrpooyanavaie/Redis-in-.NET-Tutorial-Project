using StackExchange.Redis;

namespace MyNewwRedis.MiddleWares
{
    public class MessageCenterSubMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConnectionMultiplexer _redis;

        public MessageCenterSubMiddleware(RequestDelegate next, IConnectionMultiplexer redis)
        {
            _next = next;
            _redis = redis;
            var sub = _redis.GetSubscriber();
            sub.Subscribe("redis-channel", (channel, message) =>
            {
                Console.WriteLine($"meesage recived from channel:{channel}");
                Console.WriteLine($"message:{message}");
            });
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}
