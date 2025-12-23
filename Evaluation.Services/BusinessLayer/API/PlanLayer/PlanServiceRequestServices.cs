using Aspose.Words.Drawing;
using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.Template;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.AcademicYearLayer;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Dtos.PlanDto.EditDto;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Mapster;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<PlanDetailsDto> GetPlanByIdAsync(Guid id)
    {
        var result = await planRepository.GetPlanDetailsAsync(id);
        var planDto = mapper.Map<PlanDetailsDto>(result);
        return planDto;
    }

    public async Task<PlanWithSchoolsDto> GetPlanWithSchoolsByIdAsync(Guid id)
    {
        return await planRepository.GetPlanWithSchoolsDetailsAsync(id);
    }

    public async Task<Result<bool>> DeletePlanDraft(Guid id)
    {
        return await ExecuteWithResult(async () =>
        {
            bool result = await planRepository.DeleteEvaluationPlan(id);
            return result;
        });
    }
    public async Task<Result<CreateEvaluationPlanDto>> InsertOrUpdatePlan(
    CreateEvaluationPlanDto modelDto)
    {
        ValidatedPlan(modelDto);
        return await ExecuteWithResult(async () =>
        {
            // 1️⃣ Get system values
            Guid? departmentId =
                await departmentService.GetDepartmentIdAsync();

            Guid academicYearId =
                await academicYearRepository.GetAcademicYearId(departmentId);

            Guid planStatusId =
                await unitOfWork.GetRepository<PlanStatus>()
                    .GetAllActiveNonDeleted(x =>
                        x.BackendName == StatusBackEnds.ApprovedPlans)
                    .Select(x => x.Id)
                    .FirstAsync();
            Guid depEvaluationType =
                await unitOfWork.GetRepository<DepEvaluationType>()
                    .GetAllActiveNonDeleted(x =>
                        x.DepartmentId == departmentId)
                    .Select(x => x.Id)
                    .FirstAsync();

            Guid serviceStatusId =
                await unitOfWork.GetRepository<ServiceStatus>()
                    .GetAllActiveNonDeleted(x =>
                        x.BackendName == StatusBackEnds.New)
                    .Select(x => x.Id)
                    .FirstAsync();

            // 2️⃣ Assign system fields
            modelDto.AcademicYearId = academicYearId;
            modelDto.PlanStatusId = planStatusId;

            // 3️⃣ Map DTO → Entity
            Plan plan = modelDto.ToPlan();
            plan.PlanJsonValue = JsonConvert.SerializeObject(modelDto);

            // 4️⃣ Insert Plan
            await unitOfWork.GetRepository<Plan>().InsertAsync(plan);

            //4.1 Services
            Guid serviceId = await uow.GetRepository<Service>()
            .GetAllActiveNonDeleted(x => x.BackendName == BackendServices.EvaluationPlan_P_CreatePlan)
            .Select(x => x.Id).FirstOrDefaultAsync();
            // 5️⃣ Insert Evaluation Requests
            if (modelDto.Schools?.Any() == true)
            {
                var evaluationRequests = modelDto.Schools.Select(school =>
                    new EvaluationRequest
                    {
                        Id = Guid.NewGuid(),
                        PlanId = plan.Id,
                        ServiceId = serviceId,
                        OrgTreeId = school.Id,
                        DepEvaluationTypeId = depEvaluationType,
                        FromDate = school.StartEvaluationDate,
                        ToDate = school.EndEvaluationDate,
                        ServiceStatusId = serviceStatusId,
                        CreateDate = DateTime.UtcNow,
                        IsDeleted = false,
                    }).ToList();

                await unitOfWork.GetRepository<EvaluationRequest>()
                                .InsertRange(evaluationRequests);
            }

            // 6️⃣ ONE COMMIT ONLY
            await unitOfWork.CommitAsync();

            return modelDto;
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
        return mapper.Map<PlanDto>(plan);
    }
    public async Task<PaginatedResult<PlanListDto>> GetPlansAsync(PlanDetailsRequestDto request)
    {
        PaginatedResult<PlanListDto> result = await planRepository.GetPlans(request);
        return mapper.Map<PaginatedResult<PlanListDto>>(result);
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

    private void ValidatedPlan(CreateEvaluationPlanDto model)
    {
        if (model is null || string.IsNullOrEmpty(model.Name))
            throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidApprovePlan);
        if (model.StartDate <= model.EndDate)
            throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidEvaluationDate);
    }
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
