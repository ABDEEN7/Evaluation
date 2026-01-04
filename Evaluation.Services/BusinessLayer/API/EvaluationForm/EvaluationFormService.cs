using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;

public class EvaluationFormService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<EvaluationFormDto>> GetEvaluationFormList(int Page)
    {


        var list = await uow.GetRepository<EvalForm>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*20)
                .Take(20)
                .ToListAsync();

        var result = mapper.Map<List<EvaluationFormDto>>(list, opts => opts.Items["Language"] = requestInfo.Lang);
        return result;


    }

    public async Task<List<EvaluationFormItemDto>> GetEvaluationFormItemList(Guid EvalformId)
    {
        var result = await uow.GetRepository<FormItem>()
    .GetAllNonDeleted()
    .Where(x=>x.EvalFormId==EvalformId)
    .Include(x => x.CreateBy)
    .Include(x => x.SubFormItems)   // ← let EF load children
    .OrderByDescending(x => x.CreateDate)
    .Select(x => new EvaluationFormItemDto
    {
        Id = x.Id,
        NameAr = x.NameAr,
        NameEn = x.NameEn,
        Min = x.Min,
        Max = x.Max,
        IsEvaluation = x.IsEvaluation,
        Weight = x.Weight,
        EvalFormId = x.EvalFormId,
        CalcMethodId = x.CalcMethodId,
        ScopeId = x.ScopeId,
        IsActive = x.IsActive,
        HasNote = x.HasNote,
        DropDownTypeId = x.DropDownTypeId,

        SubFormItems = x.SubFormItems.Where(x=>x.IsDeleted==false).Select(z => new EvaluationFormSubItemDto
        {
            Id = z.Id,
            FormItemId = z.FormItemId,
            NameAr = z.NameAr,
            NameEn = z.NameEn,
            IsOption = z.IsOption,
            DropDownTypeId = z.DropDownTypeId,
            IsActive = z.IsActive
        }).ToList()
    })
    .ToListAsync();

        return result;



    }

    public async Task<EvaluationFormDto> SaveEvaluationForm(EvaluationFormDto message)
    {


        EvalForm obj = new EvalForm();
        obj.EvalFormTypeId = message.EvalFormTypeId;
        obj.FormEvalMatrixId = message.FormEvalMatrixId;
        obj.NameAr = message.NameAr;
        obj.NameEn = message.NameEn;
        obj.IsRopric = message.IsRopric;
        obj.HasOneValue = message.HasOneValue;
        obj.EvaluationPartyId = message.EvaluationPartyId;
        obj.HasEvaluation = message.HasEvaluation;
        obj.CalcMethodId = message.CalcMethodId;
        obj.FormStatusId = message.FormStatusId;
        obj.IsActive = message.IsActive;

        uow.GetRepository<EvalForm>().Insert(obj);


        await uow.CommitAsync();
        var result = mapper.Map<EvaluationFormDto>(obj, opts => opts.Items["Language"] = requestInfo.Lang);
        result.ResponseStatus = DBResult.Inserted;
        return result;

    }
    public async Task<EvaluationFormDto> UpdateEvaluationForm(EvaluationFormDto message)
    {
        var result = new EvaluationFormDto();

        if (message.Id!=null)
        {
            EvalForm obj = await uow.GetRepository<EvalForm>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
            obj.EvalFormTypeId = message.EvalFormTypeId;
            obj.FormEvalMatrixId = message.FormEvalMatrixId;
            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.IsRopric = message.IsRopric;
            obj.HasOneValue = message.HasOneValue;
            obj.EvaluationPartyId = message.EvaluationPartyId;
            obj.HasEvaluation = message.HasEvaluation;
            obj.CalcMethodId = message.CalcMethodId;
            obj.FormStatusId = message.FormStatusId;
            obj.IsActive = message.IsActive;
            uow.GetRepository<EvalForm>().Update(obj);
            await uow.CommitAsync();
            result = mapper.Map<EvaluationFormDto>(obj, opts => opts.Items["Language"] = requestInfo.Lang);
            result.ResponseStatus = DBResult.Updated;
        }
        return result;

    }

    public async Task<EvaluationFormDto> DeleteEvaluationForm(Guid? Id)
    {




        var result = new EvaluationFormDto();
        if (Id is not null)
        {
            EvalForm obj = await uow.GetRepository<EvalForm>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
            

            var FormItem = await uow.GetRepository<FormItem>()
.GetAllNonDeleted()
                      .Where(x => x.EvalFormId == obj.Id)
                      .ToListAsync();
            if (FormItem.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.EvalFormExistsFormItem);
            }
            var FormScope = await uow.GetRepository<FormScope>()
.GetAllNonDeleted()
                      .Where(x => x.EvalFormId == obj.Id)
                      .ToListAsync();
            if (FormScope.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.EvalFormExistsFormScope);
            }
            uow.GetRepository<EvalForm>().Delete(obj);
            await uow.CommitAsync();
            result = mapper.Map<EvaluationFormDto>(obj, opts => opts.Items["Language"] = requestInfo.Lang);
            result.ResponseStatus = DBResult.Deleted;
        }
        return result;


    }

    public async Task<EvaluationFormItemDto> SaveEvaluationFormItem(EvaluationFormItemDto message)
    {


        FormItem obj = new FormItem();
        obj.NameAr = message.NameAr;
        obj.NameEn = message.NameEn;
        obj.Min = message.Min;
        obj.Max = message.Max;
        obj.IsEvaluation = message.IsEvaluation;
        obj.Weight = message.Weight;
        obj.EvalFormId = message.EvalFormId;
        obj.ScopeId = message.ScopeId;
        obj.CalcMethodId = message.CalcMethodId;
        obj.HasNote = message.HasNote;
        obj.DropDownTypeId = message.DropDownTypeId;
        obj.IsActive = message.IsActive;

        uow.GetRepository<FormItem>().Insert(obj);


        await uow.CommitAsync();
        message.Id = obj.Id;
        message.ResponseStatus = DBResult.Inserted;
        return message;

    }
    public async Task<EvaluationFormItemDto> UpdateEvaluationFormItem(EvaluationFormItemDto message)
    {
        if (message.Id != null)
        {
            FormItem obj = await uow.GetRepository<FormItem>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.Min = message.Min;
            obj.Max = message.Max;
            obj.IsEvaluation = message.IsEvaluation;
            obj.Weight = message.Weight;
            obj.EvalFormId = obj.EvalFormId;
            obj.ScopeId = message.ScopeId;
            obj.CalcMethodId = message.CalcMethodId;
            obj.HasNote = message.HasNote;
            obj.DropDownTypeId = message.DropDownTypeId;
            obj.IsActive = message.IsActive;
            uow.GetRepository<FormItem>().Update(obj);
            await uow.CommitAsync();
            message.ResponseStatus = DBResult.Updated;
        }
        return message;

    }

    public async Task<EvaluationFormItemDto> DeleteEvaluationFormItem(Guid? Id)
    {




        var result = new EvaluationFormItemDto();
        if (Id is not null)
        {
            FormItem obj = await uow.GetRepository<FormItem>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();


            var FormItemValues = await uow.GetRepository<FormItemValue>()
.GetAllNonDeleted()
                      .Where(x => x.FormItemId == obj.Id)
                      .ToListAsync();
            if (FormItemValues.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.FormItemExistsFormItemValues);
            }
            var SubFormItem = await uow.GetRepository<SubFormItem>()
.GetAllNonDeleted()
                      .Where(x => x.FormItemId == obj.Id)
                      .ToListAsync();
            if (SubFormItem.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.FormItemExistsSubFormItem);
            }
            uow.GetRepository<FormItem>().Delete(obj);
            await uow.CommitAsync();
            result = mapper.Map<EvaluationFormItemDto>(obj, opts => opts.Items["Language"] = requestInfo.Lang);
            result.ResponseStatus = DBResult.Deleted;
        }
        return result;


    }

    public async Task<EvaluationFormSubItemDto> SaveEvaluationSubFormItem(EvaluationFormSubItemDto message)
    {


        SubFormItem obj = new SubFormItem();
        obj.NameAr = message.NameAr;
        obj.NameEn = message.NameEn;
        obj.FormItemId = message.FormItemId;
        obj.IsOption = message.IsOption;
        obj.DropDownTypeId = message.DropDownTypeId;
        obj.IsActive = message.IsActive;

        uow.GetRepository<SubFormItem>().Insert(obj);


        await uow.CommitAsync();
        message.Id = obj.Id;
        message.ResponseStatus = DBResult.Inserted;
        return message;

    }
    public async Task<EvaluationFormSubItemDto> UpdateEvaluationSubFormItem(EvaluationFormSubItemDto message)
    {
        if (message.Id != null)
        {
            SubFormItem obj = await uow.GetRepository<SubFormItem>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.FormItemId = obj.FormItemId;
            obj.IsOption = message.IsOption;
            obj.DropDownTypeId = message.DropDownTypeId;
            obj.IsActive = message.IsActive;
            uow.GetRepository<SubFormItem>().Update(obj);
            await uow.CommitAsync();
            message.ResponseStatus = DBResult.Updated;
        }
        return message;

    }

    public async Task<EvaluationFormSubItemDto> DeleteEvaluationSubFormItem(Guid? Id)
    {




        var result = new EvaluationFormSubItemDto();
        if (Id is not null)
        {
            SubFormItem obj = await uow.GetRepository<SubFormItem>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
            uow.GetRepository<SubFormItem>().Delete(obj);
            await uow.CommitAsync();
            result = mapper.Map<EvaluationFormSubItemDto>(obj, opts => opts.Items["Language"] = requestInfo.Lang);
            result.ResponseStatus = DBResult.Deleted;
        }
        return result;


    }
}
