using System;
using System.IO;
using CSharp.CodeGenerator.Enums;
using Microsoft.Extensions.Configuration;

namespace CSharp.CodeGenerator.Core
{
    public class ConnectionManager
    {
        private const string ConnectionStringSettingKey = "ConnectionString";
        private const string DbTypeSettingKey = "DbType";


        private static IConfigurationRoot _config;

        /// <summary>
        /// 配置Root对象
        /// </summary>
        public static IConfigurationRoot Config =>
            _config ??= new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        private static string _connectionString;

        /// <summary>
        /// 数据库连接串
        /// </summary>

        public static string ConnectionString => _connectionString ??= Config[ConnectionStringSettingKey];


        private static DbTypeEnum _dbType = DbTypeEnum.SqlServer;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DbTypeEnum DbType
        {
            get
            {
                var dbType = Config[DbTypeSettingKey];
                if (!string.IsNullOrEmpty(dbType))
                    _dbType = (DbTypeEnum) Convert.ToInt16(dbType);

                return _dbType;
            }
        }
    }
}