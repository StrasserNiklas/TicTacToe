// Niklas Strasser, Felix Brandstetter, Yannick Gruber

namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using GameLibrary;
    

    public class Game
    {
        /// <summary>
        /// This field is used to save player two.
        /// </summary>
        private Player playerTwo;

        /// <summary>
        /// This field is used to save player one.
        /// </summary>
        private Player playerOne;

        /// <summary>
        /// This field is used to save the current player.
        /// </summary>
        private Player currentPlayer;

        /// <summary>
        /// This field is used to save the list of win conditions.
        /// </summary>
        private readonly List<WinCondition> winConditions;

        /// <summary>
        /// This field is used to save the indexed game.
        /// </summary>
        private int[] indexedGame = new int[9];

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="one">The one.</param>
        /// <param name="two">The two.</param>
        public Game(Player one, Player two)
        {
            this.winConditions = new List<WinCondition>()
            {
                new WinCondition(0,3,6),
                new WinCondition(1,4,7),
                new WinCondition(2,5,8),
                new WinCondition(0,1,2),
                new WinCondition(3,4,5),
                new WinCondition(6,7,8),
                new WinCondition(0,4,8),
                new WinCondition(2,4,6)
            };


            this.playerOne = one;
            this.playerTwo = two;
            this.PlayerOne.Marker = 1;
            this.PlayerTwo.Marker = 2;
            this.CurrentPlayer = this.playerOne;
            this.GameOver = false;
            this.EndMessage = string.Empty;

            Random r = new Random();
            this.GameId = r.Next(999, 1234567) + r.Next(999, 1234567);
        }

        /// <summary>
        /// Gets the game identifier.
        /// </summary>
        /// <value>
        /// The game identifier.
        /// </value>
        public int GameId { get; }

        public int Turns { get; set; }

        /// <summary>
        /// Resets the game and the players status in order for a new game to start.
        /// </summary>
        public void NewGameSetup()
        {
            this.GameOver = false;
            this.CurrentGameStatus = new int[9];
            this.PlayerOne.MarkedPositions = new List<int>();
            this.PlayerTwo.MarkedPositions = new List<int>();
            this.EndMessage = string.Empty;
            this.Turns = 0;
        }

        /// <summary>
        /// This method iterates over the possible win conditions of the current player after he made his mark.
        /// </summary>
        /// <returns>True if a win condition has been met, false otherwise.</returns>
        public bool CheckWinConditions()
        {
            bool isWin = false;

            if (!this.GameOver)
            {
                if (this.Turns > 4 && !this.GameOver)
                {
                    foreach (var condition in this.winConditions)
                    {
                        foreach (var index in condition.Condition)
                        {
                            if (this.CurrentPlayer.MarkedPositions.Contains(index))
                            {
                                isWin = true;
                                continue;
                            }
                            else
                            {
                                isWin = false;
                                break;
                            }
                        }

                        if (isWin)
                        {
                            break;
                        }
                    }
                }
            }

            if (isWin)
            {
                this.GameOver = true;
                this.CurrentPlayer.Wins++;
                this.EndMessage = $"{this.CurrentPlayer.PlayerName} wins!";
                return isWin;
            }

            if (this.Turns == 9 && !this.GameOver)
            {
                this.EndMessage = $"It´s a draw!";
                this.GameOver = true;
                return true;
            }

            return false;
        }

        private Player GetOtherPlayer(Player player)
        {
            return (player == this.PlayerOne) ? this.PlayerTwo : this.PlayerOne;
        }

        public bool IsMoveValid(int updatedPosition, Player player)
        {
            if (updatedPosition < 0 || updatedPosition >= 9)
            {
                return false;
            }

            if (this.CurrentGameStatus[updatedPosition] != 0)
            {
                return false;
            }

            if (this.CurrentPlayer != player)
            {
                return false;
            }

            return true;
        }

        public bool MakeMove(int updatedPosition, Player player)
        {
            if (IsMoveValid(updatedPosition, player))
            {
                this.CurrentGameStatus[updatedPosition] = this.CurrentPlayer.Marker;
                this.CurrentPlayer.MarkedPositions.Add(updatedPosition);
                this.Turns++;

                var gameFinished = this.CheckWinConditions();

                this.CurrentPlayer = this.GetOtherPlayer(player);

                return gameFinished;
            }

            throw new InvalidOperationException("Invalid move was tried.");
        }

        /// <summary>
        /// Gets or sets the end message.
        /// </summary>
        /// <value>
        /// The end message.
        /// </value>
        public string EndMessage { get; set; }

        /// <summary>
        /// Gets or sets the current player.
        /// </summary>
        /// <value>
        /// The current player.
        /// </value>
        /// <exception cref="ArgumentNullException">CurrentPlayer - The current player can´t be null.</exception>
        public Player CurrentPlayer
        {
            get
            {
                return this.currentPlayer;
            }
            set
            {
                this.currentPlayer = value ?? throw new ArgumentNullException(nameof(this.CurrentPlayer), "The current player can´t be null.");
            }
        }

        /// <summary>
        /// Gets or sets the player one.
        /// </summary>
        /// <value>
        /// The player one.
        /// </value>
        /// <exception cref="ArgumentNullException">PlayerOne - Player one can´t be null.</exception>
        public Player PlayerOne
        {
            get
            {
                return this.playerOne;
            }

            set
            {
                this.playerOne = value ?? throw new ArgumentNullException(nameof(this.PlayerOne), "Player one can´t be null.");
            }
        }

        /// <summary>
        /// Gets or sets the player two.
        /// </summary>
        /// <value>
        /// The player two.
        /// </value>
        /// <exception cref="ArgumentNullException">PlayerTwo - Player two can´t be null.</exception>
        public Player PlayerTwo
        {
            get
            {
                return this.playerTwo;
            }

            set
            {
                this.playerTwo = value ?? throw new ArgumentNullException(nameof(this.PlayerTwo), "Player two can´t be null.");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the game is over.
        /// </summary>
        /// <value>
        ///   <c>true</c> if game is over; otherwise, <c>false</c>.
        /// </value>
        public bool GameOver { get; set; }

        /// <summary>
        /// Gets or sets the current game status.
        /// </summary>
        /// <value>
        /// The current game status.
        /// </value>
        /// <exception cref="ArgumentException">CurrentGameStatus - The indexed game array has to be of length 9.</exception>
        public int[] CurrentGameStatus
        {
            get
            {
                return this.indexedGame;
            }
            set
            {
                if (value.Length != 9)
                {
                    throw new ArgumentException(nameof(this.CurrentGameStatus), "The indexed game array has to be of length 9.");
                }

                this.indexedGame = value;
            }
        }
    }
}
