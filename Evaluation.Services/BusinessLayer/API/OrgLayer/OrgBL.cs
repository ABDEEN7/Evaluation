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
                orgDetails = mapper.Map<OrgDetailsDto>(school, opt =>
                {
                    opt.Items["lang"] = requestInfo.Lang;
                });
                orgDetails.ManagerName = requestInfo.Lang == "ar" ? schoolManager?.NameAr : schoolManager?.NameEn;

                var evaluationRequests = await schoolRepository.GetEvaluationRequestByOrgTreeId(id);

                var currentRequest = evaluationRequests.OrderByDescending(er => er.CreateDate).FirstOrDefault();
               
                orgDetails.CurrentEvaluationResult = (requestInfo.Lang == "ar" ? currentRequest?.FormEvalMatrixValue?.NameAr : currentRequest?.FormEvalMatrixValue?.NameEn) ?? "-";

                orgDetails.CurrentEvaluationDate = currentRequest?.EvaluationDate.ToString() ?? "-";

                var lastRequest = evaluationRequests.OrderBy(er => er.CreateDate).FirstOrDefault();

                orgDetails.LastEvaluationResult = (requestInfo.Lang == "ar" ? lastRequest?.FormEvalMatrixValue?.NameAr : lastRequest?.FormEvalMatrixValue?.NameEn) ?? "-";
                orgDetails.LastEvaluationDate = lastRequest?.EvaluationDate.ToString() ?? "-";

                break;
            case DepartmentCateogry.Employee:
                orgDetails = mapper.Map<OrgDetailsDto>(await employeeService.GetEmployeeById(id), opt =>
                {
                    opt.Items["lang"] = requestInfo.Lang;
                });
                break;
            case DepartmentCateogry.Orgnization:
                orgDetails = mapper.Map<OrgDetailsDto>(await organizationService.GetOrganizationById(id), opt =>
                {
                    opt.Items["lang"] = requestInfo.Lang;
                });
                break;
            case DepartmentCateogry.OrgSelf:
                break;
        }

        return orgDetails;
    }

    public async Task<List<OrgDetailsDto>> GetEmployeesBySchoolId(Guid id)
    {
        return mapper.Map<List<OrgDetailsDto>>(await employeeService.GetEmployeesBySchoolId(id));
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