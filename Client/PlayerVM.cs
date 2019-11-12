using Client.Models;
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
            get 
            { 
                return this.player.PlayerName; 
            }
            set 
            { 
                this.player.PlayerName = value;
                this.FireOnPropertyChanged();
            }
        }

        public PlayerVM(Player player)
        {
            this.Player = player;
        }

        private Player player;

        public Player Player
        {
            get
            { 
                return player; 
            }

            set
            { 
                player = value; 
            }
        }



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
