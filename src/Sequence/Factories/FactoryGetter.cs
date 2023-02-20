using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Factories
{
    internal class FactoryGetter
    {
        public static TestFactory GetFactory(int testType)
        {
            switch (testType)
            {
                case 0:
                    return new Test01Factory();
                case 1:
                    return new Test02Factory();
                case 2:
                    return new Test03Factory();
                case 3:
                    return new Test04Factory();
                case 4:
                    return new Test05Factory();
                case 5:
                    return new Test06Factory();
                case 6:
                    return new Test07Factory();
                case 7:
                    return new Test08Factory();
                case 8:
                    return new Test09Factory();
                case 9:
                    return new Test10Factory();
                case 10:
                    return new Test11Factory();
                case 11:
                    return new Test12Factory();
                case 12:
                    return new Test13Factory();
                case 13:
                    return new Test14Factory();
                case 14:
                    return new Test15Factory();
            }
            return null;
        }
    }
}
