using GameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private Game currentGame;

        public Bot(Game currentGame)
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
            this.currentGame = currentGame;
        }

        public void Play(GameStatus status)
        {
            this.turnCount++;

            

            this.turnCount++;
        }

        public int MakeBotMove(GameStatus status)
        {
            // make intelligent choice
            if (this.turnCount > 4)
            {

                var isBotWinnable = this.IsWinningMove(this.currentGame.PlayerTwo, status);

                if (isBotWinnable.Item2)
                {
                    status.IndexedGame[isBotWinnable.Item1] = this.currentGame.PlayerTwo.Marker;
                    return isBotWinnable.Item1;
                }

                var isHumanWinnable = this.IsWinningMove(this.currentGame.PlayerTwo, status);

                if (isHumanWinnable.Item2)
                {
                    status.IndexedGame[isHumanWinnable.Item1] = this.currentGame.PlayerTwo.Marker;
                    return isHumanWinnable.Item1;
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
                        status.IndexedGame[random] = this.currentGame.PlayerTwo.Marker;
                        return random;
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

            return -1;
        }

        public (int, bool) IsWinningMove(Player player, GameStatus status)
        {
            bool isWinnable;

            for (int i = 0; i < status.IndexedGame.Length; i++)
            {
                if (status.IndexedGame[i] == 0)
                {
                    foreach (var condition in this.winConditions)
                    {
                        var tempList = new List<int>(player.MarkedPositions);
                        tempList.Add(i);

                        isWinnable = condition.Condition.All(x => tempList.Contains(x));

                        if (isWinnable)
                        {
                            return (i, isWinnable);
                        }
                    }
                }
            }

            return (-1, false);
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
