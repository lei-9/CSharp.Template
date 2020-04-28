using System;
using CSharp.Framework.Helper;
using CSharp.StackExchangeRedis.Model;
using StackExchange.Redis;

namespace CSharp.StackExchangeRedis.Core
{
    public static class RedisDatabaseManager
    {
        private static ConnectionMultiplexer _connectionMultiplexer;

        private static ConnectionMultiplexer GetConnectionMultiplexer()
        {
            if (_connectionMultiplexer != null) return _connectionMultiplexer;

            var setting = ConfigHelper.Get<RedisConfiguration>("RedisSetting");

            if (setting == null) throw new Exception("Redis配置不存在，请在配置文件种添加RedisSetting");
            
            var endPoint = $"{setting.Host}:{setting.Port ?? 6379}";
            _connectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = {endPoint},
                Password = setting.Password
            });

            return _connectionMultiplexer;
        }

        public static IDatabase GetDataBase(int dbIndex = -1, object asyncState = null)
        {
            var connectionMultiplexer = GetConnectionMultiplexer();
            return connectionMultiplexer.GetDatabase(dbIndex, asyncState);
        }
    }
}