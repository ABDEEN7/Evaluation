using AutoMapper;
using Evaluation.DAL;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.AcademicYearLayer;
using Evaluation.Services.BusinessLayer.API.OrganizationLayer;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Dtos.AcademicYearDto;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

namespace Evaluation.Services.BusinessLayer.API;


public class SchoolBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, SchoolRepository schoolRepository,
        EmployeeService employeeService, OrgnizationService orgnizationService, OrgService orgService,
        AcademicYearServices academicYearServices)
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

    [HttpPost]
    public async Task<PaginatedResult<ResponseSchools>> GetSchools([FromBody] SchoolRequest request)
    {
        var result = await schoolRepository.GetSchoolsAsyncOld(request);
        return mapper.Map<PaginatedResult<ResponseSchools>>(result);
    }
    public async Task<PaginatedResult<ResponseOrgsPlans>> GetSchoolsPlan(SchoolRequest request)
    {
        var depTargetOrgTrees = await orgService.GetDepTargetOrgTree();
        var academicYear = await academicYearServices.GetCurrentAcademicYear();

        var targetOrgTreeIds = depTargetOrgTrees
            .Select(x => (Guid?)x.TargetOrgTreeId)
            .ToList();

        var currentOrgTree = await orgService.GetCurrentOrgTreeIds(targetOrgTreeIds, academicYear);

        var dep = depTargetOrgTrees
            .GroupBy(s => s.Category?.BackendName)
            .Select(x => x.Key)
            .ToList();
        PaginatedResult<ResponseOrgsPlans> result;
        if (dep.Count == 1)
        {
            result = dep.First() switch
            {
                DepartmentCateogry.Schools =>
                    await schoolRepository.GetSchoolsAsync(request, targetOrgTreeIds, currentOrgTree),

                DepartmentCateogry.Employee =>
                    await employeeService.GetEmployeeAsync(request, targetOrgTreeIds, currentOrgTree),

                DepartmentCateogry.Orgnization =>
                    await orgnizationService.GetOrgnizationAsync(request, targetOrgTreeIds, currentOrgTree),

                _ => throw new BusinessException(ConstantKeys.ExceptionMessage.UnsupportedDepartmentCategory)
            };
        }
        else
        {
            result = await orgnizationService.GetOrgnizationAsync(request, targetOrgTreeIds, currentOrgTree);
        }
        await FillAcademicYear(result);
        return result;
    }
    private async Task FillAcademicYear(PaginatedResult<ResponseOrgsPlans> result)
    {
        var academicYears = await uow.GetRepository<AcademicYear>()
            .GetAllActiveNonDeleted(x => x.DepartmentId == requestInfo.DepId && x.Year >= DateTime.UtcNow.Year)
            .Select(x => new AcademicYearLite
            {
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Year = x.Year
            })
            .OrderBy(x => x.StartDate)
            .ToListAsync();

        foreach (var item in result.Items)
        {
            item.YearAcdemicYear = ConstantLogic.ResolveAcademicYear(
                item.AcademicYear,
                academicYears
            );
        }

    }

    public async Task<List<SchoolVisits>> GetVisitsAsync()
    {
        var responses = schoolRepository.GetVisitTypes();
        return await responses.Select(x => new SchoolVisits
        {
            Id = x.Id,
            Name = LanguageStatic.SelectLang(requestInfo.Lang, x.NameAr, x.NameEn),
            BackendName = x.EvaluationType.BackendName
        }).ToListAsync();
    }
    public async Task<List<GetEducationLevelDto>> GetEducationLevelAsync()
    {
        int? currentAcademicYear = await academicYearServices.GetCurrentAcademicYear();
        var responses = schoolRepository.GetEducationLevel(currentAcademicYear);
        return await responses.Select(x => new GetEducationLevelDto
        {
            Id = x.Id,
            Name = LanguageStatic.SelectLang(requestInfo.Lang, x.NameAr, x.NameEn),
            BackendName = x.BackendName
        }).ToListAsync();
    }
    public async Task<List<SchoolGenderDto>> GetSchoolGenderAsync()
    {
        
        var responses = schoolRepository.GetSchoolGender();
        return await responses.Select(x => new SchoolGenderDto
        {
            Id = x.Id,
            Name = LanguageStatic.SelectLang(requestInfo.Lang, x.NameAr, x.NameEn),
            BackendName = x.BackendName
        }).ToListAsync();
    }
}