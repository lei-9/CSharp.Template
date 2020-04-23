﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

namespace CSharp.Framework.Extensions
{
    [DebuggerStepThrough]
    public sealed class ExcelDynamicObject : DynamicObject
    {
        private Dictionary<string, object> Map = new Dictionary<string, object>();

        public IEnumerable<string> GetAllKeys()
        {
            return Map.Keys;
        }
        public object GetValue(string key)
        {
            return Map.ContainsKey(key) ? Map[key] : default;
        }
        
        public void AddProperty(string key, object value)
        {
            Map.Add(key, value);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (string.IsNullOrEmpty(binder.Name))
            {
                throw new Exception("The dictionary key cannot be empty.");
            }

            Map[binder.Name] = value;

            return true;
            //return base.TrySetMember(binder, value);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (string.IsNullOrEmpty(binder.Name) && !Map.ContainsKey(binder.Name)) throw new Exception("Dictionaries don't exist key！");
            
            result = Map[binder.Name];
            return true;

            //return base.TryGetMember(binder, out result);
        }

        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            if (!string.IsNullOrEmpty(binder.Name) || Map.ContainsKey(binder.Name))
            {
                Map.Remove(binder.Name);
            }

            return true;
            //return base.TryDeleteMember(binder);
        }
    }
}