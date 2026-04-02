using AutoMapper;
using Azure.Core;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.SystemLog;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.LogsDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Evaluation.SharedHelper.Enums.ConstantKeys;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvActionTransactionsLog (SrvStatus SrvStatus,IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
    {

        public async Task<Guid> UpdateStatusAndLogAction(RequestType requestType, ServiceRequest application, Guid actionId, Guid nextStatusId, string Remarks, bool saveAsDraft = false)
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();

            var status =await scopedUow.GetRepository<ServiceStatus>()
                                  .GetAllQueryFiltered()
                                  .FirstOrDefaultAsync(c => c.Id == nextStatusId);

            
            if (status != null )
            {
                var backendName = status.BackendName.ToLower();

                if (backendName.EndsWith("previousstatus"))
                {
                    var lastLog =await scopedUow.GetRepository<ActionTransactionsLog>()
                                           .GetAllQueryFiltered()
                                           .Where(c => c.RefId == application.Id)
                                           .OrderByDescending(c => c.CreateDate)
                                           .FirstOrDefaultAsync();

                    if (lastLog != null)
                        nextStatusId = lastLog.PreviousStatusId;
                }
                else if (backendName.EndsWith("previous2status"))
                {
                    var logs =await scopedUow.GetRepository<ActionTransactionsLog>()
                                        .GetAllQueryFiltered()
                                        .Where(c => c.RefId == application.Id)
                                        .OrderByDescending(c => c.CreateDate)
                                        .Take(2)
                                        .ToListAsync();

                    if (logs.Count == 2)
                        nextStatusId = logs[1].PreviousStatusId; // second last status
                }
            }

            if (saveAsDraft)
                nextStatusId = application.StatusId;

			var actionLogs = new ActionTransactionsLog
			{
				ServiceActionId = actionId,
				NextStatusId = nextStatusId,
				PreviousStatusId = application.StatusId,
				RefId = application.Id,
				IsActive = true,
				Remarks = Remarks,
				RequestType= requestType.ToString(),
			};
			await uow.GetRepository<ActionTransactionsLog>().InsertAsync(actionLogs);



            if (requestType == RequestType.Evaluation)
            {
                var er = await uow.GetRepository<EvaluationRequest>()
                    .GetAllQueryFiltered(x => x.Id == application.Id)
                    .FirstOrDefaultAsync()
                    ?? throw new BusinessException(ExceptionMessage.lblRequestNotValid);

                er.ServiceStatusId = nextStatusId;
				uow.GetRepository<EvaluationRequest>().Update(er);
            }
            else
            {
                application.StatusId = nextStatusId;


                uow.GetRepository<ServiceRequest>().Update(application);
            }

            return actionLogs.Id;
        }

        public async Task<IList<ActionTransactionLogDTO?>?> GetActionLog(Guid id, Guid ServiceId,Guid? DepartmentId,MinistryUser user)
        {
            string lang = requestInfo.Lang;
            var dateTime_Format = await cacheDataProvider.GetSystemSettingValue(ConstantKeys.SystemSettings.DateTimeFormat);
           
            List<ActionTransactionLogDTO> actionTransactionLogs;

			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var query =await scopedUow.GetRepository<ActionTransactionsLog>()
                        .GetAllQueryFiltered()
                        .Include(c => c.CreateBy)
                        .Include(c => c.ServiceAction)
                        .Include(c => c.ServiceAction!.ActionShowLogPartyTypes)
                        .Include(c => c.EvalAttachments)
                        .AsSplitQuery()
                        .Where(c => c.RefId == id).ToListAsync();


            //if (user is StudentUser)
            //{
            //    query = query.Where(c => c.ServiceAction!.ActionShowLogPartyTypes!.Count == 0 ||
            //                             c.ServiceAction.ActionShowLogPartyTypes.Any(t => t.Partytype != null &&  t.Partytype?.IsEmployeePartyType==false )).ToList();
            //}
            //else
            //{
                query = query.Where(c => c.ServiceAction!.ActionShowLogPartyTypes!.Count == 0 ||
                                         c.ServiceAction.ActionShowLogPartyTypes.Any(t => userInfo.PartyTypes.Contains(t.PartytypeId))).ToList();
            //}
            actionTransactionLogs = query.OrderBy(c => c.CreateDate).Select(c => new ActionTransactionLogDTO()
            {
                Action = lang == "ar" ? c.ServiceAction!.NameAr : c.ServiceAction!.NameEn,
                PreviousStatusId = c.PreviousStatusId,
                NextStatusId = c.NextStatusId,
                Actor = lang == "ar" ? c.CreateBy!.NameAr : c.CreateBy!.NameEn,
                CreatedDate = c.CreateDate,
                FormattedCreatedDate = c.CreateDate.ToString(dateTime_Format),
                Remarks = c.Remarks,
                Id = c.Id
            }).ToList();

            foreach (var log in actionTransactionLogs)
            {
                log.PreviousStatus = SrvStatus.GetStatusDisplayName(log.PreviousStatusId, DepartmentId);
                log.NextStatus = SrvStatus.GetStatusDisplayName(log.NextStatusId, DepartmentId);
                log.ActionTransactionAttachments = await GetActionTransactionAttachments(log.Id);
            }
         
            return actionTransactionLogs!;
        }

        public async Task<List<AttachementDTO>?> GetActionTransactionAttachments(Guid id)
        {
			using var scopedUow = serviceScopeFactory.CreateScopedUow();
			var attachments = await scopedUow.GetRepository<EvalAttachment>()
                            .GetAllActiveNonDeleted()
                            .Where(c => c.ActionTransactionsLogId == id)
                            .Select(m => new AttachementDTO() { Id = m.Id, UiFileName = m.UiFileName }).ToListAsync();
            return attachments;
        }
    }
}
