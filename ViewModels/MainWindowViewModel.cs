using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Sequence.Infrastructure.Commands;
using Sequence.Models;
using Sequence.Models.TestNIST;
using Sequence.ViewModels.Base;
using static System.Net.Mime.MediaTypeNames;

namespace Sequence.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Заголовок окна

        private string _title = "Sequence";

        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        #endregion

        #region Window State
        private WindowState _windowState = WindowState.Normal;
        /// <summary>Состояние окна</summary>
        public WindowState WindowState
        {
            get => _windowState;
            set => Set(ref _windowState, value);
        }
        #endregion

        #region openFileTxt
        private string _openFileTxt;
        ///<summary>openFileTxt</summary>
        public string OpenFileTxt
        {
            get => _openFileTxt;
            set => Set(ref _openFileTxt, value);
        }
        #endregion

        #region LSBTxt
        private string _LSBTxt= "6";
        ///<summary>LSBTxt</summary>
        public string LSBTxt
        {
            get => _LSBTxt;
            set => Set(ref _LSBTxt, value);
        }
        #endregion

        #region TimeShiftTxt
        private string _timeShiftTxt = "17";
        ///<summary>TimeShiftTxt</summary>
        public string TimeShiftTxt
        {
            get => _timeShiftTxt;
            set => Set(ref _timeShiftTxt, value);
        }
        #endregion

        #region NumberBitTxt
        private string _numberBitTxt = "8";
        ///<summary>NumberBitTxt</summary>
        public string NumberBitTxt
        {
            get => _numberBitTxt;
            set => Set(ref _numberBitTxt, value);
        }
        #endregion

        #region TimeLengthTxt
        private string _timeLengthTxt = "66632";
        ///<summary>TimeLengthTxt</summary>
        public string TimeLengthTxt
        {
            get => _timeLengthTxt;
            set => Set(ref _timeLengthTxt, value);
        }
        #endregion

        #region BitLengthTxt
        private string _bitLengthTxt;
        ///<summary>BitLengthTxt</summary>
        public string BitLengthTxt
        {
            get => _bitLengthTxt;
            set => Set(ref _bitLengthTxt, value);
        }
        #endregion

        #region TestListTxt
        private List<string>[] _testListTxt;
        ///<summary>TestListTxt</summary>
        public List<string>[] TestListTxt
        {
            get => _testListTxt;
            set => Set(ref _testListTxt, value);
        }
        #endregion

        #region WorkMode
        private bool _workMode = true;
        /// <summary>Состояние окна</summary>
        public bool WorkMode
        {
            get => _workMode;
            set => Set(ref _workMode, value);
        }
        #endregion

        #region ExecuteTestButtonEnabled
        private bool _executeTestButtonEnabled = false;
        /// <summary>Активность ExecuteTestButton</summary>
        public bool ExecuteTestButtonEnabled
        {
            get => _executeTestButtonEnabled;
            set => Set(ref _executeTestButtonEnabled, value);
        }
        #endregion

        #region SaveButtonEnabled
        private bool _saveButtonEnabled = false;
        /// <summary>Активность SaveButton</summary>
        public bool SaveButtonEnabled
        {
            get => _saveButtonEnabled;
            set => Set(ref _saveButtonEnabled, value);
        }
        #endregion

        #region TimeLengthChecked
        private bool _timeLengthChecked = false;
        /// <summary>Активность TimeLengthBox</summary>
        public bool TimeLengthChecked
        {
            get => _timeLengthChecked;
            set => Set(ref _timeLengthChecked, value);
        }
        #endregion

        #region TestChecked

        private bool[] _testChecked = new bool[] { 
            false, false, false, false, false,
            false, false, false, false, false,
            false, false, false, false, false};
        /// <summary>Активация testBox</summary>
        public bool[] TestChecked
        {
            get => _testChecked;
            set => Set(ref _testChecked, value);
        }


        #region Test1Checked
        //private bool _test1Checked = false;
        /// <summary>Активация test1Box</summary>
        public bool Test1Checked
        {
            get => _testChecked[0];
            set => Set(ref _testChecked[0], value);
        }
        #endregion

        #region Test2Checked
        //private bool _test2Checked = false;
        /// <summary>Активация test2Box</summary>
        public bool Test2Checked
        {
            get => _testChecked[1];
            set => Set(ref _testChecked[1], value);
        }
        #endregion

        #region Test3Checked
        private bool _test3Checked = false;
        /// <summary>Активация test3Box</summary>
        public bool Test3Checked
        {
            get => _test3Checked;
            set => Set(ref _test3Checked, value);
        }
        #endregion

        #region Test4Checked
        private bool _test4Checked = false;
        /// <summary>Активация test4Box</summary>
        public bool Test4Checked
        {
            get => _test4Checked;
            set => Set(ref _test4Checked, value);
        }
        #endregion

        #region Test5Checked
        private bool _test5Checked = false;
        /// <summary>Активация test5Box</summary>
        public bool Test5Checked
        {
            get => _test5Checked;
            set => Set(ref _test5Checked, value);
        }
        #endregion

        #region Test6Checked
        private bool _test6Checked = false;
        /// <summary>Активация test6Box</summary>
        public bool Test6Checked
        {
            get => _test6Checked;
            set => Set(ref _test6Checked, value);
        }
        #endregion

        #region Test7Checked
        private bool _test7Checked = false;
        /// <summary>Активация test7Box</summary>
        public bool Test7Checked
        {
            get => _test7Checked;
            set => Set(ref _test7Checked, value);
        }
        #endregion

        #region Test8Checked
        private bool _test8Checked = false;
        /// <summary>Активация test8Box</summary>
        public bool Test8Checked
        {
            get => _test8Checked;
            set => Set(ref _test8Checked, value);
        }
        #endregion

        #region Test9Checked
        private bool _test9Checked = false;
        /// <summary>Активация test9Box</summary>
        public bool Test9Checked
        {
            get => _test9Checked;
            set => Set(ref _test9Checked, value);
        }
        #endregion

        #region Test10Checked
        private bool _test10Checked = false;
        /// <summary>Активация test10Box</summary>
        public bool Test10Checked
        {
            get => _test10Checked;
            set => Set(ref _test10Checked, value);
        }
        #endregion

        #region Test11Checked
        private bool _test11Checked = false;
        /// <summary>Активация test11Box</summary>
        public bool Test11Checked
        {
            get => _test11Checked;
            set => Set(ref _test11Checked, value);
        }
        #endregion

        #region Test12Checked
        private bool _test12Checked = false;
        /// <summary>Активация test12Box</summary>
        public bool Test12Checked
        {
            get => _test12Checked;
            set => Set(ref _test12Checked, value);
        }
        #endregion

        #region Test13Checked
        private bool _test13Checked = false;
        /// <summary>Активация test3Box</summary>
        public bool Test13Checked
        {
            get => _test13Checked;
            set => Set(ref _test13Checked, value);
        }
        #endregion

        #region Test14Checked
        private bool _test14Checked = false;
        /// <summary>Активация test14Box</summary>
        public bool Test14Checked
        {
            get => _test14Checked;
            set => Set(ref _test14Checked, value);
        }
        #endregion

        #region Test15Checked
        private bool _test15Checked = false;
        /// <summary>Активация test15Box</summary>
        public bool Test15Checked
        {
            get => _test15Checked;
            set => Set(ref _test15Checked, value);
        }
        #endregion

        #endregion

        #region BackgroundCheck
        private string[] _backgroundCheck = new string[]
        {
            "White", "White", "White", "White", "White",
            "White", "White", "White", "White", "White",
            "White", "White", "White", "White", "White"
        };
        /// <summary>Цвет testBox</summary>
        public string[] BackgroundCheck
        {
            get => _backgroundCheck;
            set => Set(ref _backgroundCheck, value);
        }
        #endregion

        #region ParamPanelVisibility
        private string _paramPanelVisibility = "Visible";
        /// <summary>Видимость ParamPanel при WorkMode = Intensity fluctuation</summary>
        public string ParamPanelVisibility
        {
            get => _paramPanelVisibility;
            set => Set(ref _paramPanelVisibility, value);
        }
        #endregion

        #region RowPanelHeight
        private string _rowPanelHeight = "50";
        /// <summary>Высота ParamPanel</summary>
        public string RowPanelHeight
        {
            get => _rowPanelHeight;
            set => Set(ref _rowPanelHeight, value);
        }
        #endregion

        #region TestProgressVisibility
        private string _testProgressVisibility = "Hidden";
        /// <summary>Видимость testProgress</summary>
        public string TestProgressVisibility
        {
            get => _testProgressVisibility;
            set => Set(ref _testProgressVisibility, value);
        }
        #endregion

        #region MaximumTestProgress
        private string _maximumTestProgress;
        ///<summary>MaximumTestProgress</summary>
        public string MaximumTestProgress
        {
            get => _maximumTestProgress;
            set => Set(ref _maximumTestProgress, value);
        }
        #endregion

        #region ValueTestProgress
        private string _valueTestProgress;
        ///<summary>ValueTestProgress</summary>
        public string ValueTestProgress
        {
            get => _valueTestProgress;
            set => Set(ref _valueTestProgress, value);
        }
        #endregion


        /*
        #region ModeBoxItemsSource
        private ObservableCollection<string> _modeBoxItemsSource = new ObservableCollection<string>()
        {
            "Intensity fluctuation", "Bit sequence"
        };
        ///<ModeBoxItemsSource</summary>
        public ObservableCollection<string> ModeBoxItemsSource
        {
            get => _modeBoxItemsSource;
            set => Set(ref _modeBoxItemsSource, value);
        }
        #endregion
        */

        #region ModeBoxSelectedIndex
        private int _modeBoxSelectedIndex = 0;
        ///<summary>ModeBoxSelectedIndex</summary>
        public int ModeBoxSelectedIndex
        {
            get => _modeBoxSelectedIndex;
            set => Set(ref _modeBoxSelectedIndex, value);
        }
        #endregion

        #region ComboBoxSelectedIndex
        private int _comboBoxSelectedIndex = 0;
        ///<summary>Индекс Item у testBox</summary>
        public int ComboBoxSelectedIndex
        {
            get => _comboBoxSelectedIndex;
            set => Set(ref _comboBoxSelectedIndex, value);
        }
        #endregion

        #region Команды

        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region MinimizeApplicationCommand
        public ICommand MinimizeApplicationCommand { get; }

        private bool CanMinimizeApplicationCommandExecute(object p) => true;

        private void OnMinimizeApplicationCommandExecuted(object p) => WindowState = WindowState.Minimized;
        #endregion

        #region ModeBoxSelectionChangedCommand
        public ICommand ModeBoxSelectionChangedCommand { get; }

        private bool CanModeBoxSelectionChangedCommandExecute(object p) => true;

        private void OnModeBoxSelectionChangedCommandExecuted(object p)
        {
            if (ModeBoxSelectedIndex == 0)
            {
                WorkMode = true;
                ParamPanelVisibility = "Visible";
                RowPanelHeight = "50";
            }
            else
            {
                WorkMode = false;
                ParamPanelVisibility = "Hidden";
                RowPanelHeight = "0";
            }
            OpenFileTxt = null;
            BitLengthTxt = null;
        }
        #endregion

        #region OpenFileCommand
        public ICommand OpenFileCommand { get; }

        private bool CanOpenFileCommandExecute(object p) => true;

        private void OnOpenFileCommandExecuted(object p)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (WorkMode == true)
            {
                file.Filter = "Text files(*.csv)|*.csv";
            }
            else
            {
                file.Filter = "Text files(*.txt)|*.txt";
            }
            file.ShowDialog();
            if (file.FileName.Length > 0)
            {
                OpenFileTxt = file.FileName;
                ExecuteTestButtonEnabled = true;
                SaveButtonEnabled = false;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion

        #region ExecuteTestCommand
        public ICommand ExecuteTestCommand { get; }

        private bool CanExecuteTestCommandExecute(object p) => true;

        private void OnExecuteTestCommandExecuted(object p)
        {
            if (string.IsNullOrEmpty(LSBTxt) || string.IsNullOrEmpty(TimeShiftTxt) ||
                string.IsNullOrEmpty(NumberBitTxt) || (string.IsNullOrEmpty(TimeLengthTxt) && TimeLengthChecked == true))
            {
                MessageBox.Show("Fill in all the fields");
            }
            else if (Int32.Parse(LSBTxt.Trim()) >= Int32.Parse(NumberBitTxt.Trim()))
            {
                MessageBox.Show("The number of LSB's cannot be greater than the ADS bit's");
            }
            else
            {
                Reset();
                if (WorkMode == true)
                {
                    GetCSVData();
                    SaveButtonEnabled = true;
                }
                else
                {
                    using (StreamReader Read = new StreamReader(OpenFileTxt))
                    {
                        SequenceParameters._sequence = Read.ReadLine();
                        SequenceParameters._sequenceLength = SequenceParameters._sequence.Length;
                    }
                    SaveButtonEnabled = false;

                }
                BitLengthTxt = SequenceParameters._sequenceLength.ToString();

                AsyncExecuteTest();

                ComboBoxSelectedIndex = 0;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        #endregion

        #region SaveTestCommand
        public ICommand SaveTestCommand { get; }

        private bool CanSaveTestCommandExecute(object p) => true;

        private void OnSaveTestCommandExecuted(object p)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "Text files(*.txt)|*.txt";
            file.ShowDialog();
            if (file.FileName != "")
            {
                using (StreamWriter Save = new StreamWriter(file.FileName))//Запись в файл
                {
                    Save.WriteLine(SequenceParameters._sequence);
                    Save.Close();
                }
            }
        }
        #endregion

        #region SelectAllTestCommand
        public ICommand SelectAllTestCommand { get; }

        private bool CanSelectAllTestCommandExecute(object p) => true;

        private void OnSelectAllTestCommandExecuted(object p)
        {
            bool[] test = new bool[15];
            for (int i = 0; i < test.Length; i++) test[i] = true;
            TestChecked = test;
        }
        #endregion

        #region ResetCommand
        public ICommand ResetCommand { get; }

        private bool CanResetCommandExecute(object p) => true;

        private void OnResetCommandExecuted(object p)
        {
            /*
            TestChecked = new bool[] {
            false, false, false, false, false,
            false, false, false, false, false,
            false, false, false, false, false};
            */

            TestChecked = new bool[15];
            Reset();
        }
        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Команды
            CloseApplicationCommand = new ActionCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            MinimizeApplicationCommand = new ActionCommand(OnMinimizeApplicationCommandExecuted,CanMinimizeApplicationCommandExecute);
            OpenFileCommand = new ActionCommand(OnOpenFileCommandExecuted, CanOpenFileCommandExecute);
            ModeBoxSelectionChangedCommand = new ActionCommand(OnModeBoxSelectionChangedCommandExecuted, CanModeBoxSelectionChangedCommandExecute);
            ExecuteTestCommand = new ActionCommand(OnExecuteTestCommandExecuted, CanExecuteTestCommandExecute);
            SaveTestCommand = new ActionCommand(OnSaveTestCommandExecuted, CanSaveTestCommandExecute);
            SelectAllTestCommand = new ActionCommand(OnSelectAllTestCommandExecuted, CanSelectAllTestCommandExecute);
            ResetCommand = new ActionCommand(OnResetCommandExecuted, CanResetCommandExecute);
            #endregion
        }

        private void Reset()
        {
            string[] background = new string[15];
            for (int i = 0; i < background.Length; i++) background[i] = "White";
            BackgroundCheck = background;

            //List<string>[] pValueList = new List<string>[15];
            //TestListTxt = pValueList;
            if (TestListTxt != null)
                TestListTxt = new List<string>[15];

            //MessageBox.Show(TestListTxt.Length.ToString());

            BitLengthTxt = null;
        }

        private void GetCSVData()
        {
            CSVData csvData = new CSVData();
            csvData.LSB = Int32.Parse(LSBTxt.Trim().Replace('.', ','));
            csvData.numberBit = Int32.Parse(NumberBitTxt.Trim().Replace('.', ','));
            string[] dataCSVArray = File.ReadAllLines(OpenFileTxt);
            csvData.timeShift = double.Parse(TimeShiftTxt.Trim().Replace('.', ','));
            csvData.deltaTime = Math.Abs((double.Parse(dataCSVArray[0].Split(',')[0].Replace('.', ',')) - double.Parse(dataCSVArray[1].Split(',')[0].Replace('.', ','))) * Math.Pow(10, 9));
            if (TimeLengthChecked == true)
            {
                csvData.timeLength = double.Parse(TimeLengthTxt.Replace('.', ',')) + 2 * csvData.timeShift;
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
            csvData.GetSequence();
        }

        private async void AsyncExecuteTest()
        {
            TestProgressVisibility = "Visible";

            TestResult testResult = new TestResult();
            ITestNIST[] testNIST = { new Test1(), new Test2(), new Test3(), new Test4(), new Test5(),
                new Test6(), new Test7(), new Test8(), new Test9(), new Test10(),
                new Test11(), new Test12(), new Test13(), new Test14(), new Test15() };


            bool[] randStatus = new bool[15];
            List<string>[] pValueList = new List<string>[15];
            string[] background = new string[15];

            int numActiveCheck = NumActiveCheck();
            MaximumTestProgress = numActiveCheck.ToString();
            int numDidTest = 0;
            ValueTestProgress = numDidTest.ToString();

            var progressTask = Task.Run(() =>
            {
                while (numDidTest<numActiveCheck)
                {
                }
                BackgroundCheck = background;
                TestListTxt = pValueList;
                TestProgressVisibility = "Hidden";
            });

            int j = 0;
            foreach (var item in testNIST)
            {
                if (TestChecked[j] == true)
                {
                    var index = j;
                    var testTask = Task.Run(() => {
                        var result = testResult.Test(item);

                        randStatus[index] = result.Item2;
                        if (result.Item2)
                        {
                            background[index] = "Green";//Тест пройден
                        }
                        else if (result.Item1[0] == -1)
                        {
                            background[index] = "Yellow";//Последовательность не удовлетворяет начальным условиям
                        }
                        else
                        {
                            background[index] = "Red";//Тест не пройден
                        }

                        pValueList[index] = new List<string>();
                        for (int k = 0; k < result.Item1.Length; k++)
                        {
                            switch (index)
                            {
                                case 12:
                                    if (k == 0) pValueList[index].Add($"Forward: {result.Item1[k]}");
                                    if (k == 1) pValueList[index].Add($"Backward: {result.Item1[k]}");
                                    break;

                                case 13:
                                    if (k < 4) pValueList[index].Add($"x = {-4 + k}: {result.Item1[k]}");
                                    else if (k > 4) pValueList[index].Add($"x = {-3 + k}: {result.Item1[k]}");
                                    else pValueList[index].Add($"x = 1: {result.Item1[k]}");
                                    break;

                                case 14:
                                    if (k < 9) pValueList[index].Add($"x = {-9 + k}: {result.Item1[k]}");
                                    else if (k > 9) pValueList[index].Add($"x = {-8 + k}: {result.Item1[k]}");
                                    else pValueList[index].Add($"x = 1: {result.Item1[k]}");
                                    break;

                                default:
                                    pValueList[index].Add(result.Item1[k].ToString());
                                    break;
                            }
                        }
                        numDidTest++;
                        ValueTestProgress = numDidTest.ToString();
                    });
                }
                else
                {
                    background[j] = "White";
                }
                j++;
            }
            //Task.WaitAll(testTask);
            //BackgroundCheck = background;
            //TestListTxt = pValueList;
        }

        private int NumActiveCheck()
        {
            int countActiveTest = 0;
            for (int i = 0; i < TestChecked.Length; i++)
            {
                if (TestChecked[i] == true) countActiveTest++;
            }
            return countActiveTest;
        }
    }
}
