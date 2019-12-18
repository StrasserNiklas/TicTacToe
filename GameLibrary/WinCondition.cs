﻿//-----------------------------------------------------------------------
// <copyright file="WinCondition.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents a win condition for the game.</summary>
//-----------------------------------------------------------------------

namespace GameLibrary
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A win condition is based on the continuous indexes of a Tic-Tac-Toe game.
    /// Typical Tic-Tac-Toe-Game: 
    /// [O X O]               [0 1 2]
    /// [X O O] -> indexed -> [3 4 5]
    /// [O X O]               [6 7 8]    
    /// Based on that, a win condition would be: 2,5,8 as can be seen in the example game.
    /// </summary>
    public class WinCondition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinCondition"/> class.
        /// </summary>
        /// <param name="condition">The win condition.</param>
        public WinCondition(params int[] condition)
        {
            this.Condition = condition.ToList();
        }

        /// <summary>
        /// Gets or sets the list of integer the condition requires.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public List<int> Condition { get; set; }
    }
}
