//-----------------------------------------------------------------------
// <copyright file="PlayerVM.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents a view model for a player.</summary>
//-----------------------------------------------------------------------

namespace Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using GameLibrary;

    /// <summary>
    /// Represents a view model for a player.
    /// </summary>
    /// <seealso cref="Client.ViewModels.BaseVM" />
    public class PlayerVM : BaseVM
    {
        /// <summary>
        /// This field is used to save the marked positions.
        /// </summary>
        private List<int> markedPositions;

        /// <summary>
        /// This field is used to save the wins.
        /// </summary>
        private int wins;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerVM"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        public PlayerVM(Player player)
        {
            this.Player = player;
        }

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>
        /// The name of the player.
        /// </value>
        public string PlayerName
        {
            get
            {
                return this.Player.PlayerName;
            }

            set
            {
                this.Player.PlayerName = value;
                this.FireOnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public Player Player { get; set; }

        /// <summary>
        /// Gets or sets the wins.
        /// </summary>
        /// <value>
        /// The wins of the player.
        /// </value>
        public int Wins
        {
            get
            {
                return this.wins;
            }

            set
            {
                this.wins = value;
                this.FireOnPropertyChanged(nameof(this.Wins));
            }
        }

        /// <summary>
        /// Gets or sets the marked positions.
        /// </summary>
        /// <value>
        /// The marked positions.
        /// </value>
        /// <exception cref="ArgumentNullException">MarkedPositions - The list of marked positions can´t be null.</exception>
        public List<int> MarkedPositions
        {
            get
            {
                return this.markedPositions;
            }

            set
            {
                this.markedPositions = value ?? throw new ArgumentNullException(nameof(this.MarkedPositions), "The list of marked positions can´t be null.");
            }
        }
    }
}
