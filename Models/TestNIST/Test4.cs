using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Models.TestNIST
{
    internal class Test4:ITestNIST
    {
        /// <summary>Test for the Longest Run of Ones in a Block</summary>
        /// <returns></returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "11001100000101010110110001001100111000000000001001001101010100010001001111010110100000001101011111001100111001101101100010110010";
            //int strLength = sequence.Length;

            int blockSize;//Размер блока разбиения
            int k;
            int[] vValues;
            double[] pi;
            double[] pValue = new double[1];
            pValue[0] = -1;
            bool isRandom = false;
            if (SequenceParameters._sequenceLength < 128) return Tuple.Create(pValue, isRandom);
            else if (SequenceParameters._sequenceLength < 6272)
            {
                blockSize = 8;
                k = 3;
                vValues = new int[] { 1, 2, 3, 4 };
                pi = new double[] { 0.2148, 0.3672, 0.2305, 0.1875 };

            }
            else if (SequenceParameters._sequenceLength < 750000)
            {
                blockSize = 128;
                k = 5;
                vValues = new int[] { 4, 5, 6, 7, 8, 9 };
                pi = new double[] { 0.1174, 0.2430, 0.2493, 0.1752, 0.1027, 0.1124 };
            }
            else
            {
                //if (Sequence.Length > 750000)
                blockSize = 10000;
                k = 6;
                vValues = new int[] { 10, 11, 12, 13, 14, 15, 16 };
                pi = new double[] { 0.0882, 0.2092, 0.2483, 0.1933, 0.1208, 0.0675, 0.0727 };
            }
            int blockCount = SequenceParameters._sequenceLength / blockSize;//Количество блоков

            //Частота повторений самой длинной последовательности единиц в блоке
            int max;
            int vLength = vValues.Length;
            int[] freq = new int[vLength];
            for (int i = 0; i < blockCount; i++)
            {
                max = SequenceParameters._sequence.Substring(i * blockSize, blockSize).Split('0').Select(x => x.Length).Max();
                if (max <= vValues[0])
                {
                    freq[0] += 1;
                }
                else if (max >= vValues[vLength - 1])
                {
                    freq[vLength - 1] += 1;
                }
                else
                {
                    for (int j = 1; j < vLength - 1; j++)
                    {
                        if (max == vValues[j])
                        {
                            freq[j] += 1;
                        }
                    }
                }
            }
            //MessageBox.Show(Convert.ToString(freq[0])+" "+ Convert.ToString(freq[1]) + " " + Convert.ToString(freq[2]) + " " + Convert.ToString(freq[3]));
            double xi2 = 0;
            for (int i = 0; i < vLength; i++)
            {
                xi2 += (Math.Pow(freq[i] - blockCount * pi[i], 2)) / (blockCount * pi[i]);

            }
            //MessageBox.Show($"xi2={xi2}, k={k}, blockCount={blockCount}");
            pValue[0] = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(k / 2, xi2 / 2);

            if (pValue[0] > SequenceParameters._alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }
    }
}
