using Client.Models;
using Client.ViewModels;
using GameLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Client
{
    public class ClientVM : BaseVM
    {
        #region LocalGame - OnlineGame disabled logic
        //private bool onlineGameSelected;
        //private bool localGameSelected;

        //public ICommand LocalGameButtonPressed
        //{
        //    get
        //    {
        //        return new Command(obj =>
        //        {
        //            this.LocalGameSelected = true;
        //            this.OnlineGameSelected = false;
        //        });
        //    }
        //}

        //public ICommand OnlineGameButtonPressed
        //{
        //    get
        //    {
        //        return new Command(obj =>
        //        {
        //            this.LocalGameSelected = false;
        //            this.OnlineGameSelected = true;
        //        });
        //    }
        //}

        //public bool LocalGameSelected
        //{
        //    get
        //    {
        //        return localGameSelected;
        //    }
        //    set
        //    {
        //        localGameSelected = value;
        //        this.FireOnPropertyChanged(nameof(this.LocalGameSelected));
        //    }
        //}

        //public bool OnlineGameSelected
        //{
        //    get
        //    {
        //        return onlineGameSelected;
        //    }
        //    set
        //    {
        //        onlineGameSelected = value;
        //        this.FireOnPropertyChanged(nameof(this.OnlineGameSelected));
        //    }
        //}

        #endregion

        public ClientVM(GameVM game, GameClientService gameClientService)
        {
            this.CurrentGame = game;
            //this.LocalGameSelected = true;
            //this.OnlineGameSelected = false;
            this.PlayerList = new ObservableCollection<Player>();
            this.gameClientService = gameClientService;
            this.ClientConnected = false;
        }

        private ObservableCollection<Player> playerList;
        private Player selectedPlayer;
        private GameClientService gameClientService;
        private int clientId;

        private GameVVM game = new GameVVM();

        public GameVVM Game => this.game;

        private int[] INDEXEXGAMETEST = new int[8];

        /// <summary>
        /// This command is used when a game element button is clicked.
        /// Checks if the player is allowed to place his sign and sends the information to the server.
        /// </summary>
        public ICommand PlayerClick
        {
            get
            {
                return new Command(obj =>
                {
                    //if (!this.GameOver)
                    //{
                    var cell = (GameCellVM)obj;

                    if (this.INDEXEXGAMETEST[cell.Index] == 0)
                    {
                        var x = cell.CellBackground;
                        cell.PlayerMark = 1;

                        //this.gameClientService.

                        //this.indexedGame[index] = this.CurrentPlayer.Marker;
                        //this.CurrentPlayer.MarkedPositions.Add(index);
                        //this.gameTurns++;

                        // an server

                        //if (this.gameTurns > 4 && !this.GameOver)
                        //{
                        //    bool check = this.CheckForWin();

                        //    if (check)
                        //    {
                        //        this.GameOver = true;
                        //        this.CurrentPlayer.Wins++;
                        //        this.EndMessage = $"{this.CurrentPlayer.PlayerName} wins!";
                        //    }

                        //    if (this.gameTurns == 9 && !this.GameOver)
                        //    {
                        //        this.EndMessage = $"It´s a draw!";
                        //        this.GameOver = true;
                        //    }
                        //}


                        //if (this.CurrentPlayer == this.playerOne)
                        //{
                        //    this.CurrentPlayer = this.playerTwo;
                        //}
                        //else
                        //{
                        //    this.CurrentPlayer = this.playerOne;
                        //}
                    }
                    //}
                });
            }
        }

        public ICommand RequestGameCommand
        {
            get
            {
                return new Command(obj =>
                {
                    if (this.SelectedPlayer != null)
                    {
                        this.gameClientService.PostGameRequest(new GameRequest(this.SelectedPlayer.PlayerId, this.ClientPlayer.Player.PlayerId));
                    }
                });
            }
        }

        private bool clientConnected;

        public bool ClientConnected
        {
            get 
            { 
                return clientConnected; 
            }
            set 
            {
                this.clientConnected = value;
                this.FireOnPropertyChanged();
             }
        }



        private PlayerVM clientPlayer;

        public PlayerVM ClientPlayer
        {
            get { return clientPlayer; }
            set { clientPlayer = value; }
        }




        public ObservableCollection<Player> PlayerList
        {
            get { return playerList; }
            set 
            { 
                playerList = value;
                this.FireOnPropertyChanged();
            }
        }

        public Player SelectedPlayer
        {
            get { return selectedPlayer; }
            set { selectedPlayer = value; }
        }



        public GameVM CurrentGame { get; set; }
    }
}
