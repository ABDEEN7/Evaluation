using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.Admin;

public class SrvDepEvaluationTypeBL : AdminBase
{
    public SrvDepEvaluationTypeBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
    {
    }
    public async Task<List<DepEvaluationTypeDto>> GetDepEvaluationTypeList(int page, int pageSize)
    {
        var list = await uow.GetRepository<DepEvaluationType>()
            .GetAllActiveNonDeleted()
            .Include(x => x.CreateBy)
            .OrderByDescending(x => x.OrderNo)
            .ThenByDescending(x => x.CreateDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var result = mapper.Map<List<DepEvaluationTypeDto>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
        return result;
    }
    public async Task<DepEvaluationTypeDto> SaveDepEvaluationType(DepEvaluationTypeDto message)
    {
        var bacendName = await GenerateBackendNameByTitle(message.NameEn);

        var existBacendName = await uow.GetRepository<DepEvaluationType>()
            .GetAllActiveNonDeleted(x => x.BackendName == bacendName)
            .FirstOrDefaultAsync();
        if (existBacendName != null)
        {
            message.ResponseStatus = DBResult.Exist;
            return message;
        }
        DepEvaluationType DepEvaluationType = new DepEvaluationType();
        DepEvaluationType.NameEn = message.NameEn;
        DepEvaluationType.NameAr = message.NameAr;
        DepEvaluationType.IsActive = message.IsActive;
        DepEvaluationType.BackendName = bacendName;
        uow.GetRepository<DepEvaluationType>().Insert(DepEvaluationType);
        await uow.CommitAsync();
        message.ResponseStatus = DBResult.Updated;
        return message;
    }
    public async Task<DepEvaluationTypeDto> DeleteDepEvaluationTypeAsync(Guid? id)
    {
        var repository = uow.GetRepository<DepEvaluationType>();
        var DepEvaluationType = await repository
            .GetAllActiveNonDeleted(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (DepEvaluationType == null)
        {
            return new DepEvaluationTypeDto
            {
                ResponseStatus = DBResult.NotFound
            };
        }
        repository.Delete(DepEvaluationType);
        await uow.CommitAsync();
        var result = mapper.Map<DepEvaluationTypeDto>(DepEvaluationType, opts =>
        opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Deleted;
        return result;
    }
    public async Task<DepEvaluationTypeDto> UpdateDepEvaluationType(DepEvaluationTypeDto DepEvaluationType)
    {
        if (DepEvaluationType == null)
        {
            return new DepEvaluationTypeDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        if (DepEvaluationType.Id == null)
        {
            return new DepEvaluationTypeDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        var repository = uow.GetRepository<DepEvaluationType>();
        var job = await uow
            .GetRepository<DepEvaluationType>()
            .GetAllActiveNonDeleted()
            .FirstOrDefaultAsync(x => x.Id == DepEvaluationType.Id);
        job.NameAr = DepEvaluationType.NameAr;
        job.NameEn = DepEvaluationType.NameEn;
        job.IsActive = DepEvaluationType.IsActive;
        job.UpdateById = userInfo.UserId;
        job.UpdateDate = DateTime.UtcNow;

        repository.Update(job);
        await uow.CommitAsync().ConfigureAwait(false);
        var result = mapper.Map<DepEvaluationTypeDto>(job);
        result.ResponseStatus = DBResult.Updated;
        return result;
    }
}

