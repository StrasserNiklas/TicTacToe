//-----------------------------------------------------------------------
// <copyright file="IMainService.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents an interface for the main service.</summary>
//-----------------------------------------------------------------------

namespace Server.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GameLibrary;

    /// <summary>
    /// Represents an interface for the main service.
    /// </summary>
    public interface IMainService
    {
        /// <summary>
        /// Gets the game requests asynchronously.
        /// </summary>
        /// <returns>The collection of game requests.</returns>
        Task<IEnumerable<GameRequest>> GetGameRequestsAsync();

        /// <summary>
        /// Adds the game request asynchronously.
        /// </summary>
        /// <param name="gameRequest">The game request.</param>
        /// <returns>The game request which has been added.</returns>
        Task<GameRequest> AddGameRequestAsync(GameRequest gameRequest);

        /// <summary>
        /// Gets the games asynchronously.
        /// </summary>
        /// <returns>The collection of games.</returns>
        Task<IEnumerable<Game>> GetGamesAsync();

        /// <summary>
        /// Adds the game asynchronously.
        /// </summary>
        /// <param name="game">The game to be added.</param>
        /// <returns>The added game.</returns>
        Task<Game> AddGameAsync(Game game);

        /// <summary>
        /// Removes the game asynchronously.
        /// </summary>
        /// <param name="game">The game to be removed.</param>
        /// <returns>The game which has been removed.</returns>
        Task<Game> RemoveGameAsync(Game game);

        /// <summary>
        /// Gets the players asynchronously.
        /// </summary>
        /// <returns>The collection of all players.</returns>
        Task<IEnumerable<Player>> GetPlayersAsync();

        /// <summary>
        /// Adds the player asynchronously.
        /// </summary>
        /// <param name="player">The player to be added.</param>
        /// <returns>The player which has been added.</returns>
        Task<Player> AddPlayerAsync(Player player);

        /// <summary>
        /// Removes the player asynchronously.
        /// </summary>
        /// <param name="player">The player to be removed.</param>
        /// <returns>The player which has been removed.</returns>
        Task<Player> RemovePlayerAsync(Player player);

        /// <summary>
        /// Removes the request asynchronously.
        /// </summary>
        /// <param name="gameRequest">The game request.</param>
        /// <param name="accepted">An indication whether the request has been accepted or declined.</param>
        /// <returns>The removed game request.</returns>
        Task<GameRequest> RemoveRequestAsync(GameRequest gameRequest, bool accepted);

        /// <summary>
        /// Gets the players not in game asynchronously.
        /// </summary>
        /// <returns>The collection of players who are not in a game.</returns>
        Task<IEnumerable<Player>> GetPlayersNotInGameAsync();

        /// <summary>
        /// Gets the simple game information list asynchronously.
        /// </summary>
        /// <returns>The list of simple game information.</returns>
        Task<List<SimpleGameInformation>> GetSimpleGameInformationListAsync();
    }
}
