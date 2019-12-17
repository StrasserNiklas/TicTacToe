//-----------------------------------------------------------------------
// <copyright file="WinCondition.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents a win condition for the game.</summary>
//-----------------------------------------------------------------------

namespace Server.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A win condition is based on the continuous indexes of a TicTacToe game.
    /// Typical TicTacToe-Game: 
    /// 
    /// [O X O]               [0 1 2]
    /// [X O O] -> indexed -> [3 4 5]
    /// [O X O]               [6 7 8]    
    /// 
    /// Based on that, a win condition would be: 2,5,8 as can be seen in the example game.
    /// </summary>
    public class WinCondition
    {
        public WinCondition(params int[] condition)
        {
            this.Condition = condition.ToList();
        }

        public List<int> Condition { get; set; }
    }
}
