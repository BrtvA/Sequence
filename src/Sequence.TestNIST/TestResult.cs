using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.TestNIST
{
    public class TestResult
    {
        //Тесты на случайность выполнены согласно методике Национального Института Стандартных Технологий NIST 800-22
        //https://nvlpubs.nist.gov/nistpubs/legacy/sp/nistspecialpublication800-22r1a.pdf
        public Tuple<double[], bool> Test(ITestNIST testNIST)
        {
            return testNIST.Test();
        }
    }
}
