using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.SemesterLayer;

public class SemesterRepostiory(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork
    ) : ApiServiceBase
{
    public async Task<List<Semester>> GetSemesters(Guid acadmicYear)
    {
        return await unitOfWork.GetRepository<Semester>()
            .GetAllActiveNonDeleted()
            .Where(x => x.AcademicYearId == acadmicYear)
            .ToListAsync();
    }
}
