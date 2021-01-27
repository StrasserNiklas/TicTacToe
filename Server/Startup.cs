//-----------------------------------------------------------------------
// <copyright file="Startup.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Felix Brandstetter, Niklas Strasser, Yannick Gruber</author>
// <summary>This file represents the startup class.</summary>
//-----------------------------------------------------------------------

namespace Server
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Server.Hubs;
    using Server.Services;

    /// <summary>
    /// This class is responsible for the startup of the server. Configures services and endpoints.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Adds the required services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddControllers();
            services.AddSingleton<IMainService, MainService>();
            services.AddLogging(logging =>
            {
                logging.AddSerilog(new LoggerConfiguration().WriteTo.Debug(Serilog.Events.LogEventLevel.Debug).WriteTo.Console().WriteTo.File(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\")) + "log.txt").CreateLogger());
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                {
                    Title = "TicTacToe Service API",
                    Version = "v1",
                    Description = "TicTacToe service",
                    Contact = new OpenApiContact { Name = "Niklas Strasser, Felix Brandstetter, Yannick Gruber" },
                    License = new OpenApiLicense { Name = "MIT License" }
                });
            });
        }

        /// <summary>
        /// C// This method gets called by the runtime. This method configures the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The web host environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TicTacToe Services"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GameHub>("game");
            });
        }
    }
}
