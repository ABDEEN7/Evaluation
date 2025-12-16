using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Evaluation.SharedHelper.Enums.ConstantKeys;
namespace Evaluation.Services.Models.Admin
{
    public class SrvSchoolsBL : AdminBase
    {
        public SrvSchoolsBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }


        public async Task<List<SchoolsDTO>> GetSchoolsList(AdminSearchDTO message)
        {

            

            var list = await uow.GetRepository<School>()
                .GetAllNonDeleted()
                 .Include(x => x.CreateBy)
                  .Include(x => x.OrgType)
                  .Include(x => x.OrgClass)
                .OrderByDescending(x=>x.CreateDate)
                .ToListAsync();
            if (message.OrgClassId != null)
            {
                list = list.Where(c => c.OrgClassId == message.OrgClassId).ToList();
            }

            if (!string.IsNullOrEmpty(message.Title))
            {
                list = list.Where(c =>
    !string.IsNullOrEmpty(c.NameAr) && c.NameAr.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.NameEn) && c.NameEn.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.ManagerQID) && c.ManagerQID.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.ManageEmail) && c.ManageEmail.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.OrgEmail) && c.OrgEmail.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0
).ToList();

            }
            list = list.Skip(message.PageNum!.Value * message.PageSize!.Value).Take(message.PageSize.Value).ToList();
            var result = mapper.Map<List<SchoolsDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;

        }

        public async Task<List<OrgClass>> GetOrgClass()
        {
            var OrgTypeBackend = await uow.GetRepository<SystemSetting>()
                                     .GetAllNonDeleted()
                                     .Where(x => x.SettingKey == AdminSettings.OrgClassList.ToString())
                                     .Select(x => x.SettingValue)
                                     .FirstOrDefaultAsync();


            var result = await uow.GetRepository<OrgClass>()
               .GetAllNonDeleted()
               .Where(x=>OrgTypeBackend.Contains(x.BackendName))
               .Include(x => x.CreateBy)
               .OrderByDescending(x => x.CreateDate)
               .Distinct()
               .ToListAsync();

            return result;

        }


        public async Task<SchoolsDTO> SaveSchools(SchoolsDTO message)
        {
            var OrgTypeBackend = await uow.GetRepository<SystemSetting>()
                                     .GetAllNonDeleted()
                                     .Where(x => x.SettingKey == AdminSettings.SchoolOrgType.ToString())
                                     .Select(x => x.SettingValue)
                                     .FirstOrDefaultAsync();
            if(OrgTypeBackend==null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.OrgTypeDoesNotExists);
            }

            var orgtypeid=await uow.GetRepository<OrgType>()
                                     .GetAllNonDeleted()
                                     .Where(x => x.BackendName == OrgTypeBackend)
                                     .Select(x => x.Id)
                                     .FirstOrDefaultAsync();
            School obj = new School();
            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.OrgTypeId = orgtypeid;
            obj.OrgClassId = message.OrgClassId;
            obj.EstablishmentDate = message.EstablishmentDate;
            obj.TypeId = message.TypeId;
            obj.ManagerQID = message.ManagerQID;
            obj.ManageEmail = message.ManageEmail;
            obj.OrgEmail = message.OrgEmail;
            obj.Address = message.Address;
            obj.Phone = message.Phone;
            obj.Mobile = message.Mobile;
            obj.Code = message.Code;
            obj.Region = message.Region;
            obj.IsAccredited = message.IsAccredited;
            obj.SupportIdentity = message.SupportIdentity;
            obj.AcceditedDate = message.AcceditedDate;
            obj.SupportIdentityDate = message.SupportIdentityDate;
            obj.IsActive = message.IsActive;

            uow.GetRepository<School>().Insert(obj);
           

           
            await uow.CommitAsync();
            var result = mapper.Map<SchoolsDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            return result;

        }
        public async Task<SchoolsDTO> UpdateSchools(SchoolsDTO message)
        {



          
            var result = new SchoolsDTO();

            if (message.Id is not null)
            {


                School obj = await uow.GetRepository<School>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.OrgTypeId = obj.OrgTypeId;
                obj.OrgClassId = message.OrgClassId;
                obj.EstablishmentDate = message.EstablishmentDate;
                obj.TypeId = message.TypeId;
                obj.ManagerQID = message.ManagerQID;
                obj.ManageEmail = message.ManageEmail;
                obj.OrgEmail = message.OrgEmail;
                obj.Address = message.Address;
                obj.Phone = message.Phone;
                obj.Mobile = message.Mobile;
                obj.Code = message.Code;
                obj.Region = message.Region;
                obj.IsAccredited = message.IsAccredited;
                obj.SupportIdentity = message.SupportIdentity;
                obj.AcceditedDate = message.AcceditedDate;
                obj.SupportIdentityDate = message.SupportIdentityDate;
                obj.IsActive = message.IsActive;

                uow.GetRepository<School>().Update(obj);
              
             
                await uow.CommitAsync();
                 result = mapper.Map<SchoolsDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }

        public async Task<SchoolsDTO> DeleteSchools(Guid? Id)
        {
            
            var result = new SchoolsDTO();
            if (Id is not null)
            {
                School obj = await uow.GetRepository<School>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
               

                List<SchoolLevel> objUserSchools = await uow.GetRepository<SchoolLevel>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.SchoolId == obj.Id)
                                      .ToListAsync();
                if(objUserSchools.Count>0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SchoolsExistsSchoolLevel);
                }

               
                
                
                uow.GetRepository<School>().Delete(obj);
                await uow.CommitAsync();
                 result = mapper.Map<SchoolsDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;


        }

    }
}
