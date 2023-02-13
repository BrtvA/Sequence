using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.TestNIST
{
    public class Test01 : ITestNIST
    {
        /// <summary>Frequency (Monobit) Test</summary>
        /// <returns>pValue, isRandom</returns>
        public Tuple<double[], bool> Test()
        {
            double sObs = Math.Abs(GetSn(SequenceParameters._sequence)) / Math.Sqrt(SequenceParameters._sequenceLength);
            double[] pValue = new double[1];
            pValue[0] = MathNet.Numerics.SpecialFunctions.Erfc(sObs / Math.Sqrt(2));
            bool isRandom = false;
            if (pValue[0] > SequenceParameters._alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }

        private static int GetSn(string sequence)
        {
            int s = sequence.ToCharArray().Where(i => i == '1').Count() - sequence.ToCharArray().Where(i => i == '0').Count();
            return s;
        }
    }
}
