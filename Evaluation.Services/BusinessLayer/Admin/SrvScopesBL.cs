using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Website;
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
    public class SrvScopesBL : AdminBase
    {
        public SrvScopesBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<ScopesDTO>> GetScopesList(int Page, int PageSize)
        {
           

            
           

            var list = await uow.GetRepository<Scope>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x=>x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<ScopesDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
       
      
        public async Task<ScopesDTO> SaveScopes(ScopesDTO message)
        {





            Scope obj = new Scope();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.ScopeTypeId = message.ScopeTypeId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<Scope>().Insert(obj);
           
            await uow.CommitAsync();
            var result = mapper.Map<ScopesDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<ScopesDTO> UpdateScopes(ScopesDTO message)
        {
           
            
          
               
                var result = new ScopesDTO();

            if (message.Id is not null)
            {
                Scope obj = await uow.GetRepository<Scope>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.ScopeTypeId = message.ScopeTypeId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<Scope>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<ScopesDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }

                return result;
           
        }
        public async Task<bool> UpdateScopesOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<Scope>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }
        public async Task<ScopesDTO> DeleteScopes(Guid? Id)
        {

           

                
                var result = new ScopesDTO();
                if (Id is not null)
                {
                Scope obj = await uow.GetRepository<Scope>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                var ScopeAcademicYear = await uow.GetRepository<ScopeAcademicYear>()
.GetAllNonDeleted()
                      .Where(x => x.ScopeId == obj.Id)
                      .ToListAsync();
                if (ScopeAcademicYear.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ScopeExistsScopeAcademicYear);
                }

                var AcademicYearScope = await uow.GetRepository<ScopeAcademicYear>()
.GetAllNonDeleted()
                      .Where(x => x.ScopeId == obj.Id)
                      .ToListAsync();
                if (AcademicYearScope.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ScopeExistsAcademicYearScope);
                }

                uow.GetRepository<Scope>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<ScopesDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
