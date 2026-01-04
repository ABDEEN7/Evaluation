using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RequestInfo = Evaluation.SharedHelper.Models.RequestInfo;

namespace Evaluation.Services.BusinessLayer.API
{
    public abstract class ApiBase
    {
        protected readonly IServiceScopeFactory serviceScopeFactory;
        protected readonly CacheDataProvider cacheDataProvider;
        protected readonly UnitOfWork uow;
        protected readonly LoggingServices loggingServices;
        protected readonly IMapper mapper;
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
            IMapper mapper,
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
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
        public async Task<IEnumerable<ControlValidation>> GetAppConstraints(string PermissionbackendName)
        {



            var result = await uow.GetRepository<ControlValidation>()
                                        .GetAllActiveNonDeleted()
                                        .Include(x=>x.Permission)
                                        .Where(x => x.Permission.BackendName == PermissionbackendName).
                                        OrderBy(x=>x.RowOrder)
                                        .ThenBy(x=>x.ColumnOrder)
                                        .ToListAsync();
            return result;




        }
        public async Task<bool> ValidateObject(object obj, string permission)
        {
            var constraintList = await GetAppConstraints(permission);

            if (constraintList != null && obj != null)
            {
                foreach (var propInfo in obj.GetType().GetProperties())
                {
                    var validation = constraintList.FirstOrDefault(x => x.Name == propInfo.Name);

                    if (validation != null)
                    {
                        var data = propInfo.GetType();
                        var propValue = propInfo.GetValue(obj);
                        if (validation.IsRequired && (validation.IsDbrequired ?? false) && (propValue == null || string.IsNullOrEmpty(propValue.ToString()) || string.IsNullOrWhiteSpace(propValue.ToString())))
                        {
                            throw new BusinessException(ConstantKeys.ExceptionMessage.Requiredfield);
                        }


                        if (validation.MaxLength != null && propValue is string MaxstringValue)
                        {
                            MaxstringValue = MaxstringValue.Trim();
                            if (!string.IsNullOrEmpty(MaxstringValue))
                            {
                                if (MaxstringValue.Length > validation.MaxLength)
                                {
                                    throw new BusinessException(ConstantKeys.ExceptionMessage.ExceedMaxlength);
                                }
                            }
                        }
                        if (validation.MinLength != null && propValue is string MinstringValue)
                        {
                            MinstringValue = MinstringValue.Trim();
                            if (!string.IsNullOrEmpty(MinstringValue))
                            {

                                if (MinstringValue.Length < validation.MinLength)
                                    throw new BusinessException(ConstantKeys.ExceptionMessage.BelowMinlength);
                            }
                        }

                        if (!string.IsNullOrEmpty(validation.Regex) && propValue is string RegexstringValue)
                        {
                            bool isValid = Regex.IsMatch(RegexstringValue, validation.Regex);
                            if (!isValid)
                            {
                                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRegex);
                            }
                        }
                        if (validation.ControlType == "JSON_AREA" && propValue is string jsonstringValue)
                        {
                            bool isValid = IsValidJson(jsonstringValue);
                            if (!isValid)
                            {
                                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidJson);
                            }
                        }
                    }
                }
                return true;
            }
            return true;
        }
        public static bool IsValidJson(string jsonString)
        {
            try
            {
                JsonDocument.Parse(jsonString); // Try parsing the JSON
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
        #endregion
    }
}
