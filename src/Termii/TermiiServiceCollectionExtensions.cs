using Microsoft.Extensions.DependencyInjection;

namespace Termii;

public static class TermiiServiceCollectionExtensions
{
    public static IHttpClientBuilder AddTermii(
        this IServiceCollection services,
        Action<TermiiOptions> configureOptions)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configureOptions is null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        var options = new TermiiOptions();
        configureOptions(options);
        options.Validate();

        services.AddSingleton(options);

        return services.AddHttpClient<TermiiClient>((serviceProvider, httpClient) =>
        {
            var termiiOptions = serviceProvider.GetRequiredService<TermiiOptions>();
            httpClient.BaseAddress = termiiOptions.BaseUrl;
            httpClient.Timeout = termiiOptions.Timeout;
        });
    }
}
