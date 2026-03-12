using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing;
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
    public async Task<List<EvaluationPartyDto>> GetEvaluationPartyList(int page, int pageSize)
    {
        var list = await uow.GetRepository<EvaluationParty>()
            .GetAllActiveNonDeleted()
            .Include(x => x.CreateBy)
            .OrderByDescending(x => x.OrderNo)
            .ThenByDescending(x => x.CreateDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var result = mapper.Map<List<EvaluationPartyDto>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
        return result;
    }
    public async Task<EvaluationPartyDto> SaveEvaluationParty(EvaluationPartyDto message)
    {
        EvaluationParty EvaluationParty = new EvaluationParty();
        EvaluationParty.NameEn = message.NameEn;
        EvaluationParty.NameAr = message.NameAr;
        EvaluationParty.IsActive = message.IsActive;
        EvaluationParty.DepartmentId = message.DepartmentId;
        EvaluationParty.IsSupportFiles = message.IsSupportFiles;

        uow.GetRepository<EvaluationParty>().Insert(EvaluationParty);
        await uow.CommitAsync();
        message.ResponseStatus = DBResult.Updated;
        return message;
    }
    public async Task<EvaluationPartyDto> DeleteEvaluationPartyAsync(Guid? id)
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
        var result = mapper.Map<EvaluationPartyDto>(EvaluationParty, opts =>
        opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Deleted;
        return result;
    }
    public async Task<EvaluationPartyDto> UpdateEvaluationParty(EvaluationPartyDto EvaluationParty)
    {
        if (EvaluationParty == null)
        {
            return new EvaluationPartyDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        if (EvaluationParty.Id == null)
        {
            return new EvaluationPartyDto
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
        var result = mapper.Map<EvaluationPartyDto>(response);
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
}