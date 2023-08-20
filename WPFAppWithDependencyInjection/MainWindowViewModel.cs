using WPFAppWithDependencyInjection.Services;

namespace WPFAppWithDependencyInjection
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(IIntService intService)
        {
            int value = intService.GetValue();
            if (value == 0)
            {
                intService.SetValue(177013);
            }

            int newValue = intService.GetValue();
        }
    }
}
