using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Shared
{
    public class Cache
    {
        private readonly IDatabase _db;

        public Cache(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase();
        }

        public async Task Add<T>(string key, T obj)
        {
            var json = JsonSerializer.Serialize(obj);
            await _db.StringSetAsync(key, json);
        }

        public async Task<T?> Get<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task<IEnumerable<T>?> GetCollection<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.HasValue)
            {
                return JsonSerializer.Deserialize<T[]>(value) ?? Array.Empty<T>();
            }

            return null;
        }

        public async Task Drop(string key) => await _db.KeyDeleteAsync(key);
    }
}