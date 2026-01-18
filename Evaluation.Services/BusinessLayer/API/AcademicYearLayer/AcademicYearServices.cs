using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
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
    public async Task<Guid> GetAcademicYearId()
    {
        var userId = userInfo.UserId;

        var departmentId = await unitOfWork
            .GetRepository<Department>()
            .GetAllActiveNonDeleted(d => d.UserDepartments.Any(ud => ud.UserId == userId))
            .Select(d => new
            {
                d.Id,
                LastAssignedDate = d.UserDepartments
                    .Where(ud => ud.UserId == userId)
                    .Max(ud => ud.CreateDate)
            })
            .OrderByDescending(x => x.LastAssignedDate)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        if (departmentId == Guid.Empty)
            throw new BusinessException(ConstantKeys.ExceptionMessage.CurrentAcademiUser);

        var academicYearId = await unitOfWork
            .GetRepository<AcademicYear>()
            .GetAllActiveNonDeleted(x =>
                x.DepartmentId == departmentId &&
                x.IsCurrent)
            .OrderByDescending(x => x.CreateDate)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        if (academicYearId == Guid.Empty)
            throw new BusinessException(ConstantKeys.ExceptionMessage.CurrentAcademiUser);

        return academicYearId;
    }

    //public async Task<Result<VacationDateDto>> GetVcationDateAsync()
    //{

    //    AcademicYearRepository? academicYear = await academicYearRepository.GetBlockedDays();
    //}
}
