using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;


namespace Infrastructure.Persistence;

public static class ConfigurePersistence
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuild = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Default"));
        dataSourceBuild.EnableDynamicJson();
        var dataSource = dataSourceBuild.Build();

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseNpgsql(
                    dataSource,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddRepositories();
        services.AddServices();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<IUserRepository>(provider => provider.GetRequiredService<UserRepository>());
        services.AddScoped<IUserQueries>(provider => provider.GetRequiredService<UserRepository>());

        services.AddScoped<RoleRepository>();
        services.AddScoped<IRoleRepository>(provider => provider.GetRequiredService<RoleRepository>());
        services.AddScoped<IRoleQueries>(provider => provider.GetRequiredService<RoleRepository>());

        services.AddScoped<SkillRepository>();
        services.AddScoped<ISkillRepository>(provider => provider.GetRequiredService<SkillRepository>());
        services.AddScoped<ISkillQueries>(provider => provider.GetRequiredService<SkillRepository>());

        services.AddScoped<StatusRepository>();
        services.AddScoped<IStatusRepository>(provider => provider.GetRequiredService<StatusRepository>());
        services.AddScoped<IStatusQueries>(provider => provider.GetRequiredService<StatusRepository>());

        services.AddScoped<VacancyRepository>();
        services.AddScoped<IVacancyRepository>(provider => provider.GetRequiredService<VacancyRepository>());
        services.AddScoped<IVacancyQueries>(provider => provider.GetRequiredService<VacancyRepository>());

        services.AddScoped<VacancySkillRepository>();
        services.AddScoped<IVacancySkillRepository>(provider => provider.GetRequiredService<VacancySkillRepository>());
        services.AddScoped<IVacancySkillQueries>(provider => provider.GetRequiredService<VacancySkillRepository>());

        services.AddScoped<ResumeRepository>();
        services.AddScoped<IResumeRepository>(provider => provider.GetRequiredService<ResumeRepository>());
        services.AddScoped<IResumeQueries>(provider => provider.GetRequiredService<ResumeRepository>());

        services.AddScoped<EvaluationRepository>();
        services.AddScoped<IEvaluationRepository>(provider => provider.GetRequiredService<EvaluationRepository>());
        services.AddScoped<IEvaluationQueries>(provider => provider.GetRequiredService<EvaluationRepository>());
        
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
    
    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
}