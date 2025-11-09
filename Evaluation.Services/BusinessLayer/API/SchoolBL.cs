using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Entities.Planing;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;


public class SchoolBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, SchoolRepository schoolRepository)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{

    public async Task<School> GetSchoolDetails(Guid SchoolID)
    {
        var schoollist = await uow.GetRepository<School>().GetAllActiveNonDeleted().ToListAsync();

        var schoolData = await serviceProvider.CreateScopedUow().GetRepository<School>()
            .GetAllQueryFiltered()
            .AsNoTracking()
            .Where(c => c.Id == SchoolID)
               .FirstOrDefaultAsync();
        return schoolData;
    }
    public async Task<Result<List<ResponseSchools>>> GetSchools()
    {
        return await ExecuteWithResult(async () =>
        {
            var schoolsRequest = await schoolRepository.GetSchoolsAsync();
            var schoolResponse = schoolsRequest.Adapt<List<ResponseSchools>>();
            return schoolResponse;
        });
    }
    public async Task<Result<List<SchoolVisits>>> GetVisitsAsync()
    {
        return await ExecuteWithResult(async () =>
        {
            var responses = await schoolRepository.GetVisitTypes();
            return responses;
        });
    }

}