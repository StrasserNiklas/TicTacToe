using Client.Models;
using Client.Services;
using Client.ViewModels;
using GameLibrary;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
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
        private readonly UrlService urlService;
        private HubConnection hubConnection;
        private TicTacToeGameRepresentation gameRepresentation = new TicTacToeGameRepresentation();
        private ObservableCollection<Player> playerList;
        private Player selectedPlayer;
        private int clientId;
        private bool gameIsActive;
        private PlayerVM clientPlayer;
        private bool clientConnected;
        private bool gameWasRequested;
        private string statusMessage;
        private string activePlayerName = string.Empty;
        private Player requestingorEnemyPlayer;
        private bool myTurn = false;

        public ClientVM(UrlService urlService, ILogger<ClientVM> logger)
        {
            this.urlService = urlService;
            this.PlayerList = new ObservableCollection<Player>();
            this.ClientConnected = false;
            this.GameIsActive = false;
            this.GameWasRequested = false;
            this.CurrentGameId = 0;

            this.Setup();

            // ERLAUBT? WARUM NICHT? SONST PASSIERT DER CAST HALD WOANDERS
            this.PlayerClick = new Command(obj => this.ComputePlayerClick((GameCellVM)obj));
            this.AcceptCommand = new Command(obj => this.ComputeAcceptCommand());
            this.RequestGameCommand = new Command(obj => this.ComputeRequestGameCommand());
            this.DeclineCommand = new Command(obj => this.ComputeDeclineCommand());
        }

        /// <summary>
        /// This command is used when a game element button is clicked.
        /// Checks if the player is allowed to place his sign and sends the information to the server.
        /// </summary>
        public ICommand PlayerClick { get; }

        /// <summary>
        /// When the client accepts a game request, a correspondent message is sent to the server.
        /// </summary>
        public ICommand AcceptCommand { get; }

        /// <summary>
        /// When the client declines a game request, a correspondent message is sent to the server.
        /// The requesting player is set to default (null) and the game request bool is set to false.
        /// </summary>
        
        public ICommand DeclineCommand { get; }

        /// <summary>
        /// This command is used when the player using the client requests a game with another online player.
        /// A game request will be sent to the server containing the id of the enemy player and the id of the client player.
        /// </summary>
        public ICommand RequestGameCommand { get; }

        private async Task Setup()
        {
            await CloseConnectionAsync();
            this.hubConnection = new HubConnectionBuilder()
                .WithUrl(urlService.LobbyAddress)
                .Build();

            this.hubConnection.On<List<Player>>("ReceivePlayersAsync", this.OnPlayersReceived);
            this.hubConnection.On<GameRequest>("GameRequested", this.OnGameRequestReceived);
            this.hubConnection.On<Player>("ReturnPlayerInstance", this.OnClientPlayerInstanceReturned);
            this.hubConnection.On<string>("StatusMessage", this.OnStatusMessageReceived);
            this.hubConnection.On<GameStatus>("GameStatus", this.OnGameStatusReceived);

            await this.hubConnection.StartAsync();

        }

        private void OnClientPlayerInstanceReturned(Player player) //DOKU Receives the client player instance in order to obtain the clients connection id.
        {
            this.ClientPlayer.Player = player;
        }

        private void OnGameStatusReceived(GameStatus status)
        {
            if (this.CurrentGameStatus == null)
            {
                if (this.ClientPlayer.Player.ConnectionId == status.CurrentPlayerId)
                {
                    this.PlayerTwo = this.RequestingOrEnemyPlayer;
                    this.PlayerOne = this.ClientPlayer.Player;
                }
                else
                {
                    this.PlayerOne = this.RequestingOrEnemyPlayer;
                    this.PlayerTwo = this.ClientPlayer.Player;
                }

                this.GameIsActive = true;
            }

            this.myTurn = true;
            this.CurrentGameStatus = status;

            if (this.ClientPlayer.Player.ConnectionId == status.CurrentPlayerId)
            {
                this.ActivePlayerName = this.ClientPlayer.PlayerName;
            }
            else
            {
                this.ActivePlayerName = this.RequestingOrEnemyPlayer.PlayerName;
            }

            if (status.CurrentPlayerMarker == 1)
            {
                if (status.UpdatedPosition >= 0)
                {
                    this.GameRepresentation.GameCells[status.UpdatedPosition].PlayerMark = 2;
                }
            }
            else
            {
                if (status.UpdatedPosition >= 0)
                {
                    this.GameRepresentation.GameCells[status.UpdatedPosition].PlayerMark = 1;
                }
            }

            
        }

        private void OnStatusMessageReceived(string message)
        {
            this.StatusMessage = message;
        }

        private void OnGameRequestReceived(GameRequest gameRequest) //DOKU Responds to a received game request from another player. 
        {
            if (gameRequest.Enemy != null)
            {
                this.RequestingOrEnemyPlayer = gameRequest.RequestPlayer;
                this.GameWasRequested = true;
                this.RequestID = gameRequest.RequestID;

                // allow the player to accept or decline a game for 10 seconds (timeout)
                var task = Task.Run(() =>
                {
                    var aTimer = new System.Timers.Timer(9500);

                    aTimer.Start();

                    aTimer.Elapsed += (sender, e) =>
                    {
                        this.GameWasRequested = false;
                    };
                });
            }
        }

        private Task CloseConnectionAsync() => hubConnection?.DisposeAsync() ?? Task.CompletedTask;

        public void OnPlayersReceived(List<Player> players)
        {
            this.PlayerList = new ObservableCollection<Player>(players.Where(id => id.ConnectionId != this.ClientPlayer.Player.ConnectionId));
        }

        public int CurrentGameId { get; private set; }

        public GameStatus CurrentGameStatus { get; set; }
        public int RequestID { get; set; }        

        public ICommand ConnectCommand
        {
            get
            {
                return new Command(async obj =>
                {
                    await this.hubConnection.SendAsync("AddPlayer", clientPlayer.PlayerName);
                });
            }
        }


        private async Task ComputeAcceptCommand()
        {
            this.GameWasRequested = false;

            

            // accept request on server
            await this.hubConnection.SendAsync("DeclineOrAcceptRequest", this.RequestID, true);
            //this.gameClientService.DeclineOrAcceptGameRequest(this.RequestID, true);

            //this.GameIsActive = true;


            // affirm request
            // make new game in client and later on server
            // delete the old request
        }

        private async Task ComputeDeclineCommand()
        {
            this.GameWasRequested = false;
            this.RequestingOrEnemyPlayer = default;

            // delete request on server

            await this.hubConnection.SendAsync("DeclineOrAcceptRequest", this.RequestID, false);
            //this.gameClientService.DeclineOrAcceptGameRequest(this.RequestID, false);
            this.RequestID = 0;
        }

        private async void ComputePlayerClick(GameCellVM cell)
        {
            if (this.GameIsActive)
            {
                if (this.CurrentGameStatus.IndexedGame[cell.Index] == 0 && this.CurrentGameStatus.CurrentPlayerId == this.ClientPlayer.Player.ConnectionId && this.myTurn)
                {
                    cell.PlayerMark = this.CurrentGameStatus.CurrentPlayerMarker;//this.ClientPlayer.Player.Marker;
                    this.myTurn = false;

                    var status = new GameStatus();
                    status.CurrentPlayerId = this.ClientPlayer.Player.ConnectionId;
                    status.UpdatedPosition = cell.Index;
                    status.GameId = this.CurrentGameStatus.GameId;
                    this.ActivePlayerName = this.RequestingOrEnemyPlayer.PlayerName;

                    await this.hubConnection.SendAsync("UpdateGameStatus", status);

                    //this.gameClientService.UpdateGameStatusAsync(status);
                }
            }

            
        }

        public async Task ComputeRequestGameCommand()
        {
            if (this.SelectedPlayer != null)
            {
                this.RequestingOrEnemyPlayer = this.SelectedPlayer;
                await this.hubConnection.SendAsync("AddGameRequest", new GameRequest(this.SelectedPlayer, this.ClientPlayer.Player));
                //this.gameClientService.PostGameRequest(new GameRequest(this.SelectedPlayer, this.ClientPlayer.Player));
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

        private Player playerOne;
        private Player playerTwo;

        public Player PlayerOne
        {
            get 
            { 
                return this.playerOne; 
            }
            set 
            { 
                this.playerOne = value;
                this.FireOnPropertyChanged();
            }
        }

        public Player PlayerTwo
        {
            get
            {
                return this.playerTwo;
            }
            set
            {
                this.playerTwo = value;
                this.FireOnPropertyChanged();
            }
        }


        public string ActivePlayerName
        {
            get 
            { 
                return this.activePlayerName;
            }

            set 
            { 
                this.activePlayerName = value;
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
                        await Task.Delay(9000);
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
