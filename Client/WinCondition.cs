using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Client
{
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

    public class ClientVM : BaseVM
    {
        private bool onlineGameSelected;
        private bool localGameSelected;

        public ClientVM(Game game)
        {
            this.CurrentGame = game;
            this.LocalGameSelected = true;
            this.OnlineGameSelected = false;
        }

        public ICommand LocalGameButtonPressed
        {
            get
            {
                return new Command(obj =>
                {
                    this.LocalGameSelected = true;
                    this.OnlineGameSelected = false;
                });
            }
        }

        public ICommand OnlineGameButtonPressed
        {
            get
            {
                return new Command(obj =>
                {
                    this.LocalGameSelected = false;
                    this.OnlineGameSelected = true;
                });
            }
        }

        public Game CurrentGame { get; set; }

        public bool LocalGameSelected
        {
            get
            {
                return localGameSelected;
            }
            set
            {
                localGameSelected = value;
                this.FireOnPropertyChanged(nameof(this.LocalGameSelected));
            }
        }


        public bool OnlineGameSelected
        {
            get
            {
                return onlineGameSelected;
            }
            set
            {
                onlineGameSelected = value;
                this.FireOnPropertyChanged(nameof(this.OnlineGameSelected));
            }
        }




    }

    public class Game : BaseVM
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


        private Player currentPlayer;

        public Player CurrentPlayer
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

        private Player playerOne;
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
        private Player playerTwo;
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
        public List<WinCondition> WinConditions { get; set; }
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

        public Game(Player one, Player two)
        {
            this.WinConditions = new List<WinCondition>()
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
        }



        /// <summary>
        /// This command is used when a game element button is clicked.
        /// Updates the game status and after 5 turns, checks if a win condition has been met.
        /// </summary>
        public ICommand PlayerClick
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
        public bool CheckForWin()
        {
            bool isWin = false;

            foreach (var condition in this.WinConditions)
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
    }

    public class Player : BaseVM
    {
        private string playerName;

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        public int Marker { get; set; }

        private int wins;

        public int Wins
        {
            get
            {
                return wins;
            }
            set
            {
                wins = value;
                this.FireOnPropertyChanged(nameof(this.Wins));
            }
        }


        public Player(string playerName, int marker)
        {
            this.playerName = playerName;
            this.markedPositions = new List<int>();
            this.Marker = marker;
        }

        private List<int> markedPositions;

        public List<int> MarkedPositions
        {
            get
            {
                return this.markedPositions;
            }
            set
            {
                this.markedPositions = value ?? throw new ArgumentNullException(nameof(this.MarkedPositions), "The list of marked positions can´t be null.");
            }
        }

    }



    public class Command : ICommand
    {
        /// <summary>
        /// This field is used to save the action.
        /// </summary>
        private readonly Action<object> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public Command(Action<object> action)
        {
            this.action = action;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        /// <returns>
        ///   <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}
