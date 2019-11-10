using Client.ViewModels;

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
        }

        public GameVM CurrentGame { get; set; }
    }


}
