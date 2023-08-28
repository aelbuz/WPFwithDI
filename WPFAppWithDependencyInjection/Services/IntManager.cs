using System;

namespace WPFAppWithDependencyInjection.Services
{
    public class IntManager : IIntService
    {
        private int value;

        public int GetValue()
        {
            if (value == 0)
            {
                throw new ArgumentException("Current value is invalid.");
            }

            return value;
        }

        public void SetValue(int value)
        {
            this.value = value;
        }
    }
}
