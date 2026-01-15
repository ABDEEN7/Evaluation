using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
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

public class SrvNdaStatusBL : AdminBase
{
    public SrvNdaStatusBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
    {
    }
    public async Task<List<NdaStatusDto>> GetNdaStatusList(int page, int pageSize)
    {
        var list = await uow.GetRepository<NdaStatus>()
            .GetAllActiveNonDeleted()
            .Include(x => x.CreateBy)
            .OrderByDescending(x => x.OrderNo)
            .ThenByDescending(x => x.CreateDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var result = mapper.Map<List<NdaStatusDto>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
        return result;
    }
    public async Task<NdaStatusDto> SaveNdaStatus(NdaStatusDto message)
    {
        var bacendName = await GenerateBackendNameByTitle(message.NameEn);

        var existBacendName = await uow.GetRepository<NdaStatus>()
            .GetAllActiveNonDeleted(x => x.BackendName == bacendName)
            .FirstOrDefaultAsync();
        if (existBacendName != null)
        {
            message.ResponseStatus = DBResult.Exist;
            return message;
        }
        NdaStatus NdaStatus = new NdaStatus();
        NdaStatus.NameEn = message.NameEn;
        NdaStatus.NameAr = message.NameAr;
        NdaStatus.IsActive = message.IsActive;
        NdaStatus.BackendName = bacendName;
        uow.GetRepository<NdaStatus>().Insert(NdaStatus);
        await uow.CommitAsync();
        message.ResponseStatus = DBResult.Updated;
        return message;
    }
    public async Task<NdaStatusDto> DeleteNdaStatusAsync(Guid? id)
    {
        var repository = uow.GetRepository<NdaStatus>();
        var ndaStatus = await repository
            .GetAllActiveNonDeleted(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (ndaStatus == null)
        {
            return new NdaStatusDto
            {
                ResponseStatus = DBResult.NotFound
            };
        }
        var evaluationRequestAssignment = await uow
            .GetRepository<EvaluationRequestAssignment>()
            .GetAllActiveNonDeleted()
            .AnyAsync(x => x.NdaStatusId == ndaStatus.Id);
        if (evaluationRequestAssignment)
            throw new BusinessException(ConstantKeys.ExceptionMessage.EvaluationRequestAssignment);

        repository.Delete(ndaStatus);
        await uow.CommitAsync();
        var result = mapper.Map<NdaStatusDto>(ndaStatus, opts =>
        opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Deleted;
        return result;
    }
    public async Task<NdaStatusDto> UpdateNdaStatus(NdaStatusDto ndaStatus)
    {
        if (ndaStatus == null)
        {
            return new NdaStatusDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        if (ndaStatus.Id == null)
        {
            return new NdaStatusDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        var repository = uow.GetRepository<NdaStatus>();
        var nda = await uow
            .GetRepository<NdaStatus>()
            .GetAllActiveNonDeleted()
            .FirstOrDefaultAsync(x => x.Id == ndaStatus.Id);
        if (nda == null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.NdaStatusNotFound);
        nda.NameAr = ndaStatus.NameAr;
        nda.NameEn = ndaStatus.NameEn;
        nda.IsActive = ndaStatus.IsActive;
        nda.UpdateById = userInfo.UserId;
        nda.UpdateDate = DateTime.UtcNow;

        repository.Update(nda);
        await uow.CommitAsync().ConfigureAwait(false);
        var result = mapper.Map<NdaStatusDto>(nda);
        result.ResponseStatus = DBResult.Updated;
        return result;
    }
    public async Task<bool> UpdateNdaStatusOrderAsync(List<OrderingDTO> message)
    {
        var NdaStatuss = await uow.GetRepository<NdaStatus>()
            .GetAllActiveNonDeleted()
            .Where(n => message.Select(m => m.Id).Contains(n.Id))
            .ToListAsync();
        foreach (var NdaStatus in NdaStatuss)
        {
            NdaStatus.OrderNo = message.First(m => m.Id == NdaStatus.Id).OrderNo;
        }
        await uow.CommitAsync();
        return true;
    }
}

