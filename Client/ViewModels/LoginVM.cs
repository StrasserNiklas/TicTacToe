﻿using Client.Models;
using Client.Services;
using GameLibrary;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class LoginVM : BaseVM
    {
        public LoginVM()
        {
            this.ErrorHandler = new ErrorHandlerVM();
            this.LoginCommand = new Command(async obj => await this.ComputeLoginCommand());
            this.SignupCommand = new Command(async obj => await this.ComputeSignupCommand());
        }

        public event EventHandler<AuthenticationEventArgs> OnSuccessfulAuthentication;

        private RestService restService = new RestService();

        private bool isLoginButtonEnabled = true;

        private string loginButtonContent = "Login";

        private bool isSignUpButtonEnabled = true;

        private string signUpButtonContent = "Sign Up";

        public ErrorHandlerVM ErrorHandler { get; set; }

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

        public bool IsLoginButtonEnabled
        {
            get { return this.isLoginButtonEnabled; }
            set
            {
                this.isLoginButtonEnabled = value;
                this.FireOnPropertyChanged();
            }
        }

        public string LoginButtonContent
        {
            get { return this.loginButtonContent; }
            set
            {
                this.loginButtonContent = value;
                this.FireOnPropertyChanged();
            }
        }

        public bool IsSignupButtonEnabled
        {
            get { return this.isSignUpButtonEnabled; }
            set
            {
                this.isSignUpButtonEnabled = value;
                this.FireOnPropertyChanged();
            }
        }

        public string SignUpButtonContent
        {
            get { return this.signUpButtonContent; }
            set
            {
                this.signUpButtonContent = value;
                this.FireOnPropertyChanged();
            }
        }

        /// <summary>
        /// This command is used when the player types in his username and connects to the server.
        /// A request will be sent to the server containing the client player name.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        private async Task ComputeLoginCommand()
        {
            //this.logger.LogInformation("[ComputeConnectCommand]");
            try
            {
                if (!string.IsNullOrEmpty(this.LoginUsername))
                {
                    this.ErrorHandler.LoginUsernameErrorMessage = "";

                    if (!string.IsNullOrEmpty(this.LoginPassword))
                    {
                        this.ErrorHandler.LoginPasswordErrorMessage = "";

                        this.IsLoginButtonEnabled = false;
                        this.LoginButtonContent = "Logging in...";

                        var hash = this.ComputeSha256Hash(this.LoginPassword);

                        var response = await this.restService.Login(this.LoginUsername, hash);

                        if (!response.WasSuccessful)
                        {
                            this.ErrorHandler.LoginPasswordErrorMessage = "Incorrect credentials.";
                            this.IsLoginButtonEnabled = true;
                            this.LoginButtonContent = "Login";
                        }
                        else
                        {
                            this.FireOnSuccessfulAuthentication(response.UserId, this.LoginUsername, response.JwToken);
                        }
                    }
                    else
                    {
                        this.ErrorHandler.LoginPasswordErrorMessage = "Empty password";
                        //this.FireOnPropertyChanged(this.ErrorHandler.LoginUsernameErrorMessage);
                    }
                }
                else
                {
                    this.ErrorHandler.LoginUsernameErrorMessage = "Enter a username";
                    //this.FireOnPropertyChanged(this.ErrorHandler.LoginUsernameErrorMessage);
                }
            }
            catch
            {
                this.ErrorHandler.LoginPasswordErrorMessage = "No connection";
                this.IsLoginButtonEnabled = true;
                this.LoginButtonContent = "Login";
            }
        }

        /// <summary>
        /// This command is used when the player types in his username and connects to the server.
        /// A request will be sent to the server containing the client player name.
        /// </summary>
        /// <returns>A Task that represents the asynchronous method.</returns>
        private async Task ComputeSignupCommand()
        {
            //this.logger.LogInformation("[ComputeConnectCommand]");
            try
            {
                if (!string.IsNullOrEmpty(this.SignupUsername))
                {
                    this.ErrorHandler.SignupUsernameErrorMessage = "";

                    if (!string.IsNullOrEmpty(this.SignupPassword))
                    {
                        this.ErrorHandler.SignupPasswordErrorMessage = "";

                        this.IsSignupButtonEnabled = false;
                        this.SignUpButtonContent = "Signing up ...";

                        var hash = this.ComputeSha256Hash(this.SignupPassword);
                        var response = await this.restService.AddUser(this.SignupUsername, hash);

                        if (!response.WasSuccessful)
                        {
                            this.ErrorHandler.SignupPasswordErrorMessage = response.ErrorMessage;
                            this.IsSignupButtonEnabled = true;
                            this.SignUpButtonContent = "Sign Up";
                        }
                        else
                        {
                            this.FireOnSuccessfulAuthentication(response.UserId, this.SignupUsername, response.JwToken);
                        }
                    }
                    else
                    {
                        this.ErrorHandler.SignupPasswordErrorMessage = "Empty password";
                        //this.FireOnPropertyChanged(this.ErrorHandler.LoginUsernameErrorMessage);
                    }
                }
                else
                {
                    this.ErrorHandler.SignupUsernameErrorMessage = "Enter a username";
                    //this.FireOnPropertyChanged(this.ErrorHandler.LoginUsernameErrorMessage);
                }
            }
            catch
            {
                this.ErrorHandler.SignupPasswordErrorMessage = "No connection";
                this.IsSignupButtonEnabled = true;
                this.SignUpButtonContent = "Sign Up";
            }
        }

        private bool showLoginScreen = false;

        public bool ShowLoginScreen
        {
            get { return showLoginScreen; }
            set { showLoginScreen = value; this.FireOnPropertyChanged(); }
        }


        protected virtual void FireOnSuccessfulAuthentication(int id, string playerName, string token = "")
        {
            this.OnSuccessfulAuthentication?.Invoke(this, new AuthenticationEventArgs(id, playerName, token));
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
