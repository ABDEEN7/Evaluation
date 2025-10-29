using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Entities.Website;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.BusinessLayer.API;


public class SchoolBL : ApiBase
{
    public SchoolBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
        : base(serviceScopeFactory, cacheDataProvider, uow, loggingServices, userInfo, serviceProvider, requestInfo)
    {
    }

    public async Task<School> GetUniversityDetails(Guid SchoolID)
    {
        var schoollist = await uow.GetRepository<School>().GetAllActiveNonDeleted().ToListAsync();

        var schoolData = await serviceProvider.CreateScopedUow().GetRepository<School>()
            .GetAllQueryFiltered()
            .AsNoTracking()
            .Where(c => c.Id == SchoolID)
               .FirstOrDefaultAsync();
        return schoolData;
    }
}