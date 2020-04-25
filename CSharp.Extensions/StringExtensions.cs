using System;
using System.Diagnostics;

namespace CSharp.Extensions
{
    [DebuggerStepThrough]
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }
    }
}