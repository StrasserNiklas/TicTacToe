using Client.Models;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class LoginVM : BaseVM
    {
        public LoginVM()
        {
            this.LoginCommand = new Command(async obj => await this.ComputeLoginCommand());
            this.SignupCommand = new Command(async obj => await this.ComputeSignupCommand());

        }


        public string LoginUsername { get; set; }


        /// <summary>
        /// YES, never do this, but in this case encryption is not really an issue
        /// </summary>
        public string LoginPassword { get; set; }

        public string SignupUsername { get; set; }


        /// <summary>
        /// YES, never do this, but in this case encryption is not really an issue
        /// </summary>
        public string SignupPassword { get; set; }

        public ICommand SignupCommand { get; }

        public ICommand LoginCommand { get; }

        /// <summary>
        /// This command is used when the player types in his username and connects to the server.
        /// A request will be sent to the server containing the client player name.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        private async Task ComputeLoginCommand()
        {
            var placeholder = true;
            //this.logger.LogInformation("[ComputeConnectCommand]");

            if (!string.IsNullOrEmpty(this.LoginUsername))
            {
            //    try
            //    {


            //        await this.hubConnection.SendAsync("AddPlayer", this.clientPlayer.PlayerName);

            //        this.ClientConnected = true;
            //    }
            //    catch (HttpRequestException)
            //    {
            //        this.StatusMessage = "Unable to connect to server.";
            //    }
            //    catch (Exception)
            //    {
            //        this.statusMessage = "An unknown error occured. Please try again later.";
            //    }
            }
        }

        /// <summary>
        /// This command is used when the player types in his username and connects to the server.
        /// A request will be sent to the server containing the client player name.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        private async Task ComputeSignupCommand()
        {
            var placeholder = true;
            //this.logger.LogInformation("[ComputeConnectCommand]");

            if (!string.IsNullOrEmpty(this.SignupUsername))
            {
            //    try
            //    {


            //        await this.hubConnection.SendAsync("AddPlayer", this.clientPlayer.PlayerName);

            //        this.ClientConnected = true;
            //    }
            //    catch (HttpRequestException)
            //    {
            //        this.StatusMessage = "Unable to connect to server.";
            //    }
            //    catch (Exception)
            //    {
            //        this.statusMessage = "An unknown error occured. Please try again later.";
            //    }
            }
        }

        private bool showLoginScreen = false;

        public bool ShowLoginScreen
        {
            get { return showLoginScreen; }
            set { showLoginScreen = value; this.FireOnPropertyChanged(); }
        }


    }
}
