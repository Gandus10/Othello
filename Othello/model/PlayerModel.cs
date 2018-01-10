using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Othello.model
{
    class PlayerModel : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private long timeElapsed;
        private Stopwatch roundStartedAt;
        public string TimeElapsed
        {
            get => TimeSpan.FromMilliseconds(timeElapsed).ToString(@"hh\:mm\:ss");
        }
        public DispatcherTimer Timer { get; }

        private int score;
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                OnPropertyChanged("Score");
            }
        }
        private int tokensAvaiable;
        public int TokensAvaiable
        {
            get { return tokensAvaiable; }
            set
            {
                tokensAvaiable = value;
                OnPropertyChanged("TokensAvaiable");
            }
        }


        private bool isPlaying;
        public bool IsPlaying
        {
            get => isPlaying;
            set
            {
                isPlaying = value;
                if (value == true)
                {
                    Timer.Start();
                    roundStartedAt = Stopwatch.StartNew();
                    Console.WriteLine(Name + " is playing");
                }
                else
                {
                    Timer.Stop();
                    if (roundStartedAt != null)
                    {
                        roundStartedAt.Stop();
                        //timeElapsed += roundStartedAt.ElapsedMilliseconds;
                        OnPropertyChanged("TimeElapsed");
                    }
                    //Console.WriteLine(Name + " is not playing");
                }
                OnPropertyChanged("IsPlaying");
            }
        }

        public PlayerModel(String name)
        {
            this.name = name;
            score = 0;
            TokensAvaiable = 30;
            timeElapsed = 0;
            isPlaying = false;
            Timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            Timer.Tick += DtClockTimeTick;
        }

        public void RestartTimeElapsed()
        {
            timeElapsed = 0;
            OnPropertyChanged("TimeElapsed");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void DtClockTimeTick(object sender, EventArgs e)
        {
            timeElapsed += 1000;
            OnPropertyChanged("TimeElapsed");
        }

    }
}
