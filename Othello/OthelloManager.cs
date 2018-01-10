using Othello.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class OthelloManager : INotifyPropertyChanged, IPlayable
    {
        private static OthelloManager instance;
        private CaseModel[,] tabCaseModel;
        public PlayerModel[] TabPlayerModel { get; private set; }
        public int CurrentPlayer { get; set; }
        private List<CaseModel> listCasesPlayed;
        private List<Tuple<CaseModel, List<CaseModel>>> listSeriesToReverse;
        private bool isGameFinished;
        public bool IsGameFinished { get => isGameFinished;
            set {
                isGameFinished = value;
                OnPropertyChanged("IsGameFinished");
            } }

        private OthelloManager()
        {
            tabCaseModel = new CaseModel[8, 8];
            TabPlayerModel = new PlayerModel[2];
            TabPlayerModel[0] = new PlayerModel("NSA");
            TabPlayerModel[1] = new PlayerModel("Anonymous");
            CurrentPlayer = Properties.Settings.Default.Black;
            listCasesPlayed = new List<CaseModel>();
            listSeriesToReverse = new List<Tuple<CaseModel, List<CaseModel>>>();
            isGameFinished = false;
        }

        public static OthelloManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OthelloManager();
                }
                return instance;
            }
        }

        public void SetNextPlayer()
        {
            if (CurrentPlayer == Properties.Settings.Default.Black)
            {
                CurrentPlayer = Properties.Settings.Default.White;
                TabPlayerModel[Properties.Settings.Default.WhitePlayer].IsPlaying = true;
                TabPlayerModel[Properties.Settings.Default.BlackPlayer].IsPlaying = false;
            }
            else
            {
                CurrentPlayer = Properties.Settings.Default.Black;
                TabPlayerModel[Properties.Settings.Default.WhitePlayer].IsPlaying = false;
                TabPlayerModel[Properties.Settings.Default.BlackPlayer].IsPlaying = true;
            }
        }

        public void InitGame()
        {
            //Set Onwer
            tabCaseModel[3, 3].Owner = Properties.Settings.Default.Black;
            tabCaseModel[4, 4].Owner = Properties.Settings.Default.Black;
            tabCaseModel[3, 4].Owner = Properties.Settings.Default.White;
            tabCaseModel[4, 3].Owner = Properties.Settings.Default.White;

            //Save cases played
            AddCasePlayed(tabCaseModel[3, 3]);
            AddCasePlayed(tabCaseModel[4, 4]);
            AddCasePlayed(tabCaseModel[3, 4]);
            AddCasePlayed(tabCaseModel[4, 3]);

            UpdateAllCasesPlayable();
        }

        public void UpdateCasesOwner(CaseModel caseClicked)
        {
            foreach (Tuple<CaseModel, List<CaseModel>> tupleCaseClickedListToReverse in listSeriesToReverse)
            {
                if (tupleCaseClickedListToReverse.Item1 == caseClicked)
                    ReverseAllCases(tupleCaseClickedListToReverse.Item2);
            }
            listSeriesToReverse.Clear();
        }
        public void UpdateAllCasesPlayable()
        {
            bool isCaseAvaiableToPlay = false;
            foreach (CaseModel caseModel in tabCaseModel)
            {
                if (caseModel.Free)
                {
                    if (CheckIfNeighbordOpposent(caseModel) && CheckIfReallyPlayable(caseModel))
                    {
                        caseModel.Border = "Red";
                        caseModel.IsPlayable = true;
                        isCaseAvaiableToPlay = true;
                        isGameFinished = false;
                    }
                    else
                    {
                        caseModel.Border = "Black";
                        caseModel.IsPlayable = false;
                    }
                }
                else
                {
                    caseModel.Border = "Black";
                    caseModel.IsPlayable = false;
                }
            }
            //Check if Game finished
            if (isGameFinished || listCasesPlayed.Count == 64)
            {
                CloseGame();
            }
            else if (!isCaseAvaiableToPlay) //If there no case to play, it's the other player
            {
                isGameFinished = true;
                SetNextPlayer();
                UpdateAllCasesPlayable();
            }
        }

        private bool CheckIfReallyPlayable(CaseModel caseModel)
        {
            List<CaseModel> matchWith = new List<CaseModel>();
            foreach (CaseModel casePlayed in listCasesPlayed)
            {
                List<CaseModel> tmpListToReverse = new List<CaseModel>();

                if (casePlayed.Owner == CurrentPlayer && IsCaseInV8Path(caseModel, casePlayed))
                {
                    bool findEmptyOrOurToken = false;

                    if (caseModel.PosX == casePlayed.PosX)
                    {
                        CaseModel caseMinY = caseModel.PosY < casePlayed.PosY ? caseModel : casePlayed;
                        CaseModel caseMaxY = caseModel.PosY > casePlayed.PosY ? caseModel : casePlayed;
                        //Check if they are not neighbord
                        if (caseMinY.PosY + 1 != caseMaxY.PosY)
                        {
                            //Run throught all cases to check if all are of the opposite team
                            for (int i = caseMinY.PosY + 1; i < caseMaxY.PosY; i++)
                            {
                                if (tabCaseModel[caseModel.PosX, i].Owner == CurrentPlayer || tabCaseModel[caseModel.PosX, i].Free)
                                {
                                    findEmptyOrOurToken = true;
                                    break;
                                }
                                else
                                    tmpListToReverse.Add(tabCaseModel[caseModel.PosX, i]);
                            }
                            if (!findEmptyOrOurToken)
                            {
                                matchWith.Add(casePlayed);
                                listSeriesToReverse.Add(new Tuple<CaseModel, List<CaseModel>>(caseModel, tmpListToReverse));
                            }
                        }
                    }
                    else if (caseModel.PosY == casePlayed.PosY)
                    {
                        CaseModel caseMinX = caseModel.PosX < casePlayed.PosX ? caseModel : casePlayed;
                        CaseModel caseMaxX = caseModel.PosX > casePlayed.PosX ? caseModel : casePlayed;
                        //Check if they are not neighbord
                        if (caseMinX.PosX + 1 != caseMaxX.PosX)
                        {
                            //Run throught all cases to check if all are of the opposite team
                            for (int i = caseMinX.PosX + 1; i < caseMaxX.PosX; i++)
                            {
                                if (tabCaseModel[i, caseModel.PosY].Owner == CurrentPlayer || tabCaseModel[i, caseModel.PosY].Free)
                                {
                                    findEmptyOrOurToken = true;
                                    break;
                                }
                                else
                                    tmpListToReverse.Add(tabCaseModel[i, caseModel.PosY]);
                            }
                            if (!findEmptyOrOurToken)
                            {
                                matchWith.Add(casePlayed);
                                listSeriesToReverse.Add(new Tuple<CaseModel, List<CaseModel>>(caseModel, tmpListToReverse));
                            }
                        }
                    }
                    else
                    {
                        //Calculate the step in x and y
                        int stepX = casePlayed.PosX > caseModel.PosX ? 1 : -1;
                        int stepY = casePlayed.PosY > caseModel.PosY ? 1 : -1;
                        //Check if they are not neighbord
                        if (caseModel.PosX + stepX != casePlayed.PosX)
                        {
                            //Run throught all cases to check if all are of the opposite team
                            for (int i = 1; i < Math.Abs(caseModel.PosX - casePlayed.PosX); i++)
                            {
                                if (tabCaseModel[caseModel.PosX + i * stepX, caseModel.PosY + i * stepY].Owner == CurrentPlayer || tabCaseModel[caseModel.PosX + i * stepX, caseModel.PosY + i * stepY].Free)
                                {
                                    findEmptyOrOurToken = true;
                                    break;
                                }
                                else
                                    tmpListToReverse.Add(tabCaseModel[caseModel.PosX + i * stepX, caseModel.PosY + i * stepY]);
                            }
                            if (!findEmptyOrOurToken)
                            {
                                matchWith.Add(casePlayed);
                                listSeriesToReverse.Add(new Tuple<CaseModel, List<CaseModel>>(caseModel, tmpListToReverse));
                            }
                        }
                    }
                }
            }
            if (matchWith.Any())
            {
                return true;
            }
            return false;
        }

        private void ReverseAllCases(List<CaseModel> casesToReverse)
        {
            foreach (CaseModel caseToReverse in casesToReverse)
            {
                if (caseToReverse.Owner != CurrentPlayer)
                    caseToReverse.Owner = CurrentPlayer;
            }
        }

        private bool CheckIfNeighbordOpposent(CaseModel caseModel)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (
                        !(i == 0 && j == 0) &&
                        (caseModel.PosX + i >= 0 && caseModel.PosX + i <= 7) &&
                        (caseModel.PosY + j >= 0 && caseModel.PosY + j <= 7)
                        )
                    {
                        CaseModel currentCase = tabCaseModel[caseModel.PosX + i, caseModel.PosY + j];
                        if ((!currentCase.Free) && (currentCase.Owner != CurrentPlayer))
                            return true;
                    }
                }
            }
            return false;
        }
        private bool IsCaseInV8Path(CaseModel caseToPlay, CaseModel caseToCheck)
        {
            if (
                (caseToPlay.PosX == caseToCheck.PosX) ||
                (caseToPlay.PosY == caseToCheck.PosY) ||
                (Math.Abs(caseToPlay.PosX - caseToCheck.PosX) == Math.Abs(caseToPlay.PosY - caseToCheck.PosY))
                )
            {
                return true;
            }
            return false;
        }

        public bool IsCasePlayable(CaseModel caseModel)
        {
            return caseModel.IsPlayable;
        }

        public void AddCasePlayed(CaseModel casePlayed)
        {
            listCasesPlayed.Add(casePlayed);
            UpdateCasesOwner(casePlayed);
        }

        public void AddCaseModel(CaseModel caseModel)
        {
            tabCaseModel[caseModel.PosX, caseModel.PosY] = caseModel;
        }

        public void ResetGame()
        {
            //reset all cases
            foreach (CaseModel caseBoard in tabCaseModel)
                caseBoard.CleanCase();
            listCasesPlayed.Clear();

            //Reset the scores
            TabPlayerModel[Properties.Settings.Default.BlackPlayer].Score = 0;
            TabPlayerModel[Properties.Settings.Default.WhitePlayer].Score = 0;

            //Reset players states
            if( CurrentPlayer == Properties.Settings.Default.White)
            {
                CurrentPlayer = Properties.Settings.Default.Black;
                TabPlayerModel[Properties.Settings.Default.BlackPlayer].IsPlaying = true;
                TabPlayerModel[Properties.Settings.Default.WhitePlayer].IsPlaying = false;
            }
            TabPlayerModel[Properties.Settings.Default.BlackPlayer].RestartTimeElapsed();
            TabPlayerModel[Properties.Settings.Default.WhitePlayer].RestartTimeElapsed();

            InitGame();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseGame()
        {
            IsGameFinished = true;
            TabPlayerModel[0].Timer.Stop();
            TabPlayerModel[1].Timer.Stop();
        }


        public Tuple<int, Tuple<int, int>> MinMax(int[,] state, int depth, int minOrMax, int parentValue)
        {
            return null;
        }

        //********************************//
        //           MIN OR MAX           //
        //********************************//


        //**********************************************//
        //           IPLAYABLE IMPLEMENTATION           //
        //**********************************************//

        public string GetName()
        {
            return "Gander Laurent & Neto da Silva André";
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return tabCaseModel[column, line].IsPlayable;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            if (isWhite)
            {
                tabCaseModel[column, line].Owner = Properties.Settings.Default.White;
            }
            else
            {
                tabCaseModel[column, line].Owner = Properties.Settings.Default.Black;
            }
            AddCasePlayed(tabCaseModel[column, line]);
            SetNextPlayer();
            UpdateAllCasesPlayable();

            if (isWhite)
                return true;
            else
                return false;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            return null;
        }

        //-1 = vide
        // 0 = blanc
        // 1 = noire
        public int[,] GetBoard()
        {
            int[,] board = new int[8, 8];
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (tabCaseModel[i, j].Free)
                        board[i, j] = -1;
                    else if(tabCaseModel[i, j].Owner == Properties.Settings.Default.Black)
                        board[i, j] = 1;
                    else
                        board[i, j] = 0;
                }
            }
            return board;
        }

        public int GetWhiteScore()
        {
            return TabPlayerModel[Properties.Settings.Default.WhitePlayer].Score;
        }

        public int GetBlackScore()
        {
            return TabPlayerModel[Properties.Settings.Default.BlackPlayer].Score;
        }
    }
}
