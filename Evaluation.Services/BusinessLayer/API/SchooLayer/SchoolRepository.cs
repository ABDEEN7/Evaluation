using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.Services.BusinessLayer.API.SchooLayer;

public class SchoolRepository(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo,
        serviceProvider, requestInfo)
{
    public async Task<PaginatedResult<School>> GetSchoolsAsyncOld(SchoolRequest request)
    {
        var filter = BuildFilterExpressionOld(request);

        var query = serviceScopeFactory
             .CreateScopedUow()
             .GetRepository<School>()
             .GetAllNonDeleted(filter)
             .Include(x => x.SchoolType)
             .Include(x => x.SchoolLevel)
             .ThenInclude(x => x.EducationLevel);
        return await query.GetPaginatedResult(request.PageNumber, request.PageSize = 10);
    }
    public async Task<PaginatedResult<ResponseOrgsPlans>> GetSchoolsAsync(
    SchoolRequest request,
    List<Guid?> targetOrgTreeIds,
    List<Guid> currentSelectedSchools)
    {
        using var uow = serviceScopeFactory.CreateScopedUow();

        var filter = BuildFilterExpression(request, targetOrgTreeIds, currentSelectedSchools);

        var query = uow.GetRepository<School>()
            .GetAllNonDeleted(filter)
            .Select(s => new
            {
                School = s,
                LastEval = s.EvaluationRequests
                    .OrderByDescending(e => e.EvaluationDate)
                    .FirstOrDefault()
            })
            .Select(x => new ResponseOrgsPlans
            {
                Id = x.School.Id,
                Name = requestInfo.Lang == LanguageConst.Ar ? x.School.NameAr : x.School.NameEn,
                IsOpen = x.School.EvaluationRequests.Any(x => x.ServiceStatus.ServiceStatusType.BackendName.ToLower() == ConstantKeys.ServiceStatusTypeBackend.Open.ToLower()),
                SchoolLevel = x.School.SchoolLevel!
                    .Select(sl => new SchoolLevelDto
                    {
                        Name = sl.EducationLevel.NameEn,
                        BackendName = sl.EducationLevel.BackendName,
                        SchoolId = sl.SchoolId
                    }).ToList(),

                LastEvaluationDate = x.LastEval.EvaluationDate,
                AcademicYear = x.LastEval.NextEvaluationDate
            });

        return await query.GetPaginatedResult(request.PageNumber, request.PageSize);
    }

    public async Task<List<School>> GetSchoolsByDepartmentId(Guid depId)
    {
        var department = await serviceProvider.CreateScopedUow().GetRepository<Department>()
             .GetAllQueryFiltered()
             .AsNoTracking().Include(c => c.DepTargetOrgTrees)
             .Where(c => c.Id == depId)
             .FirstOrDefaultAsync();


        var schools = serviceProvider
           .CreateScopedUow()
           .GetRepository<School>()
           .GetAllNonDeleted()
           .Where(c => c.OrgParentId == department.DepTargetOrgTrees.Select(x => x.TargetOrgTreeId).FirstOrDefault())
           .ToList();

        return schools;
    }
    public async Task<School> GetSchoolDetails(Guid SchoolID)
    {
        var school = await serviceProvider
           .CreateScopedUow()
           .GetRepository<School>().GetByIDActiveNonDeleted(SchoolID);

        return school;
    }

    public async Task<List<EvaluationRequest>> GetEvaluationRequestByOrgTreeId(Guid OrgTreeId)
    {
        IQueryable<EvaluationRequest> query = unitOfWork.GetRepository<EvaluationRequest>()
            .GetAllActiveNonDeleted()
            .Include(d => d.ServiceStatus)
            .Include(d => d.FormEvalMatrixValue)
            .Where(er =>
                er.OrgTreeId == OrgTreeId &&
                er.DepEvaluationType.DepartmentId == requestInfo.DepId &&
                er.FormEvalMatrixValueId != null)
            .OrderByDescending(er => er.CreateDate)
            .Take(2);

        return query.ToList();
    }

    public async Task<IQueryable<DepEvaluationType>> GetVisitTypes()
    {
        var visitTypes =
        unitOfWork
        .GetRepository<DepEvaluationType>()
        .GetAllActiveNonDeleted(x => x.DepartmentId == requestInfo.DepId);
        return visitTypes;
    }


    private Expression<Func<School, bool>> BuildFilterExpression(SchoolRequest request, List<Guid?> targetOrgTreeIds, List<Guid> currentSchools)
    {

        Expression<Func<School, bool>> filter = s => true;
        filter = filter.And(c => targetOrgTreeIds.Contains(c.OrgParentId) && currentSchools.Contains(c.Id));
        //filter = filter.And(c=> c.) we will added here filter by ServiceStatus.IsOPEN
        if (request.Id != null && request.Id.Count > 0)
            filter = filter.And(c => request.Id.Contains(c.Id));
        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(s => s.NameEn.Contains(request.Name) || s.NameAr.Contains(request.Name));
        if (request.EstablishmentDate != null)
        {
            int year = request.EstablishmentDate.Value.Year;
            filter = filter.And(s => s.EstablishmentDate.Year == year);
        }
        if (request.ParentId != Guid.Empty && request.ParentId != null)
        {
            filter = filter.And(x => x.OrgParentId == request.ParentId);
        }
        if (request.FomrEvalMatrixValueId != Guid.Empty && request.FomrEvalMatrixValueId != null)
        {
            filter = filter.And(s => s.EvaluationRequests != null &&
                                     s.EvaluationRequests.Any(er => er.FormEvalMatrixValueId == request.FomrEvalMatrixValueId));
        }
        //if(request.VisitType != null)
        //    filter = filter.And(x=>x.)
        return filter;

    }


    private Expression<Func<School, bool>> BuildFilterExpressionOld(SchoolRequest request)
    {
        Expression<Func<School, bool>> filter = s => true;
        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(s => s.NameEn.Contains(request.Name) || s.NameAr.Contains(request.Name));
        if (request.EstablishmentDate != null)
        {
            int year = request.EstablishmentDate.Value.Year;
            filter = filter.And(s => s.EstablishmentDate.Year == year);
        }
        //if(request.VisitType != null)
        //    filter = filter.And(x=>x.)
        return filter;

    }
}