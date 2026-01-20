using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Evaluation.Services.BusinessLayer.API;


public class EmployeeService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<Employee> GetEmployee(string qId)
    {
        return await unitOfWork.GetRepository<Employee>()
                    .GetAllNonDeleted()
                    .Where(c => c.QID.ToLower() == qId)
                    .FirstOrDefaultAsync();
    }
    public async Task<PaginatedResult<Employee>> GetEmployeeAsync(SchoolRequest request, Guid? targetOrgTreeId, List<Guid> employees)
    {
        var filter = BuildFilterExpression(request, targetOrgTreeId, employees);
        var query = unitOfWork.GetRepository<Employee>()
                    .GetAllNonDeleted(filter);
        return await query.GetPaginatedResult(request.PageNumber, request.PageSize);
    }

    public async Task<JobTitle> GetJobTitle(string jobNo)
    {
        return await unitOfWork.GetRepository<JobTitle>()
                    .GetAllNonDeleted()
                    .Where(c => c.HRCode.ToLower() == jobNo)
                    .FirstOrDefaultAsync();
    }
    public async Task<OrgType> GetOrgType(string orgLocNo)
    {
        return await unitOfWork.GetRepository<OrgType>()
                    .GetAllNonDeleted()
                    .Where(c => c.BackendName.ToLower() == orgLocNo)
                    .FirstOrDefaultAsync();
    }
    public async Task<OrgClass> GetOrgClass(string orgClass)
    {
        return await unitOfWork.GetRepository<OrgClass>()
                    .GetAllNonDeleted()
                    .Where(c => c.HRCode.ToLower() == orgClass)
                    .FirstOrDefaultAsync();
    }
    private Expression<Func<Employee, bool>> BuildFilterExpression(SchoolRequest request, Guid? targetOrgTreeId, List<Guid> employees)
    {
        Expression<Func<Employee, bool>> filter = s => true;
        filter = filter.And(c => c.OrgParentId == targetOrgTreeId && employees.Contains(c.Id));

        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(s => s.NameEn.Contains(request.Name) || s.NameAr.Contains(request.Name));
        //if(request.VisitType != null)
        //    filter = filter.And(x=>x.)
        return filter;
    }

    public async Task<Employee> GetEmployeeById(Guid Id)
    {
        return await unitOfWork.GetRepository<Employee>()
                    .GetAllNonDeleted()
                    .Include(e=>e.UserGender)
                    .Include(e=>e.JobTitle)
                    .Where(c => c.Id == Id)
                    .FirstOrDefaultAsync();
    }

}
