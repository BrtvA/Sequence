using Sequence.Infrastructure.Commands;
using Sequence.ViewModels.Base;
using Sequence.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sequence.ViewModels
{
    class WindowControlViewModel : ViewModel
    {
        public WindowControlViewModel() {
            CloseApplicationCommand = new ActionCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            MinimizeApplicationCommand = new ActionCommand(OnMinimizeApplicationCommandExecuted, CanMinimizeApplicationCommandExecute);
        }

        #region Commands

        /// <summary>
        /// Команда закрытия окна
        /// </summary>
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Команда сворачивания окна
        /// </summary>
        public ICommand MinimizeApplicationCommand { get; }

        private bool CanMinimizeApplicationCommandExecute(object p) => true;

        private void OnMinimizeApplicationCommandExecuted(object p)
        {
            MainWindow window = (MainWindow)p;
            window.WindowState = WindowState.Minimized;
        }

        #endregion
    }
}
