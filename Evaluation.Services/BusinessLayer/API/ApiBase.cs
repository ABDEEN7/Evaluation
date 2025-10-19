using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.BusinessLayer.API
{
    public abstract class ApiBase
    {
        protected readonly IServiceScopeFactory serviceScopeFactory;
        protected readonly CacheDataProvider cacheDataProvider;
        protected readonly UnitOfWork uow;
        protected readonly LoggingServices loggingServices;
        protected readonly UserInfo userInfo;
        protected readonly IServiceProvider serviceProvider;
        protected readonly RequestInfo requestInfo;

        // Static cache to avoid repeatedly searching for types and re-creating mappers
        private static readonly ConcurrentDictionary<string, Type> TypeCache = new();

        protected ApiBase(
            IServiceScopeFactory serviceScopeFactory,
            CacheDataProvider cacheDataProvider,
            UnitOfWork uow,
            LoggingServices loggingServices,
            UserInfo userInfo,
            IServiceProvider serviceProvider,
            RequestInfo requestInfo)
        {
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            this.cacheDataProvider = cacheDataProvider ?? throw new ArgumentNullException(nameof(cacheDataProvider));
            this.uow = uow ?? throw new ArgumentNullException(nameof(uow));
            this.loggingServices = loggingServices ?? throw new ArgumentNullException(nameof(loggingServices));
            this.userInfo = userInfo ?? throw new ArgumentNullException(nameof(userInfo));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.requestInfo = requestInfo ?? throw new ArgumentNullException(nameof(requestInfo));
        }

      

        #region === RESULT WRAPPERS ===

        protected static async Task<Result<T>> ExecuteWithResult<T>(Func<Task<T>> function)
        {
            try
            {
                var result = await function();
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail<T>(ex.Message);
            }
        }

        protected static async Task<Result> ExecuteWithResult(Func<Task> function)
        {
            try
            {
                await function();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        #endregion
    }
}
