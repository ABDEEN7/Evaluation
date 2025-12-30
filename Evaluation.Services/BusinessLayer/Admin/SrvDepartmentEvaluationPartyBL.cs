using AutoMapper;
using Evaluation.DAL.Helper;
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

public class SrvDepartmentEvaluationPartyBL : AdminBase
{
    public SrvDepartmentEvaluationPartyBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
    {
    }
    public async Task<List<DepartmentEvaluationPartyDto>> GetDepartmentEvaluationPartyList(int page, int pageSize)
    {
        var list = await uow.GetRepository<DepartmentEvaluationParty>()
            .GetAllActiveNonDeleted()
            .Include(x => x.CreateBy)
            .OrderByDescending(x => x.OrderNo)
            .ThenByDescending(x => x.CreateDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var result = mapper.Map<List<DepartmentEvaluationPartyDto>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
        return result;
    }
    public async Task<DepartmentEvaluationPartyDto> SaveDepartmentEvaluationParty(DepartmentEvaluationPartyDto message)
    {
        DepartmentEvaluationParty DepartmentEvaluationParty = new DepartmentEvaluationParty();
        DepartmentEvaluationParty.NameEn = message.NameEn;
        DepartmentEvaluationParty.NameAr = message.NameAr;
        DepartmentEvaluationParty.IsActive = message.IsActive;
        DepartmentEvaluationParty.DepartmentId = message.DepartmentId;

        uow.GetRepository<DepartmentEvaluationParty>().Insert(DepartmentEvaluationParty);
        await uow.CommitAsync();
        message.ResponseStatus = DBResult.Updated;
        return message;
    }
    public async Task<DepartmentEvaluationPartyDto> DeleteDepartmentEvaluationPartyAsync(Guid? id)
    {
        var repository = uow.GetRepository<DepartmentEvaluationParty>();
        var departmentEvaluationParty = await repository
            .GetAllActiveNonDeleted(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (departmentEvaluationParty == null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.DepartmentEvaluationParty);
        repository.Delete(departmentEvaluationParty);
        await uow.CommitAsync();
        var result = mapper.Map<DepartmentEvaluationPartyDto>(departmentEvaluationParty, opts =>
        opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Deleted;
        return result;
    }
    public async Task<DepartmentEvaluationPartyDto> UpdateDepartmentEvaluationParty(DepartmentEvaluationPartyDto DepartmentEvaluationParty)
    {
        if (DepartmentEvaluationParty == null)
        {
            return new DepartmentEvaluationPartyDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        if (DepartmentEvaluationParty.Id == null)
        {
            return new DepartmentEvaluationPartyDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        var repository = uow.GetRepository<DepartmentEvaluationParty>();
        var response = await uow
            .GetRepository<DepartmentEvaluationParty>()
            .GetAllActiveNonDeleted()
            .FirstOrDefaultAsync(x => x.Id == DepartmentEvaluationParty.Id);
        response.NameAr = DepartmentEvaluationParty.NameAr;
        response.NameEn = DepartmentEvaluationParty.NameEn;
        response.IsActive = DepartmentEvaluationParty.IsActive;
        response.UpdateById = userInfo.UserId;
        response.UpdateDate = DateTime.UtcNow;

        repository.Update(response);
        await uow.CommitAsync().ConfigureAwait(false);
        var result = mapper.Map<DepartmentEvaluationPartyDto>(response);
        result.ResponseStatus = DBResult.Updated;
        return result;
    }
    public async Task<bool> UpdateDepartmentEvaluationPartyOrderAsync(List<OrderingDTO> message)
    {
        var DepartmentEvaluationPartys = await uow.GetRepository<DepartmentEvaluationParty>()
            .GetAllActiveNonDeleted()
            .Where(n => message.Select(m => m.Id).Contains(n.Id))
            .ToListAsync();
        foreach (var DepartmentEvaluationParty in DepartmentEvaluationPartys)
        {
            DepartmentEvaluationParty.OrderNo = message.First(m => m.Id == DepartmentEvaluationParty.Id).OrderNo;
        }
        await uow.CommitAsync();
        return true;
    }
}