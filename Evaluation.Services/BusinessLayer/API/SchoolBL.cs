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
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

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

    //public async Task<PaginatedResult<ResponseSchools>> GetSchools(SchoolRequest request)
    //{
    //    //TODO: Get Department Id by Department Routing Path

    //    var result = await schoolRepository.GetSchoolsAsync(request);
    //    return mapper.Map<PaginatedResult<ResponseSchools>>(result);
    //}
    public async Task<PaginatedResult<ResponseSchoolsPlans>> GetSchoolsPlan(SchoolRequest request)
    {
        Department? department = GetAcademicYear();
        int? academicYear = await GetCurrentAcademicYear(department);
        Guid? targetOrgTreeId = department.TargetOrgTreeId;
        List<Guid> currentOrgTree = await GetCurrentOrgTree(department, academicYear);

        PaginatedResult<ResponseSchoolsPlans> response;
        switch (department.Category?.BackendName)
        {
            case DepartmentCateogry.Schools:
                {
                    var result = await schoolRepository.GetSchoolsAsync(request, targetOrgTreeId, currentOrgTree);
                    response = mapper.Map<PaginatedResult<ResponseSchoolsPlans>>(result);
                    break;
                }

            case DepartmentCateogry.Employee:
                {
                    var result = await employeeService.GetEmployeeAsync(request, targetOrgTreeId, currentOrgTree);
                    response = mapper.Map<PaginatedResult<ResponseSchoolsPlans>>(result);
                    break;
                }
            case DepartmentCateogry.Orgnization:
                {
                    var result = await orgnizationService.GetOrgnizationAsync(request, targetOrgTreeId, currentOrgTree);
                    response = mapper.Map<PaginatedResult<ResponseSchoolsPlans>>(result);
                    break;
                }

            default:
                throw new BusinessException("Unsupported department category");
        }

        return response;
    }

    private Department GetAcademicYear()
    {
        var department = uow.
                    GetRepository<Department>()
                    .GetAllActiveNonDeleted(x => x.Id == new Guid("B3726A76-83B6-4EF9-B2B2-E74EC5FCC819"))
                    .Include(x => x.Category)
                    .FirstOrDefault();
        if (department == null)
            throw new Exception();
        return department;
    }

    private async Task<int?> GetCurrentAcademicYear(Department department)
    {
        int? acc = await uow.GetRepository<AcademicYear>()
                                    .GetAllActiveNonDeleted()
                                    .OrderByDescending(x => x.CreateDate)
                                    .Where(x => x.DepartmentId == department.Id && x.IsCurrent)
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
    private async Task<List<Guid>> GetCurrentOrgTree(Department department, int? academicYear)
       => await uow.GetRepository<OrgAcademicYear>()
                                           .GetAllActiveNonDeleted(x => x.Year == academicYear && x.ParentOrgTreeId == department.TargetOrgTreeId)
                                           .Select(x => x.OrgTreeId)
                                           .ToListAsync();

}