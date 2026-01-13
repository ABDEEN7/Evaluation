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
    public async Task<PaginatedResult<Employee>> GetOrgnizationAsync(SchoolRequest request, Guid? targetOrgTreeId, List<Guid> currentOrganizations)
    {
        var filter = BuildFilterExpression(request, targetOrgTreeId, currentOrganizations);
        var query = unitOfWork.GetRepository<Employee>()
                    .GetAllNonDeleted(filter);
        return await query.GetPaginatedResult(request.PageNumber, request.PageSize);
    }
    private Expression<Func<Employee, bool>> BuildFilterExpression(SchoolRequest request, Guid? targetOrgTreeId, List<Guid> currentOrganizations)
    {

        Expression<Func<Employee, bool>> filter = s => true;
        filter = filter.And(c => c.OrgParentId == targetOrgTreeId && currentOrganizations.Contains(c.Id));

        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(s => s.NameEn.Contains(request.Name) || s.NameAr.Contains(request.Name));
        //if(request.VisitType != null)
        //    filter = filter.And(x=>x.)
        return filter;
    }
}
