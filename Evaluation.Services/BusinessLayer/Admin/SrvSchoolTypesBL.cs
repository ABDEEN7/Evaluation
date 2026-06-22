using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Org;
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
    public class SrvSchoolTypesBL : AdminBase
    {
        public SrvSchoolTypesBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,IServiceScopeFactory serviceScopeFactory,RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            
        }


        public async Task<List<SchoolTypesDTO>> GetSchoolTypesList(int Page, int PageSize)
        {



            var mapper = await CreateMapperForAdmin<SchoolType, SchoolTypesDTO>();

            var list = await uow.GetRepository<SchoolType>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<SchoolTypesDTO>>(list);
            return result;


        }
       
      
        public async Task<SchoolTypesDTO> SaveSchoolTypes(SchoolTypesDTO message)
        {



            var mapper = await CreateMapperForAdmin<SchoolType, SchoolTypesDTO>();

            SchoolType obj = new SchoolType();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.IsActive = message.IsActive;

                uow.GetRepository<SchoolType>().Insert(obj);
           
            await uow.CommitAsync();
            var result = mapper.Map<SchoolTypesDTO>(obj);
            result.UpdateBy = userInfo.DBName;
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
           
            
        }
        public async Task<SchoolTypesDTO> UpdateSchoolTypes(SchoolTypesDTO message)
        {


            var mapper = await CreateMapperForAdmin<SchoolType, SchoolTypesDTO>();

            var result = new SchoolTypesDTO();

            if (message.Id is not null)
            {
                SchoolType obj = await uow.GetRepository<SchoolType>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.IsActive = message.IsActive;

                uow.GetRepository<SchoolType>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<SchoolTypesDTO>(obj);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
            }

                return result;
           
        }
        
        public async Task<SchoolTypesDTO> DeleteSchoolTypes(Guid? Id)
        {



            var mapper = await CreateMapperForAdmin<SchoolType, SchoolTypesDTO>();
            var result = new SchoolTypesDTO();
                if (Id is not null)
                {
                SchoolType obj = await uow.GetRepository<SchoolType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                var Organization = await uow.GetRepository<Organization>()
.GetAllNonDeleted()
                      .Where(x => x.OrgTypeId == obj.Id)
                      .ToListAsync();
                if (Organization.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SchoolTypesExistsOrganization);
                }

                var School = await uow.GetRepository<School>()
.GetAllNonDeleted()
                      .Where(x => x.TypeId == obj.Id)
                      .ToListAsync();
                if (School.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SchoolTypesExistsSchool);
                }

                uow.GetRepository<SchoolType>().Delete(obj);
                    await uow.CommitAsync();
                result = mapper.Map<SchoolTypesDTO>(obj);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
