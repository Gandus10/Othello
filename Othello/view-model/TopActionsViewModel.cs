using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Othello.view_model
{
    class TopActionsViewModel
    {
        public TopActionsViewModel()
        { 
        }

        private ICommand resetCommand;
        private ICommand saveCommand;
        private ICommand loadCommand;
        private ICommand undoCommand;

        public ICommand ResetCommand
        {
            get
            {
                return resetCommand ?? (resetCommand = new CommandHandler(() => Reset(), true));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new CommandHandler(() => Save(), true));
            }
        }

        public ICommand UndoCommand
        {
            get
            {
                return undoCommand ?? (undoCommand = new CommandHandler(() => Undo(), true));
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                return loadCommand ?? (loadCommand = new CommandHandler(() => Load(), true));
            }
        }


        private void Reset()
        {
            if (MessageBox.Show("Êtes-vous sûre de réinitialiser la partie?", "Reset de la partie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                OthelloManager.Instance.ResetGame();
        }

        private void Save()
        {
            Console.WriteLine("TODO: SAVE");
        }

        private void Undo()
        {
            Console.WriteLine("TODO: UNDO");
        }

        private void Load()
        {
            Console.WriteLine("TODO: LOAD");
        }


    }

    public class CommandHandler : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action action;
        private bool canExecute;

        public CommandHandler(Action action, bool canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
