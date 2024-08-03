using System.Security.Principal;

namespace MyNewwRedis.Models
{
    public class RedisModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
