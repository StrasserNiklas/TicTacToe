//-----------------------------------------------------------------------
// <copyright file="GameCellVM.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents a view model for a specific game cell.</summary>
//-----------------------------------------------------------------------

namespace Client.ViewModels
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Represents a single cell of the tic-tac-toe game with an index and a background.
    /// [O X O]               [0 1 2].
    /// [X O O] -> indexed -> [3 4 5].
    /// [O X O]               [6 7 8].
    /// </summary>
    public class GameCellVM : BaseVM
    {
        /// <summary>
        /// This field is used to save the player mark.
        /// </summary>
        private int playerMark;

        /// <summary>
        /// This field is used to save the cell background.
        /// </summary>
        private Brush cellBackground;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameCellVM"/> class.
        /// </summary>
        /// <param name="index">The index of the game cell.</param>
        /// <param name="playerMark">The player mark of the game cell.</param>
        public GameCellVM(int index, int playerMark)
        {
            this.Index = index;
            this.PlayerMark = playerMark;
        }

        /// <summary>
        /// Gets the game index the cell is representing.
        /// </summary>
        /// <value>The game index the cell is representing.</value>
        public int Index { get; }

        /// <summary>
        /// Gets or sets the player mark the cell is representing.
        /// Based on the mark, the background of the cell will be set to a different background.
        /// </summary>
        /// <value>The player mark the cell is representing.</value>
        public int PlayerMark
        {
            get
            {
                return this.playerMark;
            }

            set 
            {
                switch (value)
                {
                    case 0:
                        this.CellBackground = new SolidColorBrush(Colors.White);
                        break;

                    case 1:
                        var brush = new ImageBrush();
                        brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/O.png"));
                        this.CellBackground = brush;
                        break;

                    case 2:
                        var brush1 = new ImageBrush();
                        brush1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/X.png"));
                        this.CellBackground = brush1;
                        break;

                    default:
                        break;
                }

                this.playerMark = value;
            }
        }

        /// <summary>
        /// Gets or sets the background of the game cell that will represent a button.
        /// </summary>
        /// <value>The background of the game cell.</value>
        public Brush CellBackground
        {
            get
            {
                return this.cellBackground;
            }

            set 
            { 
                this.cellBackground = value;
                this.FireOnPropertyChanged();
            }
        }
    }
}
