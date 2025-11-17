using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.DepartmentLayer;

public class DepartmentBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, DepartmentService departmentService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<DepartmentDto>> GetAllDepartments()
    {
        var x = await departmentService.GetAllDepartments();
            return mapper.Map<List<DepartmentDto>>(x);
    }
}