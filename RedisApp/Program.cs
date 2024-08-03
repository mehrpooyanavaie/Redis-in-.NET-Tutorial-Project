using RedisApp;
using System.Text;

var sub = RedisHelper.GetSubscriber();
Console.WriteLine("Connected");
await sub.SubscribeAsync("codecell-channel", (channel, message) =>
{
    Console.WriteLine($"meeage recived from channel:{channel}");
    Console.WriteLine($"message:{message}");
});

while (true)
{
    Console.WriteLine("write a message and press enter:");
    var message = Console.ReadLine() ?? "";
    if (message.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }
    var content = Encoding.UTF8.GetBytes(message);
    await sub.PublishAsync("redis-channel", content);
    Console.Clear();
}

Console.ReadKey();