using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using WPFAppWithDependencyInjection.Services;
using WPFAppWithDependencyInjection.Types;

namespace WPFAppWithDependencyInjection
{
    public class MainWindowViewModel
    {
        private readonly IOptions<ApplicationSettings> appSettings;
        private readonly ILogger<MainWindowViewModel> logger;
        private readonly IIntService intService;

        public MainWindowViewModel(IOptions<ApplicationSettings> appSettings,
                                   ILogger<MainWindowViewModel> logger,
                                   IIntService intService)
        {
            this.appSettings = appSettings;
            this.logger = logger;
            this.intService = intService;

            int value;
            while (!TryGetValue(out value))
            {
                TrySetValue();
            }

            IntValue = value;
        }

        public int IntValue { get; private set; }

        private bool TryGetValue(out int value)
        {
            try
            {
                value = intService.GetValue();

                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error occurred while getting default value from service.");
                value = 0;

                return false;
            }
        }

        private void TrySetValue()
        {
            int defaultValue = appSettings.Value.DefaultIntValue;
            intService.SetValue(defaultValue);

            int value = intService.GetValue();
            if (value != 0)
            {
                logger.LogDebug("Successfully set value to {Value}.", value);
            }
        }
    }
}
