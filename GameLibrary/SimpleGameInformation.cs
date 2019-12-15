// Niklas Strasser, Felix Brandstetter, Yannick Gruber

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
