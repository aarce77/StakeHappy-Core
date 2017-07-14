using System;
using Ninject;

namespace StakHappy.Core.Logic
{
    internal sealed class DependencyResolver
    {
        readonly IKernel _kernal = new StandardKernel();

        /// <summary>
        /// Gets the dependency.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="T1">The object type.</param>
        /// <returns></returns>
        internal T Get<T>(T T1)
        {
            if (T1 != null)
                return T1;

            T1 = _kernal.Get<T>();
            return T1;
        }

        internal T Get<T>()
        {
            return _kernal.Get<T>();
        }

        /// <summary>
        /// Sets the dependency.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">The object type.</param>
        internal void Set<T>(Type t)
        {
            _kernal.Bind<T>().To(t);
        }
    }
}
