using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.BusinessLayer.Admin;

public class SrvFormEvalMatrixBL : AdminBase
{
    public SrvFormEvalMatrixBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
    {

    }


    public async Task<List<FormEvalMatrixDTO>> GetFormEvalMatrixList(int Page, int PageSize)
    {
        var list = await uow.GetRepository<FormEvalMatrix>()
            .GetAllNonDeleted()
            .Include(x => x.CreateBy)
            .OrderBy(x => x.OrderNo)
            .ThenByDescending(x => x.CreateDate)
            .Skip(Page * PageSize)
            .Take(PageSize)
            .ToListAsync();

        var result = mapper.Map<List<FormEvalMatrixDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
        return result;

    }

    public async Task<FormEvalMatrixDTO> SaveFormEvalMatrix(FormEvalMatrixDTO message)
    {

        var BackendName = await GenerateBackendNameByTitle(message.NameEn);
        var existBackendName = await uow
         .GetRepository<FormEvalMatrix>()
              .GetAllNonDeleted(x => x.BackenName == BackendName)
              .FirstOrDefaultAsync();

        if (existBackendName != null)
        {
            message.ResponseStatus = DBResult.BackendExist;
            return message;
        }

        FormEvalMatrix obj = new FormEvalMatrix();

        obj.NameAr = message.NameAr;
        obj.NameEn = message.NameEn;
        obj.BackenName = BackendName;
        obj.Startdate = message.Startdate;
        obj.EndDate = message.EndDate;
        obj.DepartmentId = message.DepartmentId;
        obj.IsActive = message.IsActive;

        uow.GetRepository<FormEvalMatrix>().Insert(obj);
        await uow.CommitAsync();
        var result = mapper.Map<FormEvalMatrixDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Inserted;
        return result;

    }
    public async Task<FormEvalMatrixDTO> UpdateFormEvalMatrix(FormEvalMatrixDTO message)
    {
        var result = new FormEvalMatrixDTO();

        if (message.Id is not null)
        {

            FormEvalMatrix obj = await uow.GetRepository<FormEvalMatrix>()
                              .GetAllNonDeleted()
                              .Include(x => x.CreateBy)
                              .Where(x => x.Id == message.Id)
                              .FirstAsync();

            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.BackenName = obj.BackenName;
            obj.Startdate = message.Startdate;
            obj.EndDate = message.EndDate;
            obj.DepartmentId = message.DepartmentId;
            obj.IsActive = message.IsActive;

            uow.GetRepository<FormEvalMatrix>().Update(obj);
            await uow.CommitAsync();
            result = mapper.Map<FormEvalMatrixDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.UpdateBy = userInfo.DBName;
            result.ResponseStatus = DBResult.Updated;
        }

        return result;

    }
    public async Task<bool> UpdateFormEvalMatrixOrder(List<OrderingDTO> message)
    {
        bool rtn = false;

        var updatedRows = from updatedItem in message
                          join rowToUpdate in uow.GetRepository<FormEvalMatrix>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                          select new { Row = rowToUpdate, updatedItem.OrderNo };

        updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

        await uow.CommitAsync();
        rtn = true;

        return rtn;
    }
    public async Task<FormEvalMatrixDTO> DeleteFormEvalMatrix(Guid? Id)
    {

        var result = new FormEvalMatrixDTO();
        if (Id is not null)
        {
            FormEvalMatrix obj = await uow.GetRepository<FormEvalMatrix>()
                              .GetAllNonDeleted()
                              .Where(x => x.Id == Id)
                              .FirstAsync();
            if (obj == null)
            {
                result.ResponseStatus = DBResult.NotFound;
                return result;
            }

            var evalForm = await uow.GetRepository<EvalForm>()
                  .GetAllNonDeleted()
                  .Where(x => x.FormEvalMatrixId == obj.Id)
                  .ToListAsync();

            if (evalForm.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.FormEvalMatrixExistsFormEval);
            }

            uow.GetRepository<FormEvalMatrix>().Delete(obj);
            await uow.CommitAsync();
            result = mapper.Map<FormEvalMatrixDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Deleted;
        }
        return result;

    }

}