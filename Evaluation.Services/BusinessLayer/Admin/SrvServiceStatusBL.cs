//using AutoMapper;
//using Evaluation.DAL.Entities.ActionEntities;
//using Evaluation.DAL.Entities.Authentication;
//using Evaluation.DAL.Entities.ServicesEntities;
//using Evaluation.DAL.Entities.StatusEntities;
//using Evaluation.DAL.Helper;
//using Evaluation.DAL.UnitOfWork;
//using Evaluation.Services.Extensions;
//using Evaluation.Services.Special;
//using Evaluation.SharedHelper;
//using Evaluation.SharedHelper.Enums;
//using Evaluation.SharedHelper.Exceptions;
//using Evaluation.SharedHelper.Models;
//using Evaluation.SharedHelper.Models.Admin;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json;
//namespace Evaluation.Services.Models.Admin
//{
//    public class SrvServiceStatusBL : AdminBase
//    {
//        private readonly SrvBaseBL srvApplicationBL;

//        public SrvServiceStatusBL(IServiceProvider serviceProvider, UnitOfWork uow,
//            LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
//            IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo, SrvBaseBL srvApplicationBL)
//            : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
//        {
//            this.srvApplicationBL = srvApplicationBL;
//        }

//        public async Task<List<ServiceStatusDTO>> GetAllServiceStatusList(Guid serviceId, int Page, int PageSize)
//        {
            
//            var mapper = await CreateMapperForAdmin<ServiceStatus, ServiceStatusDTO>();
//            var list = await uow.GetRepository<ServiceStatus>()
//                .GetAllNonDeleted()
//                .Where(x => x.ServiceId == serviceId)
//                .Include(x => x.CreateBy)
//                .OrderBy(x => x.OrderNo)
//                .ThenByDescending(x => x.CreateDate)
               
//                .ToListAsync();

//            var result = mapper.Map<List<ServiceStatusDTO>>(list);

//            return result;
//        }


//        public async Task<List<DropdownItem>> GetServicesForStatus()
//        {
//            using (var uow = serviceScopeFactory.CreateScopedUow())
//            {
//                var list = await uow.GetRepository<Service>()
//                    .GetAllNonDeleted()
//                    .Select(x => new DropdownItem
//                    {
//                        Id = x.Id,
//                        NameAr = x.NameAr,
//                        NameEn = x.NameEn,
//                        OrderNo = x.OrderNo,
//                        Type = "Service",
//                    })
//                    .OrderBy(x => x.OrderNo)
//                    .ToListAsync();

//                return list;
//            }
//        }
       

//        public async Task<ServiceStatusDTO> SaveServiceStatus(ManageServiceStatusDTO model)
//        {
//            if (model?.status == null || string.IsNullOrWhiteSpace(model.json))
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
//            }

//            bool isValid = await srvApplicationBL.ValidateObject(model, ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE_STATUS);
//            if (!isValid)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
//            }
//            var servicefreezecount = await uow.GetRepository<Service>()
//                      .GetAllNonDeleted()
//                      .Where(x => x.Id == model.status.ServiceId && x.IsFreez == true).ToListAsync();

//            if (servicefreezecount.Count > 0)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_ADD);
//            }
//                var mapper = await CreateMapperForAdmin<ServiceStatus, ServiceStatusDTO>();
//            var entity = mapper.Map<ServiceStatus>(model.status);

//            entity.BackendName = $"{model.status.ServiceId}_{model.status.NameEn}";
//            entity = uow.GetRepository<ServiceStatus>().Insert(entity);

//            await InsertPreventPartyTypes(model.ServiceStatusPreventPartyTypesList, entity.Id);
//            await SaveOrUpdateServiceStatusPartyTypeDisplayNameList(entity.Id, model.json);

//            await uow.CommitAsync();

//            var result = mapper.Map<ServiceStatusDTO>(entity);
//            result.ResponseStatus = DBResult.Inserted;

//            return result;
//        }
//        private async Task InsertPreventPartyTypes(List<Guid> partyTypeIds, Guid statusId)
//        {
//            if (partyTypeIds == null || !partyTypeIds.Any())
//                return;

//            var repo = uow.GetRepository<ServiceStatusPreventPartyType>();

//            foreach (var id in partyTypeIds)
//            {
//                var entity = new ServiceStatusPreventPartyType
//                {
//                    StatusId = statusId,
//                    PartyTypeId = id
//                };
//                await  repo.InsertAsync(entity);
//            }
//        }

        
//        public async Task<ServiceStatusDTO> UpdateServiceStatus(ManageServiceStatusDTO model)
//        {
//            if (model == null || model.status == null || string.IsNullOrEmpty(model.json))
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
//            }

//            if (model.status.Id is  null)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
//            }

//            bool validateObject = await srvApplicationBL.ValidateObject(model, ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE_STATUS);
//            if (!validateObject)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
//            }

//            var entity = await uow.GetRepository<ServiceStatus>()
//                                .GetAllNonDeleted()
//                                .Where(x => x.Id == model.status.Id)
//                                .FirstOrDefaultAsync();

//            if (entity == null)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
//            }
//            var servicefreezecount = await uow.GetRepository<Service>()
//.GetAllNonDeleted()
//                      .Where(x => x.Id == entity.ServiceId && x.IsFreez == true).ToListAsync();

//            if (servicefreezecount.Count > 0)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_EDIT);
//            }
//            entity.NameAr = model.status.NameAr;
//            entity.NameEn = model.status.NameEn;
//            entity.IsInitial = model.status.IsInitial;
//            entity.IsOpen = model.status.IsOpen;
//            entity.IsActive = model.status.IsActive;
//            entity.StatusGroupId = model.status.StatusGroupId;
//            entity.ColorCode = model.status.ColorCode;

//            entity = uow.GetRepository<ServiceStatus>().Update(entity);


//            var all_statusPreventPartyTypeList = uow.GetRepository<ServiceStatusPreventPartyType>()
//                           .GetAllNonDeleted()
//                           .Where(x => x.StatusId == entity.Id)
//                           .ToList();



//            if (model.ServiceStatusPreventPartyTypesList != null)
//            {
//                var statusPreventPartyTypeListToBeDeleted = all_statusPreventPartyTypeList
//                      .Where(x => !model.ServiceStatusPreventPartyTypesList.Contains(x.PartyTypeId))
//                      .ToList();

//                foreach (var item in statusPreventPartyTypeListToBeDeleted)
//                {
//                    uow.GetRepository<ServiceStatusPreventPartyType>().Delete(item);
//                }

//                var partyTypes = all_statusPreventPartyTypeList.Select(x => x.PartyTypeId).ToList();
//                var statusPreventPartyTypeListToInserted = model.ServiceStatusPreventPartyTypesList
//                    .Where(x => !partyTypes.Contains(x))
//                    .ToList();

//                foreach (var partyTypeId in statusPreventPartyTypeListToInserted)
//                {
//                    var item = new ServiceStatusPreventPartyType
//                    {
//                        PartyTypeId = partyTypeId,
//                        StatusId = entity.Id
//                    };
//                    uow.GetRepository<ServiceStatusPreventPartyType>().Insert(item);
//                }
//            }

//            var list = await SaveOrUpdateServiceStatusPartyTypeDisplayNameList(entity.Id, model.json);

//            await uow.CommitAsync();

//            var mapper = await CreateMapperForAdmin<ServiceStatus, ServiceStatusDTO>();

//            var result = mapper.Map<ServiceStatusDTO>(entity);

//            result.ResponseStatus = DBResult.Updated;

//            return result;

//        }
//        public async Task<bool> UpdateServiceStatusOrder(List<OrderingDTO> orderList)
//        {
//            orderList = orderList.OrderBy(x => x.OrderNo).ToList();

//            var idsList = orderList.Select(y => y.Id).ToList();

//            var listToBeUpdated = await uow.GetRepository<ServiceStatus>()
//                .GetAllNonDeleted()
//                //.Where(x => x.ServiceId == serviceId)
//                .Where(x => idsList.Contains(x.Id))
//                .ToListAsync();


//            foreach (var item in orderList)
//            {
//                var entity = listToBeUpdated.Where(x => x.Id == item.Id).FirstOrDefault();

//                if (entity == null)
//                {
//                    throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
//                }
//                entity.OrderNo = item.OrderNo;
//            }

//            uow.GetRepository<ServiceStatus>().UpdateRange(listToBeUpdated);
//            await uow.CommitAsync();

//            return true;
//        }
//        public async Task<bool> DeleteServiceStatus(Guid? Id)
//        {
//            if (Id is  null)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
//            }

//            var entity = await uow.GetRepository<ServiceStatus>()
//                                .GetAllNonDeleted()
//                                .Where(x => x.Id == Id)
//                                .FirstOrDefaultAsync();

//            if (entity == null)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
//            }
//            var servicefreezecount = await uow.GetRepository<Service>()
//.GetAllNonDeleted()
//                      .Where(x => x.Id == entity.ServiceId && x.IsFreez == true).ToListAsync();

//            if (servicefreezecount.Count > 0)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_DELETE);
//            }
//            var existentity = await uow.GetRepository<ActionStatusConfiguration>()
//                                .GetAllNonDeleted()
//                                .Where(x => x.CurrentStatusId == Id || x.NextStatusId == Id)
//                                .ToListAsync();
//            if (existentity.Count > 0)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.ServiceStatusUsed);
//            }
           
            
//            var ServiceStatusPreventPartyType = await uow.GetRepository<ServiceStatusPreventPartyType>()
//.GetAllNonDeleted()
//                      .Where(x => x.StatusId ==Id)
//                      .ToListAsync();
//            if (ServiceStatusPreventPartyType.Count > 0)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.StatusExistsServiceStatusPreventPartyType);
//            }
           
//            var ServiceStatusPartyTypeDisplayName = await uow.GetRepository<ServiceStatusPartyTypeDisplayName>()
//.GetAllNonDeleted()
//                      .Where(x => x.StatusId ==Id)
//                      .ToListAsync();
//            if (ServiceStatusPartyTypeDisplayName.Count > 0)
//            {
//                uow.GetRepository<ServiceStatusPartyTypeDisplayName>().DeleteRange(ServiceStatusPartyTypeDisplayName);
//            }
//            uow.GetRepository<ServiceStatus>().Delete(entity);
//            await uow.CommitAsync();

//            return true;

//        }

//        public async Task<ServiceStatusDetailsDTO> GetStatusDetails(Guid? statusId)
//        {

//            var result = new ServiceStatusDetailsDTO { };
//            if (statusId is  null)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
//            }

//            var entity = await uow.GetRepository<ServiceStatus>()
//                                .GetAllNonDeleted()
//                                .Where(x => x.Id == statusId)
//                                .FirstOrDefaultAsync();

//            if (entity == null)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
//            }


//            var mapper = await CreateMapperForAdmin<ServiceStatus, ServiceStatusDTO>();

//            var serviceStatusDTO = mapper.Map<ServiceStatusDTO>(entity);
//            result.ServiceStatus = serviceStatusDTO;


//            result.ServiceStatusPreventPartyTypesList = await uow.GetRepository<ServiceStatusPreventPartyType>()
//                    .GetAllNonDeleted()
//                    .Include(x => x.PartyType)
//                    .Where(x => x.StatusId == statusId)
//                    .Where(x => x.PartyType.IsDeleted == false)
//                    .Select(x => x.PartyTypeId)
//                    .ToListAsync();

//            result.PartyTypesList = new List<DropdownItem>();
//            result.PartyTypesList = await uow.GetRepository<PartyType>()
//                                .GetAllNonDeleted()
//                                .Select(x => new DropdownItem
//                                {
//                                    Id = x.Id,
//                                    NameAr = x.NameAr,
//                                    NameEn = x.NameEn,
//                                    Type = "PartyType",
//                                })
//                                //.OrderBy(x => x.OrderNo)
//                                .ToListAsync();

//            return result;
//        }

        
//        public async Task<List<DropdownItem>> GetPartyTypesList()
//        {
//            using (var uow = serviceScopeFactory.CreateScopedUow())
//            {
//                var partyTypesList = new List<DropdownItem>();
//                partyTypesList = await uow.GetRepository<PartyType>()
//                                    .GetAllNonDeleted()
//                                    .Select(x => new DropdownItem
//                                    {
//                                        Id = x.Id,
//                                        NameAr = x.NameAr,
//                                        NameEn = x.NameEn,
//                                        Type = "PartyType",
//                                    })
//                                    //.OrderBy(x => x.OrderNo)
//                                    .ToListAsync();
//                return partyTypesList;
//            }
//        }

//        private async Task<List<ServiceStatusPartyTypeDisplayNameDTO>> SaveOrUpdateServiceStatusPartyTypeDisplayNameList(Guid serviceStatusId, string json)
//        {
//            using (var uow = serviceScopeFactory.CreateScopedUow())
//            {
//                var list = JsonConvert.DeserializeObject<List<ServiceStatusPartyTypeDisplayNameDTO>>(json);
//                if (list == null || !list.Any()) return await GetServiceStatusPartyTypeDisplayNameList(serviceStatusId);

//                list.ForEach(item => item.StatusId = serviceStatusId);

//                await ValidateServiceStatusPartyTypeDisplayNameList(serviceStatusId, list);

//                var toDeleteIds = list
//        .Where(x => string.IsNullOrWhiteSpace(x.TitleEn) && string.IsNullOrWhiteSpace(x.TitleAr))
//        .Select(x => x.Id)
//        .ToList();

//                if (toDeleteIds.Any())
//                {
//                    var repo = uow.GetRepository<ServiceStatusPartyTypeDisplayName>();
//                    var entitiesToDelete = await repo.GetAllNonDeleted()
//            .Where(x => toDeleteIds.Contains(x.Id))
//            .ToListAsync();

//                    foreach (var entity in entitiesToDelete)
//                    {
//                        repo.Delete(entity);
//                    }
//                }



//                await UpsertServiceStatusPartyTypeDisplayNames(list);

//                return await GetServiceStatusPartyTypeDisplayNameList(serviceStatusId);
//            }
//        }

//        private async Task UpsertServiceStatusPartyTypeDisplayNames(List<ServiceStatusPartyTypeDisplayNameDTO> list)
//        {
//            var repo = uow.GetRepository<ServiceStatusPartyTypeDisplayName>();
//            var ids = list.Where(x => x.Id != Guid.Empty).Select(x => x.Id).ToList();

//            var existing = await repo.GetAllNonDeleted()
//        .Where(x => ids.Contains(x.Id))
//        .ToListAsync();

//            foreach (var dto in list)
//            {
//                var entity = existing.FirstOrDefault(x => x.Id == dto.Id && x.PartyTypeId == dto.PartyTypeId);
//                if (entity != null)
//                {
//                    entity.TitleEn = dto.TitleEn;
//                    entity.TitleAr = dto.TitleAr;
//                    entity.IsActive = dto.IsActive;
//                    repo.Update(entity);
//                }
//                else
//                {
//                    if(!string.IsNullOrWhiteSpace(dto.TitleAr) && !string.IsNullOrWhiteSpace(dto.TitleEn))
//                    {
//                        var newEntity = new ServiceStatusPartyTypeDisplayName
//                        {
//                            PartyTypeId = dto.PartyTypeId,
//                            TitleAr = dto.TitleAr,
//                            TitleEn = dto.TitleEn,
//                            StatusId = dto.StatusId,
//                            IsActive = dto.IsActive,
//                        };
//                        repo.Insert(newEntity);
//                    }
                    
//                }
//            }
//        }

//        private async Task ValidateServiceStatusPartyTypeDisplayNameList(Guid? serviceStatusId, List<ServiceStatusPartyTypeDisplayNameDTO> list)
//        {
//            if (serviceStatusId == null || serviceStatusId == Guid.Empty || list == null)
//            {
//                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
//            }

//            if (!list.Any())
//            {
//                throw new BusinessException("Display name list cannot be empty.");
//            }

//            bool hasInvalidIds = list.Any(x => x.StatusId == Guid.Empty || x.PartyTypeId == Guid.Empty);
//            if (hasInvalidIds)
//            {
//                throw new BusinessException("Invalid StatusId or PartyTypeId in the list.");
//            }

//            bool hasMismatchedTitles = list.Any(x =>
//        string.IsNullOrWhiteSpace(x.TitleEn) != string.IsNullOrWhiteSpace(x.TitleAr)
//    );

//            if (hasMismatchedTitles)
//            {
//                throw new BusinessException("Both TitleAr and TitleEn must be provided together.");
//            }

//            var itemsToValidate = list
//        .Where(x => !string.IsNullOrWhiteSpace(x.TitleAr) && !string.IsNullOrWhiteSpace(x.TitleEn))
//        .ToList();

//            foreach (var item in itemsToValidate)
//            {
//                var titleObject = new
//                {
//                    TitleAr = item.TitleAr,
//                    TitleEn = item.TitleEn,
//                };

//                bool isValid = await srvApplicationBL.ValidateObject(titleObject,
//            ConstantKeys.AdminPermission.UPDATE_STATUS_PARTY_TYPE_DISPLAY_NAME_STATUS);

//                if (!isValid)
//                {
//                    throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
//                }
//            }
//        }
//        public async Task<List<ServiceStatusPartyTypeDisplayNameDTO>> GetServiceStatusPartyTypeDisplayNameList(Guid serviceStatusId)
//        {
//            var result = new List<ServiceStatusPartyTypeDisplayNameDTO>();

//            var partyTypes = await uow.GetRepository<PartyType>()
//                .GetAllNonDeleted()
//                .OrderBy(x => x.CreateDate)
//                .ToListAsync();


//            var list = await uow.GetRepository<ServiceStatusPartyTypeDisplayName>()
//                .GetAllNonDeleted()
//                .Where(x => x.StatusId == serviceStatusId)
//                .ToListAsync();


//            foreach (var partyType in partyTypes)
//            {
//                var item = new ServiceStatusPartyTypeDisplayNameDTO
//                {
//                    PartyTypeId = partyType.Id,
//                    PartyTypeNameAr = partyType.NameAr,
//                    PartyTypeNameEn = partyType.NameEn,
//                    StatusId = serviceStatusId,
//                };

//                var serviceStatusPartyTypeDisplayName = list.Where(x => x.PartyTypeId == partyType.Id).FirstOrDefault();

//                if (serviceStatusPartyTypeDisplayName != null)
//                {
//                    item.Id = serviceStatusPartyTypeDisplayName.Id;
//                    item.TitleAr = serviceStatusPartyTypeDisplayName.TitleAr;
//                    item.TitleEn = serviceStatusPartyTypeDisplayName.TitleEn;
//                    item.IsActive = serviceStatusPartyTypeDisplayName.IsActive;
//                }
//                else
//                {
//                    item.IsActive = true;
//                }

//                result.Add(item);
//            }

//            return result;
//        }


//        //public async Task<List<ServiceStatusPartyTypeDisplayNameDTO>> GetServiceStatusPartyTypeDisplayNameList(Guid serviceStatusId)
//        //{
//        //    var partyTypes = await uow.GetRepository<PartyType>()
//        //.GetAllNonDeleted()
//        //.OrderBy(x => x.CreateDate)
//        //.ToListAsync();

//        //    var displayNames = await uow.GetRepository<ServiceStatusPartyTypeDisplayName>()
//        //.GetAllNonDeleted()
//        //.Where(x => x.StatusId == serviceStatusId)
//        //.ToListAsync();

//        //    var displayNameMap = displayNames.ToDictionary(x => x.PartyTypeId, x => x);

//        //    var result = partyTypes.Select(partyType =>
//        //    {
//        //        displayNameMap.TryGetValue(partyType.Id, out var match);

//        //        return new ServiceStatusPartyTypeDisplayNameDTO
//        //        {
//        //            Id = match?.Id ?? Guid.Empty,
//        //            PartyTypeId = partyType.Id,
//        //            PartyTypeNameAr = partyType.NameAr,
//        //            PartyTypeNameEn = partyType.NameEn,
//        //            TitleAr = match?.TitleAr,
//        //            TitleEn = match?.TitleEn,
//        //            IsActive = match?.IsActive ?? true,
//        //            StatusId = serviceStatusId
//        //        };
//        //    }).ToList();

//        //    return result;
//        //}
//        public async Task<List<ServiceAction>> GetServiceAllAction()
//        {
//            using (var uow = serviceScopeFactory.CreateScopedUow())
//            {
//                var list = await uow.GetRepository<ServiceAction>()
//                .GetAllNonDeleted()
//                .Include(x=>x.ActionType)
//                .Select(x => new ServiceAction
//                {
//                    Id = x.Id,
//                    BackendName = x.ActionType!.BackendName
//                })
//                .ToListAsync();

//                return list;
//            }
//        }

//        //public async Task<List<ControlValidationDTO>> GetStatusPartyTypeDisplayNameList_Controls()
//        //{
//        //    var list = await uow.GetRepository<ControlValidation>()
//        //         .GetAllNonDeleted()
//        //         .Where(x => x.Permission.BackendName == ConstantKeys.AdminPermission.UPDATE_STATUS_PARTY_TYPE_DISPLAY_NAME)
//        //         .Select(x => new ControlValidationDTO
//        //         {
//        //             Id = x.Id,
//        //             IsRequired = x.IsRequired,
//        //             MinLength = x.MinLength,
//        //             MaxLength = x.MaxLength,
//        //         })
//        //         .ToListAsync();


//        //    return list;
//        //}
//    }
//}
