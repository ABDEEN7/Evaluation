using AutoMapper;
using Evaluation.DAL.Entities.Attachments;
using Evaluation.DAL.Entities.ServiceRequestEntities;
using Evaluation.DAL.Entities.Template;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Attachment = System.Net.Mail.Attachment;

namespace Evaluation.Services.BusinessLayer.API.Template;

public class TemplateBl(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
    UnitOfWork uow, LoggingServices loggingServices, UserInfo userInfo, RequestInfo requestInfo,
    IServiceProvider serviceProvider,
    TemplateService templateService,
    PlaceholderService placeholderService,
    DocumentConversionService documentConversionService,
    EmailTemplateService emailTemplateService
) : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices,
    userInfo, serviceProvider, requestInfo)
{
	public async Task<Attachment> HandleAttachment(Guid attachmentId)
		=> await templateService.HandleAttachment(attachmentId);
	public async Task<string> GetRequestEmailTemplate(ServiceRequest request, string template, Guid? partyTypeId = null,
		string lang = "ar", string? remarks = null)
		=> await emailTemplateService.GetRequestEmailTemplate(request, template, partyTypeId, lang, remarks);

	public async Task<List<byte[]>> GetRequestEmailAttachment(ServiceRequest requestData, Guid emailTemplateId,
		string lang)
		=> await emailTemplateService.GetRequestEmailAttachment(requestData, emailTemplateId, lang);

	public async Task<byte[]?> GetLetterTemplate(Guid requestId, Guid templateId, string lang)
	{
		var result = new List<PlaceholderDto>();
		using var scopedUow = serviceScopeFactory.CreateScopedUow();

		var request = await scopedUow.GetRepository<ServiceRequest>().GetAllQueryFiltered(x => x.Id == requestId)
			.Include(x => x.Service).FirstOrDefaultAsync();
		if (request == null)
			throw new BusinessException(ConstantKeys.ExceptionMessage.InActiveData);

		var placeholders = scopedUow.GetRepository<PlaceHolder>().GetAllQueryFiltered()
			.Where(c => c.ServiceId == request.ServiceId).ToList();

		result.AddRange(await placeholderService.GetRequestFieldPlaceHolders(scopedUow,
			placeholders.Where(p => p.TypeDisplay == ConstantKeys.PlaceHolderTypes.RequestField).ToList(), request,
			lang));

		result.AddRange(await placeholderService.GetDepartmentPlaceHoldersByTemplateId(templateId, lang));

		result.AddRange(await placeholderService.GetScholarshipFieldPlaceHolders(scopedUow,
			placeholders.Where(p => p.TypeDisplay == ConstantKeys.PlaceHolderTypes.EvaluationField).ToList(), request,
			lang));

		var useAsposeKeyValue = await cacheDataProvider.GetSystemSettingValue(ConstantKeys.SystemSettings.useAsposeLib);
		bool useAspose = !string.IsNullOrWhiteSpace(useAsposeKeyValue) && bool.TryParse(useAsposeKeyValue, out var parsedValue) ? parsedValue : false;

		if (!useAspose)
		{
			return await GetDocumentFromHtmlSpire(templateId, result, lang, request.Service!.SystemModuleId);
		}
		else
		{
			return await GetDocumentFromHtmlAspose(templateId, result, lang, request.Service!.SystemModuleId);
		}
	}

	private async Task<byte[]?> GetDocumentFromHtmlSpire(Guid templateId, List<PlaceholderDto> placeholders, string lang,
		Guid systemModuleId)
	{

		var template = (await serviceScopeFactory.CreateScopedUow().GetRepository<TemplateDocument>()
			.GetByIDActiveNonDeleted(templateId));
		if (template == null)
		{
			throw new BusinessException(ConstantKeys.ExceptionMessage.InActiveData);
		}

		if (template.IsAttachment != true)
			return await documentConversionService.HandleNonAttachmentSpire(placeholders, template, lang);
		var result =
			await templateService.HandleAttachment(placeholders, template.AttachmentId!.Value, systemModuleId);
		return documentConversionService.ConvertDocxToPdfSpire(result);
	}

	private async Task<byte[]?> GetDocumentFromHtmlAspose(Guid templateId, List<PlaceholderDto> placeholders, string lang,
		Guid systemModuleId)
	{

		var template = (await serviceScopeFactory.CreateScopedUow().GetRepository<TemplateDocument>()
			.GetByIDActiveNonDeleted(templateId));
		if (template == null)
		{
			throw new BusinessException(ConstantKeys.ExceptionMessage.InActiveData);
		}

		if (template.IsAttachment != true)
			return await documentConversionService.HandleNonAttachmentAspose(placeholders, template, lang);
		var result =
			await templateService.HandleAttachment(placeholders, template.AttachmentId!.Value, systemModuleId);
		return documentConversionService.ConvertDocxToPdfAspose(result);
	}
}