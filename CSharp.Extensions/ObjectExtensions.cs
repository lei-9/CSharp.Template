using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CSharp.Extensions
{
    //[DebuggerStepThrough]
    public static class ObjectExtensions
    {
        private static readonly Dictionary<string, PropertyInfo[]> ModelPropertyInfoMap = new Dictionary<string, PropertyInfo[]>();

        private static readonly Dictionary<string, string> FieldDescMap = new Dictionary<string, string>();

        public static PropertyInfo[] GetPropertyInfos(this object target)
        {
            var key = target.GetType().ToString();
            if (ModelPropertyInfoMap.ContainsKey(key)) return ModelPropertyInfoMap[key];

            var properties = target.GetType().GetProperties();
            ModelPropertyInfoMap.Add(key, properties);
            return properties;
        }

        public static string GetDesc(this object target, string fieldName)
        {
            var key = $"{target.GetType().ToString()}.{fieldName}";
            if (FieldDescMap.ContainsKey(key)) return FieldDescMap[key];

            var descAttr = GetPropertyInfos(target)
                .FirstOrDefault(f => f.Name == fieldName)
                ?.GetCustomAttributes(typeof(DescriptionAttribute), false);

            var desc = descAttr?.Any() ?? false ? ((DescriptionAttribute) descAttr[0]).Description : null;

            FieldDescMap.Add(key, desc);

            return desc;
        }
    }
}