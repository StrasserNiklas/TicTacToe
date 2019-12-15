// Niklas Strasser, Felix Brandstetter, Yannick Gruber

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace GameLibrary
{
    public class Player : INotifyPropertyChanged
    {
        private int wins;
        private string playerName;
        private List<int> markedPositions;
        private string hash;
        private int playerId;
        private string connectionId;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="playerName">Name of the player.</param>
        public Player(string playerName)
        {
            this.PlayerName = playerName;
            this.MarkedPositions = new List<int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player()
        {
            this.PlayerName = "Player";
        }

        /// <summary>
        /// Gets or sets the unique connection identifier set by the signalR service.
        /// </summary>
        /// <value>
        /// The unique connection identifier set by the signalR service.
        /// </value>
        public string ConnectionId
        {
            get
            {
                return this.connectionId;
            }
            set
            {
                this.connectionId = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>
        /// The name of the player.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">PlayerName - The player name can´t be an empty string.</exception>
        public string PlayerName
        {
            get
            {
                return this.playerName;
            }
            set
            {
                if (value == string.Empty)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.PlayerName), "The player name can´t be an empty string.");
                }

                this.playerName = value;
            }
        }

        /// <summary>
        /// Gets or sets the marker of the player.
        /// </summary>
        /// <value>
        /// The marker of the player.
        /// </value>
        public int Marker { get; set; }

        /// <summary>
        /// Gets or sets the wins of the player.
        /// </summary>
        /// <value>
        /// The wins of the player.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">Wins - You can´t have less than 0 wins.</exception>
        public int Wins
        {
            get
            {
                return this.wins;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.Wins), "You can´t have less than 0 wins.");
                }

                this.wins = value;
                this.FireOnPropertyChanged();
            }
        }

        /// <summary>
        /// Fires the property changed event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void FireOnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets or sets the marked positions of the player.
        /// </summary>
        /// <value>
        /// The marked positions of the player.
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
