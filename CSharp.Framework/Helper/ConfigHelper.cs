﻿using System;
using System.IO;
using CSharp.Framework.Extensions;
using Microsoft.Extensions.Configuration;

namespace CSharp.Framework.Helper
{
    public static class ConfigHelper
    {
        //private static readonly Dictionary<string, IConfigurationRoot> ConfigMap = new Dictionary<string, IConfigurationRoot>();
        
        /// <summary>
        /// 读取给定路径配置文件Key对应Value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="jsonSettingPath"></param>
        /// <returns></returns>
        public static string Get(string key, string jsonSettingPath = "appsettings.json")
        {
            return GetConfigurationRoot(jsonSettingPath)
                .GetStringValue(key);
        }

        /// <summary>
        /// 读取给定路径配置文件Key对应Value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="jsonSettingPath"></param>
        /// <returns></returns>
        public static T Get<T>(string key, string jsonSettingPath = "appsettings.json") where T : class, new()
        {
            return GetConfigurationRoot(jsonSettingPath)
                .GetBindInstance<T>(key);
        }

        private static IConfigurationRoot GetConfigurationRoot(string jsonSettingPath)
        {
            if (string.IsNullOrEmpty(jsonSettingPath)) throw new ArgumentNullException(nameof(jsonSettingPath));
            var basePath = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(basePath, jsonSettingPath);
            
            //if (ConfigMap.ContainsKey(fullPath)) return ConfigMap[fullPath];
          
            if (!File.Exists(fullPath)) throw new FileNotFoundException(nameof(fullPath));

            var configurationRoot = CreateConfigurationRoot(basePath, jsonSettingPath);

            //ConfigMap.Add(fullPath, configurationRoot);

            return configurationRoot;
        }

        private static IConfigurationRoot CreateConfigurationRoot(string basePath, string jsonSettingPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(jsonSettingPath)
                .Build();
        }
    }
}