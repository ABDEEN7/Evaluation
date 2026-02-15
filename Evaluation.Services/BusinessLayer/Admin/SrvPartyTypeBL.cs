using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.UserEntiy;
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
    public class SrvPartyTypeBL : AdminBase
    {
        public SrvPartyTypeBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }


        public async Task<List<PartyTypeDTO>> GetPartyTypeList(AdminSearchDTO message)
        {

           

            var list = await uow.GetRepository<PartyType>()
                .GetAllNonDeleted()
                 .Include(x => x.CreateBy)
                .OrderByDescending(x=>x.CreateDate)
                .ToListAsync();
            if (message.SystemModuleId != null)
            {
                list = list.Where(c => c.DepartmentId == message.SystemModuleId).ToList();
            }

            if (!string.IsNullOrEmpty(message.Title))
            {
                list = list.Where(c =>
    !string.IsNullOrEmpty(c.NameAr) && c.NameAr.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
    !string.IsNullOrEmpty(c.NameEn) && c.NameEn.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0
).ToList();

            }
            list = list.Skip(message.PageNum!.Value * message.PageSize!.Value).Take(message.PageSize.Value).ToList();
            var result = mapper.Map<List<PartyTypeDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;

        }


     
       
        public async Task<PartyTypeDTO> SavePartyType(PartyTypeDTO message)
        {

           

            var PartyTypeBackendName = await GenerateBackendNameBySystemModule(message.NameEn, message.DepartmentId, "P");
            var existBackendName = await uow
             .GetRepository<PartyType>()
                  .GetAllNonDeleted(x => x.BackendName == PartyTypeBackendName)
                  .FirstOrDefaultAsync();

            if (existBackendName != null)
            {

                message.ResponseStatus = DBResult.Exist;
                return message;
            }
            PartyType obj = new PartyType();
            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.BackendName = PartyTypeBackendName;
            obj.IsEmployeePartyType = message.IsEmployeePartyType;
            obj.CanViewAllRequests = message.CanViewAllRequests;
            obj.CanViewAllEvaluations = message.CanViewAllEvaluations;
            obj.DepartmentId = message.DepartmentId;
            obj.IsActive = message.IsActive;

            uow.GetRepository<PartyType>().Insert(obj);
           

            //insert values to UserPartyType
            if (message.UserPartyType != null)
            {
                List<UserPartyType> objentitylist=new List<UserPartyType>();
                foreach (var item in message.UserPartyType)
                {
                    UserPartyType objentity = new UserPartyType();
                    objentity.UserId = item;
                    objentity.PartyTypeId = obj.Id;
                    objentity.IsActive = true;
                    objentitylist.Add(objentity);
                }
                if (objentitylist.Count > 0)
                {
                    await uow.GetRepository<UserPartyType>().InsertRange(objentitylist);
                }


            }
            await uow.CommitAsync();
            var result = mapper.Map<PartyTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
            result.UserPartyType = message.UserPartyType;
            return result;

        }
        public async Task<PartyTypeDTO> UpdatePartyType(PartyTypeDTO message)
        {



          
            var result = new PartyTypeDTO();

            if (message.Id is not null)
            {


                PartyType obj = await uow.GetRepository<PartyType>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                obj.NameAr = message.NameAr;
                obj.NameEn = message.NameEn;
                obj.BackendName = obj.BackendName;
                obj.IsEmployeePartyType = message.IsEmployeePartyType;
                obj.CanViewAllRequests = message.CanViewAllRequests;
                obj.CanViewAllEvaluations = message.CanViewAllEvaluations;
                obj.DepartmentId = message.DepartmentId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<PartyType>().Update(obj);
              
                //update values to UserPartyType
                List<UserPartyType>  objUserPartyTypedelete = await uow.GetRepository<UserPartyType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.PartyTypeId == obj.Id)
                                      .ToListAsync();

                var UserPartyTypeexistids =new List<Guid>();
                if (objUserPartyTypedelete.Count > 0)
                {
                    foreach (var item in objUserPartyTypedelete)
                    {
                        if (message.UserPartyType != null && message.UserPartyType.Contains(item.UserId))
                        {
                            UserPartyTypeexistids.Add(item.UserId);
                        }
                        else
                        {
                            uow.GetRepository<UserPartyType>().Delete(item);
                        }

                    }
                }
                if (message.UserPartyType != null)
                {
                    var notInSelected = message.UserPartyType.Except(UserPartyTypeexistids).ToList();
                    List<UserPartyType> objentitylist=new List<UserPartyType>();
                    foreach (var item in notInSelected)
                    {
                        UserPartyType objentity = new UserPartyType();
                        objentity.PartyTypeId = obj.Id;
                        objentity.UserId = item;
                        objentity.IsActive = true;
                        objentitylist.Add(objentity);
                    }
                    if (objentitylist.Count > 0)
                    {
                        await uow.GetRepository<UserPartyType>().InsertRange(objentitylist);
                    }


                }
                await uow.CommitAsync();
                 result = mapper.Map<PartyTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.UserPartyType = message.UserPartyType;
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }

        public async Task<PartyTypeDTO> DeletePartyType(Guid? Id)
        {
            
            var result = new PartyTypeDTO();
            if (Id is not null)
            {
                PartyType obj = await uow.GetRepository<PartyType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
               

                List<UserPartyType> objUserPartyType = await uow.GetRepository<UserPartyType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.PartyTypeId == obj.Id)
                                      .ToListAsync();
                if(objUserPartyType.Count>0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PartyTypeCannotDelete);
                }

               
                var ServiceInitiatorPartyType = await uow.GetRepository<ServiceInitiatorPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.PartyTypeId == obj.Id)
                      .ToListAsync();
                if (ServiceInitiatorPartyType.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PartyTypeExistsServiceInitiatorPartyType);
                }
                var ServiceStatusPartyTypeDisplayName = await uow.GetRepository<ServiceStatusPartyTypeDisplayName>()
.GetAllNonDeleted()
                      .Where(x => x.PartyTypeId == obj.Id)
                      .ToListAsync();
                if (ServiceStatusPartyTypeDisplayName.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PartyTypeExistsServiceStatusPartyTypeDisplayName);
                }
               
                var ServiceRequestShowPartyType = await uow.GetRepository<ServiceRequestShowPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.PartyTypeId == obj.Id)
                      .ToListAsync();
                if (ServiceRequestShowPartyType.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PartyTypeExistsServiceRequestShowPartyType);
                }
                var ServiceStatusPreventPartyType = await uow.GetRepository<ServiceStatusPreventPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.PartyTypeId == obj.Id)
                      .ToListAsync();
                if (ServiceStatusPreventPartyType.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PartyTypeExistsServiceStatusPreventPartyType);
                }
                var ActionAssignPartyType = await uow.GetRepository<ActionAssignPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.PartyTypeId == obj.Id)
                      .ToListAsync();
                if (ActionAssignPartyType.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PartyTypeExistsActionAssignPartyType);
                }
                
                
                var ActionPartyType = await uow.GetRepository<ActionPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.PartyTypeId == obj.Id)
                      .ToListAsync();
                if (ActionPartyType.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PartyTypeExistsActionPartyType);
                }
                var ActionShowLogPartyType = await uow.GetRepository<ActionShowLogPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.PartytypeId == obj.Id)
                      .ToListAsync();
                if (ActionShowLogPartyType.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PartyTypeExistsActionShowLogPartyType);
                }
               
                
               
                var FieldPartyType = await uow.GetRepository<FieldPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.PartyTypeId == obj.Id)
                      .ToListAsync();
                if (FieldPartyType.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PartyTypeExistsFieldPartyType);
                }
               
                var ActionStatusConfigNotificationPartyType = await uow.GetRepository<ActionStatusConfigNotification>()
.GetAllNonDeleted()
                      .Where(x => x.PartyTypeId == obj.Id)
                      .ToListAsync();
                if (ActionStatusConfigNotificationPartyType.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.PartyTypeExistsActionStatusConfigNotification);
                }
               
                
                
                uow.GetRepository<PartyType>().Delete(obj);
                await uow.CommitAsync();
                 result = mapper.Map<PartyTypeDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;


        }

    }
}
