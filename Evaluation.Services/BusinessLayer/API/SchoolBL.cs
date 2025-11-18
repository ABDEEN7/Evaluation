using Evaluation.DAL.Helper;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Mapster;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;

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

        var schoolsRequest = await schoolRepository.GetSchoolsAsync();
        //var schoolResponse = schoolsRequest.Adapt<List<ResponseSchools>>();
        var schoolResponse = schoolsRequest.Select(x => new ResponseSchools
        {
            Id = x.Id,
            Name = x.NameEn,
            Rating = "Aecctable",
            AcademicYear = new DateTime(2025).Year,
            LastEvaluationDate = DateTime.Now
        }).ToList();
        return schoolResponse;

    }
    public async Task<List<SchoolVisits>> GetVisitsAsync()
    {
        var responses = await schoolRepository.GetVisitTypes();
        List<SchoolVisits> schoolVisits = responses.Select(x => new SchoolVisits
        {
            Id = x.Id,
            Name = requestInfo.Lang == "ar" ? x.NameAr : x.NameEn
        }).ToList();
        return schoolVisits;
    }

}