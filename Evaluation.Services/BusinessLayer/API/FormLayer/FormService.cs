using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.FormLayer;

public class FormService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<FormItem>> GetFormItems()
    {
        //var formItems = unitOfWork.GetRepository<FormItem>()
        //    .GetAllActiveNonDeleted().Include(d => d.SubFormItems);
            
        var formItems = new List<FormItem>() {
            new FormItem() { Id = new Guid("921d891a-e0cb-4fd4-8e53-fb3443ef0199"), EvalFormId = new Guid("e4530010-a302-41f5-935c-2599674db37a"), NameAr = "بند 1", NameEn = "Item 1",
                SubFormItems = new List<SubFormItem>(){ 
                    new SubFormItem() { NameAr = "بند فرعي1" , NameEn = "Sub Item 1" },
                    new SubFormItem() { NameAr = "بند فرعي2", NameEn = "Sub Item 2" }
                } },
            new FormItem() { Id = new Guid("ecd11007-2ff6-42cb-bca2-b168de94afbc"), EvalFormId = new Guid("e4530010-a302-41f5-935c-2599674db37a"), NameAr = "بند 2", NameEn = "Item 2" },
            new FormItem() { Id = new Guid("6669fa51-2403-43e3-8cff-03ee9061722b"), EvalFormId = new Guid("9da26b46-4c06-429a-8b3f-8a50c47f1860"), NameAr = "بند 1", NameEn = "Item 1" },
        };

        return formItems;
    }
}