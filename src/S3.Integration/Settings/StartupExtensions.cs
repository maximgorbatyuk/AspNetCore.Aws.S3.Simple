using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using S3.Integration.AmazonServices;
using S3.Integration.Contracts;

namespace S3.Integration.Settings;

public static class StartupExtensions
{
    public static IServiceCollection AddS3Settings(
        this IServiceCollection services)
    {
        services.AddScoped<S3StorageSettings>();
        services.AddScoped<IS3FileValidator, S3FileValidator>();
        return services;
    }

    public static IServiceCollection AddS3Storage<TService, TImplementation>(
        this IServiceCollection services)
        where TService : class, IFileStorageBase
        where TImplementation : AmazonS3StorageBase, TService
    {
        return services.AddScoped<TService, TImplementation>();
    }

    public static IHealthChecksBuilder AddS3HealthChecks(this IHealthChecksBuilder builder, IConfiguration configuration)
    {
        var settings = new S3StorageSettings(configuration);
        return builder
            .AddS3(options =>
            {
                settings.SetupS3HealthCheck(options);
            });
    }
}