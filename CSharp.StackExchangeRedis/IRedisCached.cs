using System;
using System.Collections;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace CSharp.StackExchangeRedis
{
    public interface IRedisCached
    {
        Lazy<IDatabase> _db { get; }

        Task<T> StringGetAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.None );
        Task<bool> StringSetAsync<T>( RedisKey key,
            T value,
            TimeSpan? expiry = null,
            When when = When.Always,
            CommandFlags flags = CommandFlags.None);
        
        Task<string> StringGetAsync(RedisKey key, CommandFlags flags = CommandFlags.None );
        Task<bool> StringSetAsync( RedisKey key,
            string value,
            TimeSpan? expiry = null,
            When when = When.Always,
            CommandFlags flags = CommandFlags.None);
    }
}