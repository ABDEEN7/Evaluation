using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spire.Doc.AI.Model;
using static Evaluation.SharedHelper.Enums.ConstantKeys;
namespace Evaluation.Services.BusinessLayer.API.PlanLayer;

public class PlanRequestRepository(IServiceScopeFactory serviceScopeFactory,
    UnitOfWork unitOfWork,
    RequestInfo requestInfo
    ) : ApiServiceBase
{
    public async Task<Plan?> GetPlanAsync(Guid id)
        => await unitOfWork.GetRepository<Plan>().GetByIDActiveNonDeleted(id);
    public async Task<Plan?> GetPlanDetailsAsync(Guid id)
        => await unitOfWork.GetRepository<Plan>().GetAllActiveNonDeleted()
        .Include(x => x.EvaluationRequests)
        .FirstOrDefaultAsync(x => x.Id == id);


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
            .GetAllActiveNonDeleted(x => x.PlanStatus.BackendName == StatusBackEnds.ApprovedPlans);
        //if (request.StatusId != null)
        //{
        //    plans = plans.Where(x => x.PlanStatusId == request.StatusId);
        //}
        if (request.YearId != null)
        {
            plans = plans.Where(x => x.AcademicYearId == request.YearId);
        }
        if (!string.IsNullOrEmpty(request.SchoolName))
        {
            plans = plans.Where(x => x.EvaluationRequests.Any(er => er.OrgTree.NameAr.Contains(request.SchoolName)));
        }
        var query = plans
            .Select(x => new PlanListDto
            {
                Id = x.Id,
                Name = x.PlanName,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                StatusCode = x.PlanStatus.BackendName,
                CountSchools = x.EvaluationRequests
                .Select(er => er.OrgTreeId)
                .Distinct()
                .Count()
            });
        return await query.GetPaginatedResult(request.PageNumber, request.PageSize = 10);
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