using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.AcademicYearLayer;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Dtos.OrgDto;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;

public class OrgBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, OrgService orgService,
        SchoolRepository schoolRepository, EmployeeService employeeService, OrganizationService organizationService, DepartmentService departmentService, AcademicYearServices academicYearServices)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<OrgDetailsDto> GetOrgDetails(Guid id)
    {
        var depTargetOrgTree = await orgService.GetDepTargetOrgTree();

        if (depTargetOrgTree == null)
            throw new Exception("Department doesn't exist!");
        var dep = depTargetOrgTree
            .GroupBy(s => s.Category?.BackendName).Select(x => x.Key).ToList();
        var orgDetails = new OrgDetailsDto();
        switch (dep.FirstOrDefault())
        {
            case DepartmentCateogry.Schools:
                var school = await schoolRepository.GetSchoolDetails(id);
                var schoolManager = await employeeService.GetEmployee(school.ManagerQID);
                orgDetails = mapper.Map<OrgDetailsDto>(await schoolRepository.GetSchoolDetails(id));
                orgDetails.ManagerName = schoolManager?.NameEn;
                break;
            case DepartmentCateogry.Employee:
                orgDetails = mapper.Map<OrgDetailsDto>(await employeeService.GetEmployeeById(id));
                break;
            case DepartmentCateogry.Orgnization:
                orgDetails = mapper.Map<OrgDetailsDto>(await organizationService.GetOrganizationById(id));
                break;
            case DepartmentCateogry.OrgSelf:
                break;
        }

        return orgDetails;
    }
    public async Task<List<ParentOrgTreeDto>> GetParentOrgTreeAsync()
    {
        List<DepTargetOrgTree> depTargetOrgTrees = await orgService.GetDepTargetOrgTree();
        int? academicYear = await academicYearServices.GetCurrentAcademicYear();
        List<Guid?> targetOrgTreeIds =
                    depTargetOrgTrees
                              .Select(x => (Guid?)x.TargetOrgTreeId)
                              .ToList();
        List<OrgTree> currentOrgTree = await orgService.GetCurrentOrgTrees(targetOrgTreeIds, academicYear);
        var parentTreeDto = mapper.Map<List<ParentOrgTreeDto>>(currentOrgTree);
        return parentTreeDto;
    }
}