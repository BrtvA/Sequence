using Sequence.TestNIST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Sequence.Models
{
    internal class SequenceMaker
    {
        private InputModel inputModel;
        private int selectedIndex;

        public SequenceMaker(InputModel inputModel, int selectedIndex)
        {
            this.inputModel = inputModel;
            this.selectedIndex = selectedIndex;
        }

        public bool GetSequence()
        {
            List<double> intensityList = new List<double>();
            double deltaTime = 0;

            try
            {
                using (StreamReader read = new StreamReader(inputModel.Path))
                {
                    if (selectedIndex == 0)
                    {
                        if (!ReadTimeSeries(read, ref intensityList, ref deltaTime)) return false;
                    }
                    else
                    {
                        return ReadSequence(read);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message,
                                "File not found",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            int intensityListCount = intensityList.Count;

            int indexStartUnshifted = 0;
            int indexStartShifted = Math.Abs((int)(Convert.ToDouble(inputModel.TimeShift) / deltaTime) - 1);
            int indexCount = inputModel.IsCustomTimeSeries ? ((int)(Convert.ToDouble(inputModel.TimeSeries - inputModel.TimeShift) / deltaTime) - 1) : (intensityListCount - indexStartShifted);

            if (intensityListCount < (indexStartShifted + indexCount))
            {
                MessageBox.Show("Reduce the length of the time series or time shift",
                                "Invalid input",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            double[] unshiftedArray = new double[indexCount];
            double[] shiftedArray = new double[indexCount];

            CutArray(indexStartUnshifted, indexCount, ref intensityList, ref unshiftedArray);
            CutArray(indexStartShifted, indexCount, ref intensityList, ref shiftedArray);

            NormalizeArray(ref unshiftedArray);
            NormalizeArray(ref shiftedArray);

            int[] discreteUnshiftedArray = new int[indexCount];
            int[] discreteShiftedArray = new int[indexCount];

            DiscretizeАrray(ref discreteUnshiftedArray, ref unshiftedArray);
            DiscretizeАrray(ref discreteShiftedArray, ref shiftedArray);

            MakeSequence(ref discreteUnshiftedArray, ref discreteShiftedArray, indexCount);

            return true;
        }

        public bool ReadSequence(StreamReader read)
        {
            SequenceParameters._sequence = read.ReadLine();
            if (SequenceParameters._sequence == null || !Regex.IsMatch(SequenceParameters._sequence, @"^[0-1]+$"))
            {
                MessageBox.Show("The file must contain only the bit sequence",
                                "Invalid file content",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }
            SequenceParameters._sequenceLength = SequenceParameters._sequence.Length;
            return true;
        }

        public bool ReadTimeSeries(StreamReader read, ref List<double> intensityList, ref double deltaTime)
        {
            string line;
            string[] lineArray = new string[2];
            double[] timeArray = new double[2];

            int i = 0;
            while (!read.EndOfStream)
            {
                line = read.ReadLine();
                if (!Regex.IsMatch(line, @"^[0-9E\-,.]+$"))
                {
                    MessageBox.Show("The file must contain only the time series",
                                    "Invalid file content",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return false;
                }
                lineArray = line.Split(',');
                intensityList.Add(double.Parse(lineArray[1].Replace('.', ',')));
                if (i < 2)
                    timeArray[i] = double.Parse(lineArray[0].Replace('.', ','));
                i++;
            }

            deltaTime = Math.Abs((timeArray[0] - timeArray[1]) * Math.Pow(10, 9));
            return true;
        }

        //Создание последовательности с помощью операции XOR
        private void MakeSequence(ref int[] discreteUnshiftedArray, ref int[] discreteShiftedArray, int indexCount)
        {
            string? line = null;
            StringBuilder seq = new StringBuilder();
            for (int i = 0; i < indexCount; i++)
            {
                line = Convert.ToString(discreteUnshiftedArray[i] ^ discreteShiftedArray[i], 2);//XOR
                if (line.Length < inputModel.NumberLSB)
                {
                    line = line.PadLeft(inputModel.NumberLSB, '0');
                }
                else if (inputModel.NumberLSB < line.Length)
                {
                    line = line.Substring(line.Length - inputModel.NumberLSB);
                }
                //seq = string.Concat(seq,line);
                seq.Append(line);
            }
            SequenceParameters._sequence = seq.ToString();
            SequenceParameters._sequenceLength = SequenceParameters._sequence.Length;
        }

        //Дискретизация массива
        private void DiscretizeАrray(ref int[] discreteArray, ref double[] intensity)
        {
            int maxD = (int)Math.Pow(2, inputModel.BitADC);
            for (int i = 0; i < intensity.Length; i++)
            {
                discreteArray[i] = (int)(intensity[i] * maxD);
            }
        }

        //Специальное нормирование массива
        private void NormalizeArray(ref double[] intensity)
        {
            int arrayLength = intensity.Length;
            double minValue = intensity.Min();
            for(int i = 0; i < arrayLength; i++)
            {
                intensity[i] = intensity[i] - minValue;
            }

            double maxValue = intensity.Max();
            for(int i = 0; i < arrayLength; i++)
            {
                intensity[i] = intensity[i] / maxValue;
            }
        }

        //Обрезка массива
        private void CutArray(int startIndex, int indexCount, ref List<double> intensityList, ref double[] intensity)
        {
            int j = 0;
            for(int i = startIndex; i<=(startIndex+indexCount - 1); i++)
            {
                intensity[j] = intensityList[i];
                j++;
            }
        }
    }
}
