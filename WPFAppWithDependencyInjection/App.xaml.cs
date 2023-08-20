using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFAppWithDependencyInjection.Services;

namespace WPFAppWithDependencyInjection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;

        private MainWindowViewModel? mainWindowVm;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
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

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient(typeof(IFactory<>), typeof(Factory<>));
            services.AddSingleton<IIntService, IntManager>();
        }
    }
}
