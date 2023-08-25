using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Windows;
using WPFAppWithDependencyInjection.Services;
using WPFAppWithDependencyInjection.Types;

namespace WPFAppWithDependencyInjection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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

            // Add options service.
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            services.AddSingleton<IConfiguration>(configuration);
            var applicationSettings = new ApplicationSettings();
            configuration.Bind(nameof(ApplicationSettings), applicationSettings);
            services.AddSingleton(Options.Create(applicationSettings));

            return services.BuildServiceProvider();
        }
    }
}
