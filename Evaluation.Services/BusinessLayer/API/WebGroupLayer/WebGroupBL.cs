using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.WebSiteDto;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Evaluation.Services.BusinessLayer.API.WebGroupLayer;

public class WebGroupBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, WebGroupService webGroupService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{

    public async Task<WebGroupsDto> GetWebGroupByPath(string path)
    {
        var webGroup = await webGroupService.GetWebGroupByPath(path);

        return mapper.Map<WebGroupsDto>(webGroup, opt =>
        {
            opt.Items["lang"] = requestInfo.Lang;
        });
    }

}