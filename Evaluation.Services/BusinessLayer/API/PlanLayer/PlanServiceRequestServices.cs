using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.AcademicYearLayer;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Dtos.PlanDto.EditDto;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static Evaluation.SharedHelper.Enums.ConstantKeys;
using ValidationResult = Evaluation.SharedHelper.Models.ValidationResult;

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
        //ValidatedPlan(modelDto);
        return modelDto.Id == Guid.Empty
     ? await InsertPlan(modelDto)
     : await UpdatePlan(modelDto);
    }
    //Plan Type Module
    public async Task<List<PlanTypeDto>> GetPlanTypes()
    {
        return await planRepository.GetPlanType().Where(x => x.DepartmentId == requestInfo.DepId).Select(s => new PlanTypeDto
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
    public async Task<Result<ValidationResult>> ValidateEvaluationPlan(CreateEvaluationPlanDto model)
    {
        return CreateEvaluationPlanValidator(model);
    }
    public static ValidationResult CreateEvaluationPlanValidator(CreateEvaluationPlanDto dto)
    {
        var result = new ValidationResult();

        if (dto == null)
        {
            result.Errors.Add(ConstantKeys.ExceptionMessage.InvalidPlan);
            return result;
        }

        // ===================== Required Fields =====================
        if (string.IsNullOrWhiteSpace(dto.Name))
            result.Add(ConstantKeys.ExceptionMessage.Requiredfield);
        if (dto.PlanTypeDepId == Guid.Empty)
            result.Add(ConstantKeys.ExceptionMessage.Requiredfield);
        // ===================== Dates Validation =====================
        if (dto.StartDate == default)
            result.Add(ConstantKeys.ExceptionMessage.Requiredfield);
        if (dto.EndDate == default)
            result.Add(ConstantKeys.ExceptionMessage.Requiredfield);
        if (dto.StartDate != default &&
            dto.EndDate != default &&
            dto.EndDate < dto.StartDate)
        {
            result.Add(ConstantKeys.ExceptionMessage.CompareDateException);
        }
        // ===================== Schools Validation =====================
        if (dto.Schools != null && dto.Schools.Any())
        {
            for (int i = 0; i < dto.Schools.Count; i++)
            {
                ValidateSchool(
                    dto.Schools[i],
                    dto.StartDate,
                    dto.EndDate,
                    result,
                    i);
            }
        }
        return result;
    }
    private static void ValidateSchool(SelectedSchool school,
    DateOnly planStartDate,
    DateOnly planEndDate,
    ValidationResult result,
    int index)
    {
        var prefix = $"Schools[{index}]";

        //Required id
        if (school.Id == Guid.Empty)
            result.Add($"{prefix}: {ConstantKeys.ExceptionMessage.Requiredfield}");
        //Visit Type
        // Evaluation Dates
        if (school.StartEvaluationDate == default)
            result.Add(ConstantKeys.ExceptionMessage.Requiredfield);

        if (school.EndEvaluationDate == default)
            result.Add(ConstantKeys.ExceptionMessage.Requiredfield);

        if (school.StartEvaluationDate != default &&
            school.EndEvaluationDate != default &&
            school.EndEvaluationDate < school.StartEvaluationDate)
        {
            result.Add(ConstantKeys.ExceptionMessage.CompareDateException);
        }
        // ===================== Range Check =====================
        if (school.StartEvaluationDate != default)
        {
            var planStart = planStartDate.ToDateTime(TimeOnly.MinValue);
            var planEnd = planEndDate.ToDateTime(TimeOnly.MaxValue);
            if (school.StartEvaluationDate < planStart ||
          school.StartEvaluationDate > planEnd)
            {
                result.Add(ConstantKeys.ExceptionMessage.StartSchoolPlanDateException);
            }
        }
        if (school.EndEvaluationDate != default)
        {
            var planStart = planStartDate.ToDateTime(TimeOnly.MinValue);
            var planEnd = planEndDate.ToDateTime(TimeOnly.MaxValue);

            if (school.EndEvaluationDate < planStart ||
                school.EndEvaluationDate > planEnd)
            {
                result.Add(ConstantKeys.ExceptionMessage.EndSchoolPlanDateException);
            }
        }
    }

    private async Task<Result<CreateEvaluationPlanDto>> InsertPlan(CreateEvaluationPlanDto modelDto)
    {
        return await ExecuteWithResult(async () =>
        {
            await FillSystemFields(modelDto);
            var planId = Guid.NewGuid();
            modelDto.Id = planId;
            Plan plan = modelDto.ToPlan();
            plan.Id = planId;
            plan.PlanJsonValue = JsonConvert.SerializeObject(modelDto);

            await unitOfWork.GetRepository<Plan>().InsertAsync(plan);

            await InsertEvaluationRequests(plan.Id, modelDto);
            return modelDto;
        });
    }
    private async Task<Result<CreateEvaluationPlanDto>> UpdatePlan(
      CreateEvaluationPlanDto modelDto)
    {
        return await ExecuteWithResult(async () =>
        {
            Plan plan = await unitOfWork.GetRepository<Plan>()
                .GetAllActiveNonDeleted(x => x.Id == modelDto.Id)
                .FirstOrDefaultAsync();

            if (plan == null)
                throw new BusinessException("Plan not found");

            modelDto.AcademicYearId = plan.AcademicYearId;
            modelDto.PlanStatusId = plan.PlanStatusId;

            mapper.Map(modelDto, plan);

            plan.PlanJsonValue = JsonConvert.SerializeObject(modelDto);
            plan.UpdateDate = DateTime.UtcNow;

            unitOfWork.GetRepository<Plan>().Update(plan);

            await ReplaceEvaluationRequests(plan.Id, modelDto);
            return modelDto;
        });
    }


    private async Task FillSystemFields(CreateEvaluationPlanDto modelDto)
    {
        modelDto.AcademicYearId =
            await academicYearRepository.GetAcademicYearId(requestInfo.DepId);

        modelDto.PlanStatusId =
            await unitOfWork.GetRepository<PlanStatus>()
                .GetAllActiveNonDeleted(x =>
                    x.BackendName == StatusBackEnds.ApprovedPlans)
                .Select(x => x.Id)
                .FirstAsync();
    }
    private async Task InsertEvaluationRequests(
    Guid planId,
    CreateEvaluationPlanDto modelDto)
    {
        if (modelDto.Schools?.Any() != true)
            return;

        Guid serviceId = await uow.GetRepository<Service>()
            .GetAllActiveNonDeleted(x =>
                x.BackendName == BackendServices.EvaluationPlan_P_CreatePlan)
            .Select(x => x.Id)
            .FirstAsync();

        Guid depEvaluationType = await GetDepEvaluationType(); // i dont see this
        Guid serviceStatusId = await GetServiceStatus(); // also i don't see this

        var requests = modelDto.Schools.Select(school => new EvaluationRequest
        {
            Id = Guid.NewGuid(),
            PlanId = planId,
            ServiceId = serviceId,
            OrgTreeId = school.Id,
            DepEvaluationTypeId = depEvaluationType,
            FromDate = school.StartEvaluationDate,
            ToDate = school.EndEvaluationDate,
            ServiceStatusId = serviceStatusId,
            CreateDate = DateTime.UtcNow,
            IsDeleted = false
        });

        await unitOfWork.GetRepository<EvaluationRequest>()
            .InsertRange(requests);
    }
    private async Task ReplaceEvaluationRequests(
    Guid planId,
    CreateEvaluationPlanDto modelDto)
    {
        var repo = unitOfWork.GetRepository<EvaluationRequest>();

        var existing = await repo
            .GetAllActiveNonDeleted(x => x.PlanId == planId)
            .ToListAsync();

        if (existing.Any())
            repo.DeleteRange(existing);

        await InsertEvaluationRequests(planId, modelDto);
    }
    private async Task<Guid> GetDepEvaluationType()
    {
        return await unitOfWork.GetRepository<DepEvaluationType>()
            .GetAllActiveNonDeleted(x => x.DepartmentId == requestInfo.DepId)
            .Select(x => x.Id)
            .FirstAsync();
    }
    private async Task<Guid> GetServiceStatus()
    {
        return await unitOfWork.GetRepository<ServiceStatus>()
            .GetAllActiveNonDeleted(x => x.BackendName == StatusBackEnds.New)
            .Select(x => x.Id)
            .FirstAsync();
    }


}
