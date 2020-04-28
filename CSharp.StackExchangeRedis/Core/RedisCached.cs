using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CSharp.StackExchangeRedis.Core
{
    public class RedisCached : IRedisCached
    {
        public Lazy<IDatabase> _db => new Lazy<IDatabase>(RedisDatabaseManager.GetDataBase());


        public async Task<T> StringGetAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            var keyExists = await _db.Value.KeyExistsAsync(key, flags);
            if (!keyExists) return default;

            var val = await _db.Value.StringGetAsync(key, flags);

            return val == default ? default : JsonConvert.DeserializeObject<T>(val);
        }


        public Task<bool> StringSetAsync<T>(RedisKey key, T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var val = JsonConvert.SerializeObject(value);

            return _db.Value.StringSetAsync(key, val, expiry, when, flags);
        }

        public async Task<string> StringGetAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            var keyExists = await _db.Value.KeyExistsAsync(key, flags);
            if (!keyExists) return default;

            return await _db.Value.StringGetAsync(key, flags);
        }

        public Task<bool> StringSetAsync(RedisKey key, string value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return _db.Value.StringSetAsync(key, value, expiry, when, flags);
        }
    }
}