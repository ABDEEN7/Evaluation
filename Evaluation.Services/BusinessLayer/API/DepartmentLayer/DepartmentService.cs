using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.DepartmentLayer;

public class DepartmentService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<Department> GetDepartmentById(Guid Id)
    {
        var department = await unitOfWork.GetRepository<Department>()
            .GetAllActiveNonDeleted().Include(d => d.Category)
            .Where(d => d.Id == Id).FirstOrDefaultAsync();
        return department;
    }
    public async Task<List<Department>> GetAllDepartments()
    {
        var departments = await unitOfWork.GetRepository<Department>()
            .GetAllActiveNonDeleted().Include(d=>d.Category)
            .ToListAsync();
        return departments;
    }
    public async Task<Guid?> GetDepartmentIdAsync()
    {
        var department = await unitOfWork.GetRepository<Department>()
            .GetAllActiveNonDeleted(x =>
                x.UserDepartments.Any(ud => ud.UserId == userInfo.UserId))
            .OrderByDescending(x=>x.CreateDate)
            .FirstOrDefaultAsync();
        return department?.Id;
    }


    public async Task<List<Department>> GetDepartmentsByWebGroupId(Guid wepGroupId)
    {
        var departments = await unitOfWork.GetRepository<DepWebGroup>()
            .GetAllActiveNonDeleted(x =>
                x.WebGroupId == wepGroupId).Select(x=> x.Department).ToListAsync();
        return departments;
    }
}