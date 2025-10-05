using StackExchange.Redis;

namespace discovery_service.infrastructure.Redis
{
    /*
        Handles creation and lifetime of Redis connection
    */
    public static class RedisConnectionManager
    {
        private static Lazy<IConnectionMultiplexer> _lazyConnection = new(() =>
        {
            var config = ConfigurationOptions.Parse("localhost:6379"); // hoặc từ appsettings.json
            config.AbortOnConnectFail = false;
            return ConnectionMultiplexer.Connect(config);
        });

        public static IConnectionMultiplexer Connection => _lazyConnection.Value;
    }
}
