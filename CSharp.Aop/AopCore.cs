using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace CSharp.Aop
{
    public class AopCore : AbstractInterceptorAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                Console.WriteLine("after");
                return context.Invoke(next);
            }
            finally
            {
                Console.WriteLine("over");
            }
        }
    }
}