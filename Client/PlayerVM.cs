//-----------------------------------------------------------------------
// <copyright file="PlayerVM.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents a view model for a player.</summary>
//-----------------------------------------------------------------------

namespace Client
{
    using System;
    using System.Collections.Generic;
    using Client.Models;
    using Client.ViewModels;
    using GameLibrary;
    
    public class PlayerVM : BaseVM
    {
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
