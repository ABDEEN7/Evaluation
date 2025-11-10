using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.BusinessLayer;
using Evaluation.SharedHelper.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.JWT;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Evaluation.DAL.Context;
using Evaluation.Services.BusinessLayer.CenterServices;
using System.Net;
using MapsterMapper;
using Evaluation.DAL.Helper;

namespace Evaluation.SharedHelper
{
    public static class ServiceCollectionExtensions
    {
		#region UnitOfWork Scoped Extensions

		public static void PopulateAppSettings(this IServiceCollection services, IConfiguration config)
		{
			AppSettings.CreateById = config.GetValue<Guid>("AppSettings:CreateById");
			AppSettings.DefaultLanguage = config.GetValue<string>("AppSettings:DefaultLanguage") ?? "en";
			AppSettings.DateFormat = config.GetValue<string>("AppSettings:DateFormat") ?? "yyyy-MM-dd";
		}
		//public static void ConfigureMasterBL(this IServiceCollection services, IConfiguration config, bool isDevEnvironment)
		//{

		//	services.AddDbContext<EvaluationDbContext>(options =>
		//	{
		//		options.UseSqlServer(config.GetConnectionString("ScholarshipDBConn"));
		//	});

		//	services.AddMemoryCache();

		//	object value = services.AddHttpClient<HttpClientServices>()
		//		.SetHandlerLifetime(TimeSpan.FromMinutes(5))    // Default is 2 mins
		//		.ConfigurePrimaryHttpMessageHandler(() =>
		//		{
		//			var handler = new HttpClientHandler
		//			{
		//				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
		//				UseCookies = false,
		//				AllowAutoRedirect = false,
		//				UseDefaultCredentials = true
		//			};

		//			// Only bypass SSL certificate validation in development
		//			if (isDevEnvironment)
		//			{
		//				handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
		//			}

		//			return handler;
		//		});

		//	services.AddScoped<LoggingServices>();

		//	services.AddScoped<ISmsServices, SmsServices>();
		//	services.AddScoped<ResponseInfo>();

		//	services.Configure<AzureADConfig>(config.GetSection("AzureADConfig"));
		//	services.Configure<FormJwtConfig>(config.GetSection("FormJwtConfig"));
		//	services.Configure<CenterServicesConfig>(config.GetSection("CenterServicesConfig"));

		//	services.AddScoped(sp =>
		//	{
		//		var options = sp.GetRequiredService<IOptions<AzureADConfig>>();
		//		return options.Value;
		//	});

		//	services.AddScoped(sp =>
		//	{
		//		var options = sp.GetRequiredService<IOptions<FormJwtConfig>>();
		//		return options.Value;
		//	});

		//	//services.AddScoped(sp =>
		//	//{
		//	//	var options = sp.GetRequiredService<IOptions<CenterServicesConfig>>();
		//	//	return options.Value;
		//	//});

		//	//services.AddScoped<FromJsonWT>();
		//	services.AddScoped<MSJsonWT>();

		//	services.AddScoped<ISmsServices, SmsServices>();

		//	services.AddScoped<UnitOfWork>();
		//	services.AddScoped<CacheManager>();
		//	services.AddScoped<CacheDataProvider>();
		//	services.AddScoped<AzureBlobStorageService>();

		//	services.AddScoped<MasterBL>();



		//}

		public static UnitOfWork CreateScopedUow(this IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            return new ScopedUnitOfWork(scope);
        }

        public static UnitOfWork CreateScopedUow(this IServiceScopeFactory factory)
        {
            var scope = factory.CreateScope();
            return new ScopedUnitOfWork(scope);
        }

        public static UnitOfWork CreateScopedUow(this IServiceScope scope)
        {
            return new ScopedUnitOfWork(scope);
        }
        #endregion

        #region UserInfo / RequestInfo
        public static void ConfigureUserInfo(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<UserInfo>();
        }

        public static void ConfigureRequestInfo(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<RequestInfo>();
        }
        public static void ConfigureMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMapper>();
        }
        #endregion

        #region Session
        public static void ConfigureSession(this IServiceCollection services, IConfiguration configuration, int sessionTimeout)
        {
        //    services.AddSession(options =>
        //    {
        //        options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout);
        //        options.Cookie.HttpOnly = true;
        //        options.Cookie.IsEssential = true;
        //    });
        }
        #endregion
    }

    public sealed class ScopedUnitOfWork : IDisposable
    {
        private readonly IServiceScope _scope;
        public UnitOfWork Uow { get; }

        public ScopedUnitOfWork(IServiceScope scope)
        {
            _scope = scope;
            Uow = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }

        public static implicit operator UnitOfWork(ScopedUnitOfWork scoped) => scoped.Uow;
    }
}