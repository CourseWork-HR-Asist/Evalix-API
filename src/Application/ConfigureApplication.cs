using System.Reflection;
using Application.Common.Behaviours;
using Application.Services.Interfaces;
using Application.Services.LLM;
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
        services.AddServices();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ILLMService, LLMService>();
        services.AddHttpClient<ILLMService, LLMService>((servicePorovider, client) =>
        {
            var llmSettings = servicePorovider.GetRequiredService<LLMSetting>();
            client.BaseAddress = new Uri(llmSettings.Url);
        });
    }

    private static void AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var llmSetting = configuration.GetSection("LLM")
            .Get<LLMSetting>()!;

        services.AddSingleton(llmSetting);
    }
}