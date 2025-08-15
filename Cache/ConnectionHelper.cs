using StackExchange.Redis;
using System.Reflection.Metadata.Ecma335;

namespace RedisCacheExample.Cache
{
    public class ConnectionHelper
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        static ConnectionHelper()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(ConfigurationManager.AppSetting["RedisURL"]);
            });
        }

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
