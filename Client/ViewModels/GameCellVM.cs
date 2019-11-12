using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client.ViewModels
{
    public class GameCellVM : BaseVM
    {
        private int playerMark;
        private Brush cellBackground;

        public GameCellVM(int index, int playerMark)
        {
            this.Index = index;
            this.PlayerMark = playerMark;
        }

        public int Index { get; }



        public int PlayerMark
        {
            get 
            { 
                return playerMark;
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
                        brush.ImageSource = new BitmapImage(new Uri("Images/O.png", UriKind.Relative));
                        this.CellBackground = new SolidColorBrush(Colors.Green);//brush;
                        break;

                    case 2:
                        var brush1 = new ImageBrush();
                        brush1.ImageSource = new BitmapImage(new Uri("Images/X.png", UriKind.Relative));
                        this.CellBackground = new SolidColorBrush(Colors.Green);//brush1;
                        break;

                    default:
                        break;
                }
            }
        }

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
