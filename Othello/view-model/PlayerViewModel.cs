using Othello.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.view_model
{
    class PlayerViewModel : INotifyPropertyChanged
    {
        private PlayerModel player1;
        private PlayerModel player2;
        private OthelloManager othelloManager;
        private PlayerModel playerPlaying;
        private PlayerModel PlayerPlaying
        {
            get { return playerPlaying; }
            set
            {
                playerPlaying = value;
                OnPropertyChanged("PlayerPlaying");
            }
        }

        public int RotationAngle { get; set; }

        public PlayerModel Player1 { get { return player1; } }
        public PlayerModel Player2 { get { return player2; } }
        public PlayerViewModel()
        {
            othelloManager = OthelloManager.Instance;
            player1 = othelloManager.TabPlayerModel[0];
            player2 = othelloManager.TabPlayerModel[1];
            RotationAngle = -90;
            playerPlaying = Player1;
            PlayerPlaying.PropertyChanged += PlayerModel_PropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PlayerModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPlaying")
            {
                RotationAngle *= -1;
                OnPropertyChanged("RotationAngle");
            }
        }
    }
}
