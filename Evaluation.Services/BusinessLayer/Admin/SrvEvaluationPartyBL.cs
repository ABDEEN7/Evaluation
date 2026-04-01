using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.Planing.TeamsModule;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.UserEntiy;
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

public class SrvEvaluationPartyBL : AdminBase
{
    public SrvEvaluationPartyBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
    {
    }
    public async Task<List<EvaluationPartyDTO>> GetEvaluationPartyList(int page, int pageSize)
    {
        var list = await uow.GetRepository<EvaluationParty>()
            .GetAllActiveNonDeleted()
            .Include(x => x.CreateBy)
            .OrderByDescending(x => x.OrderNo)
            .ThenByDescending(x => x.CreateDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var result = mapper.Map<List<EvaluationPartyDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
        return result;
    }

    public async Task<List<PartyTypeEvalPartyStatusDetailDTO>> GetAllPartyTypeEvalPartyStatusList(Guid EvaluationPartyId)
    {
        var result = await uow.GetRepository<PartyTypeEvalParty>()
    .GetAllNonDeleted()
    .Where(x => x.EvaluationPartyId == EvaluationPartyId)
    .GroupJoin(
        uow.GetRepository<PartyTypeEvalPartyStatus>().GetAllNonDeleted(),
        ut => ut.Id,
        uts => uts.PartyTypeEvalPartyId,
        (ut, ServiceStatuses) => new PartyTypeEvalPartyStatusDetailDTO
        {
            Id=ut.Id,
            ServiceStatus = ServiceStatuses.Select(s => s.ServiceStatusId).ToArray(),
            PartyType = ut.PartyTypeId,
            IsActive = ut.IsActive,

        })
    .ToListAsync();
        return result;
    }
    public async Task<List<DropdownItem>> GetPartyTypeList()
    {

        var result = await uow.GetRepository<PartyType>()
                    .GetAllActiveNonDeleted()
                    .Select(x=>new DropdownItem
                    {
                        Id=x.Id,
                        Name=_requestInfo.Lang=="ar"?x.NameAr:x.NameEn
                    })
                    .ToListAsync();
        return result;
    }
    public async Task<List<DropdownItem>> GetServiceStatusList()
    {

        var result = await uow.GetRepository<ServiceStatus>()
                    .GetAllActiveNonDeleted()
                    .Select(x=>new DropdownItem
                    {
                        Id=x.Id,
                        Name=_requestInfo.Lang=="ar"?x.NameAr:x.NameEn
                    })
                    .ToListAsync();
        return result;
    }
    public async Task<EvaluationPartyDTO> SaveEvaluationParty(EvaluationPartyDTO message)
    {
        EvaluationParty EvaluationParty = new EvaluationParty();
        EvaluationParty.NameEn = message.NameEn;
        EvaluationParty.NameAr = message.NameAr;
        EvaluationParty.IsActive = message.IsActive;
        EvaluationParty.DepartmentId = message.DepartmentId;
        EvaluationParty.IsSupportFiles = message.IsSupportFiles;

        uow.GetRepository<EvaluationParty>().Insert(EvaluationParty);
        await uow.CommitAsync();
        var result = mapper.Map<EvaluationPartyDTO>(EvaluationParty, opts => opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Inserted;
        return result;
    }
    public async Task<EvaluationPartyDTO> DeleteEvaluationPartyAsync(Guid? id)
    {
        var repository = uow.GetRepository<EvaluationParty>();
        var EvaluationParty = await repository
            .GetAllActiveNonDeleted(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (EvaluationParty == null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.DepartmentEvaluationParty);
        repository.Delete(EvaluationParty);
        await uow.CommitAsync();
        var result = mapper.Map<EvaluationPartyDTO>(EvaluationParty, opts =>
        opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Deleted;
        return result;
    }
    public async Task<EvaluationPartyDTO> UpdateEvaluationParty(EvaluationPartyDTO EvaluationParty)
    {
        if (EvaluationParty == null)
        {
            return new EvaluationPartyDTO
            {
                ResponseStatus = DBResult.Error
            };
        }
        if (EvaluationParty.Id == null)
        {
            return new EvaluationPartyDTO
            {
                ResponseStatus = DBResult.Error
            };
        }
        var repository = uow.GetRepository<EvaluationParty>();
        var response = await uow
            .GetRepository<EvaluationParty>()
            .GetAllActiveNonDeleted()
            .FirstOrDefaultAsync(x => x.Id == EvaluationParty.Id);
        response.NameAr = EvaluationParty.NameAr;
        response.DepartmentId = EvaluationParty.DepartmentId;
        response.NameEn = EvaluationParty.NameEn;
        response.IsActive = EvaluationParty.IsActive;
        response.IsSupportFiles = EvaluationParty.IsSupportFiles;
        repository.Update(response);
        await uow.CommitAsync().ConfigureAwait(false);
        var result = mapper.Map<EvaluationPartyDTO>(response, opts => opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Updated;
        return result;
    }
    public async Task<bool> UpdateEvaluationPartyOrderAsync(List<OrderingDTO> message)
    {
        var EvaluationPartys = await uow.GetRepository<EvaluationParty>()
            .GetAllActiveNonDeleted()
            .Where(n => message.Select(m => m.Id).Contains(n.Id))
            .ToListAsync();
        foreach (var EvaluationParty in EvaluationPartys)
        {
            EvaluationParty.OrderNo = message.First(m => m.Id == EvaluationParty.Id).OrderNo;
        }
        await uow.CommitAsync();
        return true;
    }
    public async Task<PartyTypeEvalPartyStatusDTO> UpdatePartyTypeEvalPartyStatus(PartyTypeEvalPartyStatusDTO message)
    {
        if (message == null)
        {
            throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
        }
        if (message.EvaluationPartyId == null)
        {
            throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
        }
        if (message.PartyTypeId == null)
        {
            throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
        }
        if (message.ServiceStatusId == null)
        {
            throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
        }

        var dataexist = await uow.GetRepository<PartyTypeEvalParty>()
            .GetAllNonDeleted()
            .FirstOrDefaultAsync(x => x.Id==message.Id);
        PartyTypeEvalParty PartyTypeEvalPartyobj= new PartyTypeEvalParty();
        if (dataexist == null)
        {
            PartyTypeEvalPartyobj.EvaluationPartyId = message.EvaluationPartyId;
            PartyTypeEvalPartyobj.PartyTypeId = message.PartyTypeId;
            PartyTypeEvalPartyobj.IsActive = message.IsActive;
            uow.GetRepository<PartyTypeEvalParty>().Insert(PartyTypeEvalPartyobj);

        }
        else
        {
            PartyTypeEvalPartyobj = await uow.GetRepository<PartyTypeEvalParty>()
        .GetAllNonDeleted()
        .Where(x => x.Id == dataexist.Id).FirstAsync();

            PartyTypeEvalPartyobj.IsActive = message.IsActive;
            uow.GetRepository<PartyTypeEvalParty>().Update(PartyTypeEvalPartyobj);
        }
        List<PartyTypeEvalPartyStatus>  objPartyTypeEvalPartyStatusdelete = await uow.GetRepository<PartyTypeEvalPartyStatus>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.PartyTypeEvalPartyId == PartyTypeEvalPartyobj.Id)
                                      .ToListAsync();
        var PartyTypeEvalPartyStatusexistids =new List<Guid>();
        if (objPartyTypeEvalPartyStatusdelete.Count > 0)
        {
            foreach (var item in objPartyTypeEvalPartyStatusdelete)
            {
                if (message.ServiceStatusId.Contains(item.ServiceStatusId))
                {
                    PartyTypeEvalPartyStatusexistids.Add(item.ServiceStatusId);
                }
                else
                {
                    uow.GetRepository<PartyTypeEvalPartyStatus>().Delete(item);
                }

            }
        }
        if (message.ServiceStatusId != null)
        {
            var notInSelected = message.ServiceStatusId.Except(PartyTypeEvalPartyStatusexistids).ToList();
            List<PartyTypeEvalPartyStatus> objentitylist=new List<PartyTypeEvalPartyStatus>();
            foreach (var item in notInSelected)
            {
                PartyTypeEvalPartyStatus objentity = new PartyTypeEvalPartyStatus();
                objentity.PartyTypeEvalPartyId = PartyTypeEvalPartyobj.Id;
                objentity.ServiceStatusId = item;
                objentity.IsActive = true;
                objentitylist.Add(objentity);
            }
            if (objentitylist.Count > 0)
            {
                await uow.GetRepository<PartyTypeEvalPartyStatus>().InsertRange(objentitylist);
            }


        }

        await uow.CommitAsync();
        message.Id = PartyTypeEvalPartyobj.Id;
        message.ResponseStatus = DBResult.Updated;
        return message;
    }
    public async Task<PartyTypeEvalPartyStatusDTO> DeletePartyTypeEvalPartyStatusAsync(Guid? id)
    {
        PartyTypeEvalPartyStatusDTO result=new PartyTypeEvalPartyStatusDTO();
        var PartyTypeEvalParty = await uow.GetRepository<PartyTypeEvalParty>()
            .GetAllNonDeleted(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (PartyTypeEvalParty == null)
        {
            return new PartyTypeEvalPartyStatusDTO
            {
                ResponseStatus = DBResult.NotFound
            };
        }
        var PartyTypeEvalPartyStatus = await uow.GetRepository<PartyTypeEvalPartyStatus>()
            .GetAllNonDeleted(x => x.PartyTypeEvalPartyId == PartyTypeEvalParty.Id)
            .ToListAsync()
            .ConfigureAwait(false);
        uow.GetRepository<PartyTypeEvalPartyStatus>().DeleteRange(PartyTypeEvalPartyStatus);
        uow.GetRepository<PartyTypeEvalParty>().Delete(PartyTypeEvalParty);
        await uow.CommitAsync();
        result.ResponseStatus = DBResult.Deleted;
        return result;
    }
}