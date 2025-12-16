using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Org;
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
    public class SrvPlanStatusesBL : AdminBase
    {
        public SrvPlanStatusesBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<PlanStatusesDTO>> GetPlanStatusesList(int Page, int PageSize)
        {



            var mapper = await CreateMapperForAdmin<PlanStatus, PlanStatusesDTO>();

            var list = await uow.GetRepository<PlanStatus>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<PlanStatusesDTO>>(list);
            return result;


        }
       
      
        public async Task<PlanStatusesDTO> SavePlanStatuses(PlanStatusesDTO message)
        {



            var mapper = await CreateMapperForAdmin<PlanStatus, PlanStatusesDTO>();

            var BackendName= await GenerateBackendNameByTitle(message.NameEN);
                var existBackendName = await uow
             .GetRepository<PlanStatus>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

                if (existBackendName != null)
                {

                throw new BusinessException(ConstantKeys.ExceptionMessage.BackendNameAlreadyExists);
            }

            PlanStatus obj = new PlanStatus();

                obj.NameAr = message.NameAr;
                obj.NameEN = message.NameEN;
                obj.BackendName = BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<PlanStatus>().Insert(obj);
            
            await uow.CommitAsync();
            var result = mapper.Map<PlanStatusesDTO>(obj);
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<PlanStatusesDTO> UpdatePlanStatuses(PlanStatusesDTO message)
        {


            var mapper = await CreateMapperForAdmin<PlanStatus, PlanStatusesDTO>();

            var result = new PlanStatusesDTO();

            if (message.Id is not null)
            {
                PlanStatus obj = await uow.GetRepository<PlanStatus>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEN = message.NameEN;
                obj.BackendName = obj.BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<PlanStatus>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<PlanStatusesDTO>(obj);
                result.ResponseStatus = DBResult.Updated;
            }
                return result;
           
        }

        public async Task<bool> UpdatePlanStatusesOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<PlanStatus>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }

        public async Task<PlanStatusesDTO> DeletePlanStatuses(Guid? Id)
        {



            var mapper = await CreateMapperForAdmin<PlanStatus, PlanStatusesDTO>();
            var result = new PlanStatusesDTO();
                if (Id is not null)
                {
                PlanStatus obj = await uow.GetRepository<PlanStatus>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                var PlanHistory = await uow.GetRepository<PlanHistory>()
.GetAllNonDeleted()
                      .Where(x => x.PlanStatusId == obj.Id)
                      .ToListAsync();
                if (PlanHistory.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PlanStatusExistsPlanHistory);
                }

                var Plan = await uow.GetRepository<Plan>()
.GetAllNonDeleted()
                      .Where(x => x.PlanStatusId == obj.Id)
                      .ToListAsync();
                if (PlanHistory.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PlanStatusExistsPlan);
                }

                uow.GetRepository<PlanStatus>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<PlanStatusesDTO>(obj);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
