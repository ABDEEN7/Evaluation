using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.DepartmentHolidayLayer;

public class DepartmentHolidayService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo,
    DepartmentHolidayRepository departmentHolidayRepository
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo,
        serviceProvider, requestInfo)
{
    public async Task<Result<List<DepartmentHolidayDto>>> GetDepartmentHoliday(Guid AcademicYearId)
    {
        return await ExecuteWithResult(async () =>
        {
            List<DepartmentHoliday> model = await departmentHolidayRepository.GetDepartmentHolidays(AcademicYearId);
            List<DepartmentHolidayDto> result = model.Select(x => new DepartmentHolidayDto
            {
                Id = x.Id,
                StartDate = x.StartDate,
                Name = x.NameEn,
                IsCronExpression = x.IsCronExpression,
                CronExpression = x.CronExpression,
                EndDate = x.EndDate.HasValue ? x.EndDate.Value : null,
            }).ToList();
            return result;
        });
    }
}
