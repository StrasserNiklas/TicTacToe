using Client.Models;
using Client.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public ClientVM(GameVM game)
        {
            this.CurrentGame = game;
            //this.LocalGameSelected = true;
            //this.OnlineGameSelected = false;
            this.PlayerList = new ObservableCollection<PlayerWildcard>();
        }

        private ObservableCollection<PlayerWildcard> playerList;

        public ObservableCollection<PlayerWildcard> PlayerList
        {
            get { return playerList; }
            set 
            { 
                playerList = value;
            }
        }


        public GameVM CurrentGame { get; set; }
    }


}
