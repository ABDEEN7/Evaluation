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
    public class SrvOrgTypesBL : AdminBase
    {
        public SrvOrgTypesBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<OrgTypesDTO>> GetOrgTypesList(int Page, int PageSize)
        {



            var mapper = await CreateMapperForAdmin<OrgType, OrgTypesDTO>();

            var list = await uow.GetRepository<OrgType>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<OrgTypesDTO>>(list);
            return result;


        }
       
      
        public async Task<OrgTypesDTO> SaveOrgTypes(OrgTypesDTO message)
        {


            var mapper = await CreateMapperForAdmin<OrgType, OrgTypesDTO>();

            var BackendName= await GenerateBackendNameByTitle(message.NameEn);
                var existBackendName = await uow
             .GetRepository<OrgType>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

                if (existBackendName != null)
                {

                throw new BusinessException(ConstantKeys.ExceptionMessage.BackendNameAlreadyExists);
            }

            OrgType obj = new OrgType();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.BackendName = BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<OrgType>().Insert(obj);
           
            await uow.CommitAsync();
            var result = mapper.Map<OrgTypesDTO>(obj);
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<OrgTypesDTO> UpdateOrgTypes(OrgTypesDTO message)
        {


            var mapper = await CreateMapperForAdmin<OrgType, OrgTypesDTO>();

            var result = new OrgTypesDTO();

               
                OrgType obj = await uow.GetRepository<OrgType>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
            if(obj!=null)
            {
                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.BackendName = obj.BackendName;
                obj.IsActive = message.IsActive;

                uow.GetRepository<OrgType>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<OrgTypesDTO>(obj);
                result.ResponseStatus = DBResult.Updated;
            }

                return result;
           
        }
        
        public async Task<OrgTypesDTO> DeleteOrgTypes(Guid? Id)
        {



            var mapper = await CreateMapperForAdmin<OrgType, OrgTypesDTO>();
            var result = new OrgTypesDTO();
                if (Id is not null)
                {
                    OrgType obj = await uow.GetRepository<OrgType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                var OrgTree = await uow.GetRepository<OrgTree>()
.GetAllNonDeleted()
                      .Where(x => x.OrgTypeId == obj.Id)
                      .ToListAsync();
                if (OrgTree.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTypeExistsOrgTree);
                }
                var Organization = await uow.GetRepository<Organization>()
.GetAllNonDeleted()
                      .Where(x => x.OrgTypeId == obj.Id)
                      .ToListAsync();
                if (Organization.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTypeExistsOrganization);
                }

                uow.GetRepository<OrgType>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<OrgTypesDTO>(obj);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
