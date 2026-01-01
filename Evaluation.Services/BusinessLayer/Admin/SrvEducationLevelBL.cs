using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Org;
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
    public class SrvEducationLevelBL : AdminBase
    {
        public SrvEducationLevelBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<EducationLevelDTO>> GetEducationLevelList(int Page, int PageSize)
        {



            var mapper = await CreateMapperForAdmin<EducationLevel, EducationLevelDTO>();

            var list = await uow.GetRepository<EducationLevel>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<EducationLevelDTO>>(list);
            return result;


        }
       
      
        public async Task<EducationLevelDTO> SaveEducationLevel(EducationLevelDTO message)
        {



            var mapper = await CreateMapperForAdmin<EducationLevel, EducationLevelDTO>();

            var BackendName= await GenerateBackendNameByTitle(message.NameEn);
                var existBackendName = await uow
             .GetRepository<EducationLevel>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

                if (existBackendName != null)
                {

                throw new BusinessException(ConstantKeys.ExceptionMessage.BackendNameAlreadyExists);
            }

            EducationLevel obj = new EducationLevel();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.BackendName = BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<EducationLevel>().Insert(obj);
            
            await uow.CommitAsync();
            var result = mapper.Map<EducationLevelDTO>(obj);
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<EducationLevelDTO> UpdateEducationLevel(EducationLevelDTO message)
        {


            var mapper = await CreateMapperForAdmin<EducationLevel, EducationLevelDTO>();

            var result = new EducationLevelDTO();

            if (message.Id is not null)
            {
                EducationLevel obj = await uow.GetRepository<EducationLevel>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.BackendName = obj.BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<EducationLevel>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<EducationLevelDTO>(obj);
                result.ResponseStatus = DBResult.Updated;
            }
                return result;
           
        }

        public async Task<bool> UpdateEducationLevelOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<EducationLevel>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }

        public async Task<EducationLevelDTO> DeleteEducationLevel(Guid? Id)
        {



            var mapper = await CreateMapperForAdmin<EducationLevel, EducationLevelDTO>();
            var result = new EducationLevelDTO();
                if (Id is not null)
                {
                EducationLevel obj = await uow.GetRepository<EducationLevel>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                var SchoolLevel = await uow.GetRepository<SchoolLevel>()
.GetAllNonDeleted()
                      .Where(x => x.EducationLevelId == obj.Id)
                      .ToListAsync();
                if (SchoolLevel.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.EducationLevelExistsSchoolLevel);
                }

                
                uow.GetRepository<EducationLevel>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<EducationLevelDTO>(obj);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
