using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence.TestNIST
{
    public class Test05:ITestNIST
    {
        /// <summary>Binary Matrix Rank Test</summary>
        /// <returns></returns>
        public Tuple<double[], bool> Test()
        {
            //string sequence = "01011001001010101101";
            //int M = 3;
            //int Q = 3;
            //int strLength = sequence.Length;

            int M = 32;//Количество столбцов матрицы
            int Q = 32;//Количество строк матрицы
            int blockSize = M * Q;//Размер блока разбиения
            int blockCount = SequenceParameters._sequenceLength / blockSize;//Количество блоков
            //MessageBox.Show($"blockCount={blockCount}, blockSize={blockSize}");

            double[] pValue = new double[1];
            pValue[0] = -1;
            bool isRandom = false;
            if (SequenceParameters._sequenceLength < 38 * blockSize) return Tuple.Create(pValue, isRandom);

            string strMatrix;
            int[,] numMatrix = new int[Q, M];
            //int fM = 0;//Количество матриц с максимальным рангом
            //int fM_1 = 0;//Количество матриц с предмаксимальным рангом
            int[] fM = new int[3];//Количество матриц
            //int[] rank = new int[blockCount];
            int rank;

            for (int i = 0; i < blockCount; i++)
            {
                strMatrix = SequenceParameters._sequence.Substring(i * blockSize, blockSize);

                //Преобразование строки в массив
                for (int j = 0; j < Q; j++)
                {
                    for (int k = 0; k < M; k++)
                    {
                        numMatrix[j, k] = Int32.Parse(strMatrix[j * M + k].ToString());
                    }
                }

                //MessageBox.Show($"{numMatrix[0,0]} {numMatrix[0, 1]} {numMatrix[0, 2]} \n {numMatrix[1, 0]} {numMatrix[1, 1]} {numMatrix[1, 2]} \n {numMatrix[2, 0]} {numMatrix[2, 1]} {numMatrix[2, 2]}");

                //Прямой ход
                for (int k = 0; k < Q - 1; k++)
                {
                    if (numMatrix[k, k] == 0)
                    {
                        for (int m = k + 1; m < Q; m++)
                        {
                            if (numMatrix[m, k] == 1)
                            {
                                numMatrix = SwapRowArray(k, m, numMatrix);
                                if (k > 0) k--;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int m = k + 1; m < Q; m++)
                        {
                            if (numMatrix[m, k] == 1)
                            {
                                numMatrix = XorRowArray(k, m, numMatrix);
                            }
                        }
                    }
                }

                //Обратный ход
                for (int k = Q - 1; k > 0; k--)
                {
                    if (numMatrix[k, k] == 0)
                    {
                        for (int m = k - 1; m >= 0; m--)
                        {
                            if (numMatrix[m, k] == 1)
                            {
                                numMatrix = SwapRowArray(k, m, numMatrix);
                                if (k < Q - 1) k++;
                                break;
                            }
                        }

                    }
                    else
                    {
                        for (int m = k - 1; m >= 0; m--)
                        {
                            if (numMatrix[m, k] == 1)
                            {
                                numMatrix = XorRowArray(k, m, numMatrix);
                            }
                        }

                    }
                }

                //Определение ранга матрицы
                rank = 0;
                for (int k = 0; k < Q; k++)
                {
                    for (int m = 0; m < M; m++)
                    {
                        if (numMatrix[k, m] == 1)
                        {
                            rank += 1;
                            break;
                        }
                    }
                }

                if (rank == Q)
                {
                    fM[0]++;
                }
                else if (rank == Q - 1)
                {
                    fM[1]++;
                }
                else
                {
                    fM[2]++;
                }

                //MessageBox.Show($"rank={rank}");   
            }

            //fM = rank.Where(i => i == rank.Max()).Count();
            //fM_1 = rank.Where(i => i == (rank.Max()-1)).Count();

            //double xi2 = Math.Pow(fM - 0.2888 * (double)blockCount, 2) / (0.2888 * (double)blockCount) + Math.Pow(fM_1 - 0.5776 * (double)blockCount, 2) / (0.5776 * (double)blockCount) + Math.Pow((double)blockCount - fM - fM_1 - 0.1336 * (double)blockCount, 2) / (0.1336 * (double)blockCount);
            double[] pi = new double[3] { 0.2888, 0.5776, 0.1336 };
            double xi2 = 0;
            for (int i = 0; i < 3; i++)
            {
                xi2 += Math.Pow(fM[i] - pi[i] * (double)blockCount, 2) / (pi[i] * (double)blockCount);
            }
            //MessageBox.Show($"fM={fM[0]}, fM_1={fM[1]}, fM_2={fM[2]}, xi2={xi2}");

            pValue[0] = Math.Exp(-xi2 / 2);
            //pValue = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(1, xi2 / 2);
            //MessageBox.Show($"xi2={xi2}");

            if (pValue[0] > SequenceParameters._alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }

        private static int[,] XorRowArray(int thisIndex, int newIndex, int[,] array)
        {
            for (int i = 0; i < array.GetLength(1); i++)
            {
                array[newIndex, i] = array[thisIndex, i] ^ array[newIndex, i];
            }
            return array;
        }

        private static int[,] SwapRowArray(int thisIndex, int newIndex, int[,] array)
        {
            int thisElement;
            int newElement;
            for (int i = 0; i < array.GetLength(1); i++)
            {
                thisElement = array[thisIndex, i];
                newElement = array[newIndex, i];
                array[thisIndex, i] = newElement;
                array[newIndex, i] = thisElement;
            }
            return array;
        }
    }
}

