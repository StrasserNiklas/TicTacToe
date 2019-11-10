using Client.ViewModels;
using System;
using System.Collections.Generic;

namespace Client
{
    public class PlayerVM : BaseVM
    {
        private string playerName;

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        public int Marker { get; set; }

        private int wins;

        public int Wins
        {
            get
            {
                return wins;
            }
            set
            {
                wins = value;
                this.FireOnPropertyChanged(nameof(this.Wins));
            }
        }


        public PlayerVM(string playerName, int marker)
        {
            this.playerName = playerName;
            this.markedPositions = new List<int>();
            this.Marker = marker;
        }

        private List<int> markedPositions;

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
