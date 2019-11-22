using Client.Models;
using Client.Services;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Client
{
    public class GameVM : BaseVM
    {
        private string endGameMessage;

        public string EndMessage
        {
            get
            {
                return endGameMessage;
            }
            set
            {
                endGameMessage = value;
                this.FireOnPropertyChanged(nameof(this.EndMessage));
            }
        }


        private PlayerVM currentPlayer;

        public PlayerVM CurrentPlayer
        {
            get
            {
                return this.currentPlayer;
            }
            set
            {
                this.currentPlayer = value;
                this.FireOnPropertyChanged(nameof(this.CurrentPlayer));
            }
        }

        private PlayerVM playerOne;
        public PlayerVM PlayerOne
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
        private PlayerVM playerTwo;
        public PlayerVM PlayerTwo
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
        //public List<WinCondition> WinConditions { get; set; }
        private int gameTurns = 0;

        private bool gameOver;

        public bool GameOver
        {
            get
            {
                return gameOver;
            }
            set
            {
                gameOver = value;
                this.FireOnPropertyChanged(nameof(this.GameOver));
            }
        }

        public int[] CurrentGameStatus => this.indexedGame;
        private int[] indexedGame = new int[9];

        public GameVM(PlayerVM one, PlayerVM two)
        {
            //this.WinConditions = new List<WinCondition>()
            //{
            //    new WinCondition(0,3,6),
            //    new WinCondition(1,4,7),
            //    new WinCondition(2,5,8),
            //    new WinCondition(0,1,2),
            //    new WinCondition(3,4,5),
            //    new WinCondition(6,7,8),
            //    new WinCondition(0,4,8),
            //    new WinCondition(2,4,6)
            //};

            this.playerOne = one;
            this.playerTwo = two;
            this.CurrentPlayer = this.playerOne;
            this.GameOver = false;
            this.EndMessage = string.Empty;
        }



        /// <summary>
        /// This command is used when a game element button is clicked.
        /// Updates the game status and after 5 turns, checks if a win condition has been met.
        /// </summary>
        public ICommand PlayerClickOld
        {
            get
            {
                return new Command(obj =>
                {
                    if (!this.GameOver)
                    {
                        var index = int.Parse((string)obj);

                        if (this.indexedGame[index] == 0)
                        {
                            //this.indexedGame[index] = this.CurrentPlayer.Marker;
                            this.CurrentPlayer.MarkedPositions.Add(index);
                            this.gameTurns++;

                            // an server

                            if (this.gameTurns > 4 && !this.GameOver)
                            {
                                //bool check = this.CheckForWin();

                                //if (check)
                                //{
                                //    this.GameOver = true;
                                //    this.CurrentPlayer.Wins++;
                                //    this.EndMessage = $"{this.CurrentPlayer.PlayerName} wins!";
                                //}

                                //if (this.gameTurns == 9 && !this.GameOver)
                                //{
                                //    this.EndMessage = $"It´s a draw!";
                                //    this.GameOver = true;
                                //}
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
                });
            }
        }

        /// <summary>
        /// Resets the game and the players status in order for a new game to start.
        /// </summary>
        public ICommand NewGameCommand
        {
            get
            {
                return new Command(obj =>
                {
                    this.GameOver = false;
                    this.indexedGame = new int[9];
                    this.gameTurns = 0;
                    this.playerOne.MarkedPositions = new List<int>();
                    this.playerTwo.MarkedPositions = new List<int>();
                    this.EndMessage = string.Empty;
                });
            }
        }

        /// <summary>
        /// This method iterates over the possible win conditions of the current player after he made his mark.
        /// </summary>
        /// <returns>True if a win condition has been met, false otherwise.</returns>
        //public bool CheckForWin()
        //{
        //    bool isWin = false;

        //    foreach (var condition in this.WinConditions)
        //    {
        //        foreach (var index in condition.Condition)
        //        {
        //            if (this.CurrentPlayer.MarkedPositions.Contains(index))
        //            {
        //                isWin = true;
        //                continue;
        //            }
        //            else
        //            {
        //                isWin = false;
        //                break;
        //            }
        //        }

        //        if (isWin)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
    }


}
