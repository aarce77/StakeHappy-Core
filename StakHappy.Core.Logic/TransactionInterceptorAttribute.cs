using System;
using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;

namespace StakHappy.Core.Logic
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal class TransactionInterceptorAttribute : InterceptAttribute
    {
        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            return request.Context.Kernel.Get<TransactionInterceptor>();
        }
    }
}
