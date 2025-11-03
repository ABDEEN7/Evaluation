using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Entities.Planing;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static Evaluation.SharedHelper.Enums.ConstantKeys;
namespace Evaluation.Services.BusinessLayer.API.PlanLayer;

public class PlanServiceRequestRepository(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, userInfo,
        serviceProvider, requestInfo)
{
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
    public async Task<bool> ApprovePlans(Guid planId)
    {
        //Get the the approver request from مدير النظام
        PlanServiceRequest? planRequest = await unitOfWork
            .GetRepository<PlanServiceRequest>()
            .GetByIdAsync(planId);
        //Deseialize the value of the approver request 
        CreateEvaluationPlanDto? createplan = JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(planRequest.Value);
        Guid departmentId = unitOfWork
            .GetRepository<AcademicYear>()
            .GetAllActiveNonDeleted(x => x.Id == planRequest.AcadmicYearId)
            .Select(x => x.DepartmentId)
            .FirstOrDefault();
        Plan plan = new Plan
        {
            NameAr = createplan.NameAr,
            NameEn = createplan.NameEn,
            AcademicYearId = createplan.AcademicYearId,
            PlanStatusId = createplan.PlanStatusId,
            DepartmentId = departmentId,
            //PlanSchedules = PlanSchedules


        };
        //Get id of school that we will evaluate
        //List<Guid> schoolIds = plan.Schools.Select(s => s.Id).ToList();
        var selectedSchool = unitOfWork
            .GetRepository<School>();
        //.GetAllActiveNonDeleted(x => schoolIds.Contains(x.Id));

        //added selected school to the plan 
        //plan.PlanSchedules = selectedSchool.Select(school => new PlanSchedule
        //{
        //    SchoolId = school.Id,
        //    School = school
        //}).ToList();
        //await unitOfWork.GetRepository<Plan>().InsertAsync(plan);
        await unitOfWork.CommitAsync();
        return true;
    }
    public async Task<bool> DeleteSchoolFromPlan(Guid requestId, Guid schoolId)
    {
        var changeRequest = await unitOfWork.GetRepository<ChangeRequest>().GetByIdAsync(requestId);
        var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(changeRequest.PlanId);
        var schoolPlan = await unitOfWork.GetRepository<EvaluationRequest>()
            .GetAllActiveNonDeleted().FirstOrDefaultAsync(x => x.PlanId == plan.Id && x.SchoolId == schoolId);
        unitOfWork.GetRepository<EvaluationRequest>().Delete(schoolPlan);
        await unitOfWork.CommitAsync();
        return true;
    }
    public async Task<List<PlanTypeDto>> GetPlanTypeAsync()
    {
        return await serviceScopeFactory.CreateScopedUow().GetRepository<PlanType>()
            .GetAllActiveNonDeleted()
            .Select(s => new PlanTypeDto
            {
                Id = s.Id,
                Name = requestInfo.Lang == LanguageConst.Ar ? s.NameAr : s.NameEn,
            }).
            ToListAsync();
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
