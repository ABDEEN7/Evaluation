using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.Models.Admin
{
    public class SrvOrgAcademicYearBL : AdminBase
    {
        public SrvOrgAcademicYearBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }


        public async Task<List<OrgAcademicYearDTO>> GetOrgAcademicYearList(int Page, int PageSize)
        {


            var list = await uow.GetRepository<OrgAcademicYear>()
                .GetAllNonDeleted()
                .Include(x => x.OrgTree)
                .Include(x => x.ParentOrgTree)
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<OrgAcademicYearDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
        public async Task<OrgAcademicYearDTO> SaveOrgAcademicYear(OrgAcademicYearDTO message)
        {


            OrgAcademicYear obj = new OrgAcademicYear();

            obj.JobTitleAr = message.JobTitleAr;
            obj.JobTitleEn = message.JobTitleEn;
            obj.Year = message.Year;
            obj.OrgTreeId = message.OrgTreeId;
            obj.ParentOrgTreeId = message.ParentOrgTreeId;

            uow.GetRepository<OrgAcademicYear>().Insert(obj);
            await uow.CommitAsync();
            var result = mapper.Map<OrgAcademicYearDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            return result;


        }
        public async Task<OrgAcademicYearDTO> UpdateOrgAcademicYear(OrgAcademicYearDTO message)
        {
            var result = new OrgAcademicYearDTO();

            if (message.Id is not null)
            {

                OrgAcademicYear obj = await uow.GetRepository<OrgAcademicYear>()
                                  .GetAllNonDeleted()
                                  .Include(x => x.CreateBy)
                                  .Where(x => x.Id == message.Id)
                                  .FirstAsync();

                obj.JobTitleAr = message.JobTitleAr;
                obj.JobTitleEn = message.JobTitleEn;
                obj.Year = message.Year;
                obj.OrgTreeId = message.OrgTreeId;
                obj.ParentOrgTreeId = message.ParentOrgTreeId;

                uow.GetRepository<OrgAcademicYear>().Update(obj);
                await uow.CommitAsync();
                result = mapper.Map<OrgAcademicYearDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }

        public async Task<OrgAcademicYearDTO> DeleteOrgAcademicYear(Guid? Id)
        {
            var result = new OrgAcademicYearDTO();
            if (Id is not null)
            {
                OrgAcademicYear obj = await uow.GetRepository<OrgAcademicYear>()
                                  .GetAllNonDeleted()
                                  .Where(x => x.Id == Id)
                                  .FirstAsync();
                if (obj == null)
                {
                    result.ResponseStatus = DBResult.NotFound;
                    return result;
                }

                uow.GetRepository<OrgAcademicYear>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<OrgAcademicYearDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;

        }

    }
}
