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
}
