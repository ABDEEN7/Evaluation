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
    public async Task<PaginatedResult<Employee>> GetOrgnizationAsync(SchoolRequest request, List<Guid?> targetOrgTreeIds, List<Guid> currentOrganizations)
    {
        var filter = BuildFilterExpression(request, targetOrgTreeIds, currentOrganizations);
        var query = unitOfWork.GetRepository<Employee>()
                    .GetAllNonDeleted(filter);
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
    private Expression<Func<Employee, bool>> BuildFilterExpression(SchoolRequest request, List<Guid?> targetOrgTreeIds, List<Guid> currentOrganizations)
    {

        Expression<Func<Employee, bool>> filter = s => true;
        filter = filter.And(c => targetOrgTreeIds.Contains(c.OrgParentId) && currentOrganizations.Contains(c.Id));

        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(s => s.NameEn.Contains(request.Name) || s.NameAr.Contains(request.Name));
        return filter;
    }
}
