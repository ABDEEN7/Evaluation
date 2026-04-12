using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
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
                 .Skip(Page * 20)
                .Take(20)
                .ToListAsync();

        var result = mapper.Map<List<EvaluationFormDto>>(list, opts => opts.Items["Language"] = requestInfo.Lang);
        return result;


    }

    public async Task<List<EvaluationFormItemDto>> GetEvaluationFormItemList(Guid EvalformId)
    {
        var items = await uow.GetRepository<FormItem>()
    .GetAllNonDeleted()
    .Where(x => x.EvalFormId == EvalformId)
    .Include(x => x.SubFormItems)
    .OrderByDescending(x => x.CreateDate)
    .ToListAsync();   // <-- SQL stops here

        var relatedformitems = await uow.GetRepository<FormItemRelated>()
    .GetAllNonDeleted()
    .ToListAsync();   // memory

        var result = items.Select(x => new EvaluationFormItemDto
        {
            Id = x.Id,
            NameAr = x.NameAr,
            NameEn = x.NameEn,
            Min = x.Min,
            Max = x.Max,
            IsEvaluation = x.IsEvaluation,
            Weight = x.Weight,
            EvalFormId = x.EvalFormId,
            ScopeId = x.ScopeId,
            IsActive = x.IsActive,
            HasNote = x.HasNote,
            NoteRequired = x.NoteRequired,
            DropDownTypeId = x.DropDownTypeId,

            SubFormItems = x.SubFormItems
        .Where(s => s.IsDeleted == false)
        .Select(s => new EvaluationFormSubItemDto
        {
            Id = s.Id,
            FormItemId = s.FormItemId,
            NameAr = s.NameAr,
            NameEn = s.NameEn,
            IsOption = s.IsOption,
            DropDownTypeId = s.DropDownTypeId,
            IsActive = s.IsActive,
            HasNote = s.HasNote,
            NoteRequired = s.NoteRequired,
            OrderNo = s.OrderNo
        })
        .ToList(),

            FormItemRelated = relatedformitems
        .Where(r => r.IsDeleted == false && r.FormItemId == x.Id)
        .Select(r => r.RelatedItemId)
        .ToArray()
        })
.ToList();
        return result;

    }
    public async Task<List<FormScopeDTO>> GetAllFormScopeList(Guid formIdValue)
    {


        var list = await uow.GetRepository<FormScope>()
                .GetAllNonDeleted()
                .Where(x => x.EvalFormId == formIdValue)
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

        var result = mapper.Map<List<FormScopeDTO>>(list, opts => opts.Items["Language"] = requestInfo.Lang);
        return result;


    }
    public async Task<List<DropdownItem>> GetAllFormItemsFromDepartment(Guid EvalformId)
    {
        var Departmnentid = await uow.GetRepository<EvalForm>().GetAllNonDeleted().Include(x => x.FormEvalMatrix)
            .Where(x => x.Id == EvalformId)
            .Select(x => x.FormEvalMatrix!.DepartmentId)
            .FirstOrDefaultAsync();

        var result = await uow.GetRepository<FormItem>()
    .GetAllNonDeleted()
    .Include(x => x.EvalForm)
    .ThenInclude(x => x!.FormEvalMatrix)
    .Where(x => x.EvalFormId != EvalformId && x.EvalForm!.FormEvalMatrix!.DepartmentId == Departmnentid)
    .OrderByDescending(x => x.CreateDate)
    .Select(x => new DropdownItem
    {
        Id = x.Id,
        NameAr = x.NameAr,
        NameEn = x.NameEn
    }).ToListAsync();

        return result;



    }
    public async Task<bool> CheckEvaluationForm(Guid? evaluationId = null)
    {
        return await uow.GetRepository<EvalForm>()
            .GetAllActiveNonDeleted()
            .Where(x => x.IsFinalEval)
            .Where(x => x.EvaluationParties != null &&
                        x.EvaluationParties.DepartmentId == requestInfo.DepId)
            .AnyAsync(x => !evaluationId.HasValue || x.Id != evaluationId.Value);
    }
 
    public async Task<EvaluationFormDto> SaveEvaluationForm(EvaluationFormDto message)
    {


        EvalForm obj = new EvalForm();
        obj.EvalFormTypeId = message.EvalFormTypeId;
        obj.FormEvalMatrixId = message.FormEvalMatrixId;
        obj.NameAr = message.NameAr;
        obj.NameEn = message.NameEn;
        obj.HasOneValue = message.HasOneValue;
        obj.EvaluationPartyId = message.EvaluationPartyId;
        obj.HasEvaluation = message.HasEvaluation;
        obj.CalcMethodId = message.CalcMethodId;
        obj.FormStatusId = message.FormStatusId;
        obj.IsActive = message.IsActive;
        if (!await CheckEvaluationForm())
        {
            obj.IsFinalEval = message.IsFinalEval;
        }
        else
        {
            throw new BusinessException("لقد تجاوزت الحد الاعلى من الاستمارات التي تحتوي على IsFinalEvaluation");
        }
        uow.GetRepository<EvalForm>().Insert(obj);


        await uow.CommitAsync();
        var result = mapper.Map<EvaluationFormDto>(obj, opts => opts.Items["Language"] = requestInfo.Lang);
        result.ResponseStatus = DBResult.Inserted;
        return result;

    }
    public async Task<EvaluationFormDto> UpdateEvaluationForm(EvaluationFormDto message)
    {
        var result = new EvaluationFormDto();

        if (message.Id != null)
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
            obj.HasOneValue = message.HasOneValue;
            obj.EvaluationPartyId = message.EvaluationPartyId;
            obj.HasEvaluation = message.HasEvaluation;
            obj.CalcMethodId = message.CalcMethodId;
            obj.FormStatusId = message.FormStatusId;
            obj.IsActive = message.IsActive;
            if (!await CheckEvaluationForm(message.Id))
            {
                obj.IsFinalEval = message.IsFinalEval;
            }
            else
            {
                throw new BusinessException("لا يمكن ان يكون للدارة اكثر من استمارة بfinal evaluation");
            }
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
        obj.HasNote = message.HasNote;
        obj.NoteRequired = message.NoteRequired;
        obj.DropDownTypeId = message.DropDownTypeId;
        obj.IsActive = message.IsActive;

        uow.GetRepository<FormItem>().Insert(obj);
        //insert values to FormItemRelated
        if (message.FormItemRelated != null)
        {
            List<FormItemRelated> objentitylist = new List<FormItemRelated>();
            foreach (var item in message.FormItemRelated)
            {
                FormItemRelated objentity = new FormItemRelated();
                objentity.RelatedItemId = item;
                objentity.FormItemId = obj.Id;
                objentity.IsActive = true;
                objentitylist.Add(objentity);
            }
            if (objentitylist.Count > 0)
            {
                await uow.GetRepository<FormItemRelated>().InsertRange(objentitylist);
            }


        }

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
            obj.HasNote = message.HasNote;
            obj.NoteRequired = message.NoteRequired;
            obj.DropDownTypeId = message.DropDownTypeId;
            obj.IsActive = message.IsActive;
            uow.GetRepository<FormItem>().Update(obj);
            //update values to FormItemRelated
            List<FormItemRelated> objFormItemRelateddelete = await uow.GetRepository<FormItemRelated>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.FormItemId == obj.Id)
                                      .ToListAsync();

            var FormItemRelatedexistids = new List<Guid>();
            if (objFormItemRelateddelete.Count > 0)
            {
                foreach (var item in objFormItemRelateddelete)
                {
                    if (message.FormItemRelated != null && message.FormItemRelated.Contains(item.RelatedItemId))
                    {
                        FormItemRelatedexistids.Add(item.RelatedItemId);
                    }
                    else
                    {
                        uow.GetRepository<FormItemRelated>().Delete(item);
                    }

                }
            }
            if (message.FormItemRelated != null)
            {
                var notInSelected = message.FormItemRelated.Except(FormItemRelatedexistids).ToList();
                List<FormItemRelated> objentitylist = new List<FormItemRelated>();
                foreach (var item in notInSelected)
                {
                    FormItemRelated objentity = new FormItemRelated();
                    objentity.FormItemId = obj.Id;
                    objentity.RelatedItemId = item;
                    objentity.IsActive = true;
                    objentitylist.Add(objentity);
                }
                if (objentitylist.Count > 0)
                {
                    await uow.GetRepository<FormItemRelated>().InsertRange(objentitylist);
                }


            }
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

            var FormItemRelated = await uow.GetRepository<FormItemRelated>()
.GetAllNonDeleted()
                      .Where(x => x.FormItemId == obj.Id || x.RelatedItemId == obj.Id)
                      .ToListAsync();
            if (FormItemRelated.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.FormItemExistsFormItemRelated);
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
        obj.HasNote = message.HasNote;
        obj.NoteRequired = message.NoteRequired;
        obj.OrderNo = message.OrderNo;
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
            if (message.HasNote)
            {
                obj.HasNote = message.HasNote;
                obj.NoteRequired = message.NoteRequired;
            }
            else
            {
                obj.HasNote = false;
                obj.NoteRequired = false;
            }
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

    public async Task<FormScopeDTO> SaveFormScope(FormScopeDTO message)
    {


        FormScope obj = new FormScope();
        obj.EvalFormId = message.EvalFormId;
        obj.ScopeId = message.ScopeId;
        obj.Wegiht = message.Wegiht;
        obj.IsActive = message.IsActive;

        uow.GetRepository<FormScope>().Insert(obj);


        await uow.CommitAsync();
        var result = mapper.Map<FormScopeDTO>(obj, opts => opts.Items["Language"] = requestInfo.Lang);
        result.ResponseStatus = DBResult.Inserted;
        return result;

    }
    public async Task<FormScopeDTO> UpdateFormScope(FormScopeDTO message)
    {
        var result = new FormScopeDTO();

        if (message.Id != null)
        {
            FormScope obj = await uow.GetRepository<FormScope>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
            obj.EvalFormId = message.EvalFormId;
            obj.ScopeId = message.ScopeId;
            obj.Wegiht = message.Wegiht;
            obj.IsActive = message.IsActive;

            uow.GetRepository<FormScope>().Update(obj);
            await uow.CommitAsync();
            result = mapper.Map<FormScopeDTO>(obj, opts => opts.Items["Language"] = requestInfo.Lang);
            result.ResponseStatus = DBResult.Updated;
        }
        return result;

    }

    public async Task<FormScopeDTO> DeleteFormScope(Guid? Id)
    {




        var result = new FormScopeDTO();
        if (Id is not null)
        {
            FormScope obj = await uow.GetRepository<FormScope>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
            uow.GetRepository<FormScope>().Delete(obj);
            await uow.CommitAsync();
            result = mapper.Map<FormScopeDTO>(obj, opts => opts.Items["Language"] = requestInfo.Lang);
            result.ResponseStatus = DBResult.Deleted;
        }
        return result;


    }
}
