using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.JWT;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Evaluation.Services.Extensions;


public static class ServiceExtensions
{
#pragma warning disable  S4830

    public static void ConfigureMasterBL(this IServiceCollection services, IConfiguration config, bool isDevEnvironment)
    {

        services.AddMemoryCache();
        services.AddHttpClient<HttpClientServices>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))    // Default is 2 mins
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    //AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
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

        //services.AddScoped(sp =>
        //{
        //    var options = sp.GetRequiredService<IOptions<CenterServicesConfig>>();
        //    return options.Value;
        //});


        services.AddScoped<MSJsonWT>();

        services.AddScoped<ISmsServices, SmsServices>();

        services.AddScoped<UnitOfWork>();
        services.AddScoped<CacheManager>();

        services.AddScoped<CacheDataProvider>();
        services.AddScoped<AzureBlobStorageService>();

        services.AddScoped<MasterBL>();

    }

}