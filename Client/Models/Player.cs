using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class Player
    {
        private int wins;
        private string playerName;
        private List<int> markedPositions;
        private string hash;
        private int playerId;

        public Player(string playerName)
        {
            var time = DateTime.Now;

            this.Hash = $"{time.ToString()}{playerName}";
            this.PlayerName = playerName;
            this.MarkedPositions = new List<int>();
        }

        public int PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
        }


        public string Hash
        {
            get { return hash; }
            set { hash = value; }
        }


        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
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
                    throw new ArgumentException(nameof(this.Wins), "You can´t have less than 0 wins.");
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
