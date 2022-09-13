using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Models.TestNIST
{
    internal class Test8:ITestNIST
    {
        /// <summary> Overlapping Template Matching Test</summary>
        /// <returns></returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "10111011110010110100011100101110111110000101101001";
            //string pattern = "11";
            //int blockSize = 10;
            //double[] pi = new double[6] { 0.324652, 0.182617, 0.142670, 0.106645, 0.077147, 0.166269 };
            //int strLength = sequence.Length;

            string pattern = "111111111";//Шаблон
            int patternLength = pattern.Length;
            int blockSize = 1032;
            int blockCount = SequenceParameters._sequenceLength / blockSize;

            int[] patternCount = new int[6];
            string line;
            string subBlock;
            int count;
            //MessageBox.Show($"BlockCount={blockCount}");
            for (int i = 0; i < blockCount; i++)
            {
                line = SequenceParameters._sequence.Substring(i * blockSize, blockSize);
                count = 0;
                for (int j = 0; j < blockSize - patternLength + 1; j++)
                {
                    subBlock = line.Substring(j, patternLength);
                    if (subBlock == pattern)
                    {
                        count++;
                    }
                }
                if (count <= 4)
                {
                    patternCount[count]++;
                }
                else
                {
                    patternCount[5]++;
                }
                //MessageBox.Show($"PatternCount[0]={patternCount[0]}, PatternCount[1]={patternCount[1]}, PatternCount[2]={patternCount[2]}, PatternCount[3]={patternCount[3]}, PatternCount[4]={patternCount[4]}, PatternCount[5]={patternCount[5]}");

            }
            //MessageBox.Show($"PatternCount[0]={patternCount[0]}, PatternCount[1]={patternCount[1]}, PatternCount[2]={patternCount[2]}, PatternCount[3]={patternCount[3]}, PatternCount[4]={patternCount[4]}, PatternCount[5]={patternCount[5]}");

            double lamda = (blockSize - patternLength + 1) / Math.Pow(2, patternLength);
            double eta = lamda / 2;
            //MessageBox.Show($"lamda={lamda}, eta={eta}");

            double[] pi = new double[6] { 0.364091, 0.185659, 0.139381, 0.100571, 0.0704323, 0.139865 };

            double xi2 = 0;
            for (int i = 0; i < patternCount.Length; i++)
            {
                xi2 += Math.Pow(patternCount[i] - blockCount * pi[i], 2) / (blockCount * pi[i]);
            }
            //MessageBox.Show($"xi2={xi2}");

            double[] pValue = new double[1];
            pValue[0] = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(5 / 2, xi2 / 2);

            bool isRandom = false;
            if (pValue[0] > SequenceParameters._alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }
    }
}
