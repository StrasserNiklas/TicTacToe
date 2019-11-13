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
        private TicTacToeGameRepresentation gameRepresentation = new TicTacToeGameRepresentation();
        private ObservableCollection<Player> playerList;
        private Player selectedPlayer;
        private GameClientService gameClientService;
        private int clientId;
        private bool gameIsActive;
        private PlayerVM clientPlayer;
        private bool clientConnected;
        private bool gameWasRequested;
        private string statusMessage;
        private Player requestingorEnemyPlayer;

        public ClientVM(GameClientService gameClientService)
        {

            this.PlayerList = new ObservableCollection<Player>();
            this.gameClientService = gameClientService;
            this.ClientConnected = false;
            this.GameIsActive = false;
            this.GameWasRequested = false;
            this.CurrentGameId = 0;
        }

        public int CurrentGameId { get; private set; }

        public GameStatus CurrentGameStatus { get; set; }
        public int RequestID { get; set; }

        /// <summary>
        /// When the client accepts a game request, a correspondent http request is sent to the server.
        /// </summary>
        public ICommand AcceptCommand
        {
            get
            {
                return new Command(obj =>
                {
                    // accept request on server
                    this.gameClientService.DeclineOrAcceptGameRequest(this.RequestID, true);

                    //this.GameIsActive = true;


                    // affirm request
                    // make new game in client and later on server
                    // delete the old request
                });
            }
        }

        public ICommand ConnectCommand
        {
            get
            {
                return new Command(obj =>
                {
                    var backgroundTask = Task.Run(async () =>
                    {
                        var player = await this.gameClientService.PostPlayerInfoToServerAsync(this.ClientPlayer.PlayerName);
                        this.ClientPlayer = new PlayerVM(player);

                        while (true)
                        {
                            if (!this.GameIsActive)
                            {
                                var status = await this.gameClientService.GetPlayerListAndPostAliveAsync(player.PlayerId);

                                if (status.RequestingPlayer != null)
                                {
                                    this.RequestingOrEnemyPlayer = status.RequestingPlayer;
                                    this.GameWasRequested = true;
                                    this.RequestID = status.RequestID;
                                }

                                if (status.StatusMessage != string.Empty)
                                {
                                    this.StatusMessage = status.StatusMessage;
                                }


                                var playerList = new List<Player>(status.Players);
                                playerList.Remove(playerList.SingleOrDefault(player => player.PlayerId == this.ClientPlayer.Player.PlayerId));

                                this.PlayerList = new ObservableCollection<Player>(playerList);
                            }
                            else
                            {
                                var gameStatus = await this.gameClientService.GetGameStatusAsync(this.CurrentGameStatus.GameId);

                                // set correspondent game logic
                            }
                        }
                    });
                });
            }
        }

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

                    if (this.GameIsActive && this.CurrentGameStatus != null)
                    {
                        var cell = (GameCellVM)obj;

                        if (this.CurrentGameStatus.IndexedGame[cell.Index] == 0 && this.CurrentGameStatus.CurrentPlayerId == this.ClientPlayer.Player.PlayerId) 
                        {

                            cell.PlayerMark = this.ClientPlayer.Player.Marker;

                            var status = new GameStatus();
                            status.UpdatedPosition = cell.Index;
                            status.GameId = this.CurrentGameStatus.GameId;

                            this.gameClientService.UpdateGameStatusAsync(status);




                        }
                    }

                    
                });
            }
        }

        /// <summary>
        /// When the client declines a game request, a correspondent http request is sent to the server.
        /// The requesting player is set to default (null) and the game request bool is set to false.
        /// </summary>
        public ICommand DeclineCommand
        {
            get
            {
                return new Command(obj =>
                {
                    this.GameWasRequested = false;
                    this.RequestingOrEnemyPlayer = default;

                    // delete request on server
                    this.gameClientService.DeclineOrAcceptGameRequest(this.RequestID, false);
                    this.RequestID = 0;
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
        /// Gets or sets the player that has sent a request to play with the client or is actively playing with the client.
        /// Is set to default (null) if there is neither a request or an active game.
        /// </summary>
        public Player RequestingOrEnemyPlayer
        {
            get
            {
                return this.requestingorEnemyPlayer;
            }
            set
            {
                this.requestingorEnemyPlayer = value;
                this.FireOnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a specific status message to display in the client.
        /// E.g. a player has declined a game request.
        /// Disappears from the client after a set amount of time.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value whether a game with the client has been requested by another player.
        /// Needed for UI representation.
        /// </summary>
        public bool GameWasRequested
        {
            get
            {
                return this.gameWasRequested;
            }
            set
            {
                this.gameWasRequested = value;
                this.FireOnPropertyChanged();
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

        public TicTacToeGameRepresentation GameRepresentation => this.gameRepresentation;
    }
}
