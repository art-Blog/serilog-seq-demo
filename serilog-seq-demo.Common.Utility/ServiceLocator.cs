using Microsoft.Extensions.DependencyInjection;

namespace serilog_seq_demo.Common.Utility;

public class ServiceLocator
{
    private readonly ServiceProvider? _currentServiceProvider;
    private static ServiceProvider? _serviceProvider;

    private ServiceLocator(ServiceProvider? currentServiceProvider)
    {
        _currentServiceProvider = currentServiceProvider;
    }

    public static ServiceLocator Current => new ServiceLocator(_serviceProvider);

    public static void SetLocatorProvider(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TService? GetInstance<TService>()
    {
        return _currentServiceProvider == null ? default : _currentServiceProvider.GetService<TService>();
    }
}