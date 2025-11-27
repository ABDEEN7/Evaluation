using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvScopeAcademicYearBL : AdminBase
    {
        public SrvScopeAcademicYearBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<ScopeAcademicYearDTO>> GetScopeAcademicYearList(int Page, int PageSize)
        {
           

            var list = await uow.GetRepository<ScopeAcademicYear>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<ScopeAcademicYearDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }

        public async Task<ScopeAcademicYearDTO> SaveScopeAcademicYear(ScopeAcademicYearDTO message)
        {
           


            ScopeAcademicYear obj = new ScopeAcademicYear();

            obj.ScopeId = message.ScopeId;
            obj.DepartmentId = message.DepartmentId;
            obj.AcademicYearId = message.AcademicYearId;
            obj.IsActive = message.IsActive;

            uow.GetRepository<ScopeAcademicYear>().Insert(obj);
                await uow.CommitAsync();
            var result = mapper.Map<ScopeAcademicYearDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                return result;
            
        }
        public async Task<ScopeAcademicYearDTO> UpdateScopeAcademicYear(ScopeAcademicYearDTO message)
        {
           
            
          
               
                var result = new ScopeAcademicYearDTO();

                if (message.Id is not null)
                {
                ScopeAcademicYear obj = await uow.GetRepository<ScopeAcademicYear>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();


                obj.ScopeId = message.ScopeId;
                obj.DepartmentId = message.DepartmentId;
                obj.AcademicYearId = message.AcademicYearId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<ScopeAcademicYear>().Update(obj);
                    await uow.CommitAsync();
                result = mapper.Map<ScopeAcademicYearDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
                }

                return result;
           
        }
      
        public async Task<ScopeAcademicYearDTO> DeleteScopeAcademicYear(Guid? Id)
        {

           

               
                var result = new ScopeAcademicYearDTO();
                if (Id is not null)
                {
                    ScopeAcademicYear obj = await uow.GetRepository<ScopeAcademicYear>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
              
                uow.GetRepository<ScopeAcademicYear>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<ScopeAcademicYearDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
