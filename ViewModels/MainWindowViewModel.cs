using System;
using System.Collections.Generic;
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

        #region WorkMode
        private bool _workMode;
        /// <summary>Состояние окна</summary>
        public bool WorkMode
        {
            get => _workMode;
            set => Set(ref _workMode, value);
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

        #region OpenFileCommand
        public ICommand OpenFileCommand { get; }

        private bool CanOpenFileCommanddExecute(object p) => true;

        private void OnOpenFileCommandExecuted(object p)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Text files(*.csv)|*.csv";
            file.ShowDialog();
            OpenFileTxt = file.FileName;
        }
        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Команды
            CloseApplicationCommand = new ActionCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            MinimizeApplicationCommand = new ActionCommand(OnMinimizeApplicationCommandExecuted,CanMinimizeApplicationCommandExecute);
            OpenFileCommand = new ActionCommand(OnOpenFileCommandExecuted, CanOpenFileCommanddExecute);
            #endregion
        }
    }
}
