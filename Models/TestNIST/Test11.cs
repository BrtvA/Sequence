using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Models.TestNIST
{
    internal class Test11:ITestNIST
    {
        /// <summary>Serial Test</summary>
        /// <returns></returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "0011011101";
            //int patternLength = 3;
            //int strLength = sequence.Length;

            int patternLength = 16;
            string newSequence = SequenceParameters._sequence + SequenceParameters._sequence.Substring(0, patternLength - 1);

            int[] v1 = new int[(int)Math.Pow(2, patternLength)];
            int[] v2 = new int[(int)Math.Pow(2, patternLength - 1)];
            int[] v3 = new int[(int)Math.Pow(2, patternLength - 2)];
            string stringBlock;
            int Decimal;
            for (int i = 0; i < SequenceParameters._sequenceLength; i++)
            {
                stringBlock = newSequence.Substring(i, patternLength);
                Decimal = Convert.ToInt32(stringBlock, 2);
                v1[Decimal]++;

                stringBlock = newSequence.Substring(i, patternLength - 1);
                Decimal = Convert.ToInt32(stringBlock, 2);
                v2[Decimal]++;

                stringBlock = newSequence.Substring(i, patternLength - 2);
                Decimal = Convert.ToInt32(stringBlock, 2);
                v3[Decimal]++;
            }
            //MessageBox.Show($"V1[0]={V1[0]}, V1[1]={V1[1]}, V1[2]={V1[2]}, V1[3]={V1[3]}, V1[4]={V1[4]}, V1[5]={V1[5]}, V1[6]={V1[6]}, V1[7]={V1[7]}");
            //MessageBox.Show($"V2[0]={V2[0]}, V2[1]={V2[1]}, V2[2]={V2[2]}, V2[3]={V2[3]}");
            //MessageBox.Show($"V3[0]={V3[0]}, V3[1]={V3[1]}");

            double sumV1 = 0;
            for (int i = 0; i < v1.Length; i++) sumV1 += Math.Pow(v1[i], 2);
            double phi1 = (sumV1 * Math.Pow(2, patternLength) / SequenceParameters._sequenceLength) - SequenceParameters._sequenceLength;
            //MessageBox.Show($"Phi1={Phi1}");
            double sumV2 = 0;
            for (int i = 0; i < v2.Length; i++) sumV2 += Math.Pow(v2[i], 2);
            double phi2 = (sumV2 * Math.Pow(2, patternLength - 1) / SequenceParameters._sequenceLength) - SequenceParameters._sequenceLength;
            //MessageBox.Show($"Phi2={Phi2}");
            double sumV3 = 0;
            for (int i = 0; i < v3.Length; i++) sumV3 += Math.Pow(v3[i], 2);
            double phi3 = (sumV3 * Math.Pow(2, patternLength - 2) / SequenceParameters._sequenceLength) - SequenceParameters._sequenceLength;
            //MessageBox.Show($"Phi3={Phi3}");

            double nabla1 = phi1 - phi2;
            double nabla2 = phi1 - 2 * phi2 + phi3;
            //MessageBox.Show($"Nabla1={nabla1}, Nabla2={nabla2}");

            double[] pValue = new double[2];
            pValue[0] = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(Math.Pow(2, patternLength - 2), nabla1 / 2);
            pValue[1] = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(Math.Pow(2, patternLength - 3), nabla2 / 2);
            //MessageBox.Show($"Pvalue1={pValue1}, Pvalue2={pValue2}");

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
