﻿using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace CSharp.Framework.Extensions
{
    [DebuggerStepThrough]
    public static class IConfigurationRootExtensions
    {
        public static T GetBindInstance<T>(this IConfigurationRoot configurationRoot, string key) where T : class, new()
        {
            return configurationRoot
                .GetSection(key)
                .Get<T>();
        }

        public static string GetStringValue(this IConfigurationRoot configurationRoot, string key)
        {
            return configurationRoot[key];
        }
    }
}