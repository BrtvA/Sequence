using Sequence.TestNIST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Factories
{
    internal class Test14Factory : TestFactory
    {
        public override ITestNIST GetTestNIST()
        {
            return new Test14();
        }
    }
}
