using Microsoft.Extensions.Options;
using WPFAppWithDependencyInjection.Services;
using WPFAppWithDependencyInjection.Types;

namespace WPFAppWithDependencyInjection
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(IOptions<ApplicationSettings> appSettings, IIntService intService)
        {
            int value = intService.GetValue();
            if (value == 0)
            {
                var defaultValue = appSettings.Value.DefaultIntValue;
                intService.SetValue(defaultValue);
            }

            IntValue = intService.GetValue();
        }

        public int IntValue { get; private set; }
    }
}
