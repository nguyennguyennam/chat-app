using StackExchange.Redis;

namespace discovery_service.infrastructure.Redis
{
    /*
        This class manages the connection or usage count for service instances using Redis as the backend.
        Key format in Redis: service:{serviceName}:conn:{instanceId}
    */
    public class ServiceCount
    {
        private readonly IDatabase _db;
        private const int ExpirationMinutes = 5; // Optional TTL to auto-expire unused keys

        public ServiceCount(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        private static string BuildKey(string serviceName, string instanceId)
            => $"service:{serviceName}:conn:{instanceId}";

        /*
            Get the current connection count of a specific service instance.
        */
        public async Task<long> GetCountAsync(string serviceName, string instanceId)
        {
            string key = BuildKey(serviceName, instanceId);
            var value = await _db.StringGetAsync(key);
            return value.HasValue ? (long)value : 0;
        }

        /*
            Increment (increase) the connection count by 1.
            Used when the instance is selected for handling a new request.
        */
        public async Task<long> IncrementAsync(string serviceName, string instanceId)
        {
            string key = BuildKey(serviceName, instanceId);
            var count = await _db.StringIncrementAsync(key);
            await _db.KeyExpireAsync(key, TimeSpan.FromMinutes(ExpirationMinutes));
            return count;
        }

        /*
            Decrement (reduce) the connection count by 1.
            Used when a request finishes or connection closes.
        */
        public async Task<long> DecrementAsync(string serviceName, string instanceId)
        {
            string key = BuildKey(serviceName, instanceId);
            var count = await _db.StringDecrementAsync(key);
            if (count < 0)
            {
                // Avoid negative numbers, reset if needed
                await ResetAsync(serviceName, instanceId);
                count = 0;
            }
            return count;
        }

        /*
            Reset the connection count for a service instance (delete its key).
        */
        public async Task ResetAsync(string serviceName, string instanceId)
        {
            string key = BuildKey(serviceName, instanceId);
            await _db.KeyDeleteAsync(key);
        }

        /*
            (Optional) Get all connection counts for all instances of a service.
            Useful for debugging or monitoring load distribution.
        */
        public async Task<Dictionary<string, long>> GetAllCountsAsync(string serviceName)
        {
            var endpoints = _db.Multiplexer.GetEndPoints();
            var server = _db.Multiplexer.GetServer(endpoints.First());
            var results = new Dictionary<string, long>();

            foreach (var key in server.Keys(pattern: $"service:{serviceName}:conn:*"))
            {
                var value = await _db.StringGetAsync(key);
                results[key.ToString()] = value.HasValue ? (long)value : 0;
            }

            return results;
        }
    }
}
