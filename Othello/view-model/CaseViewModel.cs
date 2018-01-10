using Othello.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Othello.view_model
{
    class CaseViewModel : INotifyPropertyChanged
    {
        //************************************//
        // INotifyPropertyChanged implemented //
        //************************************//
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //***************************//
        // Properties and attributes //
        //***************************//
        private OthelloManager othelloManager;
        public string PathToImage
        {
            get { return CaseBoard.PathImage; }
            set
            {
                CaseBoard.PathImage = value;
                OnPropertyChanged("PathToImage");
            }
        }

        public CaseModel CaseBoard { get; set; }
        public ICommand Command { get; set; }

        //*************//
        // Constructor //
        //*************//
        public CaseViewModel(int posX, int posY)
        {
            othelloManager = OthelloManager.Instance;
            CaseBoard = new CaseModel(posX, posY);
            PathToImage = CaseBoard.PathImage; //TODO: Exception Generated...if null
            Command = new CommandeCaseClick(ExecuteCaseClick, CanExecuteCaseClick);
            othelloManager.AddCaseModel(CaseBoard);

            //Subscribe to PropertyChanged of the model
            CaseBoard.PropertyChanged += CaseModel_PropertyChanged;
        }

        //***********************//
        // Control and execution //
        //***********************//
        public void ExecuteCaseClick(object parameter)
        {
            if (othelloManager.CurrentPlayer == Properties.Settings.Default.Black)
                othelloManager.PlayMove(CaseBoard.PosX, CaseBoard.PosY, false);
            else
                othelloManager.PlayMove(CaseBoard.PosX, CaseBoard.PosY, true);
            /*CaseBoard.Owner = othelloManager.CurrentPlayer;
            othelloManager.AddCasePlayed(CaseBoard);
            othelloManager.SetNextPlayer();
            othelloManager.UpdateAllCasesPlayable();*/
        }

        public bool CanExecuteCaseClick(object parameter)
        {
            return othelloManager.IsCasePlayable(CaseBoard);
        }

        private void CaseModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PathImage")
                OnPropertyChanged("PathToImage");
        }
    }
}
