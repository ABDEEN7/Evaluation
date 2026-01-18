using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Migrations;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
    public async Task<bool> AddedRequestAssignment(List<EvalTeamRequestDto> model)
    {
        if (model == null || !model.Any())
            return true;

        var evaluationRequestId = model.First().EvaluationRequestId;

        var hasAssignments = await
            unitOfWork.GetRepository<EvaluationRequestAssignment>()
            .GetAllActiveNonDeleted()
            .AnyAsync(x =>
            x.EvaluationRequestId == evaluationRequestId);

        if (!hasAssignments)
        {
            await AddAssignments(model);
        }
        else
        {
            await UpdateOrDeleteAssignments(model);
        }
        return true;
    }
    public async Task<Result<List<EvaluationRequestAssignmentDto>>> GetTeamByEvaluationRequestId(Guid evaluationRequestId)
    {
        var team = unitOfWork
            .GetRepository<EvaluationRequestAssignment>()
            .GetAllActiveNonDeleted(x => x.EvaluationRequestId == evaluationRequestId)
            .Include(s => s.EvalRequestAssignmentScopies)
            .ToList();
        var evaluationRequestAssignmentDto = mapper.Map<List<EvaluationRequestAssignmentDto>>(team);
        return evaluationRequestAssignmentDto;
    }
    public async Task<Result<bool>> DeleteEvaluationRequestAssignment(Guid id)
    {
        var assignmentRepo = unitOfWork.GetRepository<EvaluationRequestAssignment>();
        var scopeRepo = unitOfWork.GetRepository<EvalRequestAssignmentScope>();

        var assignment = await assignmentRepo
            .GetAllActiveNonDeleted(
                x => x.Id == id,
                includeProperties: x => x.EvalRequestAssignmentScopies!
            )
            .FirstOrDefaultAsync();

        if (assignment == null)
            return false;
        if (assignment.EvalRequestAssignmentScopies?.Any() == true)
        {
            scopeRepo.DeleteRange(assignment.EvalRequestAssignmentScopies);
        }
        assignmentRepo.Delete(assignment);

        await unitOfWork.CommitAsync();

        return true;
    }
    private async Task AddAssignments(List<EvalTeamRequestDto> model)
    {
        var exsitingAssignments = unitOfWork
            .GetRepository<EvaluationRequestAssignment>()
            .GetAllActiveNonDeleted()
            .Select(x => new
            {
                x.MinistryUserId,
                x.PartyTypeId
            })
            .ToList();

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
            EvaluationRequestId = dto.EvaluationRequestId,
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
    private async Task UpdateOrDeleteAssignments(List<EvalTeamRequestDto> model)
    {
        var evaluationRequestId = model.First().EvaluationRequestId;
        var repo = unitOfWork.GetRepository<EvaluationRequestAssignment>();
        var existingAssignments = await repo
       .GetAllActiveNonDeleted(x => x.EvaluationRequestId == evaluationRequestId)
       .Include(x => x.EvalRequestAssignmentScopies)
       .ToListAsync();

        //Delete missing Assignment
        var toDelete = existingAssignments
            .Where(db => !model.Any(m =>
            m.UserId == db.MinistryUserId &&
            m.PartyTypeId == db.PartyTypeId))
            .ToList();
        if (toDelete.Any())
            repo.DeleteRange(toDelete);
        // Update exsiting 
        foreach (var dto in model)
        {
            var entity = existingAssignments.FirstOrDefault(db =>
              db.MinistryUserId == dto.UserId &&
              db.PartyTypeId == dto.PartyTypeId);
            if (entity == null)
                continue;

            entity.IsLeader = dto.IsLeader;
            entity.Note = dto.Note;
            entity.EvalRequestAssignmentScopies.Clear();
            if (dto.Scopes != null)
            {
                entity.EvalRequestAssignmentScopies = dto.Scopes.Select(s =>
                    new EvalRequestAssignmentScope
                    {
                        ScopeId = s.Id,
                    }).ToList();
            }
            repo.Update(entity);
        }
    }
}
