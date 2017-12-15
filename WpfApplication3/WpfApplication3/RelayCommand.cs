using System;
using System.Windows.Input;

namespace WpfApplication3
{
    public class RelayCommand : ICommand
    {
        readonly Func<bool> canUpdateTitleExecute;
        readonly Action commandExcute;

        public RelayCommand(Action excute)
            : this(excute, null)
        {
        }

        public RelayCommand(Action updateTitleExecute, Func<bool> canUpdateTitleExecute)
        {
            if (updateTitleExecute==null)
            {
                throw new ArgumentException("excute");
            }
            this.commandExcute = updateTitleExecute;
            this.canUpdateTitleExecute = canUpdateTitleExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canUpdateTitleExecute != null)
                    CommandManager.RequerySuggested += value;
            }

            remove
            {
                if (canUpdateTitleExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return canUpdateTitleExecute == null ? true : canUpdateTitleExecute();
        }

        public void Execute(object parameter)
        {
            commandExcute();
        }
    }
}