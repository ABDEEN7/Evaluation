using Microsoft.Extensions.DependencyInjection;
using Evaluation.DAL.UnitOfWork;
using System;
using Evaluation.SharedHelper.Helper;
using Microsoft.Extensions.Configuration;

namespace Evaluation.SharedHelper
{
    public static class ServiceCollectionExtensions
    {
        #region UnitOfWork Scoped Extensions
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