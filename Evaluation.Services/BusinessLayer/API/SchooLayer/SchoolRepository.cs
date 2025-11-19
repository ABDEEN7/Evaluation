using System.Threading.Tasks;
using Evaluation.DAL.Helper;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Mapster;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Consts;

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
    public async Task<List<ResponseSchools>> GetSchoolsAsync()
    {
        var schools = await unitOfWork
            .GetRepository<School>()
            .GetAllActiveNonDeleted()
            .OrderByDescending(x => x.EstablishmentDate)
            .Select(s => new ResponseSchools
            {
                Id = s.Id,
                Name = LanguageStatic.SelectLang(requestInfo.Lang, s.NameAr, s.NameEn),
                Rating = (s.SchoolLevel ?? Enumerable.Empty<SchoolLevel>())
                .OrderByDescending(c => c.CreateDate)
                .Select(c => c.EducationLevel.BackendName)
                .FirstOrDefault(),

            })
            .ToListAsync();
        return schools;
    }
    public async Task<List<VisitType>> GetVisitTypes()
    {
        return await unitOfWork
            .GetRepository<VisitType>()
            .GetAllActiveNonDeleted()
            .ToListAsync();
    }
}
