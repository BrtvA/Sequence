using Sequence.TestNIST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Factories
{
    internal class Test15Factory : TestFactory
    {
        public override ITestNIST GetTestNIST()
        {
            return new Test15();
        }
    }
}
