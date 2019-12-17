// Niklas Strasser, Felix Brandstetter, Yannick Gruber

namespace Client.ViewModels
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Represents a single cell of the tic tac toe game with an index and a background.
    /// 
    /// [O X O]               [0 1 2]
    /// [X O O] -> indexed -> [3 4 5]
    /// [O X O]               [6 7 8]    
    /// </summary>
    public class GameCellVM : BaseVM
    {
        private int playerMark;

        private Brush cellBackground;

        public GameCellVM(int index, int playerMark)
        {
            this.Index = index;
            this.PlayerMark = playerMark;
        }

        /// <summary>
        /// Gets the game index the cell is representing.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets or sets the player mark the cell is representing.
        /// Based on the mark, the background of the cell will be set to a different background.
        /// </summary>
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
