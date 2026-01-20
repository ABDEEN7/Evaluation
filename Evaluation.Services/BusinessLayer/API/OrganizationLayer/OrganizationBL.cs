using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.OrgDto;
using Evaluation.SharedHelper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;

public class OrganizationBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, OrganizationService organizationService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<OrgDetailsDto> GetOrganizationDetails(Guid Id)
    {

        var result = await organizationService.GetOrganizationById(Id);

        return new OrgDetailsDto();
    }
}