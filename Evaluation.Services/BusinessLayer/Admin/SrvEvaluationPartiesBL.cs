using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
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
    public class SrvEvaluationPartiesBL : AdminBase
    {
        public SrvEvaluationPartiesBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<EvaluationPartiesDTO>> GetEvaluationPartiesList(int Page, int PageSize)
        {



            

            var list = await uow.GetRepository<EvaluationParty>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<EvaluationPartiesDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }
       
      
        public async Task<EvaluationPartiesDTO> SaveEvaluationParties(EvaluationPartiesDTO message)
        {


            EvaluationParty obj = new EvaluationParty();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.DepartmentId = message.DepartmentId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<EvaluationParty>().Insert(obj);
            
            await uow.CommitAsync();
            var result = mapper.Map<EvaluationPartiesDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<EvaluationPartiesDTO> UpdateEvaluationParties(EvaluationPartiesDTO message)
        {



            var result = new EvaluationPartiesDTO();

            if (message.Id is not null)
            {
                EvaluationParty obj = await uow.GetRepository<EvaluationParty>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.DepartmentId = message.DepartmentId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<EvaluationParty>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<EvaluationPartiesDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }
                return result;
           
        }

        public async Task<bool> UpdateEvaluationPartiesOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<EvaluationParty>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }

        public async Task<EvaluationPartiesDTO> DeleteEvaluationParties(Guid? Id)
        {



           
            var result = new EvaluationPartiesDTO();
                if (Id is not null)
                {
                EvaluationParty obj = await uow.GetRepository<EvaluationParty>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                var Service = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.EvaluationPartyId == obj.Id)
                      .ToListAsync();
                if (Service.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.EvaluationPartiesExistsService);
                }
                var ServiceRequest = await uow.GetRepository<ServiceRequest>()
.GetAllNonDeleted()
                      .Where(x => x.EvaluationPartyId == obj.Id)
                      .ToListAsync();
                if (ServiceRequest.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.EvaluationPartiesExistsServiceRequest);
                }
                var EvalForm = await uow.GetRepository<EvalForm>()
.GetAllNonDeleted()
                      .Where(x => x.EvaluationPartyId == obj.Id)
                      .ToListAsync();
                if (EvalForm.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.EvaluationPartiesExistsEvalForm);
                }


                uow.GetRepository<EvaluationParty>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<EvaluationPartiesDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
