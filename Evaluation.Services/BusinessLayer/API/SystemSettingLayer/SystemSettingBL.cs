using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.TeamMemberBL;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.SystemSettingLayer;

public class SystemSettingBL(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo,
    IEmailServices emailServices,
    AssignmentService teamMemberService,
    EmailTemplateProvider emailTemplateProvider
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo,
        serviceProvider, requestInfo)
{

    public string GetSetting(string Key)
    {
        using (var newuow = serviceProvider.CreateScopedUow())
        {

            var result = newuow.GetRepository<SystemSetting>()
                                   .GetAllNonDeleted()
                                   .Where(c => c.SettingKey == Key)
                                   .Select(c => c.SettingValue).FirstOrDefault();
            return result ?? "";
        }
    }
}
