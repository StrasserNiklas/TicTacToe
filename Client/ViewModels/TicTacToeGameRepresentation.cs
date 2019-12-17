// Niklas Strasser, Felix Brandstetter, Yannick Gruber

namespace Client.ViewModels
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// This class represents a normal tic tac game field containing 9 fields.
    /// </summary>
    public class TicTacToeGameRepresentation
    {
        /// <summary>
        /// This field is used to save the game cells collection.
        /// </summary>
        private ObservableCollection<GameCellVM> gameCells;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToeGameRepresentation"/> class.
        /// </summary>
        public TicTacToeGameRepresentation()
        {
            this.gameCells = new ObservableCollection<GameCellVM>()
            {
                new GameCellVM(0, 0),
                new GameCellVM(1, 0),
                new GameCellVM(2, 0),
                new GameCellVM(3, 0),
                new GameCellVM(4, 0),
                new GameCellVM(5, 0),
                new GameCellVM(6, 0),
                new GameCellVM(7, 0),
                new GameCellVM(8, 0)
            };
        }

        /// <summary>
        /// Gets or sets the game cells.
        /// </summary>
        /// <value>
        /// The game cells.
        /// </value>
        /// <exception cref="ArgumentNullException">GameCells - The list of marked positions can´t be null.</exception>
        public ObservableCollection<GameCellVM> GameCells
        {
            get
            {
                return gameCells;
            }
            set
            {
                gameCells = value ?? throw new ArgumentNullException(nameof(this.GameCells), "The list of marked positions can´t be null.");
            }
        }
    }
}
