using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Client.Models;
using System.Net.Http;
using System.Collections.ObjectModel;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClientVM ticGame;

        public MainWindow()
        {
            InitializeComponent();

            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHttpClient("TicTacToeGame", client =>
                    {
                        client.BaseAddress = new Uri("https://localhost:5001");
                    }).AddTypedClient<GameClientService>();

                }).Build();

            this.ticGame = new ClientVM(new GameVM(new PlayerVM("Nikolaus", 1), new PlayerVM("Felixitus", 2)));
            this.DataContext = this.ticGame;

            this.gameService = host.Services.GetService<GameClientService>();

        }

        private GameClientService gameService;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.BooksDemoAsync();
            
        }

        private async Task BooksDemoAsync()
        {
            try
            {
                var list = await this.gameService.GetPlayerListAsync();


                // hier würd daweil eine exception kommen
                //var playerList = await this.gameService.PostPlayerInfoToServerAsync("Nikolaus");

                

                this.ticGame.PlayerList = new ObservableCollection<Player>(list);//.Result);
            }
            catch (HttpRequestException ex)
            {
            }

        }



        List<Button> buttons = new List<Button>();

        /// <summary>
        /// This method changes the background of the game element buttons if neccessary.
        /// </summary>
        /// <param name="sender">The button representing a void space, a "X" or a "O".</param>
        /// <param name="e">The event args.</param>
        private void GameElementClick(object sender, RoutedEventArgs e)
        {
            // only allowed when the game is running
            if (!this.ticGame.CurrentGame.GameOver)
            {
                var button = (Button)sender;

                // add the button to the list so it can be reset later
                if (!this.buttons.Contains(button))
                {
                    this.buttons.Add(button);
                }

                // get the game index the button is representating
                var commandParameter = int.Parse((string)button.CommandParameter);

                if (this.ticGame.CurrentGame.CurrentPlayer == this.ticGame.CurrentGame.PlayerOne && this.ticGame.CurrentGame.CurrentGameStatus[commandParameter] == 0)
                {
                    var brush = new ImageBrush();
                    brush.ImageSource = new BitmapImage(new Uri("Images/O.png", UriKind.Relative));
                    button.Background = brush;
                    return;
                }

                if (this.ticGame.CurrentGame.CurrentPlayer == this.ticGame.CurrentGame.PlayerTwo && this.ticGame.CurrentGame.CurrentGameStatus[commandParameter] == 0)
                {
                    var brush = new ImageBrush();
                    brush.ImageSource = new BitmapImage(new Uri("Images/X.png", UriKind.Relative));
                    button.Background = brush;
                    return;
                }
            }
        }

        /// <summary>
        /// When the "New game" button is pressed, this method resets the background of the buttons representing the game elements.
        /// </summary>
        /// <param name="sender">The "New game" button.</param>
        /// <param name="e">The event args.</param>
        private void NewGameButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var button in this.buttons)
            {
                button.Background = new SolidColorBrush(Colors.White);
            }
        }
    }
}
