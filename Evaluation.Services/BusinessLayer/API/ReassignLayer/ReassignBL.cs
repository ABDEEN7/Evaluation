using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.TeamMemberDto.ReassignDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.ReassignLayer;

public class ReassignBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
            UserService userService,
        IServiceProvider serviceProvider, RequestInfo requestInfo, EvaluationRequestService evaluationRequestService)
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
    public async Task<ResponseDto> UpdateReassignEvaluationRequestUser(
       RequestReassignDto request)
    {
        try
        {
            await userService.EnsureUserExists(request.UserId);

            await userService.EnsureUserExists(request.ToUserId);

            if (request.EvaluationRequestIds == null ||
                !request.EvaluationRequestIds.Any())
            {
                return new ResponseDto
                {
                    ResponseStatus = DBResult.Error,
                    ResponseState = false,
                    ResponseMessage =
                        ConstantKeys.ExceptionMessage
                            .NoEvaluationRequestsWereSelected
                };
            }

            var assignmentRepository =
                uow.GetRepository<EvaluationRequestAssignment>();

            var scopeRepository =
                uow.GetRepository<EvalRequestAssignmentScope>();

            // Get ONLY selected assignments
            var assignments = await assignmentRepository
                .GetAllActiveNonDeleted(
                    x =>
                        x.MinistryUserId == request.UserId &&
                        request.EvaluationRequestIds
                            .Contains(x.EvaluationRequestId),

                    includeProperties:
                        x => x.EvalRequestAssignmentScopies!)
                .ToListAsync();

            if (!assignments.Any())
            {
                return new ResponseDto
                {
                    ResponseStatus = DBResult.Error,
                    ResponseState = false,
                    ResponseMessage =
                        ConstantKeys.ExceptionMessage
                            .NoneOfTheSelectedEvaluationRequestsWereFoundForThisUser
                };
            }

            foreach (var oldAssignment in assignments)
            {
                // Create new assignment
                var newAssignment = new EvaluationRequestAssignment
                {
                    MinistryUserId = request.ToUserId,
                    EvaluationRequestId =
                        oldAssignment.EvaluationRequestId,
                    PartyTypeId = oldAssignment.PartyTypeId,
                    IsLeader = oldAssignment.IsLeader,
                    Note = oldAssignment.Note,
                };

                assignmentRepository.Insert(newAssignment);

                // Copy scopes
                if (oldAssignment.EvalRequestAssignmentScopies != null)
                {
                    foreach (var oldScope
                             in oldAssignment.EvalRequestAssignmentScopies)
                    {
                        var newScope = new EvalRequestAssignmentScope
                        {
                            EvaluationRequestAssignmentId =
                                newAssignment.Id,

                            ScopeId = oldScope.ScopeId,
                            Note = oldScope.Note,
                        };
                        scopeRepository.Insert(newScope);
                    }
                    // Delete old scopes
                    scopeRepository.DeleteRange(
                        oldAssignment.EvalRequestAssignmentScopies);
                }
                // Delete old assignment
                assignmentRepository.Delete(oldAssignment);
            }
            await uow.CommitAsync();
            return new ResponseDto
            {
                ResponseStatus = DBResult.Inserted,
                ResponseState = true
            };
        }
        catch (Exception)
        {
            return new ResponseDto
            {
                ResponseStatus = DBResult.Error,
                ResponseState = false,
                ResponseMessage =
                    ConstantKeys.ExceptionMessage.UnExpectedException
            };
        }
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
