using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Evaluation.SharedHelper.Extensions;
using System.Xml.Serialization;

namespace Evaluation.Services.BusinessLayer.API.TeamMemberBL;

public class AssignmentBL(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo,
    AssignmentService teamMemberService
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo,
        serviceProvider, requestInfo)
{
    //public async Task<List<Team>> GetTeamsMembers()
    //{
    //    var departmentId = await unitOfWork.GetRepository<Department>()
    //                            .GetAllActiveNonDeleted(d => d.UserDepartments.Any(ud => ud.UserId == userInfo.UserId))
    //                            .OrderByDescending(d => d.UserDepartments
    //                        .Where(ud => ud.UserId == userInfo.UserId)
    //                        .Max(ud => ud.CreateDate)).Select(x => x.Id).FirstOrDefaultAsync();
    //    var teams = await unitOfWork
    //                    .GetRepository<Team>()
    //                    .GetAllActiveNonDeleted(x => x.DepartmentId == departmentId)
    //                    .Include(x=>x.UserTeams)
    //                    .ToListAsync();
    //    return teams;
    //}
    public async Task<Result<AssignmentsResponse>> GetTeamsAsync()
    {
        var team = await teamMemberService.GetTeamAsync();
        bool isNdaActive = await
          unitOfWork
          .GetRepository<Department>()
          .GetAllActiveNonDeleted(d => d.IsNDA && d.UserDepartments.Any(ud => ud.UserId == userInfo.UserId))
          .OrderByDescending(x => x.CreateDate)
          .AnyAsync();
        var teamRespons = mapper.Map<List<TeamDto>>(team);
        AssignmentsResponse teamMembers = new AssignmentsResponse
        {
            Data = teamRespons,
            IsNDA = isNdaActive
        };
        return teamMembers;
    }
    public async Task<Result<List<AssignmentDto>>> GetMembersByTeamId(Guid? teamId)
    {
        var member = unitOfWork.GetRepository<MinistryUser>()
               .GetAllActiveNonDeleted();
        if (teamId != null)
            member = member.Where(x => x.UserTeams!.Any(t => t.TeamId == teamId));

        var memberWithParty = await member
            .Include(x => x.UserPartTypes!)
            .ThenInclude(w => w.PartyType).ToListAsync();
        var memberTeamDto = mapper.Map<List<AssignmentDto>>(memberWithParty);
        return memberTeamDto;
    }
    public async Task<List<ScopeDto>> GetScopesAsync()
    {
        var scopes = await teamMemberService.GetScopes();
        var scopesDto = mapper.Map<List<ScopeDto>>(scopes);
        return scopesDto;
    }
    public async Task<bool> AddedRequestAssignment(Guid evaluationRequestId, List<EvalTeamRequestDto> model)
    {
        if (model == null || !model.Any())
            return true;


        var hasAssignments = await
            unitOfWork.GetRepository<EvaluationRequestAssignment>()
            .GetAllActiveNonDeleted()
            .AnyAsync(x =>
            x.EvaluationRequestId == evaluationRequestId);

        if (!hasAssignments)
        {
            await AddAssignments(evaluationRequestId, model);
        }
        else
        {
            await UpdateOrDeleteAssignments(evaluationRequestId, model);
        }
        await unitOfWork.CommitAsync();
        return true;
    }
    public async Task<Result<List<EvaluationRequestAssignmentDto>>> GetTeamByEvaluationRequestId(Guid evaluationRequestId)
    {
        var team = unitOfWork
            .GetRepository<EvaluationRequestAssignment>()
            .GetAllQueryFiltered(x => x.EvaluationRequestId == evaluationRequestId)
            .Include(x => x.EvalRequestAssignmentScopies)
            .ToList();
        var evaluationRequestAssignmentDto = mapper.Map<List<EvaluationRequestAssignmentDto>>(team);
        return evaluationRequestAssignmentDto;
    }
    private async Task AddAssignments(Guid evaluationRequestId, List<EvalTeamRequestDto> model)
    {
        var exsitingAssignments = await unitOfWork
            .GetRepository<EvaluationRequestAssignment>()
            .GetAllActiveNonDeleted()
            .Where(x => x.EvaluationRequestId == evaluationRequestId)
            .Select(x => new
            {
                x.MinistryUserId,
                x.PartyTypeId
            })
            .ToListAsync();

        var newAssignments = model
            .Where(m => !exsitingAssignments.Any(db =>
            db.MinistryUserId == m.UserId &&
            db.PartyTypeId == m.PartyTypeId))
            .ToList();

        if (!newAssignments.Any())
            return;
        var evaluationRequestAssignments = newAssignments.Select(dto =>
        new EvaluationRequestAssignment
        {
            MinistryUserId = dto.UserId,
            EvaluationRequestId = evaluationRequestId,
            PartyTypeId = dto.PartyTypeId,
            IsLeader = dto.IsLeader,
            Note = dto.Note,
            EvalRequestAssignmentScopies = dto.Scopes?.Select(scope =>
                new EvalRequestAssignmentScope
                {
                    ScopeId = scope.Id
                }).ToList()
        }).ToList();
        await unitOfWork
            .GetRepository<EvaluationRequestAssignment>()
            .InsertRange(evaluationRequestAssignments);
    }
    private async Task UpdateOrDeleteAssignments(Guid evaluationRequestId, List<EvalTeamRequestDto> model)
    {
        var repo = unitOfWork.GetRepository<EvaluationRequestAssignment>();

        var existingAssignments = await repo
            .GetAllActiveNonDeleted(x => x.EvaluationRequestId == evaluationRequestId)
            .Include(x => x.EvalRequestAssignmentScopies)
            .ToListAsync();

        // 1️⃣ حدد المحذوفين
        var assignmentsToDelete = existingAssignments
            .Where(db => !model.Any(m =>
                m.UserId == db.MinistryUserId))
            .ToList();

        if (assignmentsToDelete.Any())
            repo.DeleteRange(assignmentsToDelete);

        // 2️⃣ حدّث فقط الموجودين
        var assignmentsToUpdate = existingAssignments
            .Except(assignmentsToDelete)
            .ToList();

        foreach (var dto in model)
        {
            var entity = assignmentsToUpdate.FirstOrDefault(db =>
                db.MinistryUserId == dto.UserId &&
                db.PartyTypeId == dto.PartyTypeId);

            if (entity == null)
                continue;

            entity.IsLeader = dto.IsLeader;
            entity.Note = dto.Note;

            if (dto.Scopes == null)
                throw new BusinessException(
                    ConstantKeys.ExceptionMessage.ScopeNotExistsFormAssignment);

            SyncAssignmentScopes(entity, dto.Scopes);
        }

        // 3️⃣ أضف الجديد
        var newAssignments = model
            .Where(m => !existingAssignments.Any(db =>
                db.MinistryUserId == m.UserId &&
                db.PartyTypeId == m.PartyTypeId))
            .Select(dto => new EvaluationRequestAssignment
            {
                MinistryUserId = dto.UserId,
                EvaluationRequestId = evaluationRequestId,
                PartyTypeId = dto.PartyTypeId,
                IsLeader = dto.IsLeader,
                Note = dto.Note,
                EvalRequestAssignmentScopies = dto.Scopes.Select(s =>
                    new EvalRequestAssignmentScope
                    {
                        ScopeId = s.Id
                    }).ToList()
            })
            .ToList();

        if (newAssignments.Any())
            await repo.InsertRange(newAssignments);
    }

    private void SyncAssignmentScopes(EvaluationRequestAssignment entity, List<EvalScopesDto> incomingScopes)
    {
        var scopeRepo = unitOfWork.GetRepository<EvalRequestAssignmentScope>();

        var existingScopes = entity.EvalRequestAssignmentScopies.ToList();

        var incomingIds = incomingScopes.Select(s => s.Id).ToHashSet();
        var existingIds = existingScopes.Select(es => es.ScopeId).ToHashSet();

        // Soft Delete
        foreach (var scope in existingScopes
            .Where(es => !incomingIds.Contains(es.ScopeId)))
        {
            scope.IsDeleted = true;
            scopeRepo.Update(scope);
        }

        // Add new
        var scopesToAdd = incomingIds.Except(existingIds)
        .Select(scopeId => new EvalRequestAssignmentScope
        {
            ScopeId = scopeId,
            EvaluationRequestAssignmentId = entity.Id
        })
        .ToList();
        if (scopesToAdd.Any())
            scopeRepo.InsertRange(scopesToAdd);

        //foreach (var scopeId in incomingIds.Except(existingIds))
        //{
        //    entity.EvalRequestAssignmentScopies.Add(
        //        new EvalRequestAssignmentScope
        //        {
        //            ScopeId = scopeId,
        //            EvaluationRequestAssignmentId = entity.Id
        //        });
        //}
    }
}
