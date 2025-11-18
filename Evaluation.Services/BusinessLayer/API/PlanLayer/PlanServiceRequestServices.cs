using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.Planing;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
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
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo,
    PlanRequestRepository planRepository
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo,
        serviceProvider, requestInfo)
{
    public async Task<Result<CreatePlanResponse>> AddEvaulationPlan(CreateEvaluationPlanDto evaluationPlanDto)
    {
        //await ValidateDraftPlan(evaluationPlanDto);
        return await ExecuteWithResult(async () =>
        {
            //PlanServiceRequest planDraft = evaluationPlanDto.Adapt<PlanServiceRequest>();
            //var result = await f.CreateServicPlan(evaluationPlanDto);

            var result = evaluationPlanDto.ConvertFromRequestToResponse(evaluationPlanDto);
            result.CreateDate = DateTime.Now;
            result.AcademicYearId = new Guid("00066600-9999-0000-7777-000000000001");
            result.IsActive = true;
            result.CreateById = new Guid("11111111-1111-1111-1111-000000000069");
            return result;
        });
    }
    public async Task<bool> UpdatePlanAsync(Guid id, UpdatePlanDto planDto)
    {
        // 1. Fetch existing plan
        var plan = await planRepository.GetPlanAsync(id);

        if (plan is null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.PlanIsNotFound);

        // 2. Save old version as JSON (audit or history)
        planDto.PlanJsonValue = JsonConvert.SerializeObject(plan);

        // 3. Validate input
        await ValidateUpdatePlan(planDto);

        // 4. Update the existing entity instead of replacing it
        UpdatePlanEntity(plan, planDto);
        // 5. Save to DB
        return await planRepository.UpdatePlanAsync(plan);
    }
    public async Task<PlanDto> GetPlanByIdAsync(Guid id)
    {
        var result = await planRepository.GetPlanAsync(id);
        var planDto = PlanDto.FromEntity(result);
        return planDto;
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
        //await ValidateApprovePlan(modelDto);

        return await ExecuteWithResult(async () =>
        {
            Plan plan = modelDto.Adapt<Plan>();
            var result = await planRepository.ApprovePlans(plan);
        });
    }

    //Plan Type Module
    public async Task<List<PlanTypeDto>> GetPlanTypes()
    {
        var result = await planRepository.GetPlanTypeAsync();
        return result;
    }
    //Validation Plans
    private async Task ValidateUpdatePlan(UpdatePlanDto model)
    {
        if (model is null || string.IsNullOrEmpty(model.Name))
            throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidDraftPlan);

        var selectedYear = await serviceScopeFactory.CreateScopedUow().GetRepository<AcademicYear>()
           .GetAllActiveNonDeleted()
           .FirstOrDefaultAsync(x => x.Id == model.AcademicYearId);

        if (selectedYear?.Year < DateTime.Now.Year)
            throw new BusinessException(ConstantKeys.ExceptionMessage.PlanInThePastIsNotAllowed);
    }
    private void UpdatePlanEntity(Plan plan, UpdatePlanDto dto)
    {
        plan.PlanName = dto.Name;
        plan.StartDate = dto.StartDate;
        plan.EndDate = dto.EndDate;
        plan.DepartmentId = dto.DepartmentId;
        plan.AcademicYearId = dto.AcademicYearId;
        plan.PlanStatusId = dto.PlanStatusId;
        plan.PlanTypeDepartmentId = dto.PlanTypeDepartmentId;
        plan.SemesterId = dto.SemesterId;
        plan.PlanJsonValue = dto.PlanJsonValue;
    }

}
