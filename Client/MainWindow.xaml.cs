//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file contains the interaction logic for MainWindow.</summary>
//-----------------------------------------------------------------------

namespace Client
{
    using System;
    using System.Windows;
    using GameLibrary;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This field is used to save the client instance.
        /// </summary>
        private readonly ClientVM client;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// Executes dependency injection and sets the data context.
        /// </summary>
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

            this.client = host.Services.GetService<ClientVM>();

            this.DataContext = this.client;
            this.client.ClientPlayer = new PlayerVM(new Player("player"));
        }
    }
}
