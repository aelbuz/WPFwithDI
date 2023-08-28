using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using WPFAppWithDependencyInjection.Services;
using WPFAppWithDependencyInjection.Types;

namespace WPFAppWithDependencyInjection
{
    public partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;

        private MainWindowViewModel? mainWindowVm;

        public App()
        {
            var services = new ServiceCollection();
            serviceProvider = ConfigureServices(services);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindowVmFactory = serviceProvider.GetRequiredService<IFactory<MainWindowViewModel>>();
            mainWindowVm = mainWindowVmFactory.New();

            var mainWindow = new MainWindow
            {
                DataContext = mainWindowVm,
            };
            mainWindow.Show();

            base.OnStartup(e);
        }

        private static IServiceProvider ConfigureServices(ServiceCollection services)
        {
            // Add view-model factory service.
            services.AddTransient(typeof(IFactory<>), typeof(Factory<>));

            services.AddSingleton<IIntService, IntManager>();

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                                                          .AddJsonFile("NLog.json")
                                                          .Build();
            services.AddSingleton<IConfiguration>(configuration);

            // Configure and add logging service.
            services.AddLogging(logBuilder =>
            {
                logBuilder.AddConfiguration(configuration.GetSection("Logging")).AddNLog(new NLogProviderOptions
                {
                    RemoveLoggerFactoryFilter = false,
                });
            });

            // Set log directory to %appdata% and use it in NLog.json.
            string baseLogDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appName = Assembly.GetExecutingAssembly().GetName().Name!;
            baseLogDir = Path.Combine(baseLogDir, appName);
            configuration["baseLogDir"] = baseLogDir;

            // Configure and add options service.
            var applicationSettings = new ApplicationSettings();
            configuration.Bind(nameof(ApplicationSettings), applicationSettings);
            services.AddSingleton(Options.Create(applicationSettings));

            return services.BuildServiceProvider();
        }
    }
}
