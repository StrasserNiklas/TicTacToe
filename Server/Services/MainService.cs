//-----------------------------------------------------------------------
// <copyright file="MainService.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents a main service for the game which handles the different requests.</summary>
//-----------------------------------------------------------------------

namespace Server.Services
{
    using GameLibrary;
    using Microsoft.Extensions.Logging;
    using Server.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// This file represents a main service for the game which handles the different requests.
    /// </summary>
    public class MainService : IMainService
    {
        /// <summary>
        /// This field is used to save the players.
        /// </summary>
        private readonly List<Player> players = new List<Player>();

        /// <summary>
        /// This field is used to save the games.
        /// </summary>
        private readonly List<Game> games = new List<Game>();

        /// <summary>
        /// This field is used to save the game requests.
        /// </summary>
        private readonly List<GameRequest> gameRequests = new List<GameRequest>();

        /// <summary>
        /// This field represents the logger.
        /// </summary>
        private readonly ILogger<MainService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MainService(ILogger<MainService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Adds the player asynchronously.
        /// </summary>
        /// <param name="player">The player to be added.</param>
        /// <returns>
        /// The player which has been added.
        /// </returns>
        public Task<Player> AddPlayerAsync(Player player)
        {
            this.logger.LogInformation("[AddPlayerAsync] ConnectionId: {0}, PlayerName: {1}", new object[] { player.ConnectionId, player.PlayerName });

            this.players.Add(player);

            return Task.FromResult(player);
        }

        /// <summary>
        /// Gets the players asynchronously.
        /// </summary>
        /// <returns>
        /// The collection of all players.
        /// </returns>
        public Task<IEnumerable<Player>> GetPlayersAsync()
        {
            this.logger.LogInformation("[GetPlayersAsync]");
            return Task.FromResult<IEnumerable<Player>>(this.players);
        }

        /// <summary>
        /// Gets the game requests asynchronously.
        /// </summary>
        /// <returns>
        /// The collection of game requests.
        /// </returns>
        public Task<IEnumerable<GameRequest>> GetGameRequestsAsync()
        {
            this.logger.LogInformation("[GetGameRequestsAsync]");
            return Task.FromResult<IEnumerable<GameRequest>>(this.gameRequests);
        }

        /// <summary>
        /// Gets the games asynchronously.
        /// </summary>
        /// <returns>
        /// The collection of games.
        /// </returns>
        public Task<IEnumerable<Game>> GetGamesAsync()
        {
            this.logger.LogInformation("[GetGamesAsync]");
            return Task.FromResult<IEnumerable<Game>>(this.games);
        }

        /// <summary>
        /// Adds the game request asynchronously.
        /// </summary>
        /// <param name="gameRequest">The game request.</param>
        /// <returns>
        /// The game request which has been added.
        /// </returns>
        public Task<GameRequest> AddGameRequestAsync(GameRequest gameRequest)
        {
            this.logger.LogInformation("[AddGameRequestAsync] Player {0} requests a game with player {1}", new object[] { gameRequest.RequestingPlayer.PlayerName, gameRequest.Enemy.PlayerName });
            this.gameRequests.Add(gameRequest);
            return Task.FromResult(gameRequest);
        }

        /// <summary>
        /// Removes the request asynchronously.
        /// </summary>
        /// <param name="gameRequest">The game request.</param>
        /// <param name="accepted">An indication whether the request has been accepted or declined.</param>
        /// <returns>
        /// The removed game request.
        /// </returns>
        public Task<GameRequest> RemoveRequestAsync(GameRequest gameRequest, bool accepted)
        {
            this.logger.LogInformation($"[RemoveRequestAsync] Game request from player {0} to player {1} has been accepted: {accepted.ToString()}", new object[] { gameRequest.RequestingPlayer.PlayerName, gameRequest.Enemy.PlayerName });
            this.gameRequests.Remove(gameRequest);
            return Task.FromResult(gameRequest);
        }

        /// <summary>
        /// Removes the player asynchronously.
        /// </summary>
        /// <param name="player">The player to be removed.</param>
        /// <returns>
        /// The player which has been removed.
        /// </returns>
        public Task<Player> RemovePlayerAsync(Player player)
        {
            this.logger.LogInformation("[RemovePlayerAsync] ConnectionId: {0}, PlayerName: {1}", new object[] { player.ConnectionId, player.PlayerName });
            this.players.Remove(player);
            return Task.FromResult(player);
        }

        /// <summary>
        /// Adds the game asynchronously.
        /// </summary>
        /// <param name="game">The game to be added.</param>
        /// <returns>
        /// The added game.
        /// </returns>
        public Task<Game> AddGameAsync(Game game)
        {
            this.logger.LogInformation("[AddGameAsync] PlayerOneName: {0}, PlayerTwoName: {1}", new object[] { game.PlayerOne.PlayerName, game.PlayerTwo.PlayerName });
            this.games.Add(game);
            return Task.FromResult(game);
        }

        /// <summary>
        /// Removes the game asynchronously.
        /// </summary>
        /// <param name="game">The game to be removed.</param>
        /// <returns>
        /// The game which has been removed.
        /// </returns>
        public Task<Game> RemoveGameAsync(Game game)
        {
            this.logger.LogInformation("[RemoveGameAsync] PlayerOneName: {0}, PlayerTwoName: {1}", new object[] { game.PlayerOne.PlayerName, game.PlayerTwo.PlayerName });
            this.games.Remove(game);
            return Task.FromResult(game);
        }

        /// <summary>
        /// Gets the players not in game asynchronously.
        /// </summary>
        /// <returns>
        /// The collection of players who are not in a game.
        /// </returns>
        public Task<IEnumerable<Player>> GetPlayersNotInGameAsync()
        {
            this.logger.LogInformation("[GetPlayersNotInGameAsync]");

            List<Player> playerList = new List<Player>();

            if (this.games.Count == 0)
            {
                return Task.FromResult<IEnumerable<Player>>(this.players);
            }

            foreach (var game in this.games)
            {
                foreach (var player in this.players)
                {
                    if (game.PlayerOne.ConnectionId != player.ConnectionId && game.PlayerTwo.ConnectionId != player.ConnectionId)
                    {
                        playerList.Add(player);
                    }
                }
            }

            return Task.FromResult<IEnumerable<Player>>(playerList);
        }

        /// <summary>
        /// Gets the simple game information list asynchronously.
        /// </summary>
        /// <returns>
        /// The list of simple game information.
        /// </returns>
        public Task<List<SimpleGameInformation>> GetSimpleGameInformationListAsync()
        {
            this.logger.LogInformation("[GetSimpleGameInformationListAsync]");

            List<SimpleGameInformation> simpleGameInformation = new List<SimpleGameInformation>();

            foreach (var item in this.games)
            {
                simpleGameInformation.Add(new SimpleGameInformation(item.PlayerOne.PlayerName, item.PlayerTwo.PlayerName));
            }

            return Task.FromResult(simpleGameInformation);
        }
    }
}
