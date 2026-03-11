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

public class SrvNdaStatusDepartmentBL : AdminBase
{
    public SrvNdaStatusDepartmentBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
    {
    }
    public async Task<List<NdaStatusDepartmentDto>> GetNdaStatusDepartmentList(int page, int pageSize)
    {
        var list = await uow.GetRepository<NdaStatusDepartment>()
            .GetAllActiveNonDeleted()
            .Include(x => x.CreateBy)
            .OrderByDescending(x => x.OrderNo)
            .ThenByDescending(x => x.CreateDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var result = mapper.Map<List<NdaStatusDepartmentDto>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
        return result;
    }
    public async Task<NdaStatusDepartmentDto> SaveNdaStatusDepartment(NdaStatusDepartmentDto message)
    {
        NdaStatusDepartment NdaStatusDepartment = new NdaStatusDepartment();
        NdaStatusDepartment.NameEn = message.NameEn;
        NdaStatusDepartment.NameAr = message.NameAr;
        NdaStatusDepartment.IsActive = message.IsActive;
        NdaStatusDepartment.DepartmentId = message.DepartmentId;
        NdaStatusDepartment.NdaStatusId = message.NdaStatusId;
        uow.GetRepository<NdaStatusDepartment>().Insert(NdaStatusDepartment);
        await uow.CommitAsync();
        message.ResponseStatus = DBResult.Updated;
        return message;
    }
    public async Task<NdaStatusDepartmentDto> DeleteNdaStatusDepartmentAsync(Guid? id)
    {
        var repository = uow.GetRepository<NdaStatusDepartment>();
        var ndaStatusDepartment = await repository
            .GetAllActiveNonDeleted(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (ndaStatusDepartment == null)
        {
            return new NdaStatusDepartmentDto
            {
                ResponseStatus = DBResult.NotFound
            };
        }
        //var evaluationRequestAssignment = await uow
        //    .GetRepository<EvaluationRequestAssignment>()
        //    .GetAllActiveNonDeleted()
        //    .AnyAsync(x => x.NdaStatusDepartmentId == ndaStatusDepartment.Id);
        //if (evaluationRequestAssignment)
        //    throw new BusinessException(ConstantKeys.ExceptionMessage.EvaluationRequestAssignment);

        repository.Delete(ndaStatusDepartment);
        await uow.CommitAsync();
        var result = mapper.Map<NdaStatusDepartmentDto>(ndaStatusDepartment, opts =>
        opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Deleted;
        return result;
    }
    public async Task<NdaStatusDepartmentDto> UpdateNdaStatusDepartment(NdaStatusDepartmentDto NdaStatusDepartment)
    {
        if (NdaStatusDepartment == null)
        {
            return new NdaStatusDepartmentDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        if (NdaStatusDepartment.Id == null)
        {
            return new NdaStatusDepartmentDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        var repository = uow.GetRepository<NdaStatusDepartment>();
        var nda = await uow
            .GetRepository<NdaStatusDepartment>()
            .GetAllActiveNonDeleted()
            .FirstOrDefaultAsync(x => x.Id == NdaStatusDepartment.Id);
        if (nda == null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.NdaStatusNotFound);
        nda.NameAr = NdaStatusDepartment.NameAr;
        nda.NameEn = NdaStatusDepartment.NameEn;
        nda.IsActive = NdaStatusDepartment.IsActive;
        nda.UpdateById = userInfo.UserId;
        nda.UpdateDate = DateTime.UtcNow;
        repository.Update(nda);
        await uow.CommitAsync().ConfigureAwait(false);
        var result = mapper.Map<NdaStatusDepartmentDto>(nda);
        result.ResponseStatus = DBResult.Updated;
        return result;
    }
    public async Task<bool> UpdateNdaStatusDepartmentOrderAsync(List<OrderingDTO> message)
    {
        var NdaStatusDepartments = await uow.GetRepository<NdaStatusDepartment>()
            .GetAllActiveNonDeleted()
            .Where(n => message.Select(m => m.Id).Contains(n.Id))
            .ToListAsync();
        foreach (var NdaStatusDepartment in NdaStatusDepartments)
        {
            NdaStatusDepartment.OrderNo = message.First(m => m.Id == NdaStatusDepartment.Id).OrderNo;
        }
        await uow.CommitAsync();
        return true;
    }
}

