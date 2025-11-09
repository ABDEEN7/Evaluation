using Evaluation.DAL.Context;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Models.JWT;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using System.Reflection;

namespace Evaluation.Services.Extensions;


public static class ServiceExtensions
{
#pragma warning disable  S4830
    public static void PopulateAppSettings(this IServiceCollection services, IConfiguration config)
    {
        AppSettings.CreateById = config.GetValue<Guid>("AppSettings:CreateById");
        AppSettings.DefaultLanguage = config.GetValue<string>("AppSettings:DefaultLanguage") ?? "en";
        AppSettings.DateFormat = config.GetValue<string>("AppSettings:DateFormat") ?? "yyyy-MM-dd";
    }
    public static void ConfigureMasterBL(this IServiceCollection services, IConfiguration config, bool isDevEnvironment)
    {

        services.AddDbContext<EvaluationDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("EvaluationDBConn"));
        });

        services.AddMemoryCache();

        services.AddHttpClient<HttpClientServices>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))    // Default is 2 mins
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    UseCookies = false,
                    AllowAutoRedirect = false,
                    UseDefaultCredentials = true
                };

                // Only bypass SSL certificate validation in development
                if (isDevEnvironment)
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                }

                return handler;
            });

        services.AddScoped<LoggingServices>();
        services.AddScoped<IMapper, Mapper>();
        services.AddScoped<ISmsServices, SmsServices>();
        services.AddScoped<ResponseInfo>();

        services.Configure<AzureADConfig>(config.GetSection("AzureADConfig"));
        services.Configure<FormJwtConfig>(config.GetSection("FormJwtConfig"));
        //services.Configure<CenterServicesConfig>(config.GetSection("CenterServicesConfig"));

        services.AddScoped(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AzureADConfig>>();
            return options.Value;
        });

        services.AddScoped(sp =>
        {
            var options = sp.GetRequiredService<IOptions<FormJwtConfig>>();
            return options.Value;
        });


        services.AddScoped<MSJsonWT>();

        services.AddScoped<UnitOfWork>();
        services.AddScoped<CacheManager>();
        services.AddScoped<CacheDataProvider>();
        services.AddScoped<AzureBlobStorageService>();

        services.AddScoped<MapperConfigServices>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Automatically scans the assembly for profiles

        services.Scan(scan => scan
            .FromAssemblies(typeof(AdminBase).GetTypeInfo().Assembly)
            .AddClasses(classes => classes.Where(x => x.IsSubclassOf(typeof(AdminBase))))
            .AsSelf()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(typeof(ApiBase).GetTypeInfo().Assembly)
            .AddClasses(classes => classes.Where(x => x.IsSubclassOf(typeof(ApiBase))))
            .AsSelf()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(typeof(ApiServiceBase).GetTypeInfo().Assembly)
            .AddClasses(classes => classes.Where(x => x.IsSubclassOf(typeof(ApiServiceBase))))
            .AsSelf()
            .WithScopedLifetime());

        services.AddScoped<MSJsonWT>();

        services.AddScoped<ISmsServices, SmsServices>();


    }
    public static UnitOfWork CreateScopedUow(this IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        return scope.CreateScopedUow();
    }
    public static UnitOfWork CreateScopedUow(this IServiceScopeFactory serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        return scope.CreateScopedUow();
    }
    public static UnitOfWork CreateScopedUow(this IServiceScope scope)
    {
        var uow = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
        return uow;
    }

        services.AddScoped<CacheDataProvider>();
        services.AddScoped<AzureBlobStorageService>();

        services.AddScoped<MasterBL>();

    public static void ConfigureRequestInfo(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<RequestInfo>();
    }



    public static void ConfigureSession(this IServiceCollection services, IConfiguration configuration, int sessionTimeout)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout); // Adjust the timeout as needed
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
    }
}