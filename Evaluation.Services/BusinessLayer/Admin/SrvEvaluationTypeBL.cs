using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
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
    public class SrvEvaluationTypeBL : AdminBase
    {
        public SrvEvaluationTypeBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<EvaluationTypeDTO>> GetEvaluationTypeList(int Page, int PageSize)
        {



            var mapper = await CreateMapperForAdmin<EvaluationType, EvaluationTypeDTO>();

            var list = await uow.GetRepository<EvaluationType>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<EvaluationTypeDTO>>(list);
            return result;


        }
       
      
        public async Task<EvaluationTypeDTO> SaveEvaluationType(EvaluationTypeDTO message)
        {



            var mapper = await CreateMapperForAdmin<EvaluationType, EvaluationTypeDTO>();

            var BackendName= await GenerateBackendNameByTitle(message.NameEn);
                var existBackendName = await uow
             .GetRepository<EvaluationType>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

                if (existBackendName != null)
                {

                throw new BusinessException(ConstantKeys.ExceptionMessage.BackendNameAlreadyExists);
            }

            EvaluationType obj = new EvaluationType();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.BackendName = BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<EvaluationType>().Insert(obj);
            
            await uow.CommitAsync();
            var result = mapper.Map<EvaluationTypeDTO>(obj);
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<EvaluationTypeDTO> UpdateEvaluationType(EvaluationTypeDTO message)
        {


            var mapper = await CreateMapperForAdmin<EvaluationType, EvaluationTypeDTO>();

            var result = new EvaluationTypeDTO();

            if (message.Id is not null)
            {
                EvaluationType obj = await uow.GetRepository<EvaluationType>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.BackendName = obj.BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<EvaluationType>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<EvaluationTypeDTO>(obj);
                result.ResponseStatus = DBResult.Updated;
            }
                return result;
           
        }

        public async Task<bool> UpdateEvaluationTypeOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<EvaluationType>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }

        public async Task<EvaluationTypeDTO> DeleteEvaluationType(Guid? Id)
        {



            var mapper = await CreateMapperForAdmin<EvaluationType, EvaluationTypeDTO>();
            var result = new EvaluationTypeDTO();
                if (Id is not null)
                {
                EvaluationType obj = await uow.GetRepository<EvaluationType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                var EvaluationRequestHistory = await uow.GetRepository<EvaluationRequestHistory>()
.GetAllNonDeleted()
                      .Where(x => x.EvaluationTypeId == obj.Id)
                      .ToListAsync();
                if (EvaluationRequestHistory.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.EvaluationTypeExistsEvaluationRequestHistory);
                }

                
                uow.GetRepository<EvaluationType>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<EvaluationTypeDTO>(obj);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
