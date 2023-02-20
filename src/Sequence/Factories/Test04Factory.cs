using Sequence.TestNIST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Factories
{
    internal class Test04Factory : TestFactory
    {
        public override ITestNIST GetTestNIST()
        {
            return new Test04();
        }
    }
}
