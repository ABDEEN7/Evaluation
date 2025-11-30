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
    public class SrvAcademicYearScopeBL : AdminBase
    {
        public SrvAcademicYearScopeBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<AcademicYearScopeDTO>> GetAcademicYearScopeList(int Page, int PageSize)
        {
           

            var list = await uow.GetRepository<AcademicYearScope>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<AcademicYearScopeDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }

        public async Task<AcademicYearScopeDTO> SaveAcademicYearScope(AcademicYearScopeDTO message)
        {
           


            AcademicYearScope obj = new AcademicYearScope();

            obj.ScopeId = message.ScopeId;
            obj.ParentId = message.ParentId;
            obj.AcademicYearId = message.AcademicYearId;
            obj.IsActive = message.IsActive;

            uow.GetRepository<AcademicYearScope>().Insert(obj);
                await uow.CommitAsync();
            var result = mapper.Map<AcademicYearScopeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                return result;
            
        }
        public async Task<AcademicYearScopeDTO> UpdateAcademicYearScope(AcademicYearScopeDTO message)
        {
           
            
          
               
                var result = new AcademicYearScopeDTO();

                if (message.Id is not null)
                {
                AcademicYearScope obj = await uow.GetRepository<AcademicYearScope>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();


                obj.ScopeId = message.ScopeId;
                obj.ParentId = message.ParentId;
                obj.AcademicYearId = message.AcademicYearId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<AcademicYearScope>().Update(obj);
                    await uow.CommitAsync();
                result = mapper.Map<AcademicYearScopeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
                }

                return result;
           
        }
      
        public async Task<AcademicYearScopeDTO> DeleteAcademicYearScope(Guid? Id)
        {

           

               
                var result = new AcademicYearScopeDTO();
                if (Id is not null)
                {
                    AcademicYearScope obj = await uow.GetRepository<AcademicYearScope>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
              
                uow.GetRepository<AcademicYearScope>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<AcademicYearScopeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
