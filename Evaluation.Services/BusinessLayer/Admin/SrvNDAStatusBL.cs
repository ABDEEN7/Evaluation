using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
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
        var NdaStatus = await repository
            .GetAllActiveNonDeleted(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (NdaStatus == null)
        {
            return new NdaStatusDto
            {
                ResponseStatus = DBResult.NotFound
            };
        }
        repository.Delete(NdaStatus);
        await uow.CommitAsync();
        var result = mapper.Map<NdaStatusDto>(NdaStatus, opts =>
        opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Deleted;
        return result;
    }
    public async Task<NdaStatusDto> UpdateNdaStatus(NdaStatusDto NdaStatus)
    {
        if (NdaStatus == null)
        {
            return new NdaStatusDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        if (NdaStatus.Id == null)
        {
            return new NdaStatusDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        var repository = uow.GetRepository<NdaStatus>();
        var job = await uow
            .GetRepository<NdaStatus>()
            .GetAllActiveNonDeleted()
            .FirstOrDefaultAsync(x => x.Id == NdaStatus.Id);
        job.NameAr = NdaStatus.NameAr;
        job.NameEn = NdaStatus.NameEn;
        job.IsActive = NdaStatus.IsActive;
        job.UpdateById = userInfo.UserId;
        job.UpdateDate = DateTime.UtcNow;

        repository.Update(job);
        await uow.CommitAsync().ConfigureAwait(false);
        var result = mapper.Map<NdaStatusDto>(job);
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

