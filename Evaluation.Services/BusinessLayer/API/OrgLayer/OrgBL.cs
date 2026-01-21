using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.OrgDto;
using Evaluation.SharedHelper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;

public class OrgBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, OrgService orgService, 
        SchoolRepository schoolRepository, EmployeeService employeeService, OrganizationService organizationService, DepartmentService departmentService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<OrgDetailsDto> GetOrgDetails(Guid Id)
    {
        var department = await departmentService.GetDepartmentById(new Guid("2db317d7-8709-4997-86c2-6b08f18c0d58"));//TODO: Read this Info from Request Info when be available
       
        if (department == null)     
            throw new Exception("Department doesn't exist!");
        
        var orgDetails = new OrgDetailsDto();
        //switch (department.Category?.BackendName)
        //{
        //    case "SCHOOL":
        //        var school = await schoolRepository.GetSchoolDetails(Id);
        //        var schoolManager = await employeeService.GetEmployee(school.ManagerQID);
        //        orgDetails =  mapper.Map<OrgDetailsDto>(await schoolRepository.GetSchoolDetails(Id));
        //        orgDetails.ManagerName = schoolManager?.NameEn;
        //        break;
        //    case "EMPLOYEE":
        //        orgDetails =  mapper.Map<OrgDetailsDto>(await employeeService.GetEmployeeById(Id));
        //        break;
        //    case "ORGANIZATION":
        //        orgDetails =  mapper.Map<OrgDetailsDto>(await organizationService.GetOrganizationById(Id));
        //        break;
        //    case "ORG_SELF":
        //        break;
        //}

        return orgDetails;
    }
}