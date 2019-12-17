//-----------------------------------------------------------------------
// <copyright file="SimpleGameInformation.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents some information about the two players inside a game.</summary>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary
{
    public class SimpleGameInformation
    {
        public SimpleGameInformation(string playerOne, string playerTwo)
        {
            this.PlayerOne = playerOne;
            this.PlayerTwo = playerTwo;
        }

        public SimpleGameInformation()
        {
        }

        public string PlayerOne { get; set; }

        public string PlayerTwo { get; set; }
    }
}
