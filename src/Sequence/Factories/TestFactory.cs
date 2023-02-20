using Sequence.TestNIST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Factories
{
    internal abstract class TestFactory
    {
        public abstract ITestNIST GetTestNIST();
    }
}
