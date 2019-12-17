// Niklas Strasser, Felix Brandstetter, Yannick Gruber

namespace Server
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Server.Hubs;
    using Server.Services;
    using System;
    using System.IO;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TicTacToe Service API",
                    Version = "v1",
                    Description = "TicTacToe service",
                    Contact = new OpenApiContact { Name = "Niklas Strasser, Felix Brandstetter, Yannick Gruber" },
                    License = new OpenApiLicense { Name = "MIT License" }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
