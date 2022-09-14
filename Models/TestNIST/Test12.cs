using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Models.TestNIST
{
    internal class Test12:ITestNIST
    {
        /// <summary>Approximate Entropy Test</summary>
        /// <returns>pValue, isRandom</returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000";
            //int blockSize = 2;
            //int strLength = sequence.Length;

            int blockSize = 10;
            string newSequence = SequenceParameters._sequence + SequenceParameters._sequence.Substring(0, blockSize + 1);

            int[] v1 = new int[(int)Math.Pow(2, blockSize)];
            int[] v2 = new int[(int)Math.Pow(2, blockSize + 1)];
            string stringBlock;
            int Decimal;
            for (int i = 0; i < SequenceParameters._sequenceLength; i++)
            {
                stringBlock = newSequence.Substring(i, blockSize);
                Decimal = Convert.ToInt32(stringBlock, 2);
                v1[Decimal]++;

                stringBlock = newSequence.Substring(i, blockSize + 1);
                Decimal = Convert.ToInt32(stringBlock, 2);
                v2[Decimal]++;
            }

            double phi1 = 0;
            double c;
            double p;
            for (int i = 0; i < v1.Length; i++)
            {
                c = (double)v1[i] / SequenceParameters._sequenceLength;
                p = c * Math.Log(c);
                if (!double.IsNaN(p)) phi1 += p;
            }
            double phi2 = 0;
            for (int i = 0; i < v2.Length; i++)
            {
                c = (double)v2[i] / SequenceParameters._sequenceLength;
                p = c * Math.Log(c);
                if (!double.IsNaN(p)) phi2 += p;
            }

            double xi2 = 2 * SequenceParameters._sequenceLength * (Math.Log(2) - (phi1 - phi2));

            double[] pValue = new double[1];
            pValue[0] = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(Math.Pow(2, blockSize - 1), xi2 / 2); ;

            bool isRandom = false;
            if (pValue[0] > SequenceParameters._alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }
    }
}
