using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.DepartmentHolidayLayer;

public class DepartmentHolidayService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<DepartmentHoliday>> GetDepartmentHolidayList(int Page)
    {
        var list = await unitOfWork.GetRepository<DepartmentHoliday>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page * 20)
                .Take(20)
                .ToListAsync();
        return list;
    }
    public async Task<List<DepartmentHoliday>> GetDepartmentHolidayList()
    {
        var academicYearId = unitOfWork
            .GetRepository<AcademicYear>()
            .GetAllActiveNonDeleted(x => x.DepartmentId == requestInfo.DepId && x.IsCurrent)
            .Select(s => s.Id)
            .FirstOrDefault();

        var list = await unitOfWork.GetRepository<DepartmentHoliday>()
                .GetAllNonDeleted(x => x.AcademicYearId == academicYearId)
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();
        return list;
    }
    public async Task<DepartmentHoliday> GetDepartmentHoliday(Guid id)
    {
        var holiday = await unitOfWork.GetRepository<DepartmentHoliday>()
                .GetAllNonDeleted()
                .FirstOrDefaultAsync(x => x.Id == id);
        return holiday ?? throw new ArgumentNullException(nameof(holiday));
    }
    public async Task<DepartmentHoliday> InsertDepartmentHoliday(DepartmentHoliday departmentHoliday)
    {
        if (departmentHoliday == null)
            throw new ArgumentNullException(nameof(departmentHoliday));

        await uow.GetRepository<DepartmentHoliday>().InsertAsync(departmentHoliday);
        await uow.CommitAsync();
        return departmentHoliday;
    }
    public async Task DeleteDepartmentHoliday(DepartmentHoliday departmentHoliday)
    {
        if (departmentHoliday == null)
            throw new ArgumentNullException(nameof(departmentHoliday));
        uow.GetRepository<DepartmentHoliday>().Delete(departmentHoliday);
        await uow.CommitAsync();
    }
    public async Task<DepartmentHoliday> UpdateDepartmentHoliday(DepartmentHoliday departmentHoliday)
    {
        if (departmentHoliday == null)
            throw new ArgumentNullException(nameof(departmentHoliday));

        uow.GetRepository<DepartmentHoliday>().Update(departmentHoliday);
        await uow.CommitAsync();
        return departmentHoliday;
    }
}
