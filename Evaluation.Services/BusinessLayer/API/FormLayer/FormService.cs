using AutoMapper;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Helper;
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
    public async Task<EvalForm> GetEvalForm(Guid id)
    {
        return await unitOfWork.GetRepository<EvalForm>().GetByIdAsync(id);
    }

    public async Task<List<FormItem>> GetFormItems()
    {
            var formItems = await unitOfWork.GetRepository<FormItem>()
                      .GetAllActiveNonDeleted().Include(d => d.SubFormItems).ToListAsync();

            return formItems;
    }
      
    public async Task<FormItemValue> UpdateFormItemValue(FormItemValue formItemValue)
    {
        unitOfWork.GetRepository<FormItemValue>().Update(formItemValue);
        await uow.CommitAsync();

        return formItemValue;
    }
    public async Task<SubFormItemValue> UpdateSubFormItemValue(SubFormItemValue subFormItemValue)
    {
        unitOfWork.GetRepository<SubFormItemValue>().Update(subFormItemValue);
        await uow.CommitAsync();

        return subFormItemValue;
    }

    public async Task<FormEvaluationValue> SaveFormItemsAndSubs(FormEvaluationValue form)
    {

        await unitOfWork.GetRepository<FormItemValue>().InsertRange(form.Items);
        await unitOfWork.GetRepository<SubFormItemValue>().InsertRange(form.SubItems);
        await uow.CommitAsync();

        return form;
    }

    public async Task<FormItemValue> GetFormItemValue(Guid ValueId)
    {
        return await unitOfWork.GetRepository<FormItemValue>().GetByIDActiveNonDeleted(ValueId!);
    }

    public async Task<SubFormItemValue> GetSubFormItemValue(Guid ValueId)
    {
        return await unitOfWork.GetRepository<SubFormItemValue>().GetByIDActiveNonDeleted(ValueId!);
    }

}