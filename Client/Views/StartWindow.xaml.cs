using Client.Services;
using Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<LoginVM>();
                    //services.AddSingleton<IUrlService, UrlService>();
                }).Build();

            this.loginManagement = host.Services.GetService<LoginVM>();

            //this.loginManagement = new LoginVM();
            this.loginManagement.OnSuccessfulAuthentication += LoginManagement_OnSuccessfulAuthentication;
            this.DataContext = this.loginManagement;

        }

        private void LoginManagement_OnSuccessfulAuthentication(object sender, Models.AuthenticationEventArgs e)
        {
            var mainWindow = new MainWindow(e.Id, e.PlayerName, e.Token);
            mainWindow.Show();
            this.Close();
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

        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var button = (Button)sender;
        //    await this.Run(button);
        //}
    }
}
