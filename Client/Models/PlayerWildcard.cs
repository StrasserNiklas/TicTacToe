using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class PlayerWildcard
    {
        private int wins;
        private string playerName;
        private List<int> markedPositions;
        private string hash;
        private int playerId;

        public PlayerWildcard(string playerName)
        {
            var time = DateTime.Now;

            this.Hash = $"{time.ToString()}{playerName}";
            this.PlayerName = playerName;
            this.MarkedPositions = new List<int>();
        }

        public int PlayerId
        {
            get
            {
                return this.playerId;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.PlayerId), "The player ID can´t be below 0.");
                }

                this.playerId = value;
            }
        }


        public string Hash
        {
            get
            {
                return this.hash;
            }
            set
            {
                if (value == string.Empty)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.Hash), "The hash identifying a player can´t be an empty string.");
                }

                this.hash = value;
            }
        }


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

        public int Marker { get; set; }


        public int Wins
        {
            get
            {
                return wins;
            }
            set
            {
                if (value > 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.Wins), "You can´t have less than 0 wins.");
                }

                wins = value;
            }
        }

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
