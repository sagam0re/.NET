using System;

namespace Tests
{
    public class ClassFixture : IDisposable
    {
        public int PlatformId = 1;
        public int CommandId = 1;
        public ClassFixture() { }

        public void Dispose()
        {
        }
    }
}
