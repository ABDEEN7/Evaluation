using Evaluation.DAL.Dtos;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.SemesterLayer;

public class SemesterRepostiory(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    UserInfo userinfo,
    RequestInfo requestInfo
    ) : ApiServiceBase
{
    //public async Task<List<Semester>> GetSemesters(Guid acadmicYear)
    //{
    //    return await unitOfWork.GetRepository<Semester>()
    //        .GetAllActiveNonDeleted()
    //        .Where(x => x.AcademicYearId == acadmicYear &&
    //        userinfo.UserId == x.AcademicYear.Department.TargetOrgTreeId)
    //        .ToListAsync();
    //}
    public async Task<List<SemesterDto>> GetSemesters()
    {
        return await unitOfWork.GetRepository<Semester>()
                .GetAllActiveNonDeleted()
                .Where(x => x.AcademicYear.Department.UserDepartments.Any(x => x.UserId == userinfo.UserId))
                .Select(x => new SemesterDto
                {
                    Id = x.Id,
                    Name = requestInfo.Lang == "Ar" ? x.NameAr : x.NameEn,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate
                }).ToListAsync();
    }
}
