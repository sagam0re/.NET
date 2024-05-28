using System;

namespace Tests
{
    public class ClassFixture : IDisposable
    {
        public int PlatformId = new Random().Next(1,10);
        public int CommandId = new Random().Next(1, 10);
        public ClassFixture() { }

        public void Dispose()
        {
        }
    }
}
