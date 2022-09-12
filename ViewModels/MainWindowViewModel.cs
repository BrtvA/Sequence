using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Sequence.Infrastructure.Commands;
using Sequence.ViewModels.Base;

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
        /// <summary>Активность SaveButton</summary>
        public bool TimeLengthChecked
        {
            get => _timeLengthChecked;
            set => Set(ref _timeLengthChecked, value);
        }
        #endregion

        #region TestChecked

        #region Test1Checked
        private bool _test1Checked = false;
        /// <summary>Активация test1Box</summary>
        public bool Test1Checked
        {
            get => _test1Checked;
            set => Set(ref _test1Checked, value);
        }
        #endregion

        #region Test2Checked
        private bool _test2Checked = false;
        /// <summary>Активация test2Box</summary>
        public bool Test2Checked
        {
            get => _test2Checked;
            set => Set(ref _test2Checked, value);
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

        #region ModeBoxSelectedValue
        private int _modeBoxSelectedIndex = 0;
        ///<summary>ModeBoxSelectedIndex</summary>
        public int ModeBoxSelectedIndex
        {
            get => _modeBoxSelectedIndex;
            set => Set(ref _modeBoxSelectedIndex, value);
        }
        #endregion

        #region Команды

        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
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

        #endregion

        public MainWindowViewModel()
        {
            #region Команды
            CloseApplicationCommand = new ActionCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            MinimizeApplicationCommand = new ActionCommand(OnMinimizeApplicationCommandExecuted,CanMinimizeApplicationCommandExecute);
            OpenFileCommand = new ActionCommand(OnOpenFileCommandExecuted, CanOpenFileCommandExecute);
            ModeBoxSelectionChangedCommand = new ActionCommand(OnModeBoxSelectionChangedCommandExecuted, CanModeBoxSelectionChangedCommandExecute);
            #endregion
        }
    }
}
