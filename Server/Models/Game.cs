using GameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Server.Models
{
    public class Game
    {
        private Player playerTwo;
        private Player playerOne;
        private string endGameMessage;
        private Player currentPlayer;
        private int gameTurns = 0;
        private List<WinCondition> winConditions;
        private int[] indexedGame = new int[9];
        private bool gameOver;

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
            this.CurrentPlayer = this.playerOne;
            this.GameOver = false;
            this.EndMessage = string.Empty;

            Random r = new Random();
            this.GameId = r.Next(999, 1234567) + r.Next(999, 1234567);
        }

        public int GameId { get; }



        // just a place holder for the code that checks if a player won
        private void CheckWinCodePlaceholder()
        {
            if (!this.GameOver)
            {
                var index = 0; //PLACEHOLDER

                if (this.indexedGame[index] == 0)
                {

                    this.indexedGame[index] = this.CurrentPlayer.Marker;
                    this.CurrentPlayer.MarkedPositions.Add(index);
                    this.gameTurns++;

                    // an server

                    if (this.gameTurns > 4 && !this.GameOver)
                    {
                        bool check = this.CheckForWin();

                        if (check)
                        {
                            this.GameOver = true;
                            this.CurrentPlayer.Wins++;
                            this.EndMessage = $"{this.CurrentPlayer.PlayerName} wins!";
                        }

                        if (this.gameTurns == 9 && !this.GameOver)
                        {
                            this.EndMessage = $"It´s a draw!";
                            this.GameOver = true;
                        }
                    }


                    if (this.CurrentPlayer == this.playerOne)
                    {
                        this.CurrentPlayer = this.playerTwo;
                    }
                    else
                    {
                        this.CurrentPlayer = this.playerOne;
                    }
                }
            }
        }


        /// <summary>
        /// Resets the game and the players status in order for a new game to start.
        /// </summary>
        public void NewGameSetup()
        {
            this.GameOver = false;
            this.indexedGame = new int[9];
            this.gameTurns = 0;
            this.playerOne.MarkedPositions = new List<int>();
            this.playerTwo.MarkedPositions = new List<int>();
            this.EndMessage = string.Empty;
        }

        /// <summary>
        /// This method iterates over the possible win conditions of the current player after he made his mark.
        /// </summary>
        /// <returns>True if a win condition has been met, false otherwise.</returns>
        public bool CheckForWin()
        {
            bool isWin = false;

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
                    return true;
                }
            }

            return false;
        }

        public string EndMessage
        {
            get
            {
                return endGameMessage;
            }
            set
            {
                endGameMessage = value;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return this.currentPlayer;
            }
            set
            {
                this.currentPlayer = value;
            }
        }

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

        public bool GameOver
        {
            get
            {
                return gameOver;
            }
            set
            {
                gameOver = value;
            }
        }

        public int[] CurrentGameStatus => this.indexedGame;
    }
}
