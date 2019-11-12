using Client.Models;
using Client.ViewModels;
using GameLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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


        private ObservableCollection<Player> playerList;
        private Player selectedPlayer;
        private GameClientService gameClientService;
        private int clientId;
        private bool gameIsActive;
        private PlayerVM clientPlayer;
        private bool clientConnected;

        public ClientVM(GameVM game, GameClientService gameClientService)
        {
            this.CurrentGame = game;
            //this.LocalGameSelected = true;
            //this.OnlineGameSelected = false;
            this.PlayerList = new ObservableCollection<Player>();
            this.gameClientService = gameClientService;
            this.ClientConnected = false;
            this.GameIsActive = false;


            this.GameWasRequested = false;
        }

        private Player requestingorEnemyPlayer;

        public int RequestID { get; set; }

        public Player RequestingOrEnemyPlayer
        {
            get { return requestingorEnemyPlayer; }
            set { requestingorEnemyPlayer = value; this.FireOnPropertyChanged(); }
        }

        private string statusMessage;

        public string StatusMessage
        {
            get { return statusMessage; }
            set 
            { 
                statusMessage = value; 
                this.FireOnPropertyChanged();

                if (value != string.Empty)
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(7000);
                        this.StatusMessage = string.Empty;
                    });
                }
            }
        }


        private bool gameWasRequested;

        public bool GameWasRequested
        {
            get { return gameWasRequested; }
            set { gameWasRequested = value; this.FireOnPropertyChanged(); }
        }

        public ICommand DeclineCommand
        {
            get
            {
                return new Command(obj =>
                {
                    this.GameWasRequested = false;
                    this.RequestingOrEnemyPlayer = default;

                    //delete request on server
                    this.gameClientService.DeclineGameRequest(this.RequestID);
                    this.RequestID = 0;
                });
            }
        }

        public ICommand AcceptCommand
        {
            get
            {
                return new Command(obj =>
                {
                    // affirm request
                    // make new game
                    // delete the old request
                });
            }
        }



        public GameVM CurrentGame { get; set; }
        private GameVVM game = new GameVVM();
        public GameVVM Game => this.game;
        private int[] INDEXEXGAMETEST = new int[9];

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
                        
                        cell.PlayerMark = 1; // oder was auch immer XDDDDDDDDDDDDDD

                        // this.gameClientService.SendGameUpdate =>















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

        /// <summary>
        /// This command is used when the player using the client requests a game with another online player.
        /// A game request will be sent to the server containing the id of the enemy player and the id of the client player.
        /// </summary>
        public ICommand RequestGameCommand
        {
            get
            {
                return new Command(obj =>
                {
                    if (this.SelectedPlayer != null)
                    {
                        this.gameClientService.PostGameRequest(new GameRequest(this.SelectedPlayer, this.ClientPlayer.Player));
                    }
                });
            }
        }

        
        /// <summary>
        /// Gets or sets a value indicating whether the player using the client connected to the server.
        /// </summary>
        public bool ClientConnected
        {
            get 
            { 
                return this.clientConnected; 
            }
            set 
            {
                this.clientConnected = value;
                this.FireOnPropertyChanged();
             }
        }

        /// <summary>
        /// Gets or sets the player that is using the client.
        /// </summary>
        public PlayerVM ClientPlayer
        {
            get 
            { 
                return clientPlayer; 
            }
            set 
            { 
                clientPlayer = value ?? throw new ArgumentNullException(nameof(this.ClientPlayer), "The client player can´t be null.");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a game with another player is currently in progress.
        /// </summary>
        public bool GameIsActive
        {
            get
            {
                return this.gameIsActive;
            }
            set
            {
                this.gameIsActive = value;
                this.FireOnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the list of currently online players.
        /// </summary>
        public ObservableCollection<Player> PlayerList
        {
            get
            { 
                return this.playerList; 
            }
            set 
            { 
                this.playerList = value;
                this.FireOnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the currently selected player in the online player list.
        /// </summary>
        public Player SelectedPlayer
        {
            get 
            { 
                return this.selectedPlayer; 
            }
            set 
            { 
                selectedPlayer = value; 
            }
        }
    }
}
