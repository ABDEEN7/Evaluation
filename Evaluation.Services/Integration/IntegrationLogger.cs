using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.IntegrationEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Enums;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;


namespace Evaluation.Services.Integration;
public class IntegrationLogger :ApiBase
{
    public IntegrationLogger(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo) : base(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
    {
    }

    public async Task<T> ExecuteAsync<T>(
        Func<Task<T>> action)
    {

        Guid integrationPointId = await uow.GetRepository<IntegrationPoint>()
                            .GetAllActiveNonDeleted()
                            .Where(x => x.BackendName == IntegrationPointEnum.HR)
                            .Select(x => x.Id)
                            .FirstOrDefaultAsync();

        var log = new IntegrationPointLog
        {
            StartDate = DateTime.UtcNow,
            IntegrationPointId = integrationPointId
        };

        var logResult = await uow
            .GetRepository<IntegrationPointLog>()
            .InsertAsync(log);

        try
        {
            // Execute actual business logic
            T result = await action();

            // Save response
            string jsonResponse = JsonConvert.SerializeObject(
                result,
                Formatting.Indented);

            // Update success log
            logResult.EndDate = DateTime.UtcNow;

            uow.GetRepository<IntegrationPointLog>()
                .Update(logResult);

            var dataLog = new IntegrationPointDataLog
            {
                IntegrationPointLogId = logResult.Id,
                DataResponse = jsonResponse
            };

            await uow
                .GetRepository<IntegrationPointDataLog>()
                .InsertAsync(dataLog);



            await uow.CommitAsync();

            return result;
        }
        catch (Exception ex)
        {
            logResult.EndDate = DateTime.UtcNow;

            uow.GetRepository<IntegrationPointLog>()
                .Update(logResult);

            await uow.CommitAsync();

            throw;
        }
    }
}