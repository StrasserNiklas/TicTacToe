//-----------------------------------------------------------------------
// <copyright file="GameRequest.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents a game request to play with another player.</summary>
//-----------------------------------------------------------------------

namespace GameLibrary
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// This class represents a game request in a game. 
    /// It contains two players, one who sent the request, another the request is addressed to, 
    /// information whether the request has been accepted or not and an unique request id.
    /// </summary>
    public class GameRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameRequest"/> class.
        /// </summary>
        /// <param name="enemy">The enemy.</param>
        /// <param name="requestPlayer">The requesting player.</param>
        [JsonConstructor]
        public GameRequest(Player enemy, Player requestPlayer)
        {
            this.Enemy = enemy;
            this.RequestingPlayer = requestPlayer;

            Random r = new Random();
            this.RequestID = r.Next(999, 1234567) + r.Next(999, 1234567);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameRequest"/> class.
        /// </summary>
        public GameRequest()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether a game request has been accepted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if accepted; otherwise, <c>false</c>.
        /// </value>
        public bool Accepted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a game request has been declined.
        /// </summary>
        /// <value>
        ///   <c>true</c> if declined; otherwise, <c>false</c>.
        /// </value>
        public bool Declined { get; set; }

        /// <summary>
        /// Gets or sets the id of the request, which is chosen randomly and can´t be set afterwards.
        /// </summary>
        /// <value>
        /// The id of the request.
        /// </value>
        public int RequestID { get; set; }

        /// <summary>
        /// Gets or sets the player that has sent/started the request.
        /// </summary>
        /// <value>
        /// The requesting player.
        /// </value>
        public Player RequestingPlayer { get; set; }

        /// <summary>
        /// Gets or sets the player the request is sent to.
        /// </summary>
        /// <value>
        /// The player the request is sent to.
        /// </value>
        public Player Enemy { get; set; }
    }
}
