using AutoMapper;
using Azure;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.OrganizationLayer;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Evaluation.Services.BusinessLayer.API;


public class SchoolBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, SchoolRepository schoolRepository,
        EmployeeService employeeService, OrgnizationService orgnizationService)
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
	public async Task<List<ResponseSchools>> GetSchools()
	{

		var result = await schoolRepository.GetSchoolsByDepartmentId(requestInfo.DepId.Value);

		var schoolsResponse = mapper.Map<List<ResponseSchools>>(result);

		return schoolsResponse;
	}
	//public async Task<PaginatedResult<ResponseSchools>> GetSchools(SchoolRequest request)
	//{
	//    //TODO: Get Department Id by Department Routing Path

	//    var result = await schoolRepository.GetSchoolsAsync(request);
	//    return mapper.Map<PaginatedResult<ResponseSchools>>(result);
	//}
	public async Task<PaginatedResult<ResponseSchoolsPlans>> GetSchoolsPlan(SchoolRequest request)
    {
        List<DepTargetOrgTree> depTargetOrgTrees = await GetDepTargetOrgTree();
        int? academicYear = await GetCurrentAcademicYear();
        if (!depTargetOrgTrees.Any())
            throw new BusinessException("Department has no target org trees");

        List<Guid?> targetOrgTreeIds =
                    depTargetOrgTrees
                              .Select(x => (Guid?)x.TargetOrgTreeId)
                              .ToList();
        List<Guid> currentOrgTree = await GetCurrentOrgTreeIds(targetOrgTreeIds, academicYear);

        PaginatedResult<ResponseSchoolsPlans> response;
        var dep = depTargetOrgTrees.Select(x => x.Category?.BackendName).ToList();
        if (dep.Count == 1)
            switch (dep.FirstOrDefault())
            {
                case DepartmentCateogry.Schools:
                    {
                        var result = await schoolRepository.GetSchoolsAsync(request, targetOrgTreeIds, currentOrgTree);
                        response = mapper.Map<PaginatedResult<ResponseSchoolsPlans>>(result);
                        break;
                    }

                case DepartmentCateogry.Employee:
                    {
                        var result = await employeeService.GetEmployeeAsync(request, targetOrgTreeIds, currentOrgTree);
                        response = mapper.Map<PaginatedResult<ResponseSchoolsPlans>>(result);
                        break;
                    }
                case DepartmentCateogry.Orgnization:
                    {
                        var result = await orgnizationService.GetOrgnizationAsync(request, targetOrgTreeIds, currentOrgTree);
                        response = mapper.Map<PaginatedResult<ResponseSchoolsPlans>>(result);
                        break;
                    }

                default:
                    throw new BusinessException("Unsupported department category");
            }
        else
        {
            var result = await orgnizationService.GetOrgnizationAsync(request, targetOrgTreeIds, currentOrgTree);
            response = mapper.Map<PaginatedResult<ResponseSchoolsPlans>>(result);
        }

        return response;
    }

    private async Task<List<DepTargetOrgTree>> GetDepTargetOrgTree()
    {
        var depTargetOrgTree = await uow.
                    GetRepository<DepTargetOrgTree>()
                    .GetAllActiveNonDeleted(x => x.DepartmentId == requestInfo.DepId)
                    .Include(x => x.TargetOrgTree)
                    .Include(x => x.Category)
                    .ToListAsync();
        if (depTargetOrgTree == null)
            throw new Exception();
        return depTargetOrgTree;
    }

    private async Task<int?> GetCurrentAcademicYear()
    {
        int? acc = await uow.GetRepository<AcademicYear>()
                                    .GetAllActiveNonDeleted()
                                    .OrderByDescending(x => x.CreateDate)
                                    .Where(x => x.DepartmentId == requestInfo.DepId && x.IsCurrent)
                                    .Select(x => x.Year)
                                    .FirstOrDefaultAsync();
        if (!acc.HasValue)
            throw new Exception();
        return acc;
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
    private async Task<List<Guid>> GetCurrentOrgTreeIds(List<Guid?> TargetOrgTreeIds, int? academicYear)
       => await uow.GetRepository<OrgAcademicYear>()
                                           .GetAllActiveNonDeleted(x => x.Year == academicYear && TargetOrgTreeIds.Contains(x.ParentOrgTreeId))
                                           .Select(x => x.OrgTreeId)
                                           .ToListAsync();

}