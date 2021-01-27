using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ViewModels
{
    public class ErrorHandlerVM : BaseVM
    {
        private string loginPasswordErrorMessage;

        private string signupPasswordErrorMessage;

        private bool signupPasswordError;

        private bool loginPasswordError;

        private bool loginUsernameError;

        private bool signupUsernameError;

        private string signupUsernameErrorMessage;

        private string loginUsernameErrorMessage;

        public string LoginUsernameErrorMessage
        {
            get
            {
                return this.loginUsernameErrorMessage;
            }

            set
            {
                this.loginUsernameErrorMessage = value;
                this.FireOnPropertyChanged();
            }
        }
        public string SignupUsernameErrorMessage
        {
            get
            {
                return this.signupUsernameErrorMessage;
            }

            set
            {
                this.signupUsernameErrorMessage = value;
                this.FireOnPropertyChanged();
            }
        }

        public bool SignupUsernameError
        {
            get
            {
                return this.signupUsernameError;
            }

            set
            {
                this.signupUsernameError = value;
                this.FireOnPropertyChanged();
            }
        }

        public bool LoginUsernameError
        {
            get
            {
                return this.loginUsernameError;
            }

            set
            {
                this.loginUsernameError = value;
                this.FireOnPropertyChanged();
            }
        }

        public bool LoginPasswordError
        {
            get
            {
                return this.loginPasswordError;
            }

            set
            {
                this.loginPasswordError = value;
                this.FireOnPropertyChanged();
            }
        }

        public bool SignupPasswordError
        {
            get
            {
                return this.signupPasswordError;
            }

            set
            {
                this.signupPasswordError = value;
                this.FireOnPropertyChanged();
            }
        }

        public string SignupPasswordErrorMessage
        {
            get
            {
                return this.signupPasswordErrorMessage;
            }

            set
            {
                this.signupPasswordErrorMessage = value;
                this.FireOnPropertyChanged();
            }
        }

        public string LoginPasswordErrorMessage
        {
            get 
            { 
                return this.loginPasswordErrorMessage; 
            }

            set 
            { 
                this.loginPasswordErrorMessage = value;
                this.FireOnPropertyChanged();
            }
        }

    }
}
