using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scholarship.DAL.Framework;
using Scholarship.DAL.Models.Attachments;
using Scholarship.DAL.Models.Base;
using Scholarship.DAL.Models.Logs;
using Scholarship.DAL.Models.ServiceRequestEntities;
using Scholarship.DAL.Models.StatusEntities;
using Scholarship.Services.Extensions;
using Scholarship.Services.Models.API;
using Scholarship.Services.Special;
using Scholarship.SharedHelper.Enums;
using Scholarship.SharedHelper.Models;
using Scholarship.SharedHelper.Models.Api.AttachmentsDTOs;
using Scholarship.SharedHelper.Models.Api.LogsDTO;
using System;

namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvActionTransactionsLog (SrvStatus SrvStatus,IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
    {

        public async Task<Guid> UpdateStatusAndLogAction(ServiceRequest application, Guid actionId, Guid nextStatusId, string Remarks, bool saveAsDraft = false)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();

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
                                           .Where(c => c.ServiceRequestId == application.Id)
                                           .OrderByDescending(c => c.CreateDate)
                                           .FirstOrDefaultAsync();

                    if (lastLog != null)
                        nextStatusId = lastLog.PreviousStatusId;
                }
                else if (backendName.EndsWith("previous2status"))
                {
                    var logs =await scopedUow.GetRepository<ActionTransactionsLog>()
                                        .GetAllQueryFiltered()
                                        .Where(c => c.ServiceRequestId == application.Id)
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
                ServiceRequestId = application.Id,
                IsActive = true,
                Remarks = Remarks
            };

            application.StatusId = nextStatusId;

           await uow.GetRepository<ActionTransactionsLog>().InsertAsync(actionLogs);
            uow.GetRepository<ServiceRequest>().Update(application);

            return actionLogs.Id;
        }

        public async Task<IList<ActionTransactionLogDTO?>?> GetActionLog(Guid id, Guid ServiceId,Guid? DepartmentId,UserProfile user)
        {
            string lang = requestInfo.Lang;
            var dateTime_Format = await cacheDataProvider.GetSystemSettingValue(ConstantKeys.SystemSettings.DateTimeFormat);
           
            List<ActionTransactionLogDTO> actionTransactionLogs;

           
            var query =await serviceScopeFactory.CreateScopedUow()
                        .GetRepository<ActionTransactionsLog>()
                        .GetAllQueryFiltered()
                        .Include(c => c.CreateBy)
                        .Include(c => c.ServiceAction)
                        .Include(c => c.ServiceAction!.ActionShowLogPartyTypes)
                        .Include(c => c.ActionTransactionAttachments)
                        .AsSplitQuery()
                        .Where(c => c.ServiceRequestId == id).ToListAsync();


            if (user is StudentUser)
            {
                query = query.Where(c => c.ServiceAction!.ActionShowLogPartyTypes!.Count == 0 ||
                                         c.ServiceAction.ActionShowLogPartyTypes.Any(t => t.Partytype != null &&  t.Partytype?.IsEmployeePartyType==false )).ToList();
            }
            else
            {
                query = query.Where(c => c.ServiceAction!.ActionShowLogPartyTypes!.Count == 0 ||
                                         c.ServiceAction.ActionShowLogPartyTypes.Any(t => userInfo.PartyTypes.Contains(t.PartytypeId))).ToList();
            }
            actionTransactionLogs = query.OrderBy(c => c.CreateDate).Select(c => new ActionTransactionLogDTO()
            {
                Action = lang == "ar" ? c.ServiceAction!.NameAr : c.ServiceAction!.NameEn,
                PreviousStatusId = c.PreviousStatusId,
                NextStatusId = c.NextStatusId,
                Actor = lang == "ar" ? c.CreateBy!.FullNameAr : c.CreateBy!.FullNameEn,
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
            //await actionTransactionLogs.ParallelForEachAsync(async (item) =>
            //{
            //    //item.PreviousStatus = SrvStatus.GetStatusDisplayName(item.PreviousStatusId, DepartmentId);
            //    //item.NextStatus = SrvStatus.GetStatusDisplayName(item.NextStatusId, DepartmentId);
            //    item.ActionTransactionAttachments = await GetActionTransactionAttachments(item.Id);
            //});
            return actionTransactionLogs!;
        }

        public async Task<List<AttachementDTO>?> GetActionTransactionAttachments(Guid id)
        {
            var attachments = await serviceScopeFactory.CreateScopedUow()
                            .GetRepository<Attachment>()
                            .GetAllActiveNonDeleted()
                            .Where(c => c.ActionTransactionsLogId == id)
                            .Select(m => new AttachementDTO() { Id = m.Id, UiFileName = m.UiFileName }).ToListAsync();
            return attachments;
        }
    }
}
