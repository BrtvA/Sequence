using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.Models.TestNIST
{
    internal class Test15:ITestNIST
    {
        /// <summary>Random Excursions Variant Test</summary>
        /// <returns>pValue, isRandom</returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "0110110101";
            //int strLength = sequence.Length;

            int[] x = new int[18];//Возможные значения элементов массива s
            x[0] = -9;
            for (int i = 1; i < 18; i++)
            {
                if ((x[i - 1] + 1) != 0)
                {
                    x[i] = x[i - 1] + 1;
                }
                else
                {
                    x[i] = 1;
                }
            }

            int[] stateCount = new int[18];//Количество раз когда состояние x происходило во всех циклах

            int[] s = new int[SequenceParameters._sequenceLength + 2];//Partial sums with zero
            s[0] = 0;
            s[SequenceParameters._sequenceLength + 1] = 0;
            int cycleCount = 0; // Количество циклов в массиве s
            for (int i = 1; i < SequenceParameters._sequenceLength + 1; i++)
            {
                s[i] = s[i - 1] + 2 * Int32.Parse(SequenceParameters._sequence[i - 1].ToString()) - 1;
                if (s[i] == 0) cycleCount++;
                for (int j = 0; j < 18; j++)
                {
                    if (s[i] == x[j]) stateCount[j]++;
                }
            }
            cycleCount++;

            bool isRandom = false;
            double[] pValue = new double[18];
            for (int i = 0; i < 18; i++) pValue[i] = -1;
            if (cycleCount < 500) return Tuple.Create(pValue, isRandom);//Если выполняется условие последовательность нерандомна

            for (int i = 0; i < 18; i++)
            {
                pValue[i] = MathNet.Numerics.SpecialFunctions.Erfc(Math.Abs(stateCount[i] - cycleCount) / Math.Sqrt(2 * cycleCount * (4 * Math.Abs(x[i]) - 2)));
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
