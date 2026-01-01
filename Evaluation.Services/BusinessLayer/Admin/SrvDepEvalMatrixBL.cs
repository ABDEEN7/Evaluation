using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.EvalResult;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.Admin;

public class SrvDepEvalMatrixBL : AdminBase
{
    public SrvDepEvalMatrixBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
    {

    }


    public async Task<List<DepEvalMatrixDTO>> GetDepEvalMatrixList(int Page, int PageSize)
    {
        var list = await uow.GetRepository<DepEvalMatrix>()
            .GetAllNonDeleted()
            .Include(x => x.CreateBy)
            .Skip(Page * PageSize)
            .Take(PageSize)
            .ToListAsync();

        var result = mapper.Map<List<DepEvalMatrixDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
        return result;

    }

    public async Task<DepEvalMatrixDTO> SaveDepEvalMatrix(DepEvalMatrixDTO message)
    {

        var BackendName = await GenerateBackendNameByTitle(message.NameEn);
        var existBackendName = await uow
         .GetRepository<DepEvalMatrix>()
              .GetAllNonDeleted(x => x.BackendName == BackendName)
              .FirstOrDefaultAsync();

        if (existBackendName != null)
        {
            message.ResponseStatus = DBResult.BackendExist;
            return message;
        }

        DepEvalMatrix obj = new DepEvalMatrix();

        obj.NameAr = message.NameAr;
        obj.NameEn = message.NameEn;
        obj.BackendName = BackendName;
        obj.ItemValue = message.ItemValue;
        obj.MaxValue= message.MaxValue;
        obj.MinValue= message.MinValue;
        obj.NextEvaluationDays= message.NextEvaluationDays;
        obj.NextFollowUpDays= message.NextFollowUpDays;
        obj.AcademicYearId = message.AcademicYearId;
        obj.IsActive = message.IsActive;
        obj.RequiredFollowUp = message.RequiredFollowUp;

        uow.GetRepository<DepEvalMatrix>().Insert(obj);
        await uow.CommitAsync();
        var result = mapper.Map<DepEvalMatrixDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Inserted;
        return result;

    }
    public async Task<DepEvalMatrixDTO> UpdateDepEvalMatrix(DepEvalMatrixDTO message)
    {
        var result = new DepEvalMatrixDTO();

        if (message.Id is not null)
        {

            DepEvalMatrix obj = await uow.GetRepository<DepEvalMatrix>()
                              .GetAllNonDeleted()
                              .Include(x => x.CreateBy)
                              .Where(x => x.Id == message.Id)
                              .FirstAsync();

            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.BackendName = obj.BackendName;
            obj.ItemValue = message.ItemValue;
            obj.MaxValue = message.MaxValue;
            obj.MinValue = message.MinValue;
            obj.NextEvaluationDays = message.NextEvaluationDays;
            obj.NextFollowUpDays = message.NextFollowUpDays;
            obj.AcademicYearId = message.AcademicYearId;
            obj.IsActive = message.IsActive;
            obj.RequiredFollowUp = message.RequiredFollowUp;

            uow.GetRepository<DepEvalMatrix>().Update(obj);
            await uow.CommitAsync();
            result = mapper.Map<DepEvalMatrixDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.UpdateBy = userInfo.DBName;
            result.ResponseStatus = DBResult.Updated;
        }

        return result;

    }

    public async Task<DepEvalMatrixDTO> DeleteDepEvalMatrix(Guid? Id)
    {

        var result = new DepEvalMatrixDTO();
        if (Id is not null)
        {
            DepEvalMatrix obj = await uow.GetRepository<DepEvalMatrix>()
                              .GetAllNonDeleted()
                              .Where(x => x.Id == Id)
                              .FirstAsync();
            if (obj == null)
            {
                result.ResponseStatus = DBResult.NotFound;
                return result;
            }

            var evalForm = await uow.GetRepository<FormItemValue>()
                 .GetAllNonDeleted()
                 .Where(x => x.DepEvalMatrixId == obj.Id)
                 .ToListAsync();

            if (evalForm.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.DepEvalMatrixExistsFormItemValue);
            }


            var orgEvalResult = await uow.GetRepository<OrgEvalResult>()
              .GetAllNonDeleted()
              .Where(x => x.DepEvalMatrixId == obj.Id)
              .ToListAsync();

            if (evalForm.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.DepEvalMatrixExistsOrgEvalResults);
            }


            uow.GetRepository<DepEvalMatrix>().Delete(obj);
            await uow.CommitAsync();
            result = mapper.Map<DepEvalMatrixDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Deleted;
        }
        return result;

    }

}