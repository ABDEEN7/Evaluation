using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;


public class OrgService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<DepTargetOrgTree>> GetDepTargetOrgTree()
    {
        using var scopUow= serviceScopeFactory.CreateScopedUow();

		var depTargetOrgTree = await scopUow.
                    GetRepository<DepTargetOrgTree>()
                    .GetAllActiveNonDeleted(x => x.DepartmentId == requestInfo.DepId)
                    .Include(x => x.TargetOrgTree)
                    .Include(x => x.Category)
                    .ToListAsync();
       
        return depTargetOrgTree;
    }
    public async Task<List<Guid>> GetCurrentOrgTreeIds(List<Guid?> TargetOrgTreeIds, int? academicYear)
	{
		using var scopUow = serviceScopeFactory.CreateScopedUow();
		return await scopUow.GetRepository<OrgAcademicYear>()
										   .GetAllActiveNonDeleted(x => x.Year == academicYear && TargetOrgTreeIds.Contains(x.ParentOrgTreeId))
										   .Select(x => x.OrgTreeId)
										   .ToListAsync();
	}
       
    public async Task<List<OrgTree>> GetCurrentOrgTrees( List<Guid?> targetOrgTreeIds, int? academicYear)
    {
		using var scopUow = serviceScopeFactory.CreateScopedUow();

		return await scopUow.GetRepository<OrgAcademicYear>()
            .GetAllActiveNonDeleted(x =>
                x.Year == academicYear &&
                targetOrgTreeIds.Contains(x.ParentOrgTreeId))
            .Select(x => x.ParentOrgTree)   
            .Distinct() 
            .Select(p => new OrgTree
            {
                Id = p.Id,
                NameEn = p.NameEn,
                NameAr = p.NameAr
            })
            .ToListAsync();
    }

}