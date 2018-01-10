using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.model
{
    class CaseModel : INotifyPropertyChanged
    {
        public bool Free { get; set; }

        private int owner;
        public int Owner
        {
            get { return owner; }
            set
            {
                //Check if owner number is correct
                Debug.Assert(value == Properties.Settings.Default.Black || value == Properties.Settings.Default.White, "Owner value not valid -> " + value);

                //Set the image of the case and update score
                if (value == Properties.Settings.Default.Black)
                {
                    PathImage = Properties.Settings.Default.ImageBlackCase;
                    OthelloManager.Instance.TabPlayerModel[Properties.Settings.Default.BlackPlayer].Score += 1;
                    if(owner != 0)
                        OthelloManager.Instance.TabPlayerModel[Properties.Settings.Default.WhitePlayer].Score -= 1;


                }
                else
                {
                    PathImage = Properties.Settings.Default.ImageWhiteCase;
                    OthelloManager.Instance.TabPlayerModel[Properties.Settings.Default.WhitePlayer].Score += 1;
                    if (owner != 0)
                        OthelloManager.Instance.TabPlayerModel[Properties.Settings.Default.BlackPlayer].Score -= 1;
                }

                Free = false;
                IsPlayable = false;

                owner = value;
                OnPropertyChanged("Owner");                
            }
        }

        private string pathImage;
        public string PathImage
        {
            get { return pathImage; }
            set
            {
                pathImage = value;
                OnPropertyChanged("PathImage");
            }
        }

        private string border;
        public string Border
        {
            get { return border; }
            set {
                border = value;
                OnPropertyChanged("Border");
            }
        }


        public bool IsPlayable { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        public CaseModel(int posX, int posY)
        {
            Free = true;
            IsPlayable = true;
            PosX = posX;
            PosY = posY;
            border = "Black";
        }

        public void CleanCase()
        {
            Free = true;
            IsPlayable = true;
            border = "Black";
            owner = 0;
            PathImage = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
