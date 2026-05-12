using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
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
        IServiceProvider serviceProvider, RequestInfo requestInfo , EvaluationRequestService evaluationRequestService)
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

        // 1. Get all evaluation request assignments for the source user
        var assignments = await GetEvaluationUserAssignments(request.UserId);

        if (assignments == null || !assignments.Any())
            throw new Exception("No evaluation requests found for the selected user.");

        // 2. Filter only the selected requests (if user passed specific IDs), otherwise take all
        var selectedIds = request.EvaluationRequestIds != null && request.EvaluationRequestIds.Any()
            ? assignments.Where(a => request.EvaluationRequestIds.Contains(a.EvaluationRequestId)).ToList()
            : assignments.ToList();

        if (!selectedIds.Any())
            throw new Exception("None of the selected evaluation requests were found for this user.");

        // 3. Reassign each request from UserId → ToUserId
        foreach (var item in selectedIds)
        {
            var existingAssignment = await uow.GetRepository<EvaluationRequestAssignment>()
                .GetAllActiveNonDeleted()
                .FirstOrDefaultAsync(x =>
                    x.EvaluationRequestId == item.EvaluationRequestId &&
                    x.MinistryUserId == request.UserId &&
                    x.PartyTypeId == item.PartyTypeId);

            if (existingAssignment == null) continue;

            // Update the assigned user to the new user
            existingAssignment.MinistryUserId = request.ToUserId;
            uow.GetRepository<EvaluationRequestAssignment>().Update(existingAssignment);
        }

        await uow.CommitAsync();
    }

    public async Task<IReadOnlyList<ReassignRequestTableDto>> GetEvaluationUserAssignments(Guid userId)
    {
        var response = await evaluationRequestService.GetUserAssignments(userId);
        return response;
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
