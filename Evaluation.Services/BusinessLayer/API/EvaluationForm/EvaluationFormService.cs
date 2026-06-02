using AutoMapper;
using Azure.Core;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

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
    public async Task<PaginatedResult<TemplateFormDto>> GetEvaluationFormList(SearchTemplateForm request)
    {
        var query = uow.GetRepository<EvalForm>()
                .GetAllNonDeleted(x => x.EvaluationParties != null
                && x.EvaluationParties.DepartmentId == requestInfo.DepId)
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                .AsNoTracking();
        var pageSizeString = await cacheDataProvider.GetSystemSettingValue(EvalFormSettings.Eval_WEB_From_PageSize);
        int pageSize = int.TryParse(pageSizeString, out var value) ? value : 20;
        var result = await query.GetPaginatedResult(request.PageNumber, pageSize);
        var dto = mapper.Map<PaginatedResult<TemplateFormDto>>(result);
        //var dto = mapper.Map<PaginatedResult<TemplateFormDto>>(result, opts => opts.Items["Language"] = requestInfo.Lang);
        return dto;
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
            Weight = x.Weight,
            EvalFormId = x.EvalFormId,
            ScopeId = x.ScopeId,
            IsActive = x.IsActive,
            HasNote = x.HasNote,
            NoteRequired = x.NoteRequired,
            DropDownTypeId = x.DropDownTypeId,
            HasMulitEvaluation = x.HasMuliEvaluation,
            SubFormItems = x.SubFormItems
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
        .Where(r => r.FormItemId == x.Id)
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
            .Include(x=>x.EvaluationParties)
            .Where(x => x.IsFinalEval)
            .Where(x => x.EvaluationParties != null &&
                        x.EvaluationParties.DepartmentId == requestInfo.DepId)
            .AnyAsync(x => !evaluationId.HasValue || x.Id != evaluationId.Value);
    }

    public async Task<TemplateFormDto> SaveEvaluationForm(TemplateFormDto message)
    {


        EvalForm obj = new EvalForm();
        obj.EvalFormTypeId = message.EvalFormTypeId;
        obj.FormEvalMatrixId = message.FormEvalMatrixId;
        obj.NameAr = message.NameAr;
        obj.NameEn = message.NameEn;
        obj.HasOneValue = message.HasOneValue;
        obj.EvaluationPartyId = message.EvaluationPartyId;
        obj.CalcMethodId = message.CalcMethodId;
        obj.IsActive = message.IsActive;
        obj.AllowRename = message.AllowRename;
        obj.HasMuliEvaluation = message.HasMuliEvaluation;
        if (message.HasMuliEvaluation)
        {
            obj.CountOfColumnsValue = message.EvalCountOfColumnsValue.Value;
        }
        if (message.IsFinalEval)
        {
            if (!await CheckEvaluationForm(message.Id))
            {
                obj.IsFinalEval = message.IsFinalEval;
                obj.FinalEvalMatrixId = message.FinalEvalMatrixId;
            }
            else
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.Max_Final_Evaluation_Forms_Exceeded);
            }
        }

        if (message.IsFinalEval)
        {
            obj.HasOneValue = true;
        }
        else
        {
            obj.HasOneValue = message.HasOneValue;
        }
        uow.GetRepository<EvalForm>().Insert(obj);


        await uow.CommitAsync();
        var result = mapper.Map<TemplateFormDto>(obj);
        result.ResponseStatus = DBResult.Inserted;
        return result;

    }
    public async Task<TemplateFormDto> UpdateEvaluationForm(TemplateFormDto message)
    {
        var result = new TemplateFormDto();

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
            obj.EvaluationPartyId = message.EvaluationPartyId;
            obj.CalcMethodId = message.CalcMethodId;
            obj.IsActive = message.IsActive;
            obj.AllowRename = message.AllowRename;
            obj.HasMuliEvaluation = message.HasMuliEvaluation;
            if (message.HasMuliEvaluation)
            {
                obj.CountOfColumnsValue = message.EvalCountOfColumnsValue.Value;
            }
            if (!await CheckEvaluationForm(message.Id))
            {
                obj.IsFinalEval = message.IsFinalEval;
                obj.FinalEvalMatrixId = message.FinalEvalMatrixId;
            }
            else
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.Final_Evaluation_Form_Limit);
            }
            if (message.IsFinalEval)
            {
                obj.HasOneValue = true;
            }
            else
            {
                obj.HasOneValue = message.HasOneValue;
            }
            uow.GetRepository<EvalForm>().Update(obj);
            await uow.CommitAsync();
            result = mapper.Map<TemplateFormDto>(obj, opts => opts.Items["Language"] = requestInfo.Lang);
            result.ResponseStatus = DBResult.Updated;
        }
        return result;

    }

    public async Task<TemplateFormDto> DeleteEvaluationForm(Guid? Id)
    {




        var result = new TemplateFormDto();
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
            result = mapper.Map<TemplateFormDto>(obj, opts => opts.Items["Language"] = requestInfo.Lang);
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
        obj.Weight = message.Weight;
        obj.EvalFormId = message.EvalFormId;
        obj.ScopeId = message.ScopeId;
        obj.HasNote = message.HasNote;
        obj.NoteRequired = message.NoteRequired;
        obj.DropDownTypeId = message.DropDownTypeId;
        obj.IsActive = message.IsActive;
        obj.HasMuliEvaluation = message.HasMulitEvaluation;

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
            obj.Weight = message.Weight;
            obj.EvalFormId = obj.EvalFormId;
            obj.ScopeId = message.ScopeId;
            obj.HasNote = message.HasNote;
            obj.NoteRequired = message.NoteRequired;
            obj.DropDownTypeId = message.DropDownTypeId;
            obj.IsActive = message.IsActive;
            obj.HasMuliEvaluation = message.HasMulitEvaluation;
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
    public async Task<List<FormItemConfigDto>> GetAllFormItemConfigAsync(Guid? EvalformId)
    {
        var result = await uow.GetRepository<FormItemConfig>()
            .GetAllNonDeleted()
            .Where(x => x.EvalFormId == EvalformId)
            .GroupBy(x => new
            {
                x.EvalFormId,
                x.PartyTypeId
            })
            .Select(g => new FormItemConfigDto
            {
                Id = g.First().Id,

                FormItemConfig_EvalFormId = g.Key.EvalFormId,
                FormItemConfig_PartyTypeId = g.Key.PartyTypeId,

                FormItemConfig_NameAr = g.First().NameAr,
                FormItemConfig_NameEn = g.First().NameEn,
                FormItemConfig_CalcMethodId = g.First().CalcMethodId,
                FormItemConfig_Percentage = g.First().WeightPercentage,

                FormItemConfig_FormItemIds = g
                    .Where(x => x.FormItemId.HasValue)
                    .Select(x => x.FormItemId.Value)
                    .ToList()
            })
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return result;
    }
    public async Task<CreateFormItemConfigDto> SaveFormItemConfig(
     List<CreateFormItemConfigDto> messages)
    {
        if (messages == null || !messages.Any())
            throw new ArgumentException(
                ConstantKeys.ExceptionMessage.Exception_No_Data_Provided);

        var repo = uow.GetRepository<FormItemConfig>();
        var formItemRepo = uow.GetRepository<FormItem>();


        var hasFormItems = messages.Any(x =>
            x.FormItemIds != null &&
            x.FormItemIds.Any());

        // =====================================================
        // PATH 1:
        // No FormItemIds + NOT MultiValue
        // =====================================================

        if (!hasFormItems)
        {
            if (messages.Sum(x => x.Percentage) != 100)
            {
                throw new BusinessException(
                    ConstantKeys.ExceptionMessage
                        .FormItemConfigPercentageMax);
            }
        }
        var formItemsConfig = new List<FormItemConfig>();


        foreach (var message in messages)
        {
            // =====================================================
            // MultiValue Path
            // =====================================================

            if (message.EvalFormHasMuliEvaluation)
            {
                var formItemIds = await formItemRepo
                    .GetAllActiveNonDeleted()
                    .Where(x => x.EvalFormId == message.EvalFormId)
                    .Select(x => x.Id)
                    .ToListAsync();

                formItemsConfig.AddRange(
                    formItemIds.Select(id => new FormItemConfig
                    {
                        EvalFormId = message.EvalFormId,
                        FormItemId = id,
                        NameAr = message.NameAr,
                        NameEn = message.NameEn,
                        CalcMethodId = message.CalcMethodId,
                        WeightPercentage = message.Percentage
                    }));

                continue;
            }
            // =====================================================
            // Normal Path
            // =====================================================
            var ids =
                message.FormItemIds == null ||
                !message.FormItemIds.Any()
                    ? new Guid?[] { null }
                    : message.FormItemIds.Select(x => (Guid?)x);

            formItemsConfig.AddRange(
                ids.Select(id => new FormItemConfig
                {
                    EvalFormId = message.EvalFormId,
                    PartyTypeId = message.PartyTypeId,
                    FormItemId = id,
                    NameAr = message.NameAr,
                    NameEn = message.NameEn,
                    CalcMethodId = message.CalcMethodId,
                    WeightPercentage = message.Percentage
                }));
        }
        // =====================================================
        // PATH 2:
        // Has FormItemIds
        // =====================================================
        if (hasFormItems)
        {
            var invalid = formItemsConfig
                .Where(x => x.FormItemId != null)
                .GroupBy(x => x.FormItemId)
                .Where(g => g.Sum(x => x.WeightPercentage) != 100)
                .Any();

            if (invalid)
            {
                throw new BusinessException(
                    ConstantKeys.ExceptionMessage
                        .FormItemConfigPercentageMax);
            }


            var hasDuplicates = formItemsConfig
                .GroupBy(x => new
                {
                    x.FormItemId,
                    x.PartyTypeId,
                    x.EvalFormId
                })
                .Any(g => g.Count() > 1);

            if (hasDuplicates)
            {
                throw new InvalidOperationException(
                    ConstantKeys.ExceptionMessage
                        .DuplicateRecordsinRequest);
            }
        }


        var evalFormId = formItemsConfig.First().EvalFormId;
        var partyTypeId = formItemsConfig.First().PartyTypeId;


        var existing = await repo.GetAllActiveNonDeleted()
            .Where(x =>
                x.EvalFormId == evalFormId &&
                x.PartyTypeId == partyTypeId)
            .ToListAsync();


        HashSet<string> newKeys;

        Dictionary<string, FormItemConfig> existingMap;


        if (hasFormItems)
        {
            existingMap = existing.ToDictionary(
                x => $"{x.FormItemId}_{x.PartyTypeId}_{x.EvalFormId}");

            newKeys = formItemsConfig
                .Select(x =>
                    $"{x.PartyTypeId}_{x.EvalFormId}_{x.NameEn}_{x.NameAr}")
                .ToHashSet();
        }
        else
        {
            existingMap = existing.ToDictionary(
            x => $"{x.PartyTypeId}_{x.EvalFormId}_{x.NameEn}_{x.NameAr}");

            newKeys = formItemsConfig
    .Select(x =>
        $"{x.PartyTypeId}_{x.EvalFormId}_{x.NameEn}_{x.NameAr}")
    .ToHashSet();
        }


        var toInsert = new List<FormItemConfig>();


        foreach (var item in formItemsConfig)
        {
            var key = hasFormItems
                ? $"{item.FormItemId}_{item.PartyTypeId}_{item.EvalFormId}"
                : $"{item.PartyTypeId}_{item.EvalFormId}_{item.NameEn}_{item.NameAr}";

            if (existingMap.TryGetValue(key, out var existingItem))
            {
                existingItem.NameAr = item.NameAr;
                existingItem.NameEn = item.NameEn;
                existingItem.CalcMethodId = item.CalcMethodId;
                existingItem.WeightPercentage =
                    item.WeightPercentage;
            }
            else
            {
                toInsert.Add(item);
            }
        }


        var toDelete = existing
            .Where(x =>
            {
                var key = hasFormItems
                    ? $"{x.FormItemId}_{x.PartyTypeId}_{x.EvalFormId}"
                    : $"{x.PartyTypeId}_{x.EvalFormId}_{x.NameEn}_{x.NameAr}";

                return !newKeys.Contains(key);
            })
            .ToList();


        if (toDelete.Any())
            repo.DeleteRange(toDelete);

        if (toInsert.Any())
            await repo.InsertRange(toInsert);

        await uow.CommitAsync();


        return new CreateFormItemConfigDto
        {
            ResponseStatus = DBResult.Inserted
        };
    }
    public async Task<CreateFormItemConfigDto> UpdateFormItemConfig(CreateFormItemConfigDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var repo = uow.GetRepository<FormItemConfig>();

        var existingRecord = await repo.GetAllActiveNonDeleted()
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (existingRecord == null)
            throw new InvalidOperationException(ConstantKeys.ExceptionMessage.RECORD_NOT_FOUND);

        var otherRecords = await repo.GetAll()
            .Where(x =>
                x.EvalFormId == existingRecord.EvalFormId &&
                x.PartyTypeId == existingRecord.PartyTypeId &&
                x.Id != existingRecord.Id)
            .ToListAsync();

        var total = await repo.GetAll()
    .Where(x =>
        x.EvalFormId == existingRecord.EvalFormId &&
        x.PartyTypeId == existingRecord.PartyTypeId &&
        x.FormItemId == existingRecord.FormItemId &&
        x.Id != existingRecord.Id)
    .SumAsync(x => (decimal?)x.WeightPercentage) ?? 0;

        total += dto.Percentage;

        if (total != 100)
            throw new BusinessException(
                ConstantKeys.ExceptionMessage.FormItemConfigPercentageMax
            );
        existingRecord.NameAr = dto.NameAr;
        existingRecord.NameEn = dto.NameEn;
        existingRecord.CalcMethodId = dto.CalcMethodId;
        existingRecord.WeightPercentage = dto.Percentage;

        repo.Update(existingRecord);

        await uow.CommitAsync();
        var result = mapper.Map<CreateFormItemConfigDto>(existingRecord);
        result.ResponseStatus = DBResult.Updated;
        return result;
    }
    public async Task<ResponseDto> DeleteFormItemConfig(Guid id)
    {
        var repo = uow.GetRepository<FormItemConfig>();
        var records = await repo.GetAllActiveNonDeleted(x => x.EvalFormId == id)
            .ToListAsync();

        if (records == null)
            throw new InvalidOperationException(ConstantKeys.ExceptionMessage.RECORD_NOT_FOUND);
        repo.DeleteRange(records);
        await uow.CommitAsync();

        return new ResponseDto
        {
            ResponseStatus = DBResult.Deleted,
            ResponseState = true
        };
    }
}