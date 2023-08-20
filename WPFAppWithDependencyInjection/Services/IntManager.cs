namespace WPFAppWithDependencyInjection.Services
{
    public class IntManager : IIntService
    {
        private int value;

        public int GetValue()
        {
            return value;
        }

        public void SetValue(int value)
        {
            this.value = value;
        }
    }
}
