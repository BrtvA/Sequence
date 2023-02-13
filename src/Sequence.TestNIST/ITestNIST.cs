using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.TestNIST
{
    public interface ITestNIST
    {
        Tuple<double[], bool> Test();
    }
}
