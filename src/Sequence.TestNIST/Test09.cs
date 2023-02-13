using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.TestNIST
{
    public class Test09:ITestNIST
    {
        /// <summary> Maurer's "Universal Statistical" Test</summary>
        /// <returns></returns>
        public Tuple<double[], bool> Test()
        {
            //string Sequence = "01011010011101010111";
            //int BlockSize = 2;
            //int BlockCountQ = 4;
            //int strLength = sequence.Length;

            int blockSize = 5;//Длина блока разбиения
            bool isRandom = false;
            double[] pValue = new double[1];
            pValue[0] = -1;
            if (SequenceParameters._sequenceLength < 387840) return Tuple.Create(pValue, isRandom);
            if (SequenceParameters._sequenceLength >= 387840) blockSize = 6;
            if (SequenceParameters._sequenceLength >= 904960) blockSize = 7;
            if (SequenceParameters._sequenceLength >= 2068480) blockSize = 8;
            if (SequenceParameters._sequenceLength >= 4654080) blockSize = 9;
            if (SequenceParameters._sequenceLength >= 10342400) blockSize = 10;
            if (SequenceParameters._sequenceLength >= 22753280) blockSize = 11;
            if (SequenceParameters._sequenceLength >= 49643520) blockSize = 12;
            if (SequenceParameters._sequenceLength >= 107560960) blockSize = 13;
            if (SequenceParameters._sequenceLength >= 231669760) blockSize = 14;
            if (SequenceParameters._sequenceLength >= 496435200) blockSize = 15;
            if (SequenceParameters._sequenceLength >= 1059061760) blockSize = 16;

            int blockCountQ = 10 * (int)Math.Pow(2, blockSize);//Количество блоков инициализации
            int blockNum = (int)(SequenceParameters._sequenceLength / blockSize);
            int blockCountK = blockNum - blockCountQ;//Количество тестовых блоков
            //MessageBox.Show($"StrLength={StrLength}, BlockNum={BlockNum}, BlockCountQ={BlockCountQ}, BlockCountK={BlockCountK}");

            string stringBlock;
            int Decimal;
            double logSum = 0;
            int[] t = new int[(int)Math.Pow(2, blockSize)];
            for (int i = 0; i < blockNum; i++)
            {
                stringBlock = SequenceParameters._sequence.Substring(i * blockSize, blockSize);
                Decimal = Convert.ToInt32(stringBlock, 2);

                if (i < blockCountQ)
                {
                    t[Decimal] = i + 1;
                }
                else
                {
                    logSum += Math.Log2(i + 1 - t[Decimal]);
                    t[Decimal] = i + 1;
                }
            }

            double fN = logSum / blockCountK;
            //MessageBox.Show($"LogSum={LogSum}, Fn={Fn}");

            double expectedValue = 0;
            double variance = 0;
            switch (blockSize)
            {
                case 6:
                    expectedValue = 5.2177052;
                    variance = 2.954;
                    break;
                case 7:
                    expectedValue = 6.1962507;
                    variance = 3.125;
                    break;
                case 8:
                    expectedValue = 7.1836656;
                    variance = 3.238;
                    break;
                case 9:
                    expectedValue = 8.1764248;
                    variance = 3.311;
                    break;
                case 10:
                    expectedValue = 9.1723243;
                    variance = 3.356;
                    break;
                case 11:
                    expectedValue = 10.170032;
                    variance = 3.384;
                    break;
                case 12:
                    expectedValue = 11.168765;
                    variance = 3.401;
                    break;
                case 13:
                    expectedValue = 12.168070;
                    variance = 3.410;
                    break;
                case 14:
                    expectedValue = 13.167693;
                    variance = 3.416;
                    break;
                case 15:
                    expectedValue = 14.167488;
                    variance = 3.419;
                    break;
                case 16:
                    expectedValue = 15.167379;
                    variance = 3.421;
                    break;
                default:
                    //ExpectedValue = 1.5374383;
                    expectedValue = 0;
                    variance = 0;
                    break;
            }

            double c = 0.7 - (0.8 / blockSize) + (4 + 32 / blockSize) * (Math.Pow(blockSize, -3 / blockSize) / 15);
            double sigma = c * Math.Sqrt(variance / blockCountK);//Стандартное теоретическое отклонение

            pValue[0] = MathNet.Numerics.SpecialFunctions.Erfc(Math.Abs((fN - expectedValue) / (Math.Sqrt(2) * sigma)));

            if (pValue[0] > SequenceParameters._alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }
    }
}
