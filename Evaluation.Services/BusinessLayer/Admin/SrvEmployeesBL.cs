using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Repositories;
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
    public class SrvEmployeesBL : AdminBase
    {
        public SrvEmployeesBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }


        public async Task<List<EmployeesDTO>> GetEmployeesList(AdminSearchDTO message)
        {

            

            var list = await uow.GetRepository<Employee>()
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
    !string.IsNullOrEmpty(c.Email) && c.Email.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.QID) && c.QID.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.EmployeeNo) && c.EmployeeNo.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0
).ToList();

            }
            list = list.Skip(message.PageNum!.Value * message.PageSize!.Value).Take(message.PageSize.Value).ToList();
            var result = mapper.Map<List<EmployeesDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;

        }

        public async Task<List<OrgClass>> GetOrgClass()
        {
            var OrgTypeBackend = await uow.GetRepository<SystemSetting>()
                                     .GetAllNonDeleted()
                                     .Where(x => x.SettingKey == AdminSettings.EmployeeOrgClassList.ToString())
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


        public async Task<EmployeesDTO> SaveEmployees(EmployeesDTO message)
        {
            var OrgTypeBackend = await uow.GetRepository<SystemSetting>()
                                     .GetAllNonDeleted()
                                     .Where(x => x.SettingKey == AdminSettings.EmployeeOrgType.ToString())
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
            Employee obj = new Employee();
            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.OrgTypeId = orgtypeid;
            obj.OrgClassId = message.OrgClassId;
            obj.EmployeeNo = message.EmployeeNo;
            obj.UserGenderId = message.UserGenderId;
            obj.BirthDate = message.BirthDate;
            obj.NationalityCode = message.NationalityCode;
            obj.JoinDate = message.JoinDate;
            obj.JobTitleId = message.JobTitleId;
            obj.Email = message.Email;
            obj.QID = message.QID;
            obj.IsOrgManager = message.IsOrgManager;
            obj.IsActive = message.IsActive;

            uow.GetRepository<Employee>().Insert(obj);
           

           
            await uow.CommitAsync();
            var result = mapper.Map<EmployeesDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            return result;

        }
        public async Task<EmployeesDTO> UpdateEmployees(EmployeesDTO message)
        {



          
            var result = new EmployeesDTO();

            if (message.Id is not null)
            {


                Employee obj = await uow.GetRepository<Employee>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.OrgTypeId = obj.OrgTypeId;
                obj.OrgClassId = message.OrgClassId;
                obj.EmployeeNo = message.EmployeeNo;
                obj.UserGenderId = message.UserGenderId;
                obj.BirthDate = message.BirthDate;
                obj.NationalityCode = message.NationalityCode;
                obj.JoinDate = message.JoinDate;
                obj.JobTitleId = message.JobTitleId;
                obj.Email = message.Email;
                obj.QID = message.QID;
                obj.IsOrgManager = message.IsOrgManager;
                obj.IsActive = message.IsActive;

                uow.GetRepository<Employee>().Update(obj);
              
             
                await uow.CommitAsync();
                 result = mapper.Map<EmployeesDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }

        public async Task<EmployeesDTO> DeleteEmployees(Guid? Id)
        {
            
            var result = new EmployeesDTO();
            if (Id is not null)
            {
                Employee obj = await uow.GetRepository<Employee>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                uow.GetRepository<Employee>().Delete(obj);
                await uow.CommitAsync();
                 result = mapper.Map<EmployeesDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;


        }

    }
}
