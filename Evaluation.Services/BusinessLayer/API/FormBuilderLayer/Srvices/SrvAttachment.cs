using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Scholarship.DAL.Framework;
using Scholarship.DAL.Models.Attachments;
using Scholarship.Services.Models.API;
using Scholarship.Services.Special;
using Scholarship.SharedHelper.Models;

namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;

public class SrvAttachment(
    IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork uow,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo)
    : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider,
        requestInfo)
{
    public async Task<Attachment> CreateAttachmentAsync(
        string fileName,
        string filePath,
        string fileUrl,
        long fileSize,
        Guid? serviceRequestId = null,
        Guid? actionTransitionLogId = null,
        Guid? fieldId = null,
        Guid? childFieldId = null,
        bool commitChange = true)
    {
        var attachment = new Attachment
        {
            FileName = fileName,
            UiFileName = filePath,
            FileExtension = Path.GetExtension(fileName),
            FileSize = fileSize,
            ServiceRequestId = serviceRequestId,
            ActionTransactionsLogId = actionTransitionLogId,
            FieldId = fieldId,
            ChildFieldId = childFieldId,
            Index = null,
            IsOthers = false,
        };

        await uow.GetRepository<Attachment>().InsertAsync(attachment);
        if (commitChange)
            await uow.CommitAsync();

        return attachment;
    }
}