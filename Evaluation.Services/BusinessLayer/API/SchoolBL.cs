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
        var schoollist = await uow.GetRepository<School>().GetAllActiveNonDeleted().ToListAsync();

        var schoolData = await serviceProvider.CreateScopedUow().GetRepository<School>()
            .GetAllQueryFiltered()
            .AsNoTracking()
            .Where(c => c.Id == SchoolID)
               .FirstOrDefaultAsync();
        return schoolData;
    }
    public async Task<List<ResponseSchools>> GetSchools()
    {
        var schools = schoolRepository.GetSchools();

        return await
            schools
            .OrderByDescending(s => s.EstablishmentDate)
            .Select(SchoolProjection(requestInfo.Lang))
            .ToListAsync();
    }
    public async Task<List<SchoolVisits>> GetVisitsAsync()
    {
        var responses = await schoolRepository.GetVisitTypes();
        List<SchoolVisits> schoolVisits = responses.Select(x => new SchoolVisits
        {
            Id = x.Id,
            Name = LanguageStatic.SelectLang(requestInfo.Lang, x.NameAr, x.NameEn)
        }).ToList();
        return schoolVisits;
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

}