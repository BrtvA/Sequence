using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Models.TestNIST
{
    internal class Test7:ITestNIST
    {
        /// <summary>Non-overlapping Template Matching Test</summary>
        /// <returns></returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "1010010010 1110010110";
            //string pattern = "001";
            //int blockCount = 2;
            //int strLength = sequence.Length;

            string pattern = "000000001";//Шаблон
            int patternLength = pattern.Length;
            int blockCount = 8;
            int blockSize = SequenceParameters._sequenceLength / blockCount;

            string line;
            string subBlock;
            int count;
            int[] patternCount = new int[blockCount];

            for (int i = 0; i < blockCount; i++)
            {
                count = 0;
                line = SequenceParameters._sequence.Substring(i * blockSize, blockSize);
                while (count < blockSize - patternLength + 1)
                {
                    subBlock = line.Substring(count, patternLength);
                    if (subBlock == pattern)
                    {
                        patternCount[i]++;
                        count += patternLength;
                    }
                    else
                    {
                        count++;
                    }
                }
            }

            double mean = (blockSize - patternLength + 1) / Math.Pow(2, patternLength);//Теоретическое среднее
            double variance = blockSize * ((1 / Math.Pow(2, patternLength)) - ((2 * patternLength - 1) / (Math.Pow(2, 2 * patternLength))));//Теоретическое отклонение

            double xi2 = 0;
            for (int i = 0; i < blockCount; i++)
            {
                xi2 += Math.Pow(patternCount[i] - mean, 2) / variance;
            }

            double[] pValue = new double[1];
            pValue[0] = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(blockCount / 2, xi2 / 2);

            bool isRandom = false;
            if (pValue[0] > SequenceParameters._alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }
    }
}

