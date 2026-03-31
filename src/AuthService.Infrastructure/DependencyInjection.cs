using Amazon;
using Amazon.CognitoIdentityProvider;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CognitoSettings>(configuration.GetSection("Cognito"));

        var region = configuration["Cognito:Region"] ?? "us-east-1";

        services.AddSingleton<IAmazonCognitoIdentityProvider>(_ =>
            new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(region)));

        services.AddScoped<IAuthRepository, CognitoAuthRepository>();

        return services;
    }
}