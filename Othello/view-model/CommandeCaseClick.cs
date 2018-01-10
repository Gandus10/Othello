using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Othello.view_model
{
    class CommandeCaseClick : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public delegate void ICommandOnExecute(object parameter);
        public delegate bool ICommandOnCanExecute(object paramter);

        private ICommandOnExecute onExecute;
        private ICommandOnCanExecute canExecute;


        public CommandeCaseClick(ICommandOnExecute commmandOnExecute, ICommandOnCanExecute commandOnCanExecute)
        {
            onExecute = commmandOnExecute;
            canExecute = commandOnCanExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            onExecute.Invoke(parameter);
        }
    }
}
