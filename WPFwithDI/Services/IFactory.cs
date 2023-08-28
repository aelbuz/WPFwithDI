namespace WPFAppWithDependencyInjection.Services
{
    public interface IFactory<T>
        where T : class
    {
        T New();
    }
}
