using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.TestNIST
{
    public class Test14:ITestNIST
    {
        /// <summary>Random Excursions Test</summary>
        /// <returns></returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "0110110101";
            //int strLength = sequence.Length;

            int[] s = new int[SequenceParameters._sequenceLength + 2];//Partial sums with zero
            s[0] = 0;
            s[SequenceParameters._sequenceLength + 1] = 0;
            int cycleCount = 0; // Количество циклов в массиве s
            for (int i = 1; i < SequenceParameters._sequenceLength + 1; i++)
            {
                s[i] = s[i - 1] + 2 * Int32.Parse(SequenceParameters._sequence[i - 1].ToString()) - 1;
                if (s[i] == 0) cycleCount++;
            }
            cycleCount++;
            //MessageBox.Show($"cycleCount={cycleCount}");

            bool isRandom = false;
            double[] pValue = new double[8] { -1, -1, -1, -1, -1, -1, -1, -1 };
            if (cycleCount < Math.Max(0.005 * Math.Sqrt(SequenceParameters._sequenceLength), 500)) return Tuple.Create(pValue, isRandom);

            int[,] freq = new int[8, cycleCount];//Частота каждого значения в массиве s
            int numCycle = 0;
            int[] x = new int[8] { -4, -3, -2, -1, 1, 2, 3, 4 };//Возможные значения элементов массива s
            for (int i = 1; i < SequenceParameters._sequenceLength + 1; i++)
            {
                if (s[i] != 0)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (s[i] == x[j])
                        {
                            freq[j, numCycle]++;
                            break;
                        }
                    }
                }
                else
                {
                    numCycle++;
                }
            }

            double[,] v = new double[8, 6];
            double sumJ;
            double xi2;
            double oneXi2;
            double[] pi = new double[6];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    for (int k = 0; k < cycleCount; k++)
                    {
                        if (freq[i, k] == j) v[i, j]++;
                    }
                }

                /*
                for (int j = 0; j < cycleCount; j++) 
                {
                    //MessageBox.Show($"freq ={freq[i, j]}");
                    //v[i, freq[i,j]]++;
                }
                */

                sumJ = 0;
                for (int j = 0; j < 6; j++) sumJ += v[i, j];

                xi2 = 0;
                for (int j = 0; j < 6; j++)
                {
                    if (j == 0)
                    {
                        pi[0] = 1 - (1 / (2 * (double)Math.Abs(x[i])));
                    }
                    else if (j > 0 && j < 5)
                    {
                        pi[j] = (1 / (4 * (double)Math.Pow(x[i], 2))) * Math.Pow(pi[0], j - 1);
                    }
                    else
                    {
                        pi[5] = (1 - pi[0]) * Math.Pow(pi[0], 4);
                    }
                    oneXi2 = Math.Pow(v[i, j] - sumJ * pi[j], 2) / (sumJ * pi[j]);
                    if (!double.IsNaN(oneXi2)) xi2 += oneXi2;
                    //MessageBox.Show($"i={i}: oneXi2={oneXi2}, xi2={xi2}");
                }
                //MessageBox.Show($"sumJ={sumJ}, xi2[{i}]={xi2} \n v[i,0]={v[i,0]}, v[i,1]={v[i, 1]}, v[i,2]={v[i, 2]}, v[i,3]={v[i, 3]}, v[i,4]={v[i, 4]}, v[i,5]={v[i, 5]} \n pi[0]={pi[0]}, pi[1]={pi[1]}, pi[2]={pi[2]}, pi[3]={pi[3]}, pi[4]={pi[4]}, pi[5]={pi[5]}");
                pValue[i] = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(5 / 2, xi2 / 2);

            }

            isRandom = true;
            for (int i = 0; i < pValue.Length; i++)
            {
                if (pValue[i] < SequenceParameters._alfa)
                {
                    isRandom = false;
                    break;
                }
            }
            return Tuple.Create(pValue, isRandom);
        }
    }
}
