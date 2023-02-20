using Sequence.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Sequence.Models;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;
using Sequence.Infrastructure.Commands;
using System.ComponentModel;
using System.Windows.Controls;
using Sequence.TestNIST;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;
using System.Reflection;
using Sequence.Factories;

namespace Sequence.ViewModels
{
    internal class MainWindowViewModel: ViewModel, IDataErrorInfo
    {
        private bool _isValidatedBitADC = false;
        private bool _isValidatedLSB = false;
        private bool _isValidatedTimeShift = false;
        private bool _isValidatedTimeSeries = false;

        private InputModel _inputModel;
        OpenFileDialog _file;
        SaveFileDialog _saveFile;

        public MainWindowViewModel()
        {
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                try
                {
                    _inputModel = JsonSerializer.Deserialize<InputModel>(fs);
                }
                catch(JsonException ex)
                {
                    _inputModel = new InputModel();
                }
            }

            TxtBitADC = _inputModel.BitADC.ToString();
            TxtLSB = _inputModel.NumberLSB.ToString();
            TxtTimeShift = _inputModel.TimeShift.ToString();
            IsCustomTime = _inputModel.IsCustomTimeSeries;
            TxtTimeSeries = _inputModel.TimeSeries.ToString();

            OpenFileCommand = new ActionCommand(OnOpenFileCommandExecuted,
                                                CanOpenFileCommandExecute);
            ExecuteTestCommand = new ActionCommand(OnExecuteTestCommandExecutedAsync,
                                                   CanExecuteTestCommandExecute);
            ModeBoxSelectionChangedCommand = new ActionCommand(OnModeBoxSelectionChangedCommandExecuted,
                                                               CanModeBoxSelectionChangedCommandExecute);
            SerializedInputModelCommand = new ActionCommand(OnSerializedInputModelCommandExecuted,
                                                            CanSerializedInputModelCommandExecute);
            SelectAllCommand = new ActionCommand(OnSelectAllCommandExecuted,
                                                 CanSelectAllCommandExecute);
            ResetCommand = new ActionCommand(OnResetCommandExecuted,
                                             CanResetCommandExecute);
            SaveSequenceCommand = new ActionCommand(OnSaveSequenceExecuted,
                                                    CanSaveSequenceExecute);
        }

        ////////////////////////////////////////

        #region Properties

        #region Path
        private string _txtPath;
        ///<summary>Путь к csv-файлу</summary>
        public string TxtPath
        {
            get => _txtPath;
            set => Set(ref _txtPath, value);
        }
        #endregion

        #region BitADC
        private string _txtBitADC;
        ///<summary>Разрядность аналого-цифрового преобразователя</summary>
        public string TxtBitADC
        {
            get => _txtBitADC;
            set => Set(ref _txtBitADC, value);
        }
        #endregion

        #region LSB
        private string _txtLSB;
        ///<summary>Количество сохраняемых младших разрядов</summary>
        public string TxtLSB
        {
            get => _txtLSB;
            set => Set(ref _txtLSB, value);
        }
        #endregion

        #region TimeShift
        private string _txtTimeShift;
        ///<summary>Временной сдвиг между двумя временными рядами</summary>
        public string TxtTimeShift
        {
            get => _txtTimeShift;
            set => Set(ref _txtTimeShift, value);
        }
        #endregion

        #region TimeSeries
        private string _txtTimeSeries;
        ///<summary>Длина временного ряда</summary>
        public string TxtTimeSeries
        {
            get => _txtTimeSeries;
            set => Set(ref _txtTimeSeries, value);
        }
        #endregion

        #region IsCustomTime
        private bool _isCustomTime;
        ///<summary>Использовать длину временного ряда свою?</summary>
        public bool IsCustomTime
        {
            get => _isCustomTime;
            set
            {
                Set(ref _isCustomTime, value);
                OnPropertyChanged(nameof(TxtTimeSeries));
            }
        }
        #endregion

        #region BitSequenceLength
        private string _bitSequenceLength;
        ///<summary>Длина битовой последовательности</summary>
        public string BitSequenceLength
        {
            get => _bitSequenceLength;
            set => Set(ref _bitSequenceLength, value);
        }
        #endregion

        #region TestListTxt
        private List<string>[] _testListTxt;
        ///<summary>Результаты проведения тестов</summary>
        public List<string>[]? TestListTxt
        {
            get => _testListTxt;
            set => Set(ref _testListTxt, value);
        }
        #endregion

        #region Technical Properties

        #region Waiting Action
        private bool _waitingAction = true;
        ///<summary>Доступность полей настройки последовательности</summary>
        public bool WaitingAction
        {
            get => _waitingAction;
            set => Set(ref _waitingAction, value);
        }
        #endregion

        #region SelectedIndex
        private int _selectedIndex = 0;
        ///<summary>Выбранный индекс ComboBox</summary>
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value);
        }
        #endregion

        #region VisibilityProgressBar
        private string _visibilityProgressBar = "Hidden";
        /// <summary>Видимость ProgressBar</summary>
        public string VisibilityProgressBar
        {
            get => _visibilityProgressBar;
            set => Set(ref _visibilityProgressBar, value);
        }
        #endregion

        #region TestChecked
        private bool[] _testChecked = new bool[] {
            false, false, false, false, false,
            false, false, false, false, false,
            false, false, false, false, false};
        /// <summary>Массив выбранных NIST-тестов</summary>
        public bool[] TestChecked
        {
            get => _testChecked;
            set => Set(ref _testChecked, value);
        }
        #endregion

        #region Maximum TestProgress
        private int _maximumTestProgress;
        /// <summary>Минимальное значение ProgressBar</summary>
        public int MaximumTestProgress
        {
            get => _maximumTestProgress;
            set => Set(ref _maximumTestProgress, value);
        }
        #endregion

        #region BackgroundCheck
        private string[] _backgroundCheck = new string[]
        {
            "White", "White", "White", "White", "White",
            "White", "White", "White", "White", "White",
            "White", "White", "White", "White", "White"
        };
        /// <summary>Цвет CheckBox результатов</summary>
        public string[] BackgroundCheck
        {
            get => _backgroundCheck;
            set => Set(ref _backgroundCheck, value);
        }
        #endregion

        #region Value TestProgress
        private int _valueTestProgress;
        ///<summary>Величина прогрессии ProgressBar</summary>
        public int ValueTestProgress
        {
            get => _valueTestProgress;
            set => Set(ref _valueTestProgress, value);
        }
        #endregion

        #region ComboBox Selected Index
        private int _comboBoxSelectedIndex = 0;
        ///<summary>Установленный индекс ComboBox результатов</summary>
        public int ComboBoxSelectedIndex
        {
            get => _comboBoxSelectedIndex;
            set => Set(ref _comboBoxSelectedIndex, value);
        }
        #endregion

        #endregion

        #endregion

        ///////////////////////////////////////

        #region Commands

        #region Open File
        public ICommand OpenFileCommand { get; }

        private bool CanOpenFileCommandExecute(object p) => WaitingAction;

        private void OnOpenFileCommandExecuted(object p)
        {
            _file = new OpenFileDialog();
            if (SelectedIndex == 0)
            {
                _file.Filter = "Text files(*.csv)|*.csv";
            }
            else
            {
                _file.Filter = "Text files(*.txt)|*.txt";
            }
            _file.ShowDialog();
            if (_file.FileName.Length > 0)
            {
                TxtPath = Path.GetFileName(_file.FileName);
            }
        }
        #endregion

        #region Execute Test
        public ICommand ExecuteTestCommand { get; }

        private bool CanExecuteTestCommandExecute(object p) => WaitingAction & _file != null 
                                                             & (SelectedIndex == 1 || (SelectedIndex == 0
                                                                                       & _isValidatedBitADC
                                                                                       & _isValidatedLSB 
                                                                                       & _isValidatedTimeShift
                                                                                       & _isValidatedTimeSeries));

        private async void OnExecuteTestCommandExecutedAsync(object p)
        {
            ClearField();
            MaximumTestProgress = NumActiveCheck();
            ChangeActionUI("Visible");

            _inputModel = new InputModel(_file.FileName, TxtBitADC, TxtLSB, TxtTimeShift, IsCustomTime, TxtTimeSeries);

            SequenceMaker sequenceMaker = new SequenceMaker(_inputModel, SelectedIndex);
            if (await Task.Run(() => sequenceMaker.GetSequence()))
            {
                BitSequenceLength = SequenceParameters._sequenceLength.ToString();
                await InitiateTestsAsync();
            }
            else
            {
                ChangeActionUI("Hidden");
            }
        }
        private async Task InitiateTestsAsync()
        {
            List<string>[] pValueList = new List<string>[15];
            string[] background = new string[15];

            TestResult testResult = new TestResult();
            List<ITestNIST> testNIST = new List<ITestNIST>();
            List<int> indexTest = SwitchOnTest(ref testNIST, ref background);

            Task[] tasks = new Task[testNIST.Count];

            int i = 0;
            foreach (var test in testNIST)
            {
                int index = indexTest[i];
                tasks[i] = new Task(() =>
                {
                    var result = testResult.Test(test);

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
                        result.Item1[k] = Math.Round(result.Item1[k], 13);
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
                    ValueTestProgress++;
                });
                tasks[i].Start();
                i++;
            }

            await Task.Run(() =>
            {
                Task.WaitAll(tasks);

                TestListTxt = pValueList;
                BackgroundCheck = background;
                ChangeActionUI("Hidden");

                GC.Collect();
                GC.WaitForPendingFinalizers();
            });
        }

        private List<int> SwitchOnTest(ref List<ITestNIST> testNIST, ref string[] background)
        {
            List<int> indexTest= new List<int>();

            for (int i = 0; i < TestChecked.Length; i++)
            {
                if (TestChecked[i])
                {
                    testNIST.Add(FactoryGetter.GetFactory(i).GetTestNIST());
                    indexTest.Add(i);
                }
                else
                {
                    background[i] = "White";
                }
            }

            return indexTest;
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

        private void ChangeActionUI(string visibility)
        {
            if (visibility == "Hidden")
            {
                VisibilityProgressBar = visibility;
                WaitingAction = true;
                ComboBoxSelectedIndex = 0;
            }
            else
            {
                ValueTestProgress = 0;
                WaitingAction = false;
                VisibilityProgressBar = visibility;
            }
        }
        #endregion

        #region ModeBox SelectionChanged
        public ICommand ModeBoxSelectionChangedCommand { get; }
        private bool CanModeBoxSelectionChangedCommandExecute(object p) => true;

        private void OnModeBoxSelectionChangedCommandExecuted(object p)
        {
            TxtPath = "";
            _file = null;
        }
        #endregion

        #region Serialized InputModel
        public ICommand SerializedInputModelCommand { get; }
        private bool CanSerializedInputModelCommandExecute(object p) => true;
        private void OnSerializedInputModelCommandExecuted(object p)
        {
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                try
                {
                    _inputModel = new InputModel("", TxtBitADC, TxtLSB, TxtTimeShift, IsCustomTime, TxtTimeSeries);
                    JsonSerializer.Serialize<InputModel>(fs, _inputModel);
                }
                catch (JsonException ex)
                {
                    MessageBox.Show("Error saving user data");
                }
            }
        }
        #endregion

        #region Save Sequence Command
        public ICommand SaveSequenceCommand { get; }
        private bool CanSaveSequenceExecute(object p) => WaitingAction
                                                      && SequenceParameters._sequenceLength > 0;
        private void OnSaveSequenceExecuted(object p)
        {
            _saveFile = new SaveFileDialog();
            _saveFile.Filter = "Text files(*.txt)|*.txt";
            _saveFile.FileName = "Bit Sequense " + Regex.Match(TxtPath, @"(.*)(?=.csv)").Value;
            _saveFile.ShowDialog();
            if (_saveFile.FileName != "")
            {
                using (StreamWriter Save = new StreamWriter(_saveFile.FileName))
                {
                    Save.WriteLine(SequenceParameters._sequence);
                    Save.Close();
                }
            }
        }
        #endregion

        #region Select All Command
        public ICommand SelectAllCommand { get; }
        private bool CanSelectAllCommandExecute(object p) => WaitingAction;
        private void OnSelectAllCommandExecuted(object p)
        {
            ChangeCheck(true);
        }

        private void ChangeCheck(bool check)
        {
            bool[] test = new bool[15];
            for (int i = 0; i < test.Length; i++) test[i] = check;
            TestChecked = test;
        }
        #endregion

        #region Reset Command
        public ICommand ResetCommand { get; }
        private bool CanResetCommandExecute(object p) => WaitingAction;
        private void OnResetCommandExecuted(object p)
        {
            ChangeCheck(false);
            ClearField();
        }
        private void ClearField()
        {
            TestListTxt = null;
            BitSequenceLength = String.Empty;

            string[] background = new string[15];
            for (int i = 0; i < background.Length; i++)
                background[i] = "White";
            BackgroundCheck = background;
        }
        #endregion

        #endregion

        ///////////////////////////////////////
        
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;

                switch (columnName)
                {
                    case nameof(TxtBitADC):
                        if (int.TryParse(TxtBitADC, out var valADC) && valADC>=2)
                        {
                            _isValidatedBitADC = true;
                        }
                        else
                        {
                            error = "Enter an integer starting with 2";
                            _isValidatedBitADC = false;
                        }
                        break;
                    case nameof(TxtLSB):
                        int.TryParse(TxtBitADC, out var valADC2);
                        if (int.TryParse(TxtLSB, out var valLSB) && valLSB >= 1 && valLSB <= valADC2)
                        {
                            _isValidatedLSB = true;
                        }
                        else
                        {
                            error = "Enter an integer between 2 and ADC bit's";
                            _isValidatedLSB = false;
                        }
                        break;
                    case nameof(TxtTimeShift):
                        if (int.TryParse(TxtTimeShift, out var valShift) && valShift >= 0)
                        {
                            _isValidatedTimeShift = true;
                        }
                        else
                        {
                            error = "Enter a positive integer";
                            _isValidatedTimeShift = false;
                        }
                        break;
                    case nameof(TxtTimeSeries):
                        if (int.TryParse(TxtTimeSeries, out var valSeries) && valSeries >= 1 || !IsCustomTime)
                        {
                            _isValidatedTimeSeries = true;
                        }
                        else
                        {
                            error = "Enter a positive integer";
                            _isValidatedTimeSeries = false;
                        }
                        break;
                }
                return error;
            }
        }

        public string Error => string.Empty;
    }
}
