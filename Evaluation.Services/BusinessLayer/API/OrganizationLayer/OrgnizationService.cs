using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Mappers.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Evaluation.Services.BusinessLayer.API.OrganizationLayer;

public class OrgnizationService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<PaginatedResult<ResponseOrgsPlans>> GetOrgnizationAsync(
      SchoolRequest request,
      List<Guid?> targetOrgTreeIds,
      List<Guid> currentOrganizations)
    {
        var filter = BuildFilterExpression(request, targetOrgTreeIds, currentOrganizations);

        var query = unitOfWork.GetRepository<OrgTree>()
            .GetAllNonDeleted(filter)
            .Select(o => new
            {
                Org = o,
                LastEval = o.EvaluationRequests
                    .OrderByDescending(e => e.EvaluationDate)
                    .FirstOrDefault()
            })
            .Select(x => new ResponseOrgsPlans
            {
                Id = x.Org.Id,
                Name = x.Org.NameEn,

                LastEvaluationDate = x.LastEval.EvaluationDate,
                AcademicYear = x.LastEval.NextEvaluationDate
            });

        return await query.GetPaginatedResult(request.PageNumber, request.PageSize);
    }
    public async Task<PaginatedResult<OrgTree>> GetMultipleOrgAsync(SchoolRequest request, List<Guid?> targetOrgTreeIds, List<Guid> currentOrganizations)
    {
        var filter = BuildFilterOrgExpression(request, targetOrgTreeIds, currentOrganizations);
        var query = unitOfWork.GetRepository<OrgTree>()
                    .GetAllNonDeleted(filter);
        return await query.GetPaginatedResult(request.PageNumber, request.PageSize);
    }
    private Expression<Func<OrgTree, bool>> BuildFilterOrgExpression(SchoolRequest request, List<Guid?> targetOrgTreeIds, List<Guid> currentOrganizations)
    {

        Expression<Func<OrgTree, bool>> filter = s => true;
        filter = filter.And(c => targetOrgTreeIds.Contains(c.OrgParentId) && currentOrganizations.Contains(c.Id));
        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(s => s.NameEn.Contains(request.Name) || s.NameAr.Contains(request.Name));
        return filter;
    }
    private Expression<Func<OrgTree, bool>> BuildFilterExpression(SchoolRequest request, List<Guid?> targetOrgTreeIds, List<Guid> currentOrganizations)
    {

        Expression<Func<OrgTree, bool>> filter = s => true;
        filter = filter.And(c => currentOrganizations.Contains(c.Id));

        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(s => s.NameEn.Contains(request.Name) || s.NameAr.Contains(request.Name));
        if (request.FomrEvalMatrixValueId != Guid.Empty && request.FomrEvalMatrixValueId != null)
        {
            filter = filter.And(s => s.EvaluationRequests != null &&
                                     s.EvaluationRequests.Any(er => er.FormEvalMatrixValueId == request.FomrEvalMatrixValueId));
        }
        return filter;
    }
}
