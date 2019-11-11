using Client.Models;
using Client.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client
{
    public class ClientVM : BaseVM
    {
        #region LocalGame - OnlineGame disabled logic
        //private bool onlineGameSelected;
        //private bool localGameSelected;

        //public ICommand LocalGameButtonPressed
        //{
        //    get
        //    {
        //        return new Command(obj =>
        //        {
        //            this.LocalGameSelected = true;
        //            this.OnlineGameSelected = false;
        //        });
        //    }
        //}

        //public ICommand OnlineGameButtonPressed
        //{
        //    get
        //    {
        //        return new Command(obj =>
        //        {
        //            this.LocalGameSelected = false;
        //            this.OnlineGameSelected = true;
        //        });
        //    }
        //}

        //public bool LocalGameSelected
        //{
        //    get
        //    {
        //        return localGameSelected;
        //    }
        //    set
        //    {
        //        localGameSelected = value;
        //        this.FireOnPropertyChanged(nameof(this.LocalGameSelected));
        //    }
        //}

        //public bool OnlineGameSelected
        //{
        //    get
        //    {
        //        return onlineGameSelected;
        //    }
        //    set
        //    {
        //        onlineGameSelected = value;
        //        this.FireOnPropertyChanged(nameof(this.OnlineGameSelected));
        //    }
        //}

        #endregion

        public ClientVM(GameVM game, GameClientService gameClientService)
        {
            this.CurrentGame = game;
            //this.LocalGameSelected = true;
            //this.OnlineGameSelected = false;
            this.PlayerList = new ObservableCollection<Player>();
            this.gameClientService = gameClientService;
        }

        private ObservableCollection<Player> playerList;
        private Player selectedPlayer;


        private GameClientService gameClientService;

        public ICommand RequestGameCommand
        {
            get
            {
                return new Command(obj =>
                {

                });
            }
        }




        public ObservableCollection<Player> PlayerList
        {
            get { return playerList; }
            set 
            { 
                playerList = value;
                this.FireOnPropertyChanged();
            }
        }

        public Player SelectedPlayer
        {
            get { return selectedPlayer; }
            set { selectedPlayer = value; }
        }



        public GameVM CurrentGame { get; set; }
    }
}
