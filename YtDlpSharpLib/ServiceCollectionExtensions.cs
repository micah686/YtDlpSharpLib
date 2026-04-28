using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using YtDlpSharpLib.Process;
using YtDlpSharpLib.Rendering;
using YtDlpSharpLib.Scheduling;

namespace YtDlpSharpLib;

/// <summary>
/// DI registration extensions for YtDlpSharp.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IYtDlpClient"/> and its supporting services in the container.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="configure">Optional configuration callback for <see cref="YtDlpClientOptions"/>.</param>
    public static IServiceCollection AddYtDlpClient(
        this IServiceCollection services,
        Action<YtDlpClientOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (configure is not null)
        {
            services.Configure(configure);
        }
        else
        {
            services.AddOptions<YtDlpClientOptions>();
        }

        services.TryAddSingleton(TimeProvider.System);
        services.TryAddSingleton<IYtDlpArgumentRenderer, YtDlpArgumentRenderer>();
        services.TryAddSingleton<IYtDlpProcessFactory, YtDlpProcessFactory>();
        services.TryAddSingleton<IYtDlpClient, YtDlpClient>();
        services.TryAddSingleton<IYtDlpExecutionScheduler, YtDlpExecutionScheduler>();

        return services;
    }
}
