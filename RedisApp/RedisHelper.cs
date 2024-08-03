using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisApp
{
    public static class RedisHelper
    {
        static ConfigurationOptions config = new ConfigurationOptions
        {
            EndPoints = { "localhost:6379" },
        };
        static ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(config);
        public static IDatabase GetDatabase()
        {
            return conn.GetDatabase();
        }

        public static ISubscriber GetSubscriber()
        {
            return conn.GetSubscriber();
        }
    }
}