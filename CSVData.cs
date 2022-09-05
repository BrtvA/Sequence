using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sequence
{
    internal class CSVData
    {
        internal protected double deltaTime { get; set; } //Разрешение временного ряда
        internal protected double timeShift { get; set; } //Время сдвига между двумя временными рядами
        internal protected double timeLength { get; set; } //Длина временного ряда
        internal protected double[] intensity { get; set; }
        internal protected int LSB { get; set; } //Количество младших разрядов
        internal protected int numberBit { get; set; } //Количество бит

        internal protected string GetSequence() {

            int startIndex = (int)Math.Truncate(timeShift / deltaTime)+1;//Индекс начала смещённого массива
            int stopIndex = (int)Math.Truncate((timeLength - timeShift) / deltaTime) + 1;//Индекс конца несмещенного массива
            double[] unshiftedArray = CutArray(0, stopIndex-1);//Несмещенный временной массив
            double[] shiftedArray = CutArray(startIndex, intensity.Length - 1);//Смещенный временной массив
            //MessageBox.Show($"Длина массивов: несмещенный {UnshiftedArray.Length}, смещенный {ShiftedArray.Length}");
            double[] normUnshiftedArray = NormalizeArray(unshiftedArray);
            double[] normShiftedArray = NormalizeArray(shiftedArray);
            //MessageBox.Show($"Длина массивов нормированных: несмещенный {NormUnshiftedArray.Length}, смещенный {NormShiftedArray.Length}");
            int[] discreteUnshiftArray = DiscretizeАrray(normUnshiftedArray);
            int[] discreteShiftedArray = DiscretizeАrray(normShiftedArray);
            //MessageBox.Show($"Длина массивов дискретных: несмещенный {NormUnshiftedArray.Length}, смещенный {NormShiftedArray.Length}");
            //Array.Clear(UnshiftedArray);
            //Array.Clear(ShiftedArray);
            return MakeSequence(discreteUnshiftArray, discreteShiftedArray);
        }
        
        //Обрезка массива
        private double[] CutArray(int startIndex,int stopIndex) 
        {
            double [] outputArray = new double[stopIndex-startIndex+1];
            int j = 0;
            for (int i = startIndex; i <= stopIndex; i++) {
                outputArray[j] = intensity[i];
                j++;
            }
            return outputArray;
        }

        //Специальное нормирование массива
        private double[] NormalizeArray(double[] arr) {
            //double[] ArrayMinus = new double[Array.Length];
            double[] normalisationArray = new double[arr.Length];
            double value = arr.Min();
            for (int i = 0; i < arr.Length; i++) { 
                normalisationArray[i] = arr[i]-value;
            }
            value = normalisationArray.Max();
            for (int i = 0; i < normalisationArray.Length; i++) {
                normalisationArray[i]= normalisationArray[i]/value;
            }
            return normalisationArray;
        }

        //Дискретизация массива
        private int[] DiscretizeАrray(double[] arr) {
            int maxD = (int)Math.Pow(2, numberBit);
            int[] discreteArray = new int[arr.Length];
            for (int i=0;i<arr.Length;i++)
            {
                discreteArray[i]=(int)(arr[i] * maxD);
            }
            return discreteArray;
        }

        //Операция XOR
        private string MakeSequence(int[] unshiftArray, int[] shiftedArray) {
            string? line=null;
            StringBuilder seq = new StringBuilder();
            for (int i = 0; i < unshiftArray.Length; i++) {
                line = Convert.ToString(unshiftArray[i] ^ shiftedArray[i], 2);//XOR
                if (line.Length < LSB)
                {
                    line = line.PadLeft(LSB, '0');
                }
                else if (LSB < line.Length)
                {
                    line = line.Substring(line.Length - LSB);
                }
                //seq = string.Concat(seq,line);
                seq.Append(line);
            }
            string strSequence = seq.ToString();
            return strSequence;
        }
    }
}
