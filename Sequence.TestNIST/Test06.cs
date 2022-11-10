using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.TestNIST
{
    public class Test06:ITestNIST
    {
        /// <summary>Discrete Fourier Transform (Spectral) Test</summary>
        /// <returns></returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000";
            //string sequence = "1001010011";
            //int strLength = sequence.Length;

            //Представление в виде -1 и 1
            MathNet.Numerics.Complex32[] s = new MathNet.Numerics.Complex32[SequenceParameters._sequenceLength];
            for (int i = 0; i < SequenceParameters._sequenceLength; i++)
            {
                if (SequenceParameters._sequence[i] == '1')
                {
                    s[i] = new MathNet.Numerics.Complex32(1, 0);
                }
                else
                {
                    s[i] = new MathNet.Numerics.Complex32(-1, 0);
                }
            }
            MathNet.Numerics.IntegralTransforms.Fourier.Forward(s, MathNet.Numerics.IntegralTransforms.FourierOptions.NoScaling);//Быстрое преобразование Фурье

            double t = Math.Sqrt(Math.Log(1 / 0.05) * SequenceParameters._sequenceLength);//95% пороговое значение высоты пика
            double n1 = 0;//Количество пиков в массиве М которые меньше Т

            //Модуль
            double absS;
            for (int i = 0; i < SequenceParameters._sequenceLength / 2 - 1; i++)
            {
                absS = MathNet.Numerics.Complex32.Abs(s[i]);
                if (absS < t) n1++;
            }

            //double t = Math.Sqrt(Math.Log(1 / 0.05) * strLength);//95% пороговое значение высоты пика
            double n0 = 0.95 * (SequenceParameters._sequenceLength / 2);//ожидаемое теоретическое (95 %) число пиков
            //double n1 = absS.Count(x => x < t);//Количество пиков в массиве М которые меньше Т
            double d = (n1 - n0) / Math.Sqrt(SequenceParameters._sequenceLength * 0.95 * 0.05 / 4);
            //MessageBox.Show($"t = {t}, N0 = {n0}, N1={n1}, d = {d}");
            double[] pValue = new double[1];
            pValue[0] = MathNet.Numerics.SpecialFunctions.Erfc(Math.Abs(d) / Math.Sqrt(2));

            bool isRandom = false;
            if (pValue[0] > SequenceParameters._alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }
    }
}
