using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.tests
{
    public sealed class FakeDisposableClass : IDisposable
    {
        public void Dispose()
        {
            // do nothing 
        }
    }
}
