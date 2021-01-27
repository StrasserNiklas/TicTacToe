//-----------------------------------------------------------------------
// <copyright file="GameStatus.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents the game status which is sent to the opponent player to refresh the game.</summary>
//-----------------------------------------------------------------------

namespace GameLibrary
{
    using Newtonsoft.Json;

    /// <summary>
    /// This class is used to update the status of a game on the server and on clients.
    /// It contains the current game status, the current player id, the updated position, the id of the game and the wins of the players.
    /// </summary>
    public class GameStatus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameStatus"/> class.
        /// </summary>
        /// <param name="indexedGame">The indexed game.</param>
        /// <param name="currentPlayerId">The current player identifier.</param>
        /// <param name="currentPlayerMarker">The current player marker.</param>
        /// <param name="gameID">The game identifier.</param>
        /// <param name="winsPlayerOne">The wins of player one.</param>
        /// <param name="winsPlayerTwo">The wins of player two.</param>
        [JsonConstructor]
        public GameStatus(int[] indexedGame, string currentPlayerId, int currentPlayerMarker, int gameID, int winsPlayerOne, int winsPlayerTwo)
        {
            this.IndexedGame = indexedGame;
            this.CurrentPlayerId = currentPlayerId;
            this.CurrentPlayerMarker = currentPlayerMarker;
            this.GameId = gameID;
            this.UpdatedPosition = -1;
            this.WinsPlayerOne = winsPlayerOne;
            this.WinsPlayerTwo = winsPlayerTwo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameStatus"/> class.
        /// </summary>
        public GameStatus()
        {
            this.UpdatedPosition = -1;
            this.WinsPlayerOne = 0;
            this.WinsPlayerTwo = 0;
        }

        /// <summary>
        /// Gets or sets the updated position.
        /// </summary>
        /// <value>
        /// The updated position.
        /// </value>
        public int UpdatedPosition { get; set; }

        /// <summary>
        /// Gets or sets the game identifier.
        /// </summary>
        /// <value>
        /// The game identifier.
        /// </value>
        public int GameId { get; set; }

        /// <summary>
        /// Gets or sets the indexed game.
        /// </summary>
        /// <value>
        /// The indexed game.
        /// </value>
        public int[] IndexedGame { get; set; }

        /// <summary>
        /// Gets or sets the current player identifier.
        /// </summary>
        /// <value>
        /// The current player identifier.
        /// </value>
        public string CurrentPlayerId { get; set; }

        /// <summary>
        /// Gets or sets the current player marker.
        /// </summary>
        /// <value>
        /// The current player marker.
        /// </value>
        public int CurrentPlayerMarker { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a new game.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a new game; otherwise, <c>false</c>.
        /// </value>
        public bool IsNewGame { get; set; }

        /// <summary>
        /// Gets or sets the wins of player one.
        /// </summary>
        /// <value>
        /// The wins of player one.
        /// </value>
        public int WinsPlayerOne { get; set; }

        /// <summary>
        /// Gets or sets the wins of player two.
        /// </summary>
        /// <value>
        /// The wins of player two.
        /// </value>
        public int WinsPlayerTwo { get; set; }
    }
}
