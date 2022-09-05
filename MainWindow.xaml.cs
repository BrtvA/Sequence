using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sequence
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool workMode;//Режим работы: true - генерация последовательности из csv-файла, содержащей флуктуации интенсивности; false - работа напрямую с битовой последовательностью

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseMove(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (workMode == true)
            {
                file.Filter = "Text files(*.csv)|*.csv";
            }
            else
            {
                file.Filter = "Text files(*.txt)|*.txt";
            }
            file.ShowDialog();
            //openFileText.Text = file.FileName;
            if (file.FileName.Length>0)
            {
                openFileText.Text = file.FileName;
                if (workMode == true)
                {
                    getSequenceButton.IsEnabled = true;
                    executeTestButton.IsEnabled = false;
                    saveButton.IsEnabled = false;
                }
                else
                {
                    getSequenceButton.IsEnabled = false;
                    executeTestButton.IsEnabled = true;
                    saveButton.IsEnabled = false;
                }
            }
            else
            {
                getSequenceButton.IsEnabled = false;
                executeTestButton.IsEnabled = false;
                saveButton.IsEnabled = false;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private async void AsyncGetSequenceButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LSBText.Text) || string.IsNullOrEmpty(timeShiftText.Text) || 
                string.IsNullOrEmpty(numberBitText.Text) || (string.IsNullOrEmpty(timeLengthText.Text) && timeLengthCheck.IsChecked==true))
            {
                MessageBox.Show("Fill in all the fields");
            }else if (Int32.Parse(LSBText.Text.Trim()) >= Int32.Parse(numberBitText.Text.Trim()))
            {
                MessageBox.Show("The number of LSB's cannot be greater than the ADS bit's");
            }
            else
            {
                CSVData csvData = new CSVData();
                csvData.LSB = Int32.Parse(LSBText.Text.Trim().Replace('.', ','));
                csvData.numberBit = Int32.Parse(numberBitText.Text.Trim().Replace('.', ','));
                string[] dataCSVArray = File.ReadAllLines(openFileText.Text);
                csvData.timeShift = double.Parse(timeShiftText.Text.Trim().Replace('.', ','));
                csvData.deltaTime = Math.Abs((double.Parse(dataCSVArray[0].Split(',')[0].Replace('.', ',')) - double.Parse(dataCSVArray[1].Split(',')[0].Replace('.', ','))) * Math.Pow(10, 9));
                if (timeLengthCheck.IsChecked == true)
                {
                    csvData.timeLength = double.Parse(timeLengthText.Text.Replace('.', ',')) + 2 * csvData.timeShift;
                }
                else
                {
                    csvData.timeLength = csvData.deltaTime * (dataCSVArray.Length - 1);
                }
                csvData.intensity = new double[(int)Math.Truncate(csvData.timeLength / csvData.deltaTime) + 1];
                string[] lineArray;
                for (int i = 0; i < csvData.intensity.Length; i++)
                {
                    lineArray = dataCSVArray[i].Split(',');
                    csvData.intensity[i] = double.Parse(lineArray[1].Replace('.', ','));
                }
                NIST800_22.sequence = await Task.Run(() => csvData.GetSequence());
                NIST800_22.strLength = NIST800_22.sequence.Length;

                executeTestButton.IsEnabled = true;
                saveButton.IsEnabled = true;
            }
        }
        private void ExecuteTestButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            //mainGrid.IsEnabled = false;
            if (workMode == true)
            {
                //bitLengthText.Text = NIST800_22.strLength.ToString();
                //string strSequence = NIST800_22.sequence.ToString();
                AsyncExecuteTest();
                //await Task.Run(() => ExecuteTest());
                saveButton.IsEnabled = true;
            }
            else
            {
                //string strSequence;
                using (StreamReader Read = new StreamReader(openFileText.Text))
                {
                    NIST800_22.sequence = Read.ReadLine();
                    NIST800_22.strLength = NIST800_22.sequence.Length;
                }
                //bitLengthText.Text = Convert.ToString(strSequence.Length);
                saveButton.IsEnabled = false;
                AsyncExecuteTest();
                //await Task.Run(() => ExecuteTest());
            }
            bitLengthText.Text = NIST800_22.strLength.ToString();
            //AsyncExecuteTest();

            GC.Collect();
            //GC.Collect(2, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "Text files(*.txt)|*.txt";
            file.ShowDialog();
            if (file.FileName != "")
            {
                using (StreamWriter Save = new StreamWriter(file.FileName))//Запись в файл
                {
                    Save.WriteLine(NIST800_22.sequence);
                    Save.Close();
                }
            }
        }

        private void ModeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (modeBox.SelectedIndex == 0)
            {
                workMode = true;
                if (numberBitText != null && numberBitText.IsEnabled == false) numberBitText.IsEnabled = true;
                if (timeShiftText != null && timeShiftText.IsEnabled == false) timeShiftText.IsEnabled = true;
                if (LSBText != null && LSBText.IsEnabled == false) LSBText.IsEnabled = true;
                if (timeLengthText != null && timeLengthText.IsEnabled == false) timeLengthText.IsEnabled = true;
                if (timeLengthCheck != null && timeLengthCheck.IsEnabled == false) timeLengthCheck.IsEnabled = true;
                openFileText.Text = null;

                if (paramPanel1 != null && paramPanel1.Visibility==Visibility.Hidden) paramPanel1.Visibility = Visibility.Visible;
                if (paramPanel2 != null && paramPanel2.Visibility == Visibility.Hidden) paramPanel2.Visibility = Visibility.Visible;
                if (getSequenceButton != null && getSequenceButton.Visibility == Visibility.Hidden) getSequenceButton.Visibility = Visibility.Visible;
                if (saveButton != null && saveButton.Visibility == Visibility.Hidden) saveButton.Visibility = Visibility.Visible;

                mainGrid.RowDefinitions[2].Height = new GridLength(50, GridUnitType.Pixel);
                mainGrid.RowDefinitions[3].Height = new GridLength(50, GridUnitType.Pixel);
                if (executeTestButton != null) executeTestButton.Margin = new Thickness(200, -29, 0, 0);
                /*
                numberBitText.IsEnabled = true;
                timeShiftText.IsEnabled = true;
                LSBText.IsEnabled = true;
                timeLengthText.IsEnabled = true;
                timeLengthCheck.IsEnabled = true;
                openFileText.Text = null;
                */
            }
            else
            {
                workMode = false;
                if (numberBitText!=null && numberBitText.IsEnabled==true) numberBitText.IsEnabled = false;
                if (timeShiftText != null && timeShiftText.IsEnabled == true) timeShiftText.IsEnabled = false;
                if (LSBText != null && LSBText.IsEnabled == true) LSBText.IsEnabled = false;
                if (timeLengthText != null && timeLengthText.IsEnabled == true) timeLengthText.IsEnabled = false;
                if (timeLengthCheck != null && timeLengthCheck.IsEnabled == true) timeLengthCheck.IsEnabled = false;
                openFileText.Text = null;

                if (paramPanel1 != null && paramPanel1.Visibility == Visibility.Visible) paramPanel1.Visibility = Visibility.Hidden;
                if (paramPanel2 != null && paramPanel2.Visibility == Visibility.Visible) paramPanel2.Visibility = Visibility.Hidden;
                if (getSequenceButton != null && getSequenceButton.Visibility == Visibility.Visible) getSequenceButton.Visibility = Visibility.Hidden;
                if (saveButton != null && saveButton.Visibility == Visibility.Visible) saveButton.Visibility = Visibility.Hidden;

                mainGrid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Pixel);
                mainGrid.RowDefinitions[3].Height = new GridLength(0, GridUnitType.Pixel);
                if (executeTestButton != null) executeTestButton.Margin = new Thickness(20, -15, 0, 0);

                /*
                numberBitText.IsEnabled = false;
                timeShiftText.IsEnabled = false;
                LSBText.IsEnabled = false;
                timeLengthText.IsEnabled = false;
                timeLengthCheck.IsEnabled = false;
                openFileText.Text = null;
                */
            }
            if (getSequenceButton != null && getSequenceButton.IsEnabled == true) getSequenceButton.IsEnabled = false;
            if (executeTestButton != null && executeTestButton.IsEnabled == true) executeTestButton.IsEnabled = false;
            if (saveButton != null && saveButton.IsEnabled == true) saveButton.IsEnabled = false;
            /*
            getSequenceButton.IsEnabled = false;
            executeTestButton.IsEnabled = false;
            saveButton.IsEnabled = false;
            */
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();

            test1Check.IsChecked = false;
            test2Check.IsChecked = false;
            test3Check.IsChecked = false;
            test4Check.IsChecked = false;
            test5Check.IsChecked = false;
            test6Check.IsChecked = false;
            test7Check.IsChecked = false;
            test8Check.IsChecked = false;
            test9Check.IsChecked = false;
            test10Check.IsChecked = false;
            test11Check.IsChecked = false;
            test12Check.IsChecked = false;
            test13Check.IsChecked = false;
            test14Check.IsChecked = false;
            test15Check.IsChecked = false;

            bitLengthText.Text = null;
            //openFileText.Text = null;
        }

        private void Reset()
        {
            test1Text.Text = null;
            test2Text.Text = null;
            test3Text.Text = null;
            test4Text.Text = null;
            test5Text.Text = null;
            test6Text.Text = null;
            test7Text.Text = null;
            test8Text.Text = null;
            test9Text.Text = null;
            test10Text.Text = null;
            test11Box.Items.Clear();
            test12Text.Text = null;
            test13Box.Items.Clear();
            test14Box.Items.Clear();
            test15Box.Items.Clear();

            test1Check.Background = Brushes.White;
            test2Check.Background = Brushes.White;
            test3Check.Background = Brushes.White;
            test4Check.Background = Brushes.White;
            test5Check.Background = Brushes.White;
            test6Check.Background = Brushes.White;
            test7Check.Background = Brushes.White;
            test8Check.Background = Brushes.White;
            test9Check.Background = Brushes.White;
            test10Check.Background = Brushes.White;
            test11Check.Background = Brushes.White;
            test12Check.Background = Brushes.White;
            test13Check.Background = Brushes.White;
            test14Check.Background = Brushes.White;
            test15Check.Background = Brushes.White;

            bitLengthText.Text = null;
        }

        private void SelectTestButton_Click(object sender, RoutedEventArgs e)
        {
            test1Check.IsChecked = true;
            test2Check.IsChecked = true;
            test3Check.IsChecked = true;
            test4Check.IsChecked = true;
            test5Check.IsChecked = true;
            test6Check.IsChecked = true;
            test7Check.IsChecked = true;
            test8Check.IsChecked = true;
            test9Check.IsChecked = true;
            test10Check.IsChecked = true;
            test11Check.IsChecked = true;
            test12Check.IsChecked = true;
            test13Check.IsChecked = true;
            test14Check.IsChecked = true;
            test15Check.IsChecked = true;
        }

        private async void AsyncExecuteTest()
        {
            if (test1Check.IsChecked == true) {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test1();
                    this.Dispatcher.Invoke(() => {
                        TextBoxChanger(test1Text, test1Check, result.Item1, result.Item2);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test1());
                //TextBoxChanger(test1Text, test1Check, TestResult.Item1, TestResult.Item2);
            }
            if (test2Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test2();
                    this.Dispatcher.Invoke(() => {
                        TextBoxChanger(test2Text, test2Check, result.Item1, result.Item2);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test2());
                //TextBoxChanger(test2Text, test2Check, TestResult.Item1, TestResult.Item2);
            }
            if (test3Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test3();
                    this.Dispatcher.Invoke(() => {
                        TextBoxChanger(test3Text, test3Check, result.Item1, result.Item2);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test3());
                //TextBoxChanger(test3Text, test3Check, TestResult.Item1, TestResult.Item2);
            }
            if (test4Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test4();
                    this.Dispatcher.Invoke(() => {
                        TextBoxChanger(test4Text, test4Check, result.Item1, result.Item2);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test4());
                //TextBoxChanger(test4Text, test4Check, TestResult.Item1, TestResult.Item2);
            }
            if (test5Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test5();
                    this.Dispatcher.Invoke(() => {
                        TextBoxChanger(test5Text, test5Check, result.Item1, result.Item2);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test5());
                //TextBoxChanger(test5Text, test5Check, TestResult.Item1, TestResult.Item2);
            }
            if (test6Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test6();
                    this.Dispatcher.Invoke(() => {
                        TextBoxChanger(test6Text, test6Check, result.Item1, result.Item2);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test6());
                //TextBoxChanger(test6Text, test6Check, TestResult.Item1, TestResult.Item2);
            }
            if (test7Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test7();
                    this.Dispatcher.Invoke(() => {
                        TextBoxChanger(test7Text, test7Check, result.Item1, result.Item2);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test7());
                //TextBoxChanger(test7Text, test7Check, TestResult.Item1, TestResult.Item2);
            }
            if (test8Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test8();
                    this.Dispatcher.Invoke(() => {
                        TextBoxChanger(test8Text, test8Check, result.Item1, result.Item2);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test8());
                //TextBoxChanger(test8Text, test8Check, TestResult.Item1, TestResult.Item2);
            }
            if (test9Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test9();
                    this.Dispatcher.Invoke(() => {
                        TextBoxChanger(test9Text, test9Check, result.Item1, result.Item2);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test9());
                //TextBoxChanger(test9Text, test9Check, TestResult.Item1, TestResult.Item2);
            }
            if (test10Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test10();
                    this.Dispatcher.Invoke(() => {
                        TextBoxChanger(test10Text, test10Check, result.Item1, result.Item2);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test10());
                //TextBoxChanger(test10Text, test10Check, TestResult.Item1, TestResult.Item2);
            }
            if (test11Check.IsChecked == true)
            {
                //var TestResult = await Task.Run(() => NIST800_22.Test11());
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test11();
                    this.Dispatcher.Invoke(() => {
                        ComboBoxChanger(test11Box, test11Check, result.Item1, result.Item2, 11);
                    });
                });
                //ComboBoxChanger(test11Box, test11Check, TestResult.Item1, TestResult.Item2, 11);
            }
            if (test12Check.IsChecked == true)
            {
                var result = NIST800_22.Test12();
                this.Dispatcher.Invoke(() => {
                    TextBoxChanger(test12Text, test12Check, result.Item1, result.Item2);
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test12());
                //TextBoxChanger(test12Text, test12Check, TestResult.Item1, TestResult.Item2);
            }
            if (test13Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test13();
                    this.Dispatcher.Invoke(() => {
                        ComboBoxChanger(test13Box, test13Check, result.Item1, result.Item2, 13);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test13());
                //ComboBoxChanger(test13Box, test13Check, TestResult.Item1, TestResult.Item2, 13);
            }
            if (test14Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test14();
                    this.Dispatcher.Invoke(() => {
                        ComboBoxChanger(test14Box, test14Check, result.Item1, result.Item2, 14);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test14());
                //ComboBoxChanger(test14Box, test14Check, TestResult.Item1, TestResult.Item2, 14);
            }
            if (test15Check.IsChecked == true)
            {
                var testResult = Task.Run(() => {
                    var result = NIST800_22.Test15();
                    this.Dispatcher.Invoke(() => {
                        ComboBoxChanger(test15Box, test15Check, result.Item1, result.Item2, 15);
                    });
                });
                //var TestResult = await Task.Run(() => NIST800_22.Test15());
                //ComboBoxChanger(test15Box, test15Check, TestResult.Item1, TestResult.Item2, 15);
            }
            //mainGrid.IsEnabled = true;
        }

        private void TextBoxChanger(TextBox textBox, CheckBox checkBox, double pValue, bool isRandom)
        {
            textBox.Text = Convert.ToString(pValue);
            if (isRandom)
            {
                checkBox.Background = Brushes.Green;//Тест пройден
            }
            else if (pValue == -1)
            {
                checkBox.Background = Brushes.Yellow;//Последовательность не проходит по длине
            }
            else
            {
                checkBox.Background = Brushes.Red;//Тест не пройден
            }
        }

        private void ComboBoxChanger(ComboBox comboBox, CheckBox checkBox, double[] pValue, bool isRandom, int numTest)
        {
            comboBox.Items.Clear();
            //for (int i = 0; i < pValue.Length; i++) comboBox.Items.Insert(i, pValue[i]);
            for (int i = 0; i < pValue.Length; i++) 
            { 
                switch (numTest)
                {
                    case 13:
                        if (i==0) comboBox.Items.Insert(i, $"Forward: {pValue[i]}");
                        if (i == 1) comboBox.Items.Insert(i, $"Backward: {pValue[i]}");
                        break;
                    case 14:
                        if (i < 4)
                        {
                            comboBox.Items.Insert(i, $"x = {-4 + i}: {pValue[i]}");
                        }
                        else if (i > 4)
                        {
                            comboBox.Items.Insert(i, $"x = {-3 + i}: {pValue[i]}");
                        }
                        else 
                        {
                            comboBox.Items.Insert(i, $"x = 1: {pValue[i]}");
                        }
                        break;
                    case 15:
                        if (i < 9)
                        {
                            comboBox.Items.Insert(i, $"x = {-9 + i}: {pValue[i]}");
                        }
                        else if (i > 9)
                        {
                            comboBox.Items.Insert(i, $"x = {-8 + i}: {pValue[i]}");
                        }
                        else
                        {
                            comboBox.Items.Insert(i, $"x = 1: {pValue[i]}");
                        }
                        break;
                    default:
                        comboBox.Items.Insert(i, pValue[i]);
                        break;
                }
            }

            comboBox.SelectedIndex = 0;

            if (isRandom)
            {
                checkBox.Background = Brushes.Green;//Тест пройден
            }
            else if (pValue.Where(i => i == -1).Count()>0)
            {
                checkBox.Background = Brushes.Yellow;//Последовательность не проходит по длине
            }
            else
            {
                checkBox.Background = Brushes.Red;//Тест не пройден
            }
        }

        /*
        static async Task<string> ReadTextAsync(string path) 
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return await reader.ReadToEndAsync().ConfigureAwait(false);
            }
        }
        */
    }    
}
