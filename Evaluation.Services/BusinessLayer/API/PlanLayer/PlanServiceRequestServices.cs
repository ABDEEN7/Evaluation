using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.AcademicYearLayer;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
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
using static Evaluation.SharedHelper.Enums.ConstantKeys;

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
    PlanRequestRepository planRepository,
    AcademicYearRepository academicYearRepository,
    DepartmentService departmentService
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
            var academicYearIdForCurrentUser = unitOfWork
                 .GetRepository<Department>()
                 .GetAllActiveNonDeleted(d => d.TargetOrgTreeId == userInfo.UserId)
                 .SelectMany(d => d.AcademicYears)
                 .OrderByDescending(a => a.CreateDate)
                 .Select(a => a.Id)
                 .FirstOrDefault();

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
    public async Task<Result<CreateEvaluationPlanDto>> InsertOrUpdatePlan(CreateEvaluationPlanDto? modelDto)
    {
        //await ValidateApprovePlan(modelDto);

        return await ExecuteWithResult(async () =>
        {
            // Get department id
            Guid? departmentId = await departmentService.GetDepartmentIdAsync();
            // Get academic year id
            Guid academicYearId = await academicYearRepository.GetAcademicYearId(departmentId);
            // Get status 'Approved'
            Guid statusId = await
            unitOfWork
            .GetRepository<PlanStatus>()
            .GetAllActiveNonDeleted(x => x.BackendName == "Approved")
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

            // Assign system-generated values
            modelDto.PlanStatusId = statusId;
            modelDto.AcademicYearId = academicYearId;

            // Convert DTO to entity
            Plan plan = modelDto.ToPlan();
            plan.PlanJsonValue = JsonConvert.SerializeObject(modelDto);
            if (plan.Id != null)
            {
                var result = await planRepository.InsertPlan(plan);
            }
        });
    }

    //Plan Type Module
    public async Task<List<PlanTypeDto>> GetPlanTypes()
    {
        var departmentId = await departmentService.GetDepartmentIdAsync();
        return await planRepository.GetPlanType().Where(x => x.DepartmentId == departmentId).Select(s => new PlanTypeDto
        {
            Id = s.Id,
            Name = requestInfo.Lang == LanguageConst.Ar ? s.NameAr : s.NameEn,
            BackendName = s.PlanType.BackendName,
        }).ToListAsync();
    }
    public async Task<PlanDto> GetPlanByIdAsyncAutoMapper(Guid planId)
    {
        Plan? plan = await planRepository.GetPlanAsync(planId);
        return plan.Adapt<PlanDto>();
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
    //private Plan ConvertFromPlanToDto(CreateEvaluationPlanDto model)
    //{
    //    return new Plan
    //    {
    //        Id = model.Id,
    //        AcademicYearId = model.AcademicYearId,
    //        PlanName = model.Name,
    //        PlanJsonValue = JsonConvert.SerializeObject(model),
    //        PlanStatusId = model.PlanStatusId,
    //        //PlanTypeDepartmentId = 

    //    };
    //}
    private void UpdatePlanEntity(Plan plan, UpdatePlanDto dto)
    {
        plan.PlanName = dto.Name;
        plan.StartDate = dto.StartDate;
        plan.EndDate = dto.EndDate;
        plan.AcademicYearId = dto.AcademicYearId;
        plan.PlanStatusId = dto.PlanStatusId;
        plan.PlanTypeDepId = dto.PlanTypeDepartmentId;
        plan.SemesterId = dto.SemesterId;
        plan.PlanJsonValue = dto.PlanJsonValue;
    }

}
