using System;
using Rhino.Mocks;

namespace StakHappy.Core.UnitTest
{
    public abstract class TestBase : IDisposable
    {
        internal MockRepository Mocks;

        protected TestBase()
        {
            Mocks = new MockRepository();
        }

        public void Dispose()
        {
            Mocks.VerifyAll();
        }
    }
}
