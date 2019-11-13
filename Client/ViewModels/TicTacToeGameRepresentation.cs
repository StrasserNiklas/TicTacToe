using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client.ViewModels
{
    public class TicTacToeGameRepresentation
    {
        private ObservableCollection<GameCellVM> gameCells;

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
