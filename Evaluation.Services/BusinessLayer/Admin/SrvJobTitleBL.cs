using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
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
        jobTitle.BackendName =  bacendName;
         uow.GetRepository<JobTitle>().Insert(jobTitle);
        await uow.CommitAsync();
        return message;
    }
}
