using System.Linq.Expressions;
using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;


public class SchoolBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, SchoolRepository schoolRepository)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{

    public async Task<School> GetSchoolDetails(Guid SchoolID)
    {
        var schoolData = await serviceProvider.CreateScopedUow().GetRepository<School>()
            .GetAllQueryFiltered()
            .AsNoTracking()
            .Where(c => c.Id == SchoolID)
               .FirstOrDefaultAsync();
        return schoolData;
    }
    public async Task<List<ResponseSchools>> GetSchools(SchoolRequest request)
    {
        var filter = BuildFilterExpression(request);
        var schools = schoolRepository.GetSchools(filter);

        return await
            schools
            .OrderByDescending(s => s.EstablishmentDate)
            .Select(SchoolProjection(requestInfo.Lang))
            .ToListAsync();
    }
    public async Task<List<SchoolVisits>> GetVisitsAsync()
    {
        var responses = schoolRepository.GetVisitTypes();
        return await responses.Select(x => new SchoolVisits
        {
            Id = x.Id,
            Name = LanguageStatic.SelectLang(requestInfo.Lang, x.NameAr, x.NameEn)
        }).ToListAsync();
    }
    private static Expression<Func<School, ResponseSchools>> SchoolProjection(string lang)
    {
        return s => new ResponseSchools
        {
            Id = s.Id,
            Name = LanguageStatic.SelectLang(lang, s.NameAr, s.NameEn),
            Rating = s.SchoolLevel
            .OrderByDescending(c => c.CreateDate)
            .Select(c => c.EducationLevel.BackendName)
            .FirstOrDefault()
        };
    }
    private Expression<Func<School, bool>> BuildFilterExpression(SchoolRequest request)
    {
        Expression<Func<School, bool>> filter = s => true;
        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(s => s.NameEn.Contains(request.Name));
        //if(request.VisitDateFrom.HasValue)
        //    filter = filter.Date
        //if(request.VisitType != null)
        //    filter = filter.And(x=>x.)
        return filter;

    }
}