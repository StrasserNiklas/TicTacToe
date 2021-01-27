using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private LoginVM loginManagement;

        public StartWindow()
        {
            //this.DataContext = this;
            InitializeComponent();

            this.loginManagement = new LoginVM();
            this.DataContext = this.loginManagement;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void PlayBots_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void PlayHumans_Click(object sender, RoutedEventArgs e)
        {
            this.loginManagement.ShowLoginScreen = !this.loginManagement.ShowLoginScreen;
        }

        /// <summary>
        /// Fires the property changed event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void FireOnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
