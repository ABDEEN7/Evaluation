using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

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
    public async Task<PaginatedResult<School>> GetSchoolsAsync(SchoolRequest request)
    {
        var filter = BuildFilterExpression(request);

        var query = serviceScopeFactory
             .CreateScopedUow()
             .GetRepository<School>()
             .GetAllNonDeleted(filter)
             .Include(x => x.SchoolLevel)
             .ThenInclude(x => x.EducationLevel);
        return await query.GetPaginatedResult(request.PageNumber, request.PageSize = 10);
    }

    public async Task<List<School>> GetSchoolsByDepartmentId(Guid depId)
    {
        var department = await serviceProvider.CreateScopedUow().GetRepository<Department>()
             .GetAllQueryFiltered()
             .AsNoTracking()
             .Where(c => c.Id == depId)
             .FirstOrDefaultAsync();


        var schools = serviceProvider
           .CreateScopedUow()
           .GetRepository<School>()
           .GetAllNonDeleted()
           .Where(c => c.OrgParentId == department.TargetOrgTreeId)
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

    public IQueryable<VisitType> GetVisitTypes()
        => unitOfWork
            .GetRepository<VisitType>()
            .GetAllActiveNonDeleted();


    private Expression<Func<School, bool>> BuildFilterExpression(SchoolRequest request)
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