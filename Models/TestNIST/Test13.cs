using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Models.TestNIST
{
    internal class Test13:ITestNIST
    {
        /// <summary>Cumulative Sums (Cusum) Test</summary>
        /// <returns></returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000";
            //string sequence = "1011010111";
            //int strLength = sequence.Length;

            int sForward = 2 * Int32.Parse(SequenceParameters._sequence[0].ToString()) - 1;
            //MessageBox.Show($"sForward[{0}]={sForward}");
            int sBackward = 2 * Int32.Parse(SequenceParameters._sequence[SequenceParameters._sequenceLength - 1].ToString()) - 1;
            double[] maxS = new double[2];
            int ii = SequenceParameters._sequenceLength - 2;
            for (int i = 1; i < SequenceParameters._sequenceLength; i++)
            {
                sForward += 2 * Int32.Parse(SequenceParameters._sequence[i].ToString()) - 1;
                //MessageBox.Show($"sForward[{i}]={sForward}");
                if (maxS[0] < Math.Abs(sForward)) maxS[0] = Math.Abs(sForward);
                sBackward += 2 * Int32.Parse(SequenceParameters._sequence[ii].ToString()) - 1;
                if (maxS[1] < Math.Abs(sBackward)) maxS[1] = Math.Abs(sBackward);
                ii--;
            }
            //maxS[0] = maxS[0]/strLength;
            //maxS[1] = maxS[1]/strLength;
            //MessageBox.Show($"maxSForward={maxS[0]}, maxSBackward={maxS[1]}");

            double[] pValue = new double[2];//Forward and Backward value
            double sum1;
            double sum2;
            double x1;
            double x2;
            for (int i = 0; i < 2; i++)
            {
                sum1 = 0;
                sum2 = 0;
                for (int j = (int)Math.Floor(0.25 * Math.Floor(-SequenceParameters._sequenceLength / maxS[i]) + 1); j <= (int)Math.Floor(0.25 * Math.Floor(SequenceParameters._sequenceLength / maxS[i]) - 1) + 1; j++)
                {
                    x1 = (4 * j + 1) * maxS[i] / Math.Sqrt(SequenceParameters._sequenceLength);
                    x2 = (4 * j - 1) * maxS[i] / Math.Sqrt(SequenceParameters._sequenceLength);
                    sum1 += MathNet.Numerics.Distributions.Normal.CDF(0, 1, x1) - MathNet.Numerics.Distributions.Normal.CDF(0, 1, x2);
                    //sum1 += CumulativeFunction.CumulativeDistributionFunction(x1) - CumulativeFunction.CumulativeDistributionFunction(x2);
                }
                for (int j = (int)Math.Floor(0.25 * Math.Floor(-SequenceParameters._sequenceLength / maxS[i]) - 3); j <= (int)Math.Floor(0.25 * Math.Floor(SequenceParameters._sequenceLength / maxS[i]) - 1) + 1; j++)
                {
                    x1 = (4 * j + 3) * maxS[i] / Math.Sqrt(SequenceParameters._sequenceLength);
                    x2 = (4 * j + 1) * maxS[i] / Math.Sqrt(SequenceParameters._sequenceLength);
                    sum2 += MathNet.Numerics.Distributions.Normal.CDF(0, 1, x1) - MathNet.Numerics.Distributions.Normal.CDF(0, 1, x2);
                    //sum2 += CumulativeFunction.CumulativeDistributionFunction(x1) - CumulativeFunction.CumulativeDistributionFunction(x2);
                }
                pValue[i] = 1 - sum1 + sum2;
            }

            bool isRandom = true;
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
