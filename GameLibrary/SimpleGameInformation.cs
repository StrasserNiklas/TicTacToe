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

        public string PlayerOne { get; }

        public string PlayerTwo { get; }
    }
}
