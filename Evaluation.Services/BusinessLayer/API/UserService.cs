using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;

public class UserService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public IQueryable<MinistryUser> GetUsers()
    {
        return uow.GetRepository<MinistryUser>()
            .GetAllActiveNonDeleted()
            .AsNoTracking();
    }
    public async Task EnsureUserExists(Guid? userId)
    {
        var exists = await uow.GetRepository<MinistryUser>()
            .ExistsAsync(x => x.Id == userId);

        if (!exists)
            throw new BusinessException(ConstantKeys.ExceptionMessage.UserNotExist);
    }

}
