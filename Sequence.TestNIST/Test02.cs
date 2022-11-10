using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.TestNIST
{
    public class Test02:ITestNIST
    {
        /// <summary>Frequency Test within a Block</summary>
        /// <returns>pValue, isRandom</returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000";
            //int blockSize = 10;
            //int strLength = sequence.Length;

            int blockSize = 128;//Размер блока разбиения
            if (SequenceParameters._sequenceLength < 100) blockSize = SequenceParameters._sequenceLength;
            int blockCount = SequenceParameters._sequenceLength / blockSize;//Количество блоков
            double sumElements;
            double pi;
            double piSum = 0;
            for (int i = 0; i < blockCount; i++)
            {
                sumElements = SequenceParameters._sequence.ToCharArray(i * blockSize, blockSize).Where(j => j == '1').Count();
                pi = sumElements / blockSize;
                piSum += Math.Pow(pi - 0.5, 2);

            }
            double xi2 = 4 * blockSize * piSum;
            double[] pValue = new double[1];
            pValue[0] = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(blockCount / 2, xi2 / 2);

            bool isRandom = false;
            if (pValue[0] > SequenceParameters._alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }
    }
}
