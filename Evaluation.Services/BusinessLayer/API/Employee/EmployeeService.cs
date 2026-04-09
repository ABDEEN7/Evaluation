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
    public async Task<PaginatedResult<Employee>> GetEmployeeAsync(SchoolRequest request, List<Guid?> targetOrgTreeIds, List<Guid> employees)
    {
        var filter = BuildFilterExpression(request, targetOrgTreeIds, employees);
        var query = unitOfWork.GetRepository<Employee>()
                    .GetAllNonDeleted(filter)
                    .Include(x=>x.OrgParent);
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
    private Expression<Func<Employee, bool>> BuildFilterExpression(SchoolRequest request, List<Guid?> targetOrgTreeIds, List<Guid> employees)
    {
        Expression<Func<Employee, bool>> filter = s => true;
        filter = filter.And(c => targetOrgTreeIds.Contains(c.OrgParentId) && employees.Contains(c.Id));

        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(s => s.NameEn.Contains(request.Name) || s.NameAr.Contains(request.Name));
        //if(request.VisitType != null)
        //    filter = filter.And(x=>x.)
        if (request.ParentId != Guid.Empty && request.ParentId != null)
        {
            filter = filter.And(x => x.OrgParentId == request.ParentId);
        }
        return filter;
    }

    public async Task<Employee> GetEmployeeById(Guid Id)
    {
        return await unitOfWork.GetRepository<Employee>()
                    .GetAllNonDeleted()
                    .Include(e => e.UserGender)
                    .Include(e => e.JobTitle)
                    .Where(c => c.Id == Id)
                    .FirstOrDefaultAsync();
    }


    public async Task<List<Employee>> GetEmployeesBySchoolId(Guid Id, Guid? JobTitleId = null)
    {
        var query = unitOfWork.GetRepository<Employee>()
                    .GetAllNonDeleted()
                    .Include(e => e.UserGender)
                    .Include(e => e.JobTitle)
                    .Where(c => c.OrgParentId == Id);

        if (JobTitleId != null)
            query = query.Where(c => c.JobTitleId == JobTitleId);

        return await query.ToListAsync();
    }
}
