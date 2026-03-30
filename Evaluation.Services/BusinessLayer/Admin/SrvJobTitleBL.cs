using System.Runtime.InteropServices;
using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.PermissionEntity;
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

public class SrvJobTitleBL : AdminBase
{
    public SrvJobTitleBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
    {
    }
    public async Task<List<JobTitleDto>> GetJobTitleList(int page, int pageSize)
    {
        var list = await uow.GetRepository<JobTitle>()
            .GetAllActiveNonDeleted()
            .Include(x => x.CreateBy)
            .OrderByDescending(x => x.OrderNo)
            .ThenByDescending(x => x.CreateDate)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var result = mapper.Map<List<JobTitleDto>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
        return result;
    }
    public async Task<JobTitleDto> SaveJobTitle(JobTitleDto message)
    {
        var bacendName = await GenerateBackendNameByTitle(message.NameEn);

        var existBacendName = await uow.GetRepository<JobTitle>()
            .GetAllActiveNonDeleted(x => x.BackendName == bacendName)
            .FirstOrDefaultAsync();
        if (existBacendName != null)
        {
            message.ResponseStatus = DBResult.Exist;
            return message;
        }
        JobTitle jobTitle = new JobTitle();
        jobTitle.NameEn = message.NameEn;
        jobTitle.NameAr = message.NameAr;
        jobTitle.IsActive = message.IsActive;
        jobTitle.BackendName = bacendName;
        jobTitle.IsOrgManager = message.IsOrgManager;
        jobTitle.HRCode = message.HrCode;
        uow.GetRepository<JobTitle>().Insert(jobTitle);
        await uow.CommitAsync();
        message.ResponseStatus = DBResult.Inserted;
        return message;
    }
    public async Task<JobTitleDto> DeleteJobTitleAsync(Guid? id)
    {
        var repository = uow.GetRepository<JobTitle>();
        var jobTitle = await repository
            .GetAllActiveNonDeleted(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        var hasEmployee = await uow.GetRepository<Employee>().GetAllActiveNonDeleted().AnyAsync(x => x.JobTitleId == id);
        if (hasEmployee)
            throw new BusinessException(ConstantKeys.ExceptionMessage.JobTitleCannotDelete);
        
        if (jobTitle == null)
        {
            return new JobTitleDto
            {
                ResponseStatus = DBResult.NotFound
            };
        }
        repository.Delete(jobTitle);
        await uow.CommitAsync();
        var result = mapper.Map<JobTitleDto>(jobTitle, opts =>
        opts.Items["Language"] = _requestInfo.Lang);
        result.ResponseStatus = DBResult.Deleted;
        return result;
    }
    public async Task<JobTitleDto> UpdateJobTitle(JobTitleDto jobTitle)
    {
        if (jobTitle == null)
        {
            return new JobTitleDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        if (jobTitle.Id == null)
        {
            return new JobTitleDto
            {
                ResponseStatus = DBResult.Error
            };
        }
        var repository = uow.GetRepository<JobTitle>();
        var job = await uow
            .GetRepository<JobTitle>()
            .GetAllActiveNonDeleted()
            .FirstOrDefaultAsync(x => x.Id == jobTitle.Id);
        job.NameAr = jobTitle.NameAr;
        job.NameEn = jobTitle.NameEn;
        job.IsActive = jobTitle.IsActive;
        job.UpdateById = userInfo.UserId;
        job.UpdateDate = DateTime.UtcNow;
        job.HRCode = jobTitle.HrCode;
        job.IsOrgManager = jobTitle.IsOrgManager;

        repository.Update(job);
        await uow.CommitAsync().ConfigureAwait(false);
        var result = mapper.Map<JobTitleDto>(job);
        result.ResponseStatus = DBResult.Updated;
        return result;
    }
    public async Task<bool> UpdateJobTitleOrderAsync(List<OrderingDTO> message)
    {
        var jobTitles = await uow.GetRepository<JobTitle>()
            .GetAllActiveNonDeleted()
            .Where(n => message.Select(m => m.Id).Contains(n.Id))
            .ToListAsync();
        foreach (var jobTitle in jobTitles)
        {
            jobTitle.OrderNo = message.First(m => m.Id == jobTitle.Id).OrderNo;
        }
        await uow.CommitAsync();
        return true;
    }
}

