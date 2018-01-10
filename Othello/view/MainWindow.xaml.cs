using Othello.view;
using Othello.view_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Create the board
            for (int i = 0; i < 8; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                RowDefinition rowDefinition = new RowDefinition();
                board.ColumnDefinitions.Add(columnDefinition);
                board.RowDefinitions.Add(rowDefinition);
            }


            //Fill each case of the board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    CaseView caseBoard = new CaseView(j, i);
                    Grid.SetRow(caseBoard, i);
                    Grid.SetColumn(caseBoard, j);
                    board.Children.Add(caseBoard);
                }
            }
            //Init the game
            OthelloManager.Instance.InitGame();

            this.DataContext = new ViewModel();


        }
    }
}
