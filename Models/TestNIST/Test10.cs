using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Models.TestNIST
{
    internal class Test10:ITestNIST
    {
        /// <summary>Linear Complexity Test</summary>
        /// <returns>pValue, isRandom</returns>
        public Tuple<double[], bool> Test()
        {
            //int strLength = sequence.Length;

            int degreeFreedom = 6;//Степень свободы
            double[] pi = new double[7] { 0.01047, 0.03125, 0.125, 0.5, 0.25, 0.0625, 0.020833 };
            int blockSize = 500;
            int blockCount = SequenceParameters._sequenceLength / blockSize;

            bool isRandom = false;
            double[] pValue = new double[1];
            pValue[0] = -1;
            if (blockCount < 1) Tuple.Create(pValue, isRandom);

            double mean = (blockSize / 2) + ((9 + Math.Pow(-1, blockSize + 1)) / 36) - (((blockSize / 3) + (2 / 9)) / Math.Pow(2, blockSize));//Теоретическое среднее

            string stringBlock;
            int complexities;
            double t;
            int[] v = new int[7];
            for (int i = 0; i < blockCount; i++)
            {
                stringBlock = SequenceParameters._sequence.Substring(i * blockSize, blockSize);
                complexities = BerlekampMasseyAlgoritm(stringBlock);
                t = Math.Pow(-1, blockSize) * (complexities - mean) + (2 / 9);

                if (t <= -2.5) v[0]++;
                if ((t <= -1.5) && (t > -2.5)) v[1]++;
                if ((t <= -0.5) && (t > -1.5)) v[2]++;
                if ((t <= 0.5) && (t > -0.5)) v[3]++;
                if ((t <= 1.5) && (t > 0.5)) v[4]++;
                if ((t <= 2.5) && (t > 1.5)) v[5]++;
                if (t > 2.5) v[6]++;
            }

            double xi2 = 0;
            for (int i = 0; i < 7; i++)
            {
                xi2 += Math.Pow(v[i] - blockCount * pi[i], 2) / (blockCount * pi[i]);
            }

            pValue[0] = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(degreeFreedom / 2, xi2 / 2);

            if (pValue[0] > SequenceParameters._alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }

        private static int BerlekampMasseyAlgoritm(string stringBlock)
        {
            int L, N, m, d;
            int n = stringBlock.Length;
            char[] charBlock = stringBlock.ToCharArray();
            int[] b = new int[n];
            int[] c = new int[n];
            int[] t = new int[n];

            c[0] = b[0] = 1;
            N = L = 0;
            m = -1;

            while (N < n)
            {
                d = int.Parse(charBlock[N].ToString());
                for (int i = 1; i <= L; i++)
                    d ^= c[i] & int.Parse(charBlock[N - i].ToString());//(d+=c[i]*s[N-i] mod 2)
                if (d == 1)
                {
                    Array.Copy(c, t, n);    //T(D)<-C(D)
                    for (int i = 0; (i + N - m) < n; i++)
                        c[i + N - m] ^= b[i];
                    if (L <= (N >> 1))
                    {
                        L = N + 1 - L;
                        m = N;
                        Array.Copy(t, b, n);//B(D)<-T(D)
                    }
                }
                N++;
            }
            return L;
        }
    }
}
