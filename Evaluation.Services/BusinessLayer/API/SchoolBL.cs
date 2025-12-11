using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Evaluation.Services.BusinessLayer.API;


public class SchoolBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, SchoolRepository schoolRepository)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{

    public async Task<ResponseSchools> GetSchoolDetails(Guid SchoolID)
    {
        var result = await schoolRepository.GetSchoolDetails(SchoolID);

        var schoolsResponse = mapper.Map<ResponseSchools>(result);

        return schoolsResponse;
    }

    public async Task<List<ResponseSchools>> GetSchoolsByDepartmentId(Guid depId)
    {
        var result = await schoolRepository.GetSchoolsByDepartmentId(depId);

        var schoolsResponse = mapper.Map<List<ResponseSchools>>(result);

        return schoolsResponse;
    }

    public async Task<PaginatedResult<ResponseSchools>> GetSchools(SchoolRequest request)
    {
        //TODO: Get Department Id by Department Routing Path
        
        var result = await schoolRepository.GetSchoolsAsync(request);
        return mapper.Map<PaginatedResult<ResponseSchools>>(result);
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