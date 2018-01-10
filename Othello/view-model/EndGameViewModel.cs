using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Othello.view_model
{
    class EndGameViewModel : INotifyPropertyChanged
    {
        private OthelloManager othelloManager;

        public String Winner
        {
            get
            {
                if (othelloManager.TabPlayerModel[Properties.Settings.Default.BlackPlayer].Score > othelloManager.TabPlayerModel[Properties.Settings.Default.WhitePlayer].Score)
                    return othelloManager.TabPlayerModel[Properties.Settings.Default.BlackPlayer].Name;
                else if (othelloManager.TabPlayerModel[Properties.Settings.Default.BlackPlayer].Score < othelloManager.TabPlayerModel[Properties.Settings.Default.WhitePlayer].Score)
                    return othelloManager.TabPlayerModel[Properties.Settings.Default.WhitePlayer].Name;
                else
                    return "nobody";
            }
        }

        public String ImageWinner
        {
            get
            {
                if (Winner == othelloManager.TabPlayerModel[Properties.Settings.Default.BlackPlayer].Name)
                    return Properties.Settings.Default.ImageBlackCase;
                else if (Winner == othelloManager.TabPlayerModel[Properties.Settings.Default.WhitePlayer].Name)
                    return Properties.Settings.Default.ImageWhiteCase;
                else
                    return "/images/turn.png";
            }
        }

        public EndGameViewModel()
        {
            othelloManager = OthelloManager.Instance;
            othelloManager.PropertyChanged += OthelloManager_PropertyChanged;
        }

        private void OthelloManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsGameFinished")
            {
                OnPropertyChanged("Winner");
                OnPropertyChanged("ImageWinner");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private ICommand closeBash;
        public ICommand CloseBash
        {
            get
            {
                closeBash = closeBash ?? new CommandeCaseClick(
                        param => this.CloseWindow(),
                        param => this.CanClose()
                    );
                return closeBash;
            }
        }

        private bool CanClose()
        {
            return true;
        }

        private void CloseWindow()
        {
            othelloManager.IsGameFinished = false;
        }
    }
}
