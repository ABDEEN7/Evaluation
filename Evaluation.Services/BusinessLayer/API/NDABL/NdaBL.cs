using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.Services.BusinessLayer.API.NDABL;

public class NdaBL(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo,
        serviceProvider, requestInfo)
{
    public async Task<Result<NDADto>> GetPendingNda()
    {
        var pendingData =
            await unitOfWork
            .GetRepository<NdaStatusDepartment>()
            .GetAllActiveNonDeleted()
            .Include(x => x.NdaStatus)
            .Where(x => x.NdaStatus.BackendName == NDAStatic.Pending)
            .FirstOrDefaultAsync();
        NDADto result = new NDADto
        {
            Id = pendingData.Id
            //,
            //Name = pendingData.NameEn
        };
        return result;
    }
}
