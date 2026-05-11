using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.TeamMemberDto.ReassignDto;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.ReassignLayer;

public class ReassignBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
            UserService userService,
        IServiceProvider serviceProvider, RequestInfo requestInfo)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{

    public async Task<Dictionary<string, object>> GetReAssignedDropList(Guid? userId)
    {
        Dictionary<string, object> response = new Dictionary<string, object>();
        var fromUserList = await GetReassignUser();
        var toUserList = await GetReassignUser(userId);

        response.Add("FormUserList", fromUserList);
        response.Add("ToUserList", toUserList);
        return response;
    }
    public async Task UpdateReassignEvaluationRequestUser(RequestReassignDto request)
    {
        await userService.EnsureUserExists(request.UserId);
        await userService.EnsureUserExists(request.ToUserId);
        
    }
    private async Task<IReadOnlyList<DropdownItem>> GetReassignUser(Guid? selectedUserId = null)
    {
        var usersQuery = userService.GetUsers();

        if (selectedUserId.HasValue)
        {
            usersQuery = usersQuery.Where(x => x.Id != selectedUserId.Value);
        }

        return await usersQuery
            .Select(x => new DropdownItem
            {
                Id = x.Id,
                Name = requestInfo.Lang == "ar"
                    ? x.NameAr
                    : x.NameEn
            })
            .ToListAsync();
    }

}
