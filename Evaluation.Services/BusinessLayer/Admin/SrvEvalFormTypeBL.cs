using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
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
    public class SrvEvalFormTypeBL : AdminBase
    {
        public SrvEvalFormTypeBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<EvalFormTypeDTO>> GetEvalFormTypeList(int Page, int PageSize)
        {



            var mapper = await CreateMapperForAdmin<EvalFormType, EvalFormTypeDTO>();

            var list = await uow.GetRepository<EvalFormType>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<EvalFormTypeDTO>>(list);
            return result;


        }
       
      
        public async Task<EvalFormTypeDTO> SaveEvalFormType(EvalFormTypeDTO message)
        {



            var mapper = await CreateMapperForAdmin<EvalFormType, EvalFormTypeDTO>();

            var BackendName= await GenerateBackendNameByTitle(message.NameEn);
                var existBackendName = await uow
             .GetRepository<EvalFormType>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

                if (existBackendName != null)
                {

                throw new BusinessException(ConstantKeys.ExceptionMessage.BackendNameAlreadyExists);
            }

            EvalFormType obj = new EvalFormType();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.BackendName = BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<EvalFormType>().Insert(obj);
            
            await uow.CommitAsync();
            var result = mapper.Map<EvalFormTypeDTO>(obj);
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<EvalFormTypeDTO> UpdateEvalFormType(EvalFormTypeDTO message)
        {


            var mapper = await CreateMapperForAdmin<EvalFormType, EvalFormTypeDTO>();

            var result = new EvalFormTypeDTO();

            if (message.Id is not null)
            {
                EvalFormType obj = await uow.GetRepository<EvalFormType>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.BackendName = obj.BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<EvalFormType>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<EvalFormTypeDTO>(obj);
                result.ResponseStatus = DBResult.Updated;
            }
                return result;
           
        }

        public async Task<bool> UpdateEvalFormTypeOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<EvalFormType>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }

        public async Task<EvalFormTypeDTO> DeleteEvalFormType(Guid? Id)
        {



            var mapper = await CreateMapperForAdmin<EvalFormType, EvalFormTypeDTO>();
            var result = new EvalFormTypeDTO();
                if (Id is not null)
                {
                EvalFormType obj = await uow.GetRepository<EvalFormType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                var EvalForms = await uow.GetRepository<EvalForm>()
.GetAllNonDeleted()
                      .Where(x => x.EvalFormTypeId == obj.Id)
                      .ToListAsync();
                if (EvalForms.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.EvalFormTypeExistsEvalForms);
                }

                
                uow.GetRepository<EvalFormType>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<EvalFormTypeDTO>(obj);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
