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
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Server.Authentication;
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

        private string accessTokenTemp = "";

        /// <summary>
        /// This method gets called by the runtime. Adds the required services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(options =>
            //{
            //    // Identity made Cookie authentication the default.
            //    // However, we want JWT Bearer Auth to be the default.
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    // Configure the Authority to the expected value for your authentication provider
            //    // This ensures the token is appropriately validated

            //    // DYNAMISCH MACHEN OHNE HARD CODEN
            //    //options.Authority = "https://localhost:5001/api";
            //    options.Authority = "https://ticnic.azurewebsites.net/game";

            //    // We have to hook the OnMessageReceived event in order to
            //    // allow the JWT authentication handler to read the access
            //    // token from the query string when a WebSocket or 
            //    // Server-Sent Events request comes in.

            //    // Sending the access token in the query string is required due to
            //    // a limitation in Browser APIs. We restrict it to only calls to the
            //    // SignalR hub in this code.
            //    // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
            //    // for more information about security considerations when using
            //    // the query string to transmit the access token.
            //    options.Events = new JwtBearerEvents
            //    {
            //        OnMessageReceived = context =>
            //        {
            //            var accessToken = context.Request.Query["access_token"];

            //            if (accessToken.ToString() == string.Empty)
            //            {
            //                accessToken = this.accessTokenTemp;
            //            }
            //            else
            //            {
            //                this.accessTokenTemp = accessToken;
            //            }

            //            // If the request is for our hub...
            //            var path = context.HttpContext.Request.Path;
            //            if (!string.IsNullOrEmpty(accessToken))
            //            {
            //                // Read the token out of the query string
            //                context.Token = accessToken;
            //            }
            //            return Task.CompletedTask;
            //        }
            //    };
            //});

            services.AddSignalR();
            services.AddControllers();
            services.AddSingleton<IMainService, MainService>();
            var key = "pintusharmaqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqweqwe";
            services.AddSingleton<IJwtAuthenticationManager>(new JwtAuthenticationManager(key));
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

            app.UseAuthentication();
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
