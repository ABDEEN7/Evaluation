using AutoMapper;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Server;

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
        return await unitOfWork.GetRepository<EvalForm>()
            .GetByIdAsync(id);
    }

    public async Task<EvalForm> GetEvalForm(Guid id, bool IncludeCalcMethod)
    {
        var query = unitOfWork.GetRepository<EvalForm>()
            .GetAllQueryFiltered();

        if (IncludeCalcMethod)
            query = query.Include(d => d.CalcMethod);

        return await query.Where(f => f.Id == id).FirstOrDefaultAsync();
    }

    public async Task<FormEvalMatrix> GetFormEvalMatrix(Guid id)
    {
        return await unitOfWork.GetRepository<FormEvalMatrix>()
            .GetByIdAsync(id);
    }

    public async Task<List<FormEvalMatrixValue>> GetFormEvalMatrixValues(Guid id)
    {
        return await unitOfWork.GetRepository<FormEvalMatrixValue>()
            .GetAllActiveNonDeleted()
            .Where(x => x.FormEvalMatrixId == id)
            .OrderBy(x => x.OrderNo)
            .ToListAsync();
    }

    public async Task<List<FormItem>> GetFormItems(Guid formId)
    {
        var formItems = await unitOfWork.GetRepository<FormItem>()
                  .GetAllActiveNonDeleted()
                  .Where(s => s.EvalFormId == formId)
                  .Include(f => f.FormItemConfigs)
                  .Include(d => d.SubFormItems)
                  .Include(f => f.RelatedFrom)
                  .ThenInclude(y => y.RelatedItem)
                  .OrderBy(x => x.OrderNo)
                  .ToListAsync();

        return formItems;
    }

    public async Task<FormItem> GetFormItem(Guid Id)
    {
        return await unitOfWork.GetRepository<FormItem>()
            .GetAllQueryFiltered()
            .Include(d => d.SubFormItems)
            .Where(f => f.Id == Id!)
            .FirstOrDefaultAsync();
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
        if (form.Items.Count > 0)
            await unitOfWork.GetRepository<FormItemValue>().InsertRange(form.Items);
        if (form.SubItems.Count > 0)
            await unitOfWork.GetRepository<SubFormItemValue>().InsertRange(form.SubItems);
        await uow.CommitAsync();

        return form;
    }

    public async Task<FormItemValue> GetFormItemValue(Guid ValueId)
    {
        return await unitOfWork.GetRepository<FormItemValue>().GetByIDActiveNonDeleted(ValueId!);
    }

    public async Task<FormItemValue> GetFormItemValueByItemId(Guid ItemId)
    {
        return await unitOfWork.GetRepository<FormItemValue>().GetAllActiveNonDeleted().Where(x => x.FormItemId == ItemId).FirstOrDefaultAsync();
    }

    public async Task<SubFormItemValue> GetSubFormItemValue(Guid ValueId)
    {
        return await unitOfWork.GetRepository<SubFormItemValue>().GetByIDActiveNonDeleted(ValueId!);
    }

}