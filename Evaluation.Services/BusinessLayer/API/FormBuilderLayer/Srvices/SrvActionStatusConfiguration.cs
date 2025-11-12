using Azure.Core;
using Evaluation.DAL.Entities.ActionEntities;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.ServiceRequestEntities;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs;
using Evaluation.SharedHelper.Models.Api.TemplatesDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Evaluation.SharedHelper.Enums.ConstantKeys;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvActionStatusConfiguration (SrvUser srvUser,SrvAction SrvAction,IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices,  UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, userInfo, serviceProvider, _requestInfo)
    { 
     
        public async Task<List<ActionDTO>> GetActionsByStatus(Guid serviceId,Guid? statusId, Guid? requestId, Guid? planId, string lang, bool CheckActionCondition=true)
        {
            var ActionStatusConfiguration = (await cacheDataProvider.GetActionStatusConfiguration())
                                            .Where(c =>c.ServiceAction!.ServiceId== serviceId && (c.CurrentStatusId == statusId || (statusId==null && requestId==null && c.CurrentStatus!.IsInitial==true)))
                                            .OrderBy(c => c.OrderNo)
                                            .Select(c => c.ServiceActionId)
                                            .ToList();

            var ActionStatusConfigurationOrder = ActionStatusConfiguration
                                                 .Select((id, index) => new { ActionId = id, Order = index });

            var actionPartType = (await cacheDataProvider.GetActionPartyTypes())
                                  .Where(c => userInfo.PartyTypes.Contains(c.PartyTypeId))
                                  .Select(c => c.ServiceActionId)
                                  .ToList();

            var actionsId = ActionStatusConfiguration.Intersect(actionPartType).ToList();

            if (!actionsId.Any()) return new List<ActionDTO>();

            var actionsquery = serviceScopeFactory.CreateScopedUow()
                                        .GetRepository<ServiceAction>()
                                        .GetAllQueryFiltered()
                                        .AsNoTracking()
                                        .Include(c => c.ActionType!)
                                        .Include(c => c.ActionTemplateDocs!)
                                        .ThenInclude(c => c.TemplateDoc)
                                        .Where(c => actionsId.Contains(c.Id) &&
                                                    ((requestId == null && c.IsInitialAction == true) ||
                                                     (requestId != null && c.IsInitialAction == false)));

            var actions = await actionsquery.Select(c => new ActionDTO
            {
                BakendName = c.BackendName,
                Id = c.Id,
                Title = lang == "ar" ? c.NameAr! : c.NameEn!,
                OrderNo = c.ActionType!.OrderNo,
                ActionTypeBackEndKey = c.ActionType.BackendName,
                IsInitialAction = c.IsInitialAction,
                Templates = c.ActionTemplateDocs!.Select(m => new TempLateDocDTO()
                {
                    Id = m.TemplateDocId,
                    Name = lang == "ar" ? m.TemplateDoc!.NameAr : m.TemplateDoc!.NameEn,
                    TemplateConfiguration = m.TemplateDoc.TemplateGenrationTypeId,
                    ActionBackEndName = c.BackendName
                }).ToList()
            }).OrderBy(c => c.OrderNo).ToListAsync();

            var validActions = new List<ActionDTO>();

            if (actions.Any())
            {
                if (CheckActionCondition)
                {
                    foreach (var action in actions)
                    {
                        bool isValid = await ValidateActionConditions(action.Id!.Value, requestId, planId);
                        if (isValid)
                        {
                            validActions.Add(action);
                        }
                    }
                }
                    else
                {
                    validActions = actions;
                }
            }

            validActions = validActions
                           .OrderBy(c => ActionStatusConfigurationOrder?.ToList()?.FindIndex(item => item.ActionId == c.Id) ?? -1)
                           .ToList();

            return validActions;
        }

        public async Task<List<TempLateDocDTO>> GetActionTemplatesByStatus(Guid requestId)
        {
            string lang = _requestInfo.Lang;
            var userTask = srvUser.GetByIDActiveNonDeleted(userInfo.UserId!.Value);

			var request = await serviceScopeFactory.CreateScopedUow()
                                       .GetRepository<ServiceRequest>().GetByIDActiveNonDeleted(requestId);

            if (request == null) { throw new BusinessException(ConstantKeys.ExceptionMessage.ServiceRequestNotFound); }
            var statusId = request.StatusId;
            var user = await userTask;

            var isMinistry = user is MinistryUser;
            if (isMinistry)
            {
                var ActionIdsInStatus = (await cacheDataProvider.GetActionStatusConfiguration())
                                           .Where(c => c.CurrentStatusId == statusId)
                                           .OrderBy(c => c.OrderNo)
                                           .Select(c => c.ServiceActionId)
                                           .ToList();
               
                var actionsquery = serviceScopeFactory.CreateScopedUow()
                                       .GetRepository<ServiceAction>()
                                       .GetAllQueryFiltered()
                                       .Include(c => c.ActionType!)
                                       .Include(c => c.ActionTemplateDocs!)
                                       .ThenInclude(c => c.TemplateDoc)
                                      .Where(c => ActionIdsInStatus.Contains(c.Id));
                var allTemplates = actionsquery.SelectMany(x => x.ActionTemplateDocs!.Select(m => new TempLateDocDTO
                {
                    Id = m.TemplateDocId,
                    Name = lang == "ar" ? m.TemplateDoc!.NameAr : m.TemplateDoc!.NameEn,
                    TemplateConfiguration = m.TemplateDoc.TemplateGenrationTypeId,
                    ActionBackEndName = x.BackendName,
                })).ToList();
                
                return allTemplates;
            }
            else
                return null!;
        }

        public async Task<bool> ValidateActionConditions(Guid ActionId, Guid? RequestId, Guid? planId)
        {
            var uow = serviceScopeFactory.CreateScopedUow();
            var conditions = await uow.GetRepository<ActionCondition>()
                .GetAllQueryFiltered()
                .Where(c => c.ServiceActionId == ActionId)
                .ToListAsync();

            if (!conditions.Any())
            {
                return true;
            }

            foreach (var condition in conditions)
            {
                object? fieldValue;

                if (condition.Type.Equals("Service", StringComparison.OrdinalIgnoreCase))
                {
                    var fieldValueEntity = await uow.GetRepository<ServiceRequestFieldsValue>()
                        .GetAllQueryFiltered()
                        .FirstOrDefaultAsync(c => c.ServiceRequestId == RequestId && c.FieldId == condition.RefID);

                    if (fieldValueEntity == null)
                    {
                        return false;
                    }

                    fieldValue = fieldValueEntity.Value;
                }
                //else if (condition.Type.Equals("scholarship", StringComparison.OrdinalIgnoreCase))
                //{
                //    var sysField = await uow.GetRepository<SystemField>()
                //        .GetAllQueryFiltered()
                //        .FirstOrDefaultAsync(f => f.Id == condition.RefID);

                //    if (sysField == null)
                //    {
                //        return false;
                //    }

                //    if (sysField.IsCoreColumn)
                //    {
                //        var schData = await uow.GetRepository<ScholarshipData>()
                //            .GetAllQueryFiltered()
                //            .FirstOrDefaultAsync(c => c.Id == planId);

                //        if (schData == null)
                //        {
                //            return false;
                //        }

                //        fieldValue = sysField.BackendName switch
                //        {
                //            "MajorId" => schData.MajorId,
                //            "CountryId" => schData.CountryId,
                //            "UniversityId" => schData.UniversityId,
                //            "AcademicDegreeId" => schData.AcademicDegreeId,
                //            "ParentAcademicDegreeId" => schData.ParentAcademicDegreeId,
                //            "SchPlanId" => schData.SchPlanId,
                //            "SchStatusId" => schData.SchStatusId,
                //            _ => throw new ArgumentException($"Unsupported BackendName: {sysField.BackendName}")
                //        };
                //    }
                //    else
                //    {
                //        var schFieldValueEntity = await uow.GetRepository<SchFieldValue>()
                //            .GetAllQueryFiltered()
                //            .FirstOrDefaultAsync(c => c.planId == planId && c.SystemFieldId == condition.RefID);

                //        if (schFieldValueEntity == null)
                //        {
                //            return false;
                //        }

                //        fieldValue = schFieldValueEntity.Value;
                //    }
                //}
                else
                {
                    throw new ArgumentException($"Unsupported condition type: {condition.Type}");
                }

                bool isValid = await SrvAction.ValidatedConditionAsync(fieldValue, condition.operators, condition.FieldValue);

                if (!isValid)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<ActionStatusConfiguration> GetActionConfigurationDetails(Guid actionId, Guid StatusId)
        {

            var action = (await cacheDataProvider.GetActionStatusConfiguration())
                           .FirstOrDefault(c => c.ServiceActionId == actionId && c.CurrentStatusId == StatusId);
            if (action == null)
            { throw new BusinessException(ExceptionMessage.lbl_you_are_not_authorized_to_perform_this_action_in_the_current_status); }
            return action;

        }

    }


}
