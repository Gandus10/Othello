﻿using Othello.view_model;
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

namespace Othello.view
{
    /// <summary>
    /// Logique d'interaction pour Case.xaml
    /// </summary>
    public partial class CaseView : UserControl
    {
        public CaseView(int posX, int posY)
        {
            InitializeComponent();
            this.DataContext = new CaseViewModel(posX, posY);
        }
    }
}
