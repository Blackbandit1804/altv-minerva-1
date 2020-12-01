﻿using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.Entities;
using PlayGermany.Server.ServerJobs.Base;
using PlayGermany.Server.Extensions;
using System.IO;
using PlayGermany.Server.Handlers;
using PlayGermany.Server.ScheduledJobs.Base;

namespace PlayGermany.Server
{
    public class Server
        : AsyncResource
    {
        private readonly ServiceProvider _serviceProvider;

        public IConfiguration Configuration { get; }

        public ILogger<Server> Logger { get; }

        public Server()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.local.json", true, true)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton(Configuration);

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            Logger = _serviceProvider.GetService<ILogger<Server>>();
        }

        public override void OnStart()
        {
            _serviceProvider.InstanciateRegisteredServices();

            var serverJobs = _serviceProvider.GetServices<IServerJob>();
            foreach (var job in serverJobs)
            {
                job.OnStartup();
            }

            var scheduleJobManager = _serviceProvider.GetService<ScheduleJobManager>();
            if (scheduleJobManager != null)
            {
                scheduleJobManager.EnableWorker();
            }
        }

        public override void OnTick()
        {
            base.OnTick();
        }

        public override void OnStop()
        {
            var serverJobs = _serviceProvider.GetServices<IServerJob>();
            foreach (var job in serverJobs)
            {
                job.OnShutdown();
            }
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.AddConfiguration(Configuration.GetSection("Logging"));
                config.AddDebug();
                config.AddConsole();
            });

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("Database"), MariaDbServerVersion.LatestSupportedServerVersion);
            });

            // register all server jobs
            services.AddAllTypes<IServerJob>();

            // register scheduled jobs
            services.AddSingletonAndInstanciate<ScheduleJobManager>();
            services.AddAllTypes<BaseScheduledJob>();

            // register handlers
            services.AddSingletonAndInstanciate<SessionHandler>();
            services.AddSingletonAndInstanciate<VehicleHandler>();
            services.AddSingletonAndInstanciate<ClientConsoleHandler>();
            services.AddSingletonAndInstanciate<VoiceHandler>();
        }

        #region Entities

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new ServerPlayerFactory();
        }

        public override IEntityFactory<IVehicle> GetVehicleFactory()
        {
            return new ServerVehicleFactory();
        }

        #endregion
    }
}
