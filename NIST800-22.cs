using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sequence
{
    internal class NIST800_22
    {
        //Тесты на случайность выполнены согласно методике Национального Института Стандартных Технологий NIST 800-22
        //https://nvlpubs.nist.gov/nistpubs/legacy/sp/nistspecialpublication800-22r1a.pdf

        internal protected static string sequence { get; set; } //битовая последовательность
        internal protected static int strLength { get; set; } //длина битовой последовательности
        private static readonly double alfa = 0.01; //уровень значимости

        /// <summary>
        /// Frequency (Monobit) Test
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double, bool> Test1()
        {
            //int strLength = sequence.Length;

            double sObs = Math.Abs(GetSn(sequence)) / Math.Sqrt(strLength);
            double pValue = MathNet.Numerics.SpecialFunctions.Erfc(sObs / Math.Sqrt(2));
            bool isRandom = false;
            if (pValue > alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }

        /// <summary>
        /// Frequency Test within a Block
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double, bool> Test2()
        {
            //string sequence = "1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000";
            //int blockSize = 10;
            //int strLength = sequence.Length;

            int blockSize = 128;//Размер блока разбиения
            if (strLength < 100) blockSize = strLength;
            int blockCount = strLength / blockSize;//Количество блоков
            double sumElements;
            double pi;
            double piSum = 0;
            for (int i = 0; i < blockCount; i++)
            {
                sumElements = sequence.ToCharArray(i * blockSize, blockSize).Where(j => j == '1').Count();
                pi = sumElements / blockSize;
                piSum += Math.Pow(pi - 0.5, 2);

            }
            double xi2 = 4 * blockSize * piSum;
            double pValue = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(blockCount / 2, xi2 / 2);

            bool isRandom = false;
            if (pValue > alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }

        /// <summary>
        /// Runs Test
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double, bool> Test3()
        {
            //string sequence = "1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000";
            //int strLength = sequence.Length;

            double pi = sequence.ToCharArray().Where(i => i == '1').Count();
            pi = pi / strLength;
            double r = 0;
            double vN;
            double pValue;
            if ((Math.Abs(pi - 0.5)) <= (2 / Math.Sqrt(2)))
            {
                for (int i = 0; i < strLength - 1; i++)
                {
                    r = sequence[i] == sequence[i + 1] ? r += 0 : r += 1;
                }
                vN = r + 1;
                pValue = MathNet.Numerics.SpecialFunctions.Erfc(Math.Abs(vN - 2 * strLength * pi * (1 - pi)) / (2 * Math.Pow(2 * strLength, 0.5) * pi * (1 - pi)));
            } else
            {
                pValue = 0;
            }

            bool isRandom = false;
            if (pValue > alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }

        /// <summary>
        /// Test for the Longest Run of Ones in a Block
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double, bool> Test4()
        {
            //string sequence = "11001100000101010110110001001100111000000000001001001101010100010001001111010110100000001101011111001100111001101101100010110010";
            //int strLength = sequence.Length;

            int blockSize;//Размер блока разбиения
            int k;
            int[] vValues;
            double[] pi;
            double pValue = -1;
            bool isRandom = false;
            if (strLength < 128) return Tuple.Create(pValue, isRandom);
            else if (strLength < 6272)
            {
                blockSize = 8;
                k = 3;
                vValues = new int[] { 1, 2, 3, 4 };
                pi = new double[] { 0.2148, 0.3672, 0.2305, 0.1875 };

            } else if (strLength < 750000)
            {
                blockSize = 128;
                k = 5;
                vValues = new int[] { 4, 5, 6, 7, 8, 9 };
                pi = new double[] { 0.1174, 0.2430, 0.2493, 0.1752, 0.1027, 0.1124 };
            }
            else
            {
                //if (Sequence.Length > 750000)
                blockSize = 10000;
                k = 6;
                vValues = new int[] { 10, 11, 12, 13, 14, 15, 16 };
                pi = new double[] { 0.0882, 0.2092, 0.2483, 0.1933, 0.1208, 0.0675, 0.0727 };
            }
            int blockCount = strLength / blockSize;//Количество блоков

            //Частота повторений самой длинной последовательности единиц в блоке
            int max;
            int vLength = vValues.Length;
            int[] freq = new int[vLength];
            for (int i = 0; i < blockCount; i++)
            {
                max = sequence.Substring(i * blockSize, blockSize).Split('0').Select(x => x.Length).Max();
                if (max <= vValues[0])
                {
                    freq[0] += 1;
                } else if (max >= vValues[vLength - 1])
                {
                    freq[vLength - 1] += 1;
                }
                else
                {
                    for (int j = 1; j < vLength - 1; j++)
                    {
                        if (max == vValues[j])
                        {
                            freq[j] += 1;
                        }
                    }
                }
            }
            //MessageBox.Show(Convert.ToString(freq[0])+" "+ Convert.ToString(freq[1]) + " " + Convert.ToString(freq[2]) + " " + Convert.ToString(freq[3]));
            double xi2 = 0;
            for (int i = 0; i < vLength; i++)
            {
                xi2 += (Math.Pow(freq[i] - blockCount * pi[i], 2)) / (blockCount * pi[i]);

            }
            //MessageBox.Show($"xi2={xi2}, k={k}, blockCount={blockCount}");
            pValue = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(k / 2, xi2 / 2);

            if (pValue > alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }

        /// <summary>
        /// Binary Matrix Rank Test
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double, bool> Test5()
        {
            //string sequence = "01011001001010101101";
            //int M = 3;
            //int Q = 3;
            //int strLength = sequence.Length;

            int M = 32;//Количество столбцов матрицы
            int Q = 32;//Количество строк матрицы
            int blockSize = M * Q;//Размер блока разбиения
            int blockCount = strLength / blockSize;//Количество блоков
            //MessageBox.Show($"blockCount={blockCount}, blockSize={blockSize}");

            double pValue = -1;
            bool isRandom = false;
            if (strLength < 38*blockSize) return Tuple.Create(pValue, isRandom);
   
            string strMatrix;
            int[,] numMatrix = new int[Q, M];
            //int fM = 0;//Количество матриц с максимальным рангом
            //int fM_1 = 0;//Количество матриц с предмаксимальным рангом
            int[] fM = new int[3];//Количество матриц
            //int[] rank = new int[blockCount];
            int rank;

            for (int i = 0; i < blockCount; i++)
            {
                strMatrix = sequence.Substring(i * blockSize, blockSize);

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
            for (int i = 0; i<3; i++)
            {
                xi2 += Math.Pow(fM[i] - pi[i] * (double)blockCount, 2) / (pi[i] * (double)blockCount);
            }
            //MessageBox.Show($"fM={fM[0]}, fM_1={fM[1]}, fM_2={fM[2]}, xi2={xi2}");

            pValue = Math.Exp(-xi2 / 2);
            //pValue = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(1, xi2 / 2);
            //MessageBox.Show($"xi2={xi2}");

            if (pValue > alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);

        }

        /// <summary>
        /// Discrete Fourier Transform (Spectral) Test
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double, bool> Test6()
        {
            //string sequence = "1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000";
            //string sequence = "1001010011";
            //int strLength = sequence.Length;

            //Представление в виде -1 и 1
            MathNet.Numerics.Complex32[] s = new MathNet.Numerics.Complex32[strLength];
            for (int i = 0; i < strLength; i++)
            {
                if (sequence[i] == '1')
                {
                    s[i] = new MathNet.Numerics.Complex32(1, 0);
                }
                else
                {
                    s[i] = new MathNet.Numerics.Complex32(-1, 0);
                }
            }
            MathNet.Numerics.IntegralTransforms.Fourier.Forward(s, MathNet.Numerics.IntegralTransforms.FourierOptions.NoScaling);//Быстрое преобразование Фурье

            double t = Math.Sqrt(Math.Log(1 / 0.05) * strLength);//95% пороговое значение высоты пика
            double n1 = 0;//Количество пиков в массиве М которые меньше Т

            //Модуль
            double absS;
            for (int i = 0; i < strLength / 2 - 1; i++)
            {
                absS = MathNet.Numerics.Complex32.Abs(s[i]);
                if (absS < t) n1++;
            }

            //double t = Math.Sqrt(Math.Log(1 / 0.05) * strLength);//95% пороговое значение высоты пика
            double n0 = 0.95 * (strLength / 2);//ожидаемое теоретическое (95 %) число пиков
            //double n1 = absS.Count(x => x < t);//Количество пиков в массиве М которые меньше Т
            double d = (n1 - n0) / Math.Sqrt(strLength * 0.95 * 0.05 / 4);
            //MessageBox.Show($"t = {t}, N0 = {n0}, N1={n1}, d = {d}");
            double pValue = MathNet.Numerics.SpecialFunctions.Erfc(Math.Abs(d) / Math.Sqrt(2));

            bool isRandom = false;
            if (pValue > alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);

        }

        /// <summary>
        /// Non-overlapping Template Matching Test
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double, bool> Test7()
        {
            //string sequence = "1010010010 1110010110";
            //string pattern = "001";
            //int blockCount = 2;
            //int strLength = sequence.Length;

            string pattern = "000000001";//Шаблон
            int patternLength = pattern.Length;
            int blockCount = 8;
            int blockSize = strLength / blockCount;

            string line;
            string subBlock;
            int count;
            int[] patternCount = new int[blockCount];

            for (int i = 0; i < blockCount; i++)
            {
                count = 0;
                line = sequence.Substring(i * blockSize, blockSize);
                while (count < blockSize - patternLength + 1)
                {
                    subBlock = line.Substring(count, patternLength);
                    if (subBlock == pattern)
                    {
                        patternCount[i]++;
                        count += patternLength;
                    }
                    else
                    {
                        count++;
                    }
                }
            }

            double mean = (blockSize - patternLength + 1) / Math.Pow(2, patternLength);//Теоретическое среднее
            double variance = blockSize * ((1 / Math.Pow(2, patternLength)) - ((2 * patternLength - 1) / (Math.Pow(2, 2 * patternLength))));//Теоретическое отклонение

            double xi2 = 0;
            for (int i = 0; i < blockCount; i++)
            {
                xi2 += Math.Pow(patternCount[i] - mean, 2) / variance;
            }

            double pValue = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(blockCount / 2, xi2 / 2);

            bool isRandom = false;
            if (pValue > alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }

        /// <summary>
        /// Overlapping Template Matching Test
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double, bool> Test8()
        {
            //string sequence = "10111011110010110100011100101110111110000101101001";
            //string pattern = "11";
            //int blockSize = 10;
            //double[] pi = new double[6] { 0.324652, 0.182617, 0.142670, 0.106645, 0.077147, 0.166269 };
            //int strLength = sequence.Length;

            string pattern = "111111111";//Шаблон
            int patternLength = pattern.Length;
            int blockSize = 1032;
            int blockCount = strLength / blockSize;

            int[] patternCount = new int[6];
            string line;
            string subBlock;
            int count;
            //MessageBox.Show($"BlockCount={blockCount}");
            for (int i = 0; i < blockCount; i++)
            {
                line = sequence.Substring(i * blockSize, blockSize);
                count = 0;
                for (int j = 0; j < blockSize - patternLength+1; j++) 
                {
                    subBlock = line.Substring(j, patternLength);
                    if (subBlock == pattern)
                    {
                        count++;
                    }
                }
                if (count <= 4)
                {
                    patternCount[count]++;
                }
                else
                {
                    patternCount[5]++;
                }
                //MessageBox.Show($"PatternCount[0]={patternCount[0]}, PatternCount[1]={patternCount[1]}, PatternCount[2]={patternCount[2]}, PatternCount[3]={patternCount[3]}, PatternCount[4]={patternCount[4]}, PatternCount[5]={patternCount[5]}");

            }
            //MessageBox.Show($"PatternCount[0]={patternCount[0]}, PatternCount[1]={patternCount[1]}, PatternCount[2]={patternCount[2]}, PatternCount[3]={patternCount[3]}, PatternCount[4]={patternCount[4]}, PatternCount[5]={patternCount[5]}");

            double lamda = (blockSize - patternLength + 1) / Math.Pow(2, patternLength);
            double eta = lamda / 2;
            //MessageBox.Show($"lamda={lamda}, eta={eta}");

            double[] pi = new double[6] {  0.364091, 0.185659, 0.139381, 0.100571, 0.0704323, 0.139865 };

            double xi2 = 0;
            for (int i = 0; i < patternCount.Length; i++)
            {
                xi2 += Math.Pow(patternCount[i] - blockCount * pi[i], 2) / (blockCount * pi[i]);
            }
            //MessageBox.Show($"xi2={xi2}");

            double pValue = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(5 / 2, xi2 / 2);

            bool isRandom = false;
            if (pValue > alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }

        /// <summary>
        /// Maurer's "Universal Statistical" Test
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double, bool> Test9()
        {
            //string Sequence = "01011010011101010111";
            //int BlockSize = 2;
            //int BlockCountQ = 4;
            //int strLength = sequence.Length;

            int blockSize = 5;//Длина блока разбиения
            bool isRandom = false;
            double pValue = -1;
            if (strLength < 387840) return Tuple.Create(pValue, isRandom);
            if (strLength >= 387840) blockSize = 6;
            if (strLength >= 904960) blockSize = 7;
            if (strLength >= 2068480) blockSize = 8;
            if (strLength >= 4654080) blockSize = 9;
            if (strLength >= 10342400) blockSize = 10;
            if (strLength >= 22753280) blockSize = 11;
            if (strLength >= 49643520) blockSize = 12;
            if (strLength >= 107560960) blockSize = 13;
            if (strLength >= 231669760) blockSize = 14;
            if (strLength >= 496435200) blockSize = 15;
            if (strLength >= 1059061760) blockSize = 16;

            int blockCountQ = 10 * (int)Math.Pow(2, blockSize);//Количество блоков инициализации
            int blockNum = (int)(strLength / blockSize);
            int blockCountK = blockNum - blockCountQ;//Количество тестовых блоков
            //MessageBox.Show($"StrLength={StrLength}, BlockNum={BlockNum}, BlockCountQ={BlockCountQ}, BlockCountK={BlockCountK}");

            string stringBlock;
            int Decimal;
            double logSum = 0;
            int[] t = new int[(int)Math.Pow(2, blockSize)];
            for (int i = 0; i < blockNum; i++)
            {
                stringBlock = sequence.Substring(i * blockSize, blockSize);
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

            pValue = MathNet.Numerics.SpecialFunctions.Erfc(Math.Abs((fN - expectedValue) / (Math.Sqrt(2) * sigma)));

            if (pValue > alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }

        /// <summary>
        /// Linear Complexity Test
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double, bool> Test10()
        {
            //int strLength = sequence.Length;

            int degreeFreedom = 6;//Степень свободы
            double[] pi = new double[7] { 0.01047, 0.03125, 0.125, 0.5, 0.25, 0.0625, 0.020833 };
            int blockSize = 500;
            int blockCount = strLength / blockSize;

            bool isRandom = false;
            double pValue = -1;
            if (blockCount < 1) Tuple.Create(pValue, isRandom);

            double mean = (blockSize/2) + ((9 + Math.Pow(-1,blockSize+1))/36) - (((blockSize/3) + (2/9))/Math.Pow(2,blockSize));//Теоретическое среднее

            string stringBlock;
            int complexities;
            double t;
            int[] v = new int[7];
            for (int i = 0; i < blockCount; i++)
            {
                stringBlock = sequence.Substring(i * blockSize, blockSize);
                complexities = BerlekampMasseyAlgoritm(stringBlock);
                t = Math.Pow(-1, blockSize)*(complexities-mean) + (2/9);

                if (t <= -2.5) v[0]++;
                if ((t <= -1.5) && (t>-2.5)) v[1]++;
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

            pValue = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(degreeFreedom / 2, xi2 / 2);

            if (pValue > alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);

        }

        /// <summary>
        /// Serial Test
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double[], bool> Test11() 
        {
            //string sequence = "0011011101";
            //int patternLength = 3;
            //int strLength = sequence.Length;

            int patternLength = 16;
            string newSequence = sequence + sequence.Substring(0, patternLength - 1);

            int[] v1 = new int[(int)Math.Pow(2,patternLength)];
            int[] v2 = new int[(int)Math.Pow(2, patternLength-1)];
            int[] v3 = new int[(int)Math.Pow(2, patternLength-2)];
            string stringBlock;
            int Decimal;
            for (int i = 0; i < strLength; i++) 
            {
                stringBlock = newSequence.Substring(i, patternLength);
                Decimal = Convert.ToInt32(stringBlock, 2);
                v1[Decimal]++;

                stringBlock = newSequence.Substring(i, patternLength-1);
                Decimal = Convert.ToInt32(stringBlock, 2);
                v2[Decimal]++;

                stringBlock = newSequence.Substring(i, patternLength-2);
                Decimal = Convert.ToInt32(stringBlock, 2);
                v3[Decimal]++;
            }
            //MessageBox.Show($"V1[0]={V1[0]}, V1[1]={V1[1]}, V1[2]={V1[2]}, V1[3]={V1[3]}, V1[4]={V1[4]}, V1[5]={V1[5]}, V1[6]={V1[6]}, V1[7]={V1[7]}");
            //MessageBox.Show($"V2[0]={V2[0]}, V2[1]={V2[1]}, V2[2]={V2[2]}, V2[3]={V2[3]}");
            //MessageBox.Show($"V3[0]={V3[0]}, V3[1]={V3[1]}");

            double sumV1 = 0;
            for (int i = 0; i < v1.Length; i++) sumV1 += Math.Pow(v1[i],2);
            double phi1 = (sumV1 * Math.Pow(2, patternLength) / strLength) - strLength;
            //MessageBox.Show($"Phi1={Phi1}");
            double sumV2 = 0;
            for (int i = 0; i < v2.Length; i++) sumV2 += Math.Pow(v2[i], 2);
            double phi2 = (sumV2 * Math.Pow(2, patternLength-1) / strLength) - strLength;
            //MessageBox.Show($"Phi2={Phi2}");
            double sumV3 = 0;
            for (int i = 0; i < v3.Length; i++) sumV3 += Math.Pow(v3[i], 2);
            double phi3 = (sumV3 * Math.Pow(2, patternLength-2) / strLength) - strLength;
            //MessageBox.Show($"Phi3={Phi3}");

            double nabla1 = phi1 - phi2;
            double nabla2 = phi1 - 2 * phi2 + phi3;
            //MessageBox.Show($"Nabla1={nabla1}, Nabla2={nabla2}");

            double[] pValue = new double[2];
            pValue[0] = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(Math.Pow(2, patternLength - 2), nabla1/2);
            pValue[1] = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(Math.Pow(2, patternLength - 3), nabla2/2);
            //MessageBox.Show($"Pvalue1={pValue1}, Pvalue2={pValue2}");

            bool isRandom = true;
            for (int i = 0; i < pValue.Length; i++)
            {
                if (pValue[i] < alfa)
                {
                    isRandom = false;
                    break;
                }
            }
            return Tuple.Create(pValue, isRandom);
        }

        /// <summary>
        /// Approximate Entropy Test
        /// </summary>
        /// <returns>A double[] and boolean</returns>
        internal protected static Tuple<double, bool> Test12()
        {
            //string sequence = "1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000";
            //int blockSize = 2;
            //int strLength = sequence.Length;

            int blockSize = 10;
            string newSequence = sequence + sequence.Substring(0, blockSize + 1);

            int[] v1 = new int[(int)Math.Pow(2, blockSize)];
            int[] v2 = new int[(int)Math.Pow(2, blockSize+1)];
            //MessageBox.Show($"v1Length={v1.Length}, v2Length={v2.Length}");
            string stringBlock;
            int Decimal;
            for (int i = 0; i < strLength; i++)
            {
                stringBlock = newSequence.Substring(i, blockSize);
                Decimal = Convert.ToInt32(stringBlock, 2);
                v1[Decimal]++;

                stringBlock = newSequence.Substring(i, blockSize + 1);
                Decimal = Convert.ToInt32(stringBlock, 2);
                v2[Decimal]++;
            }
            //MessageBox.Show($"v1[0]={v1[0]}, v1[1]={v1[1]}, v1[2]={v1[2]}, v1[3]={v1[3]}, v1[4]={v1[4]}, v1[5]={v1[5]}, v1[6]={v1[6]}, v1[7]={v1[7]}");
            //MessageBox.Show($"v2[0]={v2[0]}, v2[1]={v2[1]}, v2[2]={v2[2]}, v2[3]={v2[3]}, v2[4]={v2[4]}, v2[5]={v2[5]}, v2[6]={v2[6]}, v2[7]={v2[7]},");

            double phi1 = 0;
            double c;
            double p;
            for (int i = 0; i < v1.Length; i++) 
            {
                c = (double)v1[i] / strLength;
                p = c * Math.Log(c);
                //MessageBox.Show($"1 - {c}");
                if (!double.IsNaN(p)) phi1 += p;
                //MessageBox.Show($"phi1 = {phi1}");
            }
            double phi2 = 0;
            for (int i = 0; i < v2.Length; i++)
            {
                c = (double)v2[i] / strLength;
                p = c * Math.Log(c);
                //MessageBox.Show($"2 - {c}");
                if (!double.IsNaN(p)) phi2 += p;
                //MessageBox.Show($"phi12= {phi2}");
            }
            //MessageBox.Show($"phi1={phi1}, phi2={phi2}");

            //double apEn = phi1 - phi2;
            //MessageBox.Show($"apEn={apEn}");
            //MessageBox.Show($"log2={Math.Log(2)}");
            double xi2 = 2 * strLength * (Math.Log(2) - (phi1-phi2));
            //MessageBox.Show($"xi2={xi2}");

            double pValue = MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(Math.Pow(2, blockSize - 1), xi2 / 2); ;

            bool isRandom = false;
            if (pValue > alfa) isRandom = true;
            return Tuple.Create(pValue, isRandom);
        }

        /// <summary>
        /// Cumulative Sums (Cusum) Test
        /// </summary>
        /// <returns>A double and boolean</returns>
        internal protected static Tuple<double[], bool> Test13()
        {
            //string sequence = "1100100100001111110110101010001000100001011010001100001000110100110001001100011001100010100010111000";
            //string sequence = "1011010111";
            //int strLength = sequence.Length;

            int sForward = 2 * Int32.Parse(sequence[0].ToString()) - 1;
            //MessageBox.Show($"sForward[{0}]={sForward}");
            int sBackward = 2 * Int32.Parse(sequence[strLength-1].ToString()) - 1;
            double[] maxS = new double[2];
            int ii = strLength - 2;
            for (int i = 1; i < strLength; i++) 
            {
                sForward += 2 * Int32.Parse(sequence[i].ToString()) - 1;
                //MessageBox.Show($"sForward[{i}]={sForward}");
                if (maxS[0] < Math.Abs(sForward)) maxS[0] = Math.Abs(sForward);
                sBackward += 2 * Int32.Parse(sequence[ii].ToString()) - 1;
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
                for (int j = (int)Math.Floor(0.25 * Math.Floor(-strLength / maxS[i])+1); j <= (int)Math.Floor(0.25 * Math.Floor(strLength / maxS[i])-1)+1; j++)
                {
                    x1 = (4 * j + 1) * maxS[i] / Math.Sqrt(strLength);
                    x2 = (4 * j - 1) * maxS[i] / Math.Sqrt(strLength);
                    sum1 += MathNet.Numerics.Distributions.Normal.CDF(0, 1, x1) - MathNet.Numerics.Distributions.Normal.CDF(0, 1, x2);
                    //sum1 += CumulativeFunction.CumulativeDistributionFunction(x1) - CumulativeFunction.CumulativeDistributionFunction(x2);
                }
                for (int j = (int)Math.Floor(0.25 * Math.Floor(-strLength / maxS[i])-3); j <= (int)Math.Floor(0.25 * Math.Floor(strLength / maxS[i])-1)+1; j++)
                {
                    x1 = (4 * j + 3) * maxS[i] / Math.Sqrt(strLength);
                    x2 = (4 * j + 1) * maxS[i] / Math.Sqrt(strLength);
                    sum2 += MathNet.Numerics.Distributions.Normal.CDF(0, 1, x1) - MathNet.Numerics.Distributions.Normal.CDF(0, 1, x2);
                    //sum2 += CumulativeFunction.CumulativeDistributionFunction(x1) - CumulativeFunction.CumulativeDistributionFunction(x2);
                }
                pValue[i] = 1 - sum1 + sum2;
            }
            

            bool isRandom = true;
            for (int i = 0; i < pValue.Length; i++)
            {
                if (pValue[i] < alfa)
                {
                    isRandom = false;
                    break;
                }
            }
            return Tuple.Create(pValue, isRandom);
        }

        /// <summary>
        /// Random Excursions Test
        /// </summary>
        /// <returns>A double[] and boolean</returns>
        internal protected static Tuple<double[], bool> Test14()
        {
            //string sequence = "0110110101";
            //int strLength = sequence.Length;

            int[] s = new int[strLength + 2];//Partial sums with zero
            s[0] = 0;
            s[strLength+1] = 0;
            int cycleCount = 0; // Количество циклов в массиве s
            for (int i = 1; i < strLength + 1; i++) 
            {
                s[i] = s[i - 1] + 2 * Int32.Parse(sequence[i - 1].ToString()) - 1;
                if (s[i]==0) cycleCount++;
            }
            cycleCount++;
            //MessageBox.Show($"cycleCount={cycleCount}");

            bool isRandom = false;
            double[] pValue = new double[8] {-1,-1,-1,-1,-1,-1,-1,-1};
            if (cycleCount < Math.Max(0.005*Math.Sqrt(strLength),500)) return Tuple.Create(pValue, isRandom);

            int[,] freq = new int[8,cycleCount];//Частота каждого значения в массиве s
            int numCycle = 0;
            int[] x = new int[8] { -4, -3, -2, -1, 1, 2, 3, 4 };//Возможные значения элементов массива s
            for (int i = 1; i < strLength + 1; i++) 
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
                    for (int k=0; k < cycleCount; k++)
                    {
                        if (freq[i, k] == j) v[i,j]++;
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
                    } else if (j > 0 && j < 5)
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
                pValue[i]= MathNet.Numerics.SpecialFunctions.GammaUpperRegularized(5/2, xi2 / 2);

            }

            isRandom = true;
            for (int i =0; i<pValue.Length; i++)
            {
                if (pValue[i] < alfa)
                {
                    isRandom = false;
                    break;
                }
            }
            return Tuple.Create(pValue, isRandom);
        }

        /// <summary>
        /// Random Excursions Variant Test 
        /// </summary>
        /// <returns>A double[] and boolean</returns>
        internal protected static Tuple<double[], bool> Test15()
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

            int[] s = new int[strLength + 2];//Partial sums with zero
            s[0] = 0;
            s[strLength + 1] = 0;
            int cycleCount = 0; // Количество циклов в массиве s
            for (int i = 1; i < strLength + 1; i++)
            {
                s[i] = s[i - 1] + 2 * Int32.Parse(sequence[i - 1].ToString()) - 1;
                if (s[i] == 0) cycleCount++;
                for (int j=0; j<18; j++)
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
                //MessageBox.Show($"i={i}: stateCount={stateCount[i]}, cycleCount={cycleCount}, x={x[i]}");
            }

            isRandom = true;
            for (int i = 0; i < pValue.Length; i++)
            {
                if (pValue[i] < alfa)
                {
                    isRandom = false;
                    break;
                }
            }
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
                    d ^= c[i] & int.Parse(charBlock[N-i].ToString());//(d+=c[i]*s[N-i] mod 2)
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

        private static int[,] XorRowArray(int thisIndex, int newIndex, int[,] array)
        {
            for (int i=0; i < array.GetLength(1); i++)
            {
                array[newIndex, i] = array[thisIndex, i] ^ array[newIndex, i];
            }
            return array;
        }

        private static int[,] SwapRowArray(int thisIndex, int newIndex, int[,] array)
        {
            int thisElement;
            int newElement;
            for (int i = 0; i< array.GetLength(1); i++)
            {
                thisElement = array[thisIndex, i];
                newElement = array[newIndex, i];
                array[thisIndex, i] = newElement;
                array[newIndex, i] = thisElement;
            }
            return array;
        }

        private static int GetSn(string sequence)
        {
            int s = sequence.ToCharArray().Where(i => i == '1').Count() - sequence.ToCharArray().Where(i => i == '0').Count();
            return s;
        }

        /*
        private static double[] DFT(double[] data)
        {
            int n = data.Length;
            int m = n;// I use m = n / 2d;
            double[] real = new double[n];
            double[] imag = new double[n];
            double[] result = new double[m];
            double pi_div = 2.0 * Math.PI / n;
            for (int w = 0; w < m; w++)
            {
                double a = w * pi_div;
                for (int t = 0; t < n; t++)
                {
                    real[w] += data[t] * Math.Cos(a * t);
                    imag[w] += data[t] * Math.Sin(a * t);
                }
                result[w] = Math.Sqrt(real[w] * real[w] + imag[w] * imag[w]) / n;
            }
            return result;
        }
        */
    }

    /*
    public class CumulativeFunction
    {
        public static double CumulativeDistributionFunction(double x)
        {
            MathNet.Numerics.Distributions.Normal result = new MathNet.Numerics.Distributions.Normal();
            return result.CumulativeDistribution(x);

        }

    }
    */
}
