using AutoMapper;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Extensions;
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
    public async Task<EvalForm?> GetEvalForm(Guid id)
    {
        using var scope = serviceProvider.CreateScopedUow();

		var result= await scope.GetRepository<EvalForm>()
            .GetAllQueryFiltered(x=>x.Id== id)
            .Include(x=>x.FormItems)
            .ThenInclude(x=>x.FormItemValues)
            .FirstOrDefaultAsync();

        return result;

	}

    public async Task<EvalForm?> GetEvalForm(Guid id, bool IncludeCalcMethod)
    {
		using var scope = serviceProvider.CreateScopedUow();
		var query = scope.GetRepository<EvalForm>()
            .GetAllQueryFiltered();

        if (IncludeCalcMethod)
            query = query.Include(d => d.CalcMethod);

        return await query.Where(f => f.Id == id).FirstOrDefaultAsync();
    }

    public async Task<FormEvalMatrix?> GetFormEvalMatrix(Guid id)
    {
		using var scope = serviceProvider.CreateScopedUow();

		var matrix= await scope.GetRepository<FormEvalMatrix>()
            .GetByIdAsync(id);
        return matrix;

	}

    public async Task<List<FormEvalMatrixValue>> GetFormEvalMatrixValues(Guid id)
    {
		using var scope = serviceProvider.CreateScopedUow();

		var result= await scope.GetRepository<FormEvalMatrixValue>()
            .GetAllActiveNonDeleted()
            .Where(x => x.FormEvalMatrixId == id)
            .OrderBy(x => x.OrderNo)
            .ToListAsync();
        return result;
    }

    public async Task<List<FormItem>> GetFormItems(Guid formId)
    {
		using var scope = serviceProvider.CreateScopedUow();

		var formItems= await scope.GetRepository<FormItem>()
			.GetAllActiveNonDeleted()
			.Where(s => s.EvalFormId == formId)
			.Include(f => f.FormItemConfigs)
			.Include(x => x.SubFormItems)
				.ThenInclude(x => x.DropDownType)
					.ThenInclude(x => x.FieldDropDownValues)
			.Include(f => f.RelatedFrom)
				.ThenInclude(y => y.RelatedItem)
			.OrderBy(x => x.OrderNo)
			.ToListAsync();

		return formItems;
    }

    public async Task<FormItem?> GetFormItem(Guid Id)
    {
		using var scope = serviceProvider.CreateScopedUow();

		return await scope.GetRepository<FormItem>()
            .GetAllQueryFiltered()
            .Include(d => d.SubFormItems)
            .Where(f => f.Id == Id!)
            .FirstOrDefaultAsync();
    }

    public async Task<FormEvaluationValue> SaveFormItemsAndSubs(FormEvaluationValue form)
    {
        if (form.Items?.Any() == true)
            await unitOfWork.GetRepository<FormItemValue>().InsertRange(form.Items);
        if (form.SubItems?.Any() == true)
            await unitOfWork.GetRepository<SubFormItemValue>().InsertRange(form.SubItems);
        //await uow.CommitAsync();

        return form;
    }

    public async Task<FormItemValue?> GetFormItemValue(Guid ValueId)
    {
		using var scope = serviceProvider.CreateScopedUow();

		return await scope.GetRepository<FormItemValue>().GetByIDActiveNonDeleted(ValueId!);
    }

    public async Task<FormItemValue?> GetFormItemValueByItemId(Guid ItemId)
    {
		using var scope = serviceProvider.CreateScopedUow();

		return await scope.GetRepository<FormItemValue>().GetAllActiveNonDeleted().Where(x => x.FormItemId == ItemId).FirstOrDefaultAsync();
    }

    public async Task<SubFormItemValue?> GetSubFormItemValue(Guid ValueId)
    {
		using var scope = serviceProvider.CreateScopedUow();

		return await scope.GetRepository<SubFormItemValue>().GetByIDActiveNonDeleted(ValueId!);
    }

}