using Microsoft.Extensions.DependencyInjection;
using System;

namespace WPFAppWithDependencyInjection.Services
{
    public class Factory<T> : IFactory<T>
        where T : class
    {
        private readonly static ObjectFactory objectFactory;

        private readonly IServiceProvider services;

        static Factory()
        {
            objectFactory = ActivatorUtilities.CreateFactory(typeof(T), Array.Empty<Type>());
        }

        public Factory(IServiceProvider services)
        {
            this.services = services;
        }

        public T New()
        {
            return (T)objectFactory(services, Array.Empty<object>());
        }
    }
}
