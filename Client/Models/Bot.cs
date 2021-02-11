using GameLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class Bot
    {
        /// <summary>
        /// This field is used to save the list of win conditions.
        /// </summary>
        private readonly List<WinCondition> winConditions;

        private int turnCount;

        private Random random;

        private int botMarker;

        public Bot(int botMarker)
        {
            this.winConditions = new List<WinCondition>()
            {
                new WinCondition(0, 3, 6),
                new WinCondition(1, 4, 7),
                new WinCondition(2, 5, 8),
                new WinCondition(0, 1, 2),
                new WinCondition(3, 4, 5),
                new WinCondition(6, 7, 8),
                new WinCondition(0, 4, 8),
                new WinCondition(2, 4, 6)
            };

            this.turnCount = 0;
            this.random = new Random();
            this.botMarker = botMarker;
        }

        public void Play(GameStatus status)
        {
            this.turnCount++;

            

            this.turnCount++;
        }

        public bool CheckWinConditions(GameStatus status)
        {            
            bool isWin = false;

            // make intelligent choice
            if (this.turnCount > 4)
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
            // make random choice
            else
            {
                while (true)
                {
                    var random = this.random.Next(0, 8);

                    if (status.IndexedGame[random] == 0)
                    {
                        status.IndexedGame[random] = this.botMarker;
                        break;
                    }
                }
            }
            
            //if (isWin)
            //{
            //    this.GameOver = true;
            //    this.CurrentPlayer.Wins++;
            //    this.EndMessage = $"{this.CurrentPlayer.PlayerName} wins!";
            //    return isWin;
            //}

            //if (this.Turns == 9 && !this.GameOver)
            //{
            //    this.EndMessage = $"It´s a draw!";
            //    this.GameOver = true;
            //    return true;
            //}

            return false;
        }

        public bool IsGameOver()
        {
            if (this.turnCount > 8)
            {
                return true;
            }

            return false;
        }
    }
}
