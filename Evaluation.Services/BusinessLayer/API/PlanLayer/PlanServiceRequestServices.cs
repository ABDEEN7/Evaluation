using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.Planing;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Evaluation.Services.BusinessLayer.API.PlanLayer;

public class PlanServiceRequestServices(
    IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo,
    PlanServiceRequestRepository planRepository
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, userInfo,
        serviceProvider, requestInfo)
{
    public async Task<Result<CreateEvaluationPlanDto>> AddEvaulationPlan(CreateEvaluationPlanDto evaluationPlanDto)
    {
        await ValidateDraftPlan(evaluationPlanDto);
        return await ExecuteWithResult(async () =>
        {
            //PlanServiceRequest planDraft = evaluationPlanDto.Adapt<PlanServiceRequest>();
            //var result = await planRepository.CreateServicPlan(evaluationPlanDto);
            var result = evaluationPlanDto;
            return result;
        });
    }
    public async Task<Result<PlanServiceRequest>> UpdatePlanDraft(Guid id, string planDto)
    {
        return await ExecuteWithResult(async () =>
        {
            PlanServiceRequest? oldPlan = await unitOfWork.GetRepository<PlanServiceRequest>().GetByIdAsync(id);

            if (oldPlan is null)
                throw new BusinessException(ConstantKeys.ExceptionMessage.PlanIsNotFound);

            string planJson = oldPlan.Value;
            var currentPlan = JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(planJson);

            //if (currentPlan == null)
            //    return Result<bool>.Failure("Failed to deserialize the existing plan.");
            var dto = JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(planDto);
            await ValidateDraftPlan(dto);
            currentPlan = dto.Adapt<CreateEvaluationPlanDto>();
            var updatedPlanJson = JsonConvert.SerializeObject(currentPlan);
            oldPlan.Value = updatedPlanJson;
            //unitOfWork.GetRepository<PlanServiceRequest>().Update(oldPlan);
            PlanServiceRequest model = new PlanServiceRequest { Id = id, Value = planDto };
            return model;
        });
    }
    public async Task<Result<bool>> DeletePlanDraft(Guid id)
    {
        return await ExecuteWithResult(async () =>
        {
            bool result = await planRepository.DeleteEvaluationPlan(id);
            return result;
        });
    }
    public async Task<Result<CreateEvaluationPlanDto>> ApprovePlan(ApproveEvaluationPlanDto? modelDto)
    {
        await ValidateApprovePlan(modelDto);

        return await ExecuteWithResult(async () =>
        {
            Plan plan = modelDto.Adapt<Plan>();
            var result = await planRepository.ApprovePlans(plan.Id);
        });
    }
    public async Task<Result<List<PlanTypeDto>>> GetPlanType()
    {
        var result = await ExecuteWithResult(async () => await planRepository.GetPlanTypeAsync());
        return result;
    }

    //public async Task<bool> ApproveDeleteSchool(Guid requestId)
    //{
    //    return await ExecuteWithResult(async () =>
    //    {
    //        var result = await planService.ApprovePlans(requestId);
    //    });
    //}
    private async Task ValidateApprovePlan(CreateEvaluationPlanDto model)
    {
        if (model is null || string.IsNullOrEmpty(model.NameEn) || string.IsNullOrEmpty(model.NameAr))
            throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidApprovePlan);

        var selectedYear = await serviceScopeFactory.CreateScopedUow().GetRepository<AcademicYear>()
           .GetAllActiveNonDeleted()
           .FirstOrDefaultAsync(x => x.Id == model.AcademicYearId);

        if (selectedYear?.Year < DateTime.Now.Year)
            throw new BusinessException(ConstantKeys.ExceptionMessage.PlanInThePastIsNotAllowed);
        //if(model.PlanStatusId)
    }
    private async Task ValidateDraftPlan(CreateEvaluationPlanDto model)
    {
        if (model is null || string.IsNullOrEmpty(model.NameEn) || string.IsNullOrEmpty(model.NameAr))
            throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidDraftPlan);

        var selectedYear = await serviceScopeFactory.CreateScopedUow().GetRepository<AcademicYear>()
           .GetAllActiveNonDeleted()
           .FirstOrDefaultAsync(x => x.Id == model.AcademicYearId);

        if (selectedYear?.Year < DateTime.Now.Year)
            throw new BusinessException(ConstantKeys.ExceptionMessage.PlanInThePastIsNotAllowed);
    }

}
