﻿//-----------------------------------------------------------------------
// <copyright file="GameHub.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents a hub for the game which sends the messages to the players.</summary>
//-----------------------------------------------------------------------

namespace Server.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GameLibrary;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Server.Extensions;
    using Server.Services;

    /// <summary>
    /// The SignalR Hub for the Tic-Tac-Toe server.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.SignalR.Hub" />
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GameHub : Hub
    {
        /// <summary>
        /// This field is used to save the main service.
        /// </summary>
        private readonly IMainService mainService;

        private readonly DBManager dBManager = new DBManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="GameHub"/> class.
        /// </summary>
        /// <param name="main">The main service.</param>
        public GameHub(IMainService main)
        {
            this.mainService = main;
        }

        /// <summary>
        /// Gets the players from the MainService and sends them to all clients.
        /// </summary>
        /// <param name="requestedPlayerName">Name of the requested player.</param>
        /// <returns>A Task that represents the asynchronous method.</returns>
        public async Task GetPlayers(string requestedPlayerName)
        {
            var allPlayers = await this.mainService.GetPlayersAsync();

            // select all players except the requested one
            // requested player should not be included in the result
            allPlayers = allPlayers.Where(name => name.PlayerName != requestedPlayerName);
            await Clients.All.SendAsync("ReceivePlayersAsync", await this.mainService.GetPlayersNotInGameAsync());
        }

        /// <summary>
        /// Adds a new player to the list of players, send all players to the clients, send all games and the new player instance to the caller.
        /// </summary>
        /// <param name="nameForNewPlayer">The name for the new player.</param>
        /// <returns>A Task that represents the asynchronous method.</returns>
        public async Task AddPlayer(string nameForNewPlayer, int clientId)
        {
            Player newPlayer = new Player(nameForNewPlayer) { ConnectionId = Context.ConnectionId, ClientId = clientId };

            // datenbank

            var allPlayers = await this.mainService.GetPlayersAsync();
            bool playerExists = false;

            //foreach (var player in allPlayers)
            //{
            //    if (player.PlayerName == nameForNewPlayer)
            //    {
            //        playerExists = true;
            //        break;
            //    }
            //}

            //if (playerExists)
            //{
            //    await Clients.Caller.SendAsync("DuplicateName");
            //    return;
            //}

            await this.mainService.AddPlayerAsync(newPlayer);
            await Clients.Caller.SendAsync("ReturnPlayerInstance", newPlayer);

            await Clients.Caller.SendAsync("ReceiveGames", await this.mainService.GetSimpleGameInformationListAsync());
            await Clients.All.SendAsync("ReceivePlayersAsync", await this.mainService.GetPlayersNotInGameAsync());
        }

        /// <summary>
        /// Adds the new game request to the list of game requests and send the request to the desired player.
        /// </summary>
        /// <param name="gameRequest">The game request.</param>
        /// <returns>A Task that represents the asynchronous method.</returns>
        public async Task AddGameRequest(GameRequest gameRequest)
        {
            var list = await this.mainService.GetPlayersAsync();
            var player = list.SingleOrDefault(player => player.ConnectionId == gameRequest.Enemy.ConnectionId);

            if (player != null)
            {
                // wenn der Enemy schon ein request von wem anderen hat, schicken wir Statusnachricht an den Caller
                var allRequests = await this.mainService.GetGameRequestsAsync();

                var existingRequest = allRequests.SingleOrDefault(request =>
                (request.Enemy.ConnectionId == gameRequest.Enemy.ConnectionId || request.Enemy.ConnectionId == gameRequest.RequestingPlayer.ConnectionId) && 
                (request.RequestingPlayer.ConnectionId == gameRequest.Enemy.ConnectionId || request.RequestingPlayer.ConnectionId == gameRequest.RequestingPlayer.ConnectionId));

                if (existingRequest == null)
                {
                    var request = await this.mainService.AddGameRequestAsync(gameRequest);

                    var task = Task.Run(() =>
                    {
                        var aTimer = new System.Timers.Timer(9000) { AutoReset = false };

                        aTimer.Start();

                        aTimer.Elapsed += async (sender, e) =>
                        {
                            aTimer.Stop();

                            if (!request.Accepted)
                            {
                                await this.mainService.RemoveRequestAsync(request, false);
                            }
                        };
                    });

                    await Clients.Client(player.ConnectionId).SendAsync("GameRequested", gameRequest);
                }
                else
                {
                    await Clients.Caller.SendAsync("StatusMessage", "A request already exists.");
                }
            }
        }

        /// <summary>
        /// Declines or accepts the game request and sends a decline message or various game data to different clients.
        /// </summary>
        /// <param name="id">The game request identifier.</param>
        /// <param name="accept">If set to <c>true</c> [accept].</param>
        /// <returns>A Task that represents the asynchronous method.</returns>
        public async Task DeclineOrAcceptRequest(int id, bool accept)
        {
            var requests = new List<GameRequest>(await this.mainService.GetGameRequestsAsync());
            var existingRequest = requests.SingleOrDefault(request => request.RequestID == id);

            if (existingRequest != null)
            {
                if (!accept)
                {
                    // existingRequest.Declined = true;
                    await Clients.Client(existingRequest.RequestingPlayer.ConnectionId).SendAsync("StatusMessage", $"{existingRequest.Enemy.PlayerName} has declined the request.");
                    await this.mainService.RemoveRequestAsync(existingRequest, false);
                }
                else
                {
                    // existingRequest.Accepted = true;

                    // new game in DB => id, playerOne, playerTwo,
                    var game = new Game(existingRequest.RequestingPlayer, existingRequest.Enemy);

                    // game.CurrentGameStatus.ConvertToString();

                    await this.mainService.RemoveRequestAsync(existingRequest, true);

                    await this.mainService.AddGameAsync(game);

                    await Clients.All.SendAsync("ReceivePlayersAsync", await this.mainService.GetPlayersNotInGameAsync());

                    var simpleGameInfo = await this.mainService.GetSimpleGameInformationListAsync();
                    await Clients.All.SendAsync("ReceiveGames", simpleGameInfo);

                    var gameStatus = this.CreateNewGameStatus(game, true);

                    await Clients.Clients(existingRequest.RequestingPlayer.ConnectionId, existingRequest.Enemy.ConnectionId).SendAsync("GameStatus", gameStatus);
                }
            }
        }

        /// <summary>
        /// Updates the game status after a move.
        /// </summary>
        /// <param name="update">The GameStatus to update.</param>
        /// <returns>A Task that represents the asynchronous method.</returns>
        public async Task UpdateGameStatus(GameStatus update)
        {
            var games = new List<Game>(await this.mainService.GetGamesAsync());
            var game = games.SingleOrDefault(g => g.GameId == update.GameId);

            if (game != null)
            {
                if (game.PlayerOne.ConnectionId == update.CurrentPlayerId)
                {
                    await this.UpdatePlayerSpecificGameStatus(game, update.UpdatedPosition, game.PlayerOne);
                }
                else if (game.PlayerTwo.ConnectionId == update.CurrentPlayerId)
                {
                    await this.UpdatePlayerSpecificGameStatus(game, update.UpdatedPosition, game.PlayerTwo);
                }
            }
        }

        /// <summary>
        /// Removes the game from the list, send update to the enemy and send game and player list to all clients.
        /// </summary>
        /// <param name="id">The identifier of the player who left the game.</param>
        /// <param name="enemyId">The enemy identifier.</param>
        /// <returns>A Task that represents the asynchronous method.</returns>
        public async Task ReturnToLobby(string id, string enemyId)
        {
            var games = new List<Game>(await this.mainService.GetGamesAsync());

            foreach (var game in games)
            {
                if (game.PlayerOne.ConnectionId == id || game.PlayerTwo.ConnectionId == id)
                {
                    await this.mainService.RemoveGameAsync(game);
                    await Clients.Client(enemyId).SendAsync("EnemyLeftGame");
                    var simpleGameInfo = await this.mainService.GetSimpleGameInformationListAsync();
                    await Clients.All.SendAsync("ReceiveGames", simpleGameInfo);
                    await Clients.All.SendAsync("ReceivePlayersAsync", await this.mainService.GetPlayersNotInGameAsync());
                }
            }
        }

        /// <summary>
        /// Called when a new connection is established with the hub.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous connect.
        /// </returns>
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Called when a connection with the hub is terminated, removes the player and sends the list to all clients.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A Task that represents the asynchronous method.</returns>
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;
            var allPlayers = await this.mainService.GetPlayersAsync();
            var disconnectedPlayer = allPlayers.FirstOrDefault(player => player.ConnectionId == id);

            var games = new List<Game>(await this.mainService.GetGamesAsync());

            foreach (var game in games)
            {
                if (game.PlayerOne.ConnectionId == id || game.PlayerTwo.ConnectionId == id)
                {
                    Player enemyPlayer;

                    if (game.PlayerOne.ConnectionId == id)
                    {
                        enemyPlayer = game.PlayerOne;
                    }
                    else
                    {
                        enemyPlayer = game.PlayerTwo;
                    }

                    await this.ReturnToLobby(Context.ConnectionId, enemyPlayer.ConnectionId);
                    await this.mainService.RemoveGameAsync(game);
                }
            }

            await this.mainService.RemovePlayerAsync(disconnectedPlayer);
            await Clients.All.SendAsync("ReceivePlayersAsync", await this.mainService.GetPlayersNotInGameAsync());
        }

        /// <summary>
        /// Updates the game after a move and sends the game status to concerned clients.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="updatedPosition">The updated position.</param>
        /// <param name="player">The player instance.</param>
        /// <returns>A Task that represents the asynchronous method.</returns>
        private async Task UpdatePlayerSpecificGameStatus(Game game, int updatedPosition, Player player)
        {
            if (game.GameOver)
            {
                return;
            }

            if (game.IsMoveValid(updatedPosition, player))
            {
                var gameFinished = game.MakeMove(updatedPosition, player);

                if (gameFinished)
                {
                    // db call
                    this.dBManager.UpdatePlayerWins(player.ClientId);

                    await Clients.Clients(game.PlayerOne.ConnectionId, game.PlayerTwo.ConnectionId).SendAsync("StatusMessage", game.EndMessage + " New game in 5 seconds");
                    var oldGameStatus = this.CreateNewGameStatus(game, false, updatedPosition);
                    game.GameOver = true;

                    await Clients.Client(game.CurrentPlayer.ConnectionId).SendAsync("GameStatus", oldGameStatus);
                    await Task.Delay(5000);

                    game.NewGameSetup();
                    var gameStatus = this.CreateNewGameStatus(game, true, updatedPosition);

                    game.GameOver = false;
                    await Clients.Clients(game.PlayerOne.ConnectionId, game.PlayerTwo.ConnectionId).SendAsync("GameStatus", gameStatus);
                }
                else
                {
                    var gameStatus = this.CreateNewGameStatus(game, false, updatedPosition);

                    await Clients.Client(game.CurrentPlayer.ConnectionId).SendAsync("GameStatus", gameStatus);
                }
            }
        }

        /// <summary>
        /// Creates a new game status.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="isNewGame">If set to <c>true</c> [is new game].</param>
        /// <param name="updatedPosition">The updated position in the field.</param>
        /// <returns>The new game status.</returns>
        private GameStatus CreateNewGameStatus(Game game, bool isNewGame, int updatedPosition = -1)
        {
            var gameStatus = new GameStatus(game.CurrentGameStatus, game.CurrentPlayer.ConnectionId, game.CurrentPlayer.Marker, game.GameId, game.PlayerOne.Wins, game.PlayerTwo.Wins)
            {
                UpdatedPosition = updatedPosition
            };

            if (isNewGame)
            {
                gameStatus.IsNewGame = true;
            }

            return gameStatus;
        }
    }
}
