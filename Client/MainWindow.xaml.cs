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
using GameLibrary;
using Serilog;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ClientVM client;

        public MainWindow()
        {
            InitializeComponent();

            IHost host = Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddSerilog(new LoggerConfiguration().WriteTo.Debug(Serilog.Events.LogEventLevel.Debug).WriteTo.Console().WriteTo.File(System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\")) + "log.txt").CreateLogger());
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ClientVM>();
                    services.AddSingleton<Services.UrlService>();
                }).Build();

            this.client = host.Services.GetService<ClientVM>();//new ClientVM(this.gameService, new Services.UrlService());
            
            this.DataContext = this.client;
            this.client.ClientPlayer = new PlayerVM(new Player("player"));
        }
    }
}
