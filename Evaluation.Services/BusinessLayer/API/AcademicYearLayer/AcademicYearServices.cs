using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.AcademicYearLayer;

public class AcademicYearServices(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo,
    AcademicYearRepository academicYearRepository
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo,
        serviceProvider, requestInfo)
{
    //public async Task<Result<VacationDateDto>> GetVcationDateAsync()
    //{

    //    AcademicYearRepository? academicYear = await academicYearRepository.GetBlockedDays();
    //}
}
