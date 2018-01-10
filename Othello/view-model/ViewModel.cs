using Othello.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.view_model
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private OthelloManager othelloManager;
        public string VisibilityGameFinishedWindow
        {
            get
            {
                if (othelloManager.IsGameFinished)
                    return "Visible";
                else
                    return "Hidden";
            }
        }

        protected virtual void OnPropertyChanged(string propertyname)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
                Console.Write("Property changed: " + propertyname);
            }
        }

        public ViewModel()
        {
            othelloManager = OthelloManager.Instance;
            othelloManager.PropertyChanged += OthelloManager_PropertyChanged;
        }

        private void OthelloManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsGameFinished")
                OnPropertyChanged("VisibilityGameFinishedWindow");
        }

    }
}
