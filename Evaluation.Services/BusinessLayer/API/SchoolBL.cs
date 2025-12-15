using System.Linq.Expressions;
using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using FluentResults;
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
        var schoolData = await serviceProvider.CreateScopedUow().GetRepository<School>()
            .GetAllQueryFiltered()
            .AsNoTracking()
            .Where(c => c.Id == SchoolID)
               .FirstOrDefaultAsync();
        return schoolData;
    }
    public async Task<PaginatedResult<ResponseSchools>> GetSchools(SchoolRequest request)
    {
        var result = await schoolRepository.GetSchoolsAsync(request);
        return mapper.Map<PaginatedResult<ResponseSchools>>(result);
    }
    public async Task<PaginatedResult<ResponseSchoolsPlans>> GetSchoolsPlan(SchoolRequest request)
    {
        var result = await schoolRepository.GetSchoolsAsync(request);
        return mapper.Map<PaginatedResult<ResponseSchoolsPlans>>(result);
    }
    public async Task<List<SchoolVisits>> GetVisitsAsync()
    {
        var responses = schoolRepository.GetVisitTypes();
        return await responses.Select(x => new SchoolVisits
        {
            Id = x.Id,
            Name = LanguageStatic.SelectLang(requestInfo.Lang, x.NameAr, x.NameEn)
        }).ToListAsync();
    }
}