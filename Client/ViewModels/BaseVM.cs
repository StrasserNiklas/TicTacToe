//-----------------------------------------------------------------------
// <copyright file="BaseVM.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents a base view model.</summary>
//-----------------------------------------------------------------------

namespace Client.ViewModels
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents a base view model.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class BaseVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
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
