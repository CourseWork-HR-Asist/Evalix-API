using System.Reflection;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Application.Common.Behaviours;
using Application.Services.Interfaces;
using Application.Services.LLM;
using Application.Services.LLMSettings;
using Application.Services.S3;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureApplication
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddSettings(configuration);
        services.AddServices(configuration);
        services.AddAws(configuration);
    }

    private static void AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var llmSetting = configuration.GetSection("LLM").Get<LLMSetting>()!;
        services.AddSingleton(llmSetting);
    }

    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILLMService, LLMService>();
        services.AddHttpClient<ILLMService, LLMService>((provider, client) =>
        {
            var llmSettings = provider.GetRequiredService<LLMSetting>();
            client.BaseAddress = new Uri(llmSettings.Url);
            client.Timeout = TimeSpan.FromMinutes(5);
        });

        services.AddSingleton<ILLMSettingsService, LLMSettingsService>();
    }

    private static void AddAws(this IServiceCollection services, IConfiguration configuration)
    {
        var awsOptions = configuration.GetSection("AWS");
        var credentials = new BasicAWSCredentials(
            awsOptions["AccessKey"],
            awsOptions["SecretKey"]
        );

        services.AddDefaultAWSOptions(new AWSOptions
        {
            Region = RegionEndpoint.GetBySystemName(awsOptions["Region"]),
            Credentials = credentials
        });

        services.AddAWSService<IAmazonS3>();
        services.AddScoped<IS3FileService, S3FileService>();
    }
}
