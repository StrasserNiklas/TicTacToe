// Niklas Strasser, Felix Brandstetter, Yannick Gruber

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Client.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the property changed event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void FireOnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
