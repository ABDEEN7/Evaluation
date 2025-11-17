using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Evaluation.DAL.Repositories;
using Evaluation.DAL.Models.Org;

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
    public async Task<Employee> GetEmployee(string email)
    {
        return await unitOfWork.GetRepository<Employee>()
                    .GetAllNonDeleted()
                    .Where(c => c.Email.ToLower() == email.ToLower())
                    .FirstOrDefaultAsync();
    }
}
