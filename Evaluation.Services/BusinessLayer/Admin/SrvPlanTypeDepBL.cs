using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing;
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
    public class SrvPlanTypeDepBL : AdminBase
    {
        public SrvPlanTypeDepBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<PlanTypeDepDTO>> GetPlanTypeDepList(int Page, int PageSize)
        {
           

            
           

            var list = await uow.GetRepository<PlanTypeDep>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x=>x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<PlanTypeDepDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
       
      
        public async Task<PlanTypeDepDTO> SavePlanTypeDep(PlanTypeDepDTO message)
        {





            PlanTypeDep obj = new PlanTypeDep();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.DepartmentId = message.DepartmentId;
                obj.PlanTypeId = message.PlanTypeId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<PlanTypeDep>().Insert(obj);
           
            await uow.CommitAsync();
            var result = mapper.Map<PlanTypeDepDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<PlanTypeDepDTO> UpdatePlanTypeDep(PlanTypeDepDTO message)
        {
           
            
          
               
                var result = new PlanTypeDepDTO();


            PlanTypeDep obj = await uow.GetRepository<PlanTypeDep>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.DepartmentId = message.DepartmentId;
            obj.PlanTypeId = message.PlanTypeId;
            obj.IsActive = message.IsActive;

            uow.GetRepository<PlanTypeDep>().Update(obj);
           
            await uow.CommitAsync();
                result = mapper.Map<PlanTypeDepDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
           

                return result;
           
        }
        public async Task<bool> UpdatePlanTypeDepOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<PlanTypeDep>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }
        public async Task<PlanTypeDepDTO> DeletePlanTypeDep(Guid? Id)
        {

           

                
                var result = new PlanTypeDepDTO();
                if (Id is not null)
                {
                PlanTypeDep obj = await uow.GetRepository<PlanTypeDep>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                var Plan = await uow.GetRepository<Plan>()
.GetAllNonDeleted()
                      .Where(x => x.PlanTypeDepId == obj.Id)
                      .ToListAsync();
                if (Plan.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PlanTypeDepExistsPlan);
                }

                

                uow.GetRepository<PlanTypeDep>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<PlanTypeDepDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
