using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Dtos.PlanDto.EditDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
namespace Evaluation.Services.BusinessLayer.API.PlanLayer;

public class PlanRequestRepository(IServiceScopeFactory serviceScopeFactory, SrvServiceRequest _srvServiceRequest,
    UnitOfWork unitOfWork,
    RequestInfo requestInfo
    ) : ApiServiceBase
{
    public async Task<Plan?> GetPlanAsync(Guid id)
        => await unitOfWork.GetRepository<Plan>().GetByIDActiveNonDeleted(id);
    public async Task<Plan?> GetPlanDetailsAsync(Guid id)
        => await unitOfWork.GetRepository<Plan>().GetAllQueryFiltered()
        .Include(x => x.EvaluationRequests)
        .ThenInclude(x => x.OrgTree)
        .FirstOrDefaultAsync(x => x.Id == id);

    public async Task GetPlanEvaluationWithSchools(Guid planId)
    {
        var planQuery =
            unitOfWork
            .GetRepository<Plan>()
            .GetAllActiveNonDeleted(
                filter: p => p.Id == planId,
                includeProperties:
                        p => p.EvaluationRequests!
                );
        var schoolsQuery = unitOfWork
            .GetRepository<School>()
            .GetAllActiveNonDeleted();

        var plan = await planQuery
            .Select(p => new PlanEditDto
            {
                Id = p.Id,
                PlanName = p.PlanName,
                PlanTypeDepId = p.PlanTypeDepId,
                StartDate = p.StartDate,
                EndDate = p.EndDate,

                Schools = schoolsQuery.Select(s => new SchoolEvaluationEditDto
                {
                    SchoolId = s.Id,
                    SchoolName = s.NameEn, // or NameAr by culture

                    IsSelected = p.EvaluationRequests!
                        .Any(er => er.OrgTreeId == s.Id),

                    DepEvaluationTypeId = p.EvaluationRequests!
                        .Where(er => er.OrgTreeId == s.Id)
                        .Select(er => (Guid?)er.DepEvaluationTypeId)
                        .FirstOrDefault(),

                    FromDate = p.EvaluationRequests!
                        .Where(er => er.OrgTreeId == s.Id)
                        .Select(er => (DateTime?)er.FromDate)
                        .FirstOrDefault(),

                    ToDate = p.EvaluationRequests!
                        .Where(er => er.OrgTreeId == s.Id)
                        .Select(er => (DateTime?)er.ToDate)
                        .FirstOrDefault()
                }).ToList()
            })
    .FirstOrDefaultAsync();

    }
    public async Task<PlanWithSchoolsDto> GetPlanWithSchoolsDetailsAsync(Guid id)
    {
        var planRepo = unitOfWork.GetRepository<Plan>();
        var schoolRepo = unitOfWork.GetRepository<School>();

        // Query 1: Get the plan with evaluation requests
        var plan = await planRepo
            .GetAllActiveNonDeleted(p => p.Id == id && p.EvaluationRequests.Any())
            .Include(p => p.EvaluationRequests)
            .FirstOrDefaultAsync();

        if (plan == null)
            return null;

        // Query 2: Get all schools
        var schools = await schoolRepo
            .GetAllActiveNonDeleted()
            .ToListAsync();

        // Map in memory (fast)
        return new PlanWithSchoolsDto
        {
            Id = plan.Id,
            Name = plan.PlanName,
            startDate = plan.StartDate,
            endDate = plan.EndDate,
            PlanTypeId = plan.PlanTypeDepId,
            Schools = schools.Select(s =>
            {
                var evalRequest = plan.EvaluationRequests
                    .FirstOrDefault(er => er.OrgTreeId == s.Id);

                return new SchoolEvaluationDto
                {
                    Id = s.Id,
                    Name = s.NameEn,
                    HasEvaluationRequest = evalRequest != null,
                    EvaluationRequestId = evalRequest?.Id,
                    VisitTypeId = evalRequest?.DepEvaluationTypeId,
                    FromDate = evalRequest?.ToDate,
                    ToDate = evalRequest?.ToDate
                };
            }).ToList()
        };
    }

    public async Task<string> CreateServicPlan(PlanServiceRequest model)
    {
        if (await IsThereExistingDraftPlanForSameAcadmicYear(model))
            throw new BusinessException(ConstantKeys.ExceptionMessage.DraftPlanWithSameAcademicYearAlreadyExists);
        var planValue = JsonConvert.SerializeObject(model.Value);
        return planValue;
        //await unitOfWork.GetRepository<PlanServiceRequest>().InsertAsync(model);
        //await unitOfWork.CommitAsync();
        //return (await unitOfWork.GetRepository<PlanServiceRequest>().GetByIdAsync(model.Id));
    }
    public async Task<bool> UpdatePlanAsync(Plan plan)
    {
        var repo = unitOfWork.GetRepository<Plan>();
        repo.Update(plan);
        await unitOfWork.CommitAsync();
        return true;
    }

    public async Task InsertAsync(Plan plan)
    {
        if (plan == null)
            throw new ArgumentNullException(nameof(plan));

        await unitOfWork.GetRepository<Plan>().InsertAsync(plan);
    }

    public async Task<Plan> UpdatePlan(Plan model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        //Get old Plan
        var oldPlan = unitOfWork.GetRepository<Plan>().GetAllActiveNonDeleted(x => x.Id == model.Id);
        oldPlan.Adapt(model);
        //Insert Plan
        unitOfWork.GetRepository<Plan>().Update(model);
        // Link EvaluationRequest to the new Plan

        //await unitOfWork.SaveChangesAsync()
        await unitOfWork.CommitAsync();

        return model;
    }
    public async Task<bool> DeleteEvaluationPlan(Guid? id)
    {
        if (id is null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
        PlanServiceRequest? evaluationPlan =
            await unitOfWork.GetRepository<PlanServiceRequest>().GetByIdAsync(id);
        if (evaluationPlan is null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.PlanIsNotFound);
        unitOfWork.GetRepository<PlanServiceRequest>().Delete(evaluationPlan);
        await unitOfWork.CommitAsync();
        return true;
    }
    public async Task<bool> DeletePlan(Guid? id)
    {
        if (id is null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
        var model = await unitOfWork.GetRepository<Plan>()
            .GetAllActiveNonDeleted(x => x.Id == id)
            .Include(x => x.EvaluationRequests)
            .FirstOrDefaultAsync();
        if (model == null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.UserPartyTypeSignatureHeightError);
        if (false)
            throw new BusinessException(ConstantKeys.ExceptionMessage.UserPartyTypeSignatureHeightError);
        unitOfWork.GetRepository<Plan>().Delete(model);
        unitOfWork.GetRepository<EvaluationRequest>().DeleteRange(model.EvaluationRequests);
        await unitOfWork.CommitAsync();
        return true;
    }

    //public async Task<bool> DeleteSchoolFromPlan(Guid requestId, Guid schoolId)
    //{
    //    var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(changeRequest.PlanId);
    //    var schoolPlan = await unitOfWork.GetRepository<EvaluationRequest>()
    //        .GetAllActiveNonDeleted().FirstOrDefaultAsync(x => x.PlanId == plan.Id && x.OrgTreeId == schoolId);
    //    unitOfWork.GetRepository<EvaluationRequest>().Delete(schoolPlan);
    //    await unitOfWork.CommitAsync();
    //    return true;
    //}
    public IQueryable<PlanTypeDep> GetPlanType()
    {
        return serviceScopeFactory.CreateScopedUow()
            .GetRepository<PlanTypeDep>()
            .GetAllActiveNonDeleted();
    }
    public async Task<PaginatedResult<PlanListDto>> GetPlans(PlanDetailsRequestDto request)
    {
        IQueryable<Plan> plans = unitOfWork.GetRepository<Plan>()
            .GetAllQueryFiltered(x => x.PlanStatus.BackendName == StatusBackEnds.ApprovedPlans && x.PlanTypeDep.DepartmentId == requestInfo.DepId);

        if (request.YearId != null)
            plans = plans.Where(x => x.AcademicYearId == request.YearId);

        if (!string.IsNullOrEmpty(request.SchoolName))
            plans = plans.Where(x => x.EvaluationRequests.Any(er => er.OrgTree.NameAr.Contains(request.SchoolName)));

        var query = plans
            .Select(x => new PlanListDto
            {
                Id = x.Id,
                Name = x.PlanName,
                StartDate = x.StartDate,
                EndDate = x.EndDate,

                PlanStatusId = x.PlanStatusId.Value,
                StatusCode = x.PlanStatus.BackendName,

                CountSchools = x.EvaluationRequests
                    .Select(er => er.OrgTreeId)
                    .Distinct()
                    .Count()
            })
            .OrderByDescending(x => x.StartDate);

        var finalResult = await query.GetPaginatedResult(request.PageNumber, request.PageSize = 10);

        if (finalResult.Items.Any())
        {
            var statusIds = finalResult.Items
                .Select(x => x.PlanStatusId)
                .Distinct()
                .ToList();

            var servicesByStatusTask = _srvServiceRequest.GetServicesByStatusesAsync(statusIds, ConstantKeys.ModuleTypeIds.EvaluationPlan, requestInfo.Lang);

            await Task.WhenAll(servicesByStatusTask);

            var servicesByStatus = servicesByStatusTask.Result;

            foreach (var item in finalResult.Items)
            {
                if (servicesByStatus.TryGetValue(item.PlanStatusId, out var services))
                {
                    item.Services = services;
                }
            }
        }

        return finalResult;
    }
    public async Task<List<GetPlansPR>> GetPlans()
    {
        var result = await unitOfWork.GetRepository<Plan>()
              .GetAllActiveNonDeleted(x => x.PlanTypeDep!.DepartmentId == requestInfo.DepId)
              .Select(x => new GetPlansPR
              {
                  Id = x.Id,
                  PlanName = x.PlanName
              }).ToListAsync();
        return result;
    }
    private async Task<bool> IsThereExistingDraftPlanForSameAcadmicYear(PlanServiceRequest model)
    {
        return await
            serviceScopeFactory
            .CreateScopedUow()
            .GetRepository<PlanServiceRequest>()
            .GetAllActiveNonDeleted()
            .AnyAsync(x => (x.AcademicYear.DepartmentId == model.AcademicYear.DepartmentId) && x.HasOnePlan);
    }

}