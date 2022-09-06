﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

        /// <summary>Состояние окна</summary>
        private WindowState _windowState = WindowState.Normal;

        public WindowState WindowState
        {
            get => _windowState;
            set => Set(ref _windowState, value);
        }

        #endregion

        #region Mouse e
        /// <summary>Windows Tag</summary>
        private MouseButtonEventArgs _e;
        public MouseButtonEventArgs E
        {
            get => _e;
            set => Set(ref _e, value);
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

        #endregion

        public MainWindowViewModel()
        {
            #region Команды
            CloseApplicationCommand = new ActionCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            MinimizeApplicationCommand = new ActionCommand(OnMinimizeApplicationCommandExecuted,CanMinimizeApplicationCommandExecute);
            ///MouseMoveCommand = new ActionCommand(OnMouseMoveCommandExecuted, CanMouseMoveCommandExecute);
            #endregion
        }
    }
}
