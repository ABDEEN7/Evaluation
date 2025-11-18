using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.SemesterLayer;

public class SemesterRepostiory(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    UserInfo userinfo
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
    public async Task<List<Semester>> GetSemesters()
    {
        userinfo.UserId = new Guid("BB133AA8-A93D-44BE-AA96-0003BD130921");
        return await unitOfWork.GetRepository<Semester>()
                .GetAllActiveNonDeleted()
                .Where(x => x.AcademicYear.Department.TargetOrgTreeId == userinfo.UserId)
                .ToListAsync();
    }
}
