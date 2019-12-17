//-----------------------------------------------------------------------
// <copyright file="ClientVM.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents the main view model for the client game.</summary>
//-----------------------------------------------------------------------

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
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client
{
    public class ClientVM : BaseVM
    {
        private readonly UrlService urlService;
        private HubConnection hubConnection;
        private ObservableCollection<Player> playerList;
        private ObservableCollection<SimpleGameInformation> gameList;
        private Player selectedPlayer;
        private bool gameIsActive;
        private PlayerVM clientPlayer;
        private bool clientConnected;
        private bool gameWasRequested;
        private string statusMessage;
        private string activePlayerName = string.Empty;
        private Player requestingorEnemyPlayer;
        private bool myTurn = false;
        private readonly ILogger<ClientVM> logger;
        private Player playerOne;
        private Player playerTwo;
        private System.Timers.Timer timer;

        public ClientVM(UrlService urlService, ILogger<ClientVM> logger)
        {
            this.logger = logger;
            this.urlService = urlService;
            this.PlayerList = new ObservableCollection<Player>();
            this.GameList = new ObservableCollection<SimpleGameInformation>();
            this.ClientConnected = false;
            this.GameIsActive = false;
            this.GameWasRequested = false;
            this.CurrentGameId = 0;

            this.SetupCommand = new Command(async obj => await this.Setup());
            this.PlayerClick = new Command(async obj => await this.ComputePlayerClick((GameCellVM)obj));
            this.AcceptCommand = new Command(async obj => await this.ComputeAcceptCommand());
            this.RequestGameCommand = new Command(async obj => await this.ComputeRequestGameCommand());
            this.DeclineCommand = new Command(async obj => await this.ComputeDeclineCommand());
            this.ReturnToLobbyCommand = new Command(async obj => await this.ComputeReturnToLobbyCommand());
            this.ConnectCommand = new Command(async obj => await this.ComputeConnectCommand());

            this.SetupCommand.Execute(new object());
        }

        public ICommand SetupCommand { get; }

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
        /// Gets the return to lobby command.
        /// </summary>
        /// <value>
        /// The return to lobby command.
        /// </value>
        public ICommand ReturnToLobbyCommand { get; }

        /// <summary>
        /// This command is used when the player using the client requests a game with another online player.
        /// A game request will be sent to the server containing the id of the enemy player and the id of the client player.
        /// </summary>
        public ICommand RequestGameCommand { get; }

        public ICommand ConnectCommand { get; }

        /// <summary>
        /// Gets or sets the player that has sent a request to play with the client or is actively playing with the client.
        /// Is set to default (null) if there is neither a request or an active game.
        /// </summary>
        /// <value>
        /// The requesting or enemy player.
        /// </value>
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
        /// Gets the current game identifier.
        /// </summary>
        /// <value>
        /// The current game identifier.
        /// </value>
        public int CurrentGameId { get; private set; }

        /// <summary>
        /// Gets or sets the current game status.
        /// </summary>
        /// <value>
        /// The current game status.
        /// </value>
        public GameStatus CurrentGameStatus { get; set; }

        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        /// <value>
        /// The request identifier.
        /// </value>
        public int RequestID { get; set; }

        /// <summary>
        /// Gets or sets the player one.
        /// </summary>
        /// <value>
        /// The player one.
        /// </value>
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

        /// <summary>
        /// Gets or sets the player two.
        /// </summary>
        /// <value>
        /// The player two.
        /// </value>
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

        /// <summary>
        /// Gets or sets the name of the active player.
        /// </summary>
        /// <value>
        /// The name of the active player.
        /// </value>
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
        /// Gets or sets the list of currently available players.
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
        /// Gets or sets the list of currently online players.
        /// </summary>
        public ObservableCollection<SimpleGameInformation> GameList
        {
            get
            {
                return this.gameList;
            }
            set
            {
                this.gameList = value;
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

        /// <summary>
        /// Gets the game representation.
        /// </summary>
        /// <value>
        /// The game representation.
        /// </value>
        public TicTacToeGameRepresentation GameRepresentation { get; } = new TicTacToeGameRepresentation();

        /// <summary>
        /// Resets the game field.
        /// </summary>
        private void ResetField()
        {
            foreach (var item in this.GameRepresentation.GameCells)
            {
                item.PlayerMark = 0;
            }
        }

        /// <summary>
        /// Closes the connection asynchronously.
        /// </summary>
        /// <returns></returns>
        private Task CloseConnectionAsync() => hubConnection?.DisposeAsync() ?? Task.CompletedTask;

        private async Task Setup()
        {
            try
            {
                await CloseConnectionAsync();
                this.hubConnection = new HubConnectionBuilder()
                    .WithUrl(urlService.LobbyAddress)
                    .Build();

                this.hubConnection.On<List<Player>>("ReceivePlayersAsync", this.OnPlayersReceived);
                this.hubConnection.On<List<SimpleGameInformation>>("ReceiveGames", this.OnGamesReceived);
                this.hubConnection.On<GameRequest>("GameRequested", this.OnGameRequestReceived);
                this.hubConnection.On<Player>("ReturnPlayerInstance", this.OnClientPlayerInstanceReturned);
                this.hubConnection.On<string>("StatusMessage", this.OnStatusMessageReceived);
                this.hubConnection.On<GameStatus>("GameStatus", this.OnGameStatusReceived);
                this.hubConnection.On("EnemyLeftGame", this.OnEnemyLeftGame);
                this.hubConnection.On("DuplicateName", this.OnDuplicateName);

                await this.hubConnection.StartAsync();
            }
            catch (HttpRequestException)
            {
                this.StatusMessage = "Unable to connect to server.";
            }
            catch (Exception)
            {
                this.statusMessage = "An unknown error occured. Please try again later.";
            }
        }

        /// <summary>
        /// Called when the client player instance is returned in order to obtain the clients connection id.
        /// </summary>
        /// <param name="player">The client player.</param>
        private void OnClientPlayerInstanceReturned(Player player)
        {
            this.ClientPlayer.Player = player;
        }

        private void OnGamesReceived(List<SimpleGameInformation> games)
        {
            this.GameList = new ObservableCollection<SimpleGameInformation>(games);
        }

        /// <summary>
        /// Called when a name already in the list of players has been chosen.
        /// </summary>
        private void OnDuplicateName()
        {
            this.ClientConnected = false;
            this.StatusMessage = "Duplicate name, please choose a new one.";
        }

        /// <summary>
        /// Called when a game status has been received from the server.
        /// </summary>
        /// <param name="status">The status.</param>
        private void OnGameStatusReceived(GameStatus status)
        {
            this.logger.LogInformation("[OnGameStatusReceived] GameId: {0}", new object[] { status.GameId });

            if (this.CurrentGameStatus == null || status.IsNewGame)
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
            this.timer = new System.Timers.Timer(10000) { AutoReset = false };
            this.timer.Start();
            this.timer.Elapsed += Timer_Elapsed;
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

            if (status.IndexedGame.All<int>(x => x == 0))
            {
                ResetField();
            }

            PlayerOne.Wins = status.WinsPlayerOne;
            PlayerTwo.Wins = status.WinsPlayerTwo;
        }

        /// <summary>
        /// Handles the Elapsed event of the Timer control. Is responsible for the timeout message if a client player is afk in a game.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.myTurn)
            {
                this.StatusMessage = "It's your turn. Play, or the game will end in 5 seconds!";

                Task.Run(() =>
                {
                    this.timer = new System.Timers.Timer(5000) { AutoReset = false };
                    this.timer.Start();

                    this.timer.Elapsed += async (sender, e) =>
                    {
                        this.timer.Enabled = false;
                        this.StatusMessage = "Game ended because of inactivity.";
                        await this.ComputeReturnToLobbyCommand();
                    };
                });

                this.timer.Enabled = false;
            }

            this.timer.Enabled = false;
        }

        /// <summary>
        /// Called when a status message has been received from the server.
        /// </summary>
        /// <param name="message">The message.</param>
        private void OnStatusMessageReceived(string message)
        {
            this.StatusMessage = message;
        }

        /// <summary>
        /// Called when an enemy has left the game. Resets the properties responsible for the view.
        /// </summary>
        private void OnEnemyLeftGame()
        {
            this.timer.Enabled = false;
            this.logger.LogInformation("[OnEnemyLeftGame]");
            this.StatusMessage = "Enemy left the game.";
            this.PlayerOne = new Player();
            this.PlayerTwo = new Player();
            this.GameIsActive = false;
            this.ResetField();
        }

        /// <summary>
        /// Called when a game request has been received from the server. Sets the properties for the view to enable declining or accepting.
        /// </summary>
        /// <param name="gameRequest">The game request.</param>
        private void OnGameRequestReceived(GameRequest gameRequest)
        {
            this.logger.LogInformation("[OnGameRequestReceived] Player {0} requests a game with player {1}", new object[] { gameRequest.RequestingPlayer.PlayerName, gameRequest.Enemy.PlayerName });

            if (gameRequest.Enemy != null)
            {
                this.RequestingOrEnemyPlayer = gameRequest.RequestingPlayer;
                this.GameWasRequested = true;
                this.RequestID = gameRequest.RequestID;

                // allow the player to accept or decline a game for 10 seconds (timeout)
                var task = Task.Run(() =>
                {
                    var aTimer = new System.Timers.Timer(9500) { AutoReset = false };

                    aTimer.Start();

                    aTimer.Elapsed += (sender, e) =>
                    {
                        this.GameWasRequested = false;
                    };
                });
            }
        }

        /// <summary>
        /// Called when the list of players has been received from the server.
        /// </summary>
        /// <param name="players">The players.</param>
        private void OnPlayersReceived(List<Player> players)
        {
            if (this.ClientConnected)
            {
                this.logger.LogInformation("[OnPlayersReceived]");
                this.PlayerList = new ObservableCollection<Player>(players.Where(id => id.ConnectionId != this.ClientPlayer.Player.ConnectionId));
            }
        }

        private async Task ComputeConnectCommand()
        {
            this.logger.LogInformation("[ComputeConnectCommand]");

            if (!string.IsNullOrEmpty(this.clientPlayer.PlayerName))
            {
                try
                {
                    this.ClientConnected = true;

                    await this.hubConnection.SendAsync("AddPlayer", clientPlayer.PlayerName);
                }
                catch (HttpRequestException)
                {
                    this.StatusMessage = "Unable to connect to server.";
                }
                catch (Exception)
                {
                    this.statusMessage = "An unknown error occured. Please try again later.";
                }
            }

        }

        private async Task ComputeAcceptCommand()
        {
            this.logger.LogInformation("[ComputeAcceptCommand]");
            this.GameWasRequested = false;
            
            try
            {
                await this.hubConnection.SendAsync("DeclineOrAcceptRequest", this.RequestID, true);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task ComputeDeclineCommand()
        {
            this.logger.LogInformation("[ComputeDeclineCommand]");
            this.GameWasRequested = false;
            this.RequestingOrEnemyPlayer = default;

            try
            {
                await this.hubConnection.SendAsync("DeclineOrAcceptRequest", this.RequestID, false);
            }
            catch (HttpRequestException)
            {
                this.StatusMessage = "Unable to reach server. Please try again later.";
            }
            catch (Exception)
            {
                this.statusMessage = "An unknown error occured. Please try again later.";
            }

            this.RequestID = 0;
        }

        private async Task ComputeReturnToLobbyCommand()
        {
            this.timer.Enabled = false;
            this.timer = new System.Timers.Timer { AutoReset = false };

            this.logger.LogInformation("[ComputeReturnToLobbyCommand]");

            Application.Current.Dispatcher.Invoke(new ThreadStart(() =>
            {
                this.PlayerOne = new Player();
                this.PlayerTwo = new Player();
                this.GameIsActive = false;
                this.ResetField();
            }));

            try
            {
                await this.hubConnection.SendAsync("ReturnToLobby", this.ClientPlayer.Player.ConnectionId, this.RequestingOrEnemyPlayer.ConnectionId);
            }
            catch (HttpRequestException)
            {
                this.StatusMessage = "Unable to reach server. Please try again later.";
            }
            catch (Exception)
            {
                this.statusMessage = "An unknown error occured. Please try again later.";
            }
        }

        private async Task ComputePlayerClick(GameCellVM cell)
        {
            this.timer.Stop();
            this.timer.Enabled = false;

            this.logger.LogInformation("[ComputePlayerClick] CellIndex: {0}", new object[] { cell.Index });
            if (this.GameIsActive)
            {
                if (this.CurrentGameStatus.IndexedGame[cell.Index] == 0 && this.CurrentGameStatus.CurrentPlayerId == this.ClientPlayer.Player.ConnectionId && this.myTurn)
                {
                    cell.PlayerMark = this.CurrentGameStatus.CurrentPlayerMarker; //this.ClientPlayer.Player.Marker;
                    this.myTurn = false;

                    var status = new GameStatus
                    {
                        CurrentPlayerId = this.ClientPlayer.Player.ConnectionId,
                        UpdatedPosition = cell.Index,
                        GameId = this.CurrentGameStatus.GameId
                    };

                    this.ActivePlayerName = this.RequestingOrEnemyPlayer.PlayerName;

                    try
                    {
                        await this.hubConnection.SendAsync("UpdateGameStatus", status);
                    }
                    catch (HttpRequestException)
                    {
                        this.StatusMessage = "Unable to reach server. Please try again later.";
                    }
                    catch (Exception)
                    {
                        this.StatusMessage = "An unknown error occured. Please try again later.";
                    }

                    //this.gameClientService.UpdateGameStatusAsync(status);
                }
            }


        }

        private async Task ComputeRequestGameCommand()
        {
            this.logger.LogInformation("[ComputeRequestGameCommand]");

            if (this.SelectedPlayer != null && !this.gameIsActive)
            {
                this.RequestingOrEnemyPlayer = this.SelectedPlayer;

                try
                {
                    await this.hubConnection.SendAsync("AddGameRequest", new GameRequest(this.SelectedPlayer, this.ClientPlayer.Player));
                }
                catch (HttpRequestException)
                {
                    this.StatusMessage = "Unable to reach server. Please try again later.";
                }
                catch (Exception)
                {
                    this.statusMessage = "An unknown error occured. Please try again later.";
                }
            }
        }
    }
}
