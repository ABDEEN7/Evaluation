using AutoMapper;
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
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

    //public async Task<PaginatedResult<ResponseSchools>> GetSchools(SchoolRequest request)
    //{
    //    //TODO: Get Department Id by Department Routing Path

    //    var result = await schoolRepository.GetSchoolsAsync(request);
    //    return mapper.Map<PaginatedResult<ResponseSchools>>(result);
    //}
    public async Task<PaginatedResult<ResponseOrgsPlans>> GetSchoolsPlan(SchoolRequest request)
    {
        List<DepTargetOrgTree> depTargetOrgTrees = await orgService.GetDepTargetOrgTree();
        int? academicYear = await academicYearServices.GetCurrentAcademicYear();
        List<Guid?> targetOrgTreeIds =
                    depTargetOrgTrees
                              .Select(x => (Guid?)x.TargetOrgTreeId)
                              .ToList();
        List<Guid> currentOrgTree = await orgService.GetCurrentOrgTreeIds(targetOrgTreeIds, academicYear);

        PaginatedResult<ResponseOrgsPlans> response;
        var dep = depTargetOrgTrees
            .GroupBy(s => s.Category?.BackendName).Select(x => x.Key).ToList();
        if (dep.Count == 1)
            switch (dep.FirstOrDefault())
            {
                case DepartmentCateogry.Schools:
                    {
                        var result = await schoolRepository.GetSchoolsAsync(request, targetOrgTreeIds, currentOrgTree);
                        response = mapper.Map<PaginatedResult<ResponseOrgsPlans>>(result);
                        break;
                    }

                case DepartmentCateogry.Employee:
                    {
                        var result = await employeeService.GetEmployeeAsync(request, targetOrgTreeIds, currentOrgTree);
                        response = mapper.Map<PaginatedResult<ResponseOrgsPlans>>(result);
                        break;
                    }
                case DepartmentCateogry.Orgnization:
                    {
                        var result = await orgnizationService.GetOrgnizationAsync(request, targetOrgTreeIds, currentOrgTree);
                        response = mapper.Map<PaginatedResult<ResponseOrgsPlans>>(result);
                        break;
                    }

                default:
                    throw new BusinessException("Unsupported department category");
            }
        else
        {
            var result = await orgnizationService.GetOrgnizationAsync(request, targetOrgTreeIds, currentOrgTree);
            response = mapper.Map<PaginatedResult<ResponseOrgsPlans>>(result);
        }

        return response;
    }

    public async Task<List<SchoolVisits>> GetVisitsAsync()
    {
        var responses = await schoolRepository.GetVisitTypes();
        return await responses.Select(x => new SchoolVisits
        {
            Id = x.Id,
            Name = LanguageStatic.SelectLang(requestInfo.Lang, x.NameAr, x.NameEn)
        }).ToListAsync();
    }
}