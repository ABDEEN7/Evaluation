using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.ReassignLayer;

public class ReassignService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{

    public async Task<IReadOnlyList<DropdownItem>> GetReassignUser(Guid? selectedUserId)
    {
        var users = uow.GetRepository<MinistryUser>()
            .GetAllActiveNonDeleted();

        if (selectedUserId.HasValue)
        {
            users = users.Where(x => x.Id != selectedUserId);
        }

        return await users
            .Select(x => new DropdownItem
            {
                Id = x.Id,
                Name = requestInfo.Lang == "ar" ? x.NameAr : x.NameEn
            })
            .AsNoTracking()
            .ToListAsync();
    }
}
