using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static Evaluation.SharedHelper.Enums.ConstantKeys;
namespace Evaluation.Services.BusinessLayer.API.PlanLayer;

public class PlanRequestRepository(IServiceScopeFactory serviceScopeFactory,
    UnitOfWork unitOfWork,
    RequestInfo requestInfo
    ) : ApiServiceBase
{
    public async Task<Plan?> GetPlanAsync(Guid id)
        => await unitOfWork.GetRepository<Plan>().GetByIDActiveNonDeleted(id);

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
    return  true;
}
    public async Task<Plan> ApprovePlansAsync(Plan model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        await unitOfWork.GetRepository<Plan>().InsertAsync(model);
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
    public IQueryable<PlanType> GetPlanType()
    {
        return  serviceScopeFactory.CreateScopedUow().GetRepository<PlanType>()
            .GetAllActiveNonDeleted();            
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
    public async Task<List<Plan>> GetPlans()
    {
        var model = await unitOfWork
            .GetRepository<Plan>()
                .GetAllActiveNonDeleted().ToListAsync();
        return model;
    }
}
