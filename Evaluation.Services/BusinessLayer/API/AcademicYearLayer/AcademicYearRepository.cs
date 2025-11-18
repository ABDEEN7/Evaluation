using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.AcademicYearLayer;

public class AcademicYearRepository(IServiceScopeFactory serviceScopeFactory,
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
    public async Task<Guid> GetAcademicYearId(Guid academicYearId)
    {
        return await unitOfWork.GetRepository<AcademicYear>()
            .GetAllActiveNonDeleted(x => x.DepartmentId == academicYearId)
            .OrderByDescending(x => x.StartDate)
            .Select(s => s.Id)
            .FirstOrDefaultAsync();
    }
    public async Task<VacationDateDto?> GetBlockedDays(Guid academicYearId)
    {
        var query = unitOfWork.GetRepository<DepartmentHoliday>()
            .GetAllActiveNonDeleted(x => x.AcademicYearId == academicYearId)
            .AsNoTracking()
            .OrderByDescending(x => x.CreateDate)
            .ThenBy(x => x.UpdateDate);

        if (await query.AnyAsync())
            throw new Exception("Blocked days already exist.");
        
        return new VacationDateDto { };
        //return await query.ProjectToType<VacationDateDto>().FirstOrDefaultAsync();
    }
}