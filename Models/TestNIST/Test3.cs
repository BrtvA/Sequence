using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Models.TestNIST
{
    internal class Test3:ITestNIST
    {
        /// <summary>Runs Test</summary>
        /// <returns></returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000";
            //int strLength = sequence.Length;

            double pi = SequenceParameters._sequence.ToCharArray().Where(i => i == '1').Count();
            pi = pi / SequenceParameters._sequenceLength;
            double r = 0;
            double vN;
            double[] pValue = new double[1];
            if ((Math.Abs(pi - 0.5)) <= (2 / Math.Sqrt(2)))
            {
                for (int i = 0; i < SequenceParameters._sequenceLength - 1; i++)
                {
                    r = SequenceParameters._sequence[i] == SequenceParameters._sequence[i + 1] ? r += 0 : r += 1;
                }
                vN = r + 1;
                pValue[0] = MathNet.Numerics.SpecialFunctions.Erfc(Math.Abs(vN - 2 * SequenceParameters._sequenceLength * pi * (1 - pi)) / (2 * Math.Pow(2 * SequenceParameters._sequenceLength, 0.5) * pi * (1 - pi)));
            }
            else
            {
                pValue[0] = 0;
            }

            bool isRandom = false;
            if (pValue[0] > SequenceParameters._alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }
    }
}
