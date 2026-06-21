using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.SystemLog;
using Evaluation.DAL.Models.Template;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.BusinessLayer.API.FormLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Dtos.Form;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;


namespace Evaluation.Services.BusinessLayer.API.Template;


	public class PlaceholderService(
        IServiceProvider serviceProvider,
        SrvDropdown dropdownService,
        CacheDataProvider cacheDataProvider, 
        RequestInfo requestInfo, UserInfo userInfo)
        : ApiServiceBase
    {
        public async Task<List<PlaceholderDto>> GetRequestPlaceHolders(Guid requestId, string lang, 
            string? partyTypeId = null, string? remarks = null)
        {
            var scopedUow = serviceProvider.CreateScopedUow();
            var request = await scopedUow.GetRepository<ServiceRequest>().GetAllQueryFiltered(x=>x.Id== requestId).Include(x=>x.Service).FirstOrDefaultAsync();
            if (request == null)
                throw new BusinessException(ConstantKeys.ExceptionMessage.InActiveData);

            var placeholders = scopedUow.GetRepository<PlaceHolder>()
                .GetAllQueryFiltered().Where(ph => ph.ServiceId == request.ServiceId).ToList();

            var result = new List<PlaceholderDto>();
            result.AddRange(await GetRequestFieldPlaceHolders(scopedUow, 
                placeholders.Where(p => p.TypeDisplay == ConstantKeys.PlaceHolderTypes.RequestField).ToList(), 
                request, lang));

       
        result.AddRange(await GetEvaluationRequestFieldPlaceHolders(scopedUow, 
                placeholders.Where(p => p.TypeDisplay == ConstantKeys.PlaceHolderTypes.EvaluationField).ToList(), 
                request.EvaluationRequest, lang));

            return result;
        }

        public async Task<List<PlaceholderDto>> GetSystemModulePlaceHoldersByTemplateId(Guid templateId, string lang)
        {
            var result = new List<PlaceholderDto>();
            var scopedUow = serviceProvider.CreateScopedUow();

            var template = await scopedUow.GetRepository<TemplateDocument>()
                .GetAllQueryFiltered(x => x.Id == templateId)
                .FirstOrDefaultAsync();

            if (template == null) return result;

            var userPartyTypesIdsList = await scopedUow.GetRepository<UserPartyType>()
                .GetAllQueryFiltered()
                .Include(x => x.PartyType)
                .Where(x => x.SignaturePlaceHolder != null && x.PartyType!.DepartmentId == template.DepartmentId)
                .Select(x => x.Id)
                .Distinct()
                .ToListAsync();

            var query = scopedUow.GetRepository<UserPartyTypeSignature>()
                .GetAllQueryFiltered(x => userPartyTypesIdsList.Contains(x.UserPartyTypeId))
                .Include(x => x.UserPartyType)
                .Where(x => x.UserPartyType!.SignaturePlaceHolder != null)
                .Select(x => new PlaceholderDto
                {
                    Key = x.UserPartyType!.SignaturePlaceHolder!,
                    Value = Convert.ToBase64String(x.Signature),
                    PlaceholderType = PlaceholderType.Image,
                });

            result = await query.ToListAsync();
            return result;
        }

        public async Task<List<PlaceholderDto>> GetRequestFieldPlaceHolders(UnitOfWork scopedUow, List<PlaceHolder> placeHolders, 
            ServiceRequest request, string lang, string wordFilePath = "")
        {
            var requestFields = scopedUow.GetRepository<ServiceRequestFieldsValue>().GetAllQueryFiltered()
                .Include(x => x.Field)
                .ThenInclude(f => f!.FieldType)
                .Include(x => x.Field)
                .ThenInclude(f => f!.FormGroupList)
                .ThenInclude(f => f!.Fields)
                .ThenInclude(f => f.FieldType)
                .Where(c => c.RefId == request.Id && placeHolders.Select(p => p.FieldId).Contains(c.FieldId))
                .ToList();

            var result = new List<PlaceholderDto>();
            result.AddRange(await GetStaticPlaceholders(request, string.Empty));

            foreach (var holder in placeHolders)
            {
                var fieldValue = requestFields.FirstOrDefault(f => f.FieldId == holder.FieldId);
                if (fieldValue == null) continue;

                var langToUse = GetLangFromPlaceHolderName(holder.PlaceHolderName!);
                if (string.IsNullOrEmpty(langToUse)) langToUse = lang;

                var placeholderType = PlaceholderType.Text;

                if (fieldValue.Field!.FieldType!.BackendName == ConstantKeys.FieldTypeConstant.list)
                {
                    var keyInsideBraces = holder.PlaceHolderName!.Trim('{', '}');
                    if (keyInsideBraces.Contains('.'))
                    {
                        var parts = keyInsideBraces.Split('.');
                        if (parts.Length >= 2 && parts[1].Equals("L", StringComparison.OrdinalIgnoreCase))
                        {
                            placeholderType = PlaceholderType.Table;
                        }
                    }
                    else
                    {
                        placeholderType = PlaceholderType.HTMLTable;
                    }
                }

                var firstChildFieldId= Guid.Parse(holder.ChildFieldIds?.Split(',').FirstOrDefault() ?? Guid.Empty.ToString());
                result.Add(new PlaceholderDto
                {
                    PlaceholderType = placeholderType,
                    Key = holder.PlaceHolderName!,
                    FieldListId = holder.FieldId,
                    ChildFieldId = holder.ChildFieldIds,
                    ChildField = fieldValue.Field!.FieldType!.BackendName == ConstantKeys.FieldTypeConstant.list
                                  ? fieldValue.Field!.FormGroupList?.Fields.FirstOrDefault(x => x.Id == firstChildFieldId) : null,
                    Value = placeholderType == PlaceholderType.HTMLTable
                            ? fieldValue.Value
                            : await RetrieveValueAsync(fieldValue.Field!.FieldType!.BackendName,
                                fieldValue.Field.DropDownTypeId, fieldValue.Value!, langToUse,
                                holder.PlaceHolderName,
                                holder.ChildFieldIds, fieldValue.Field!.FormGroupList?.Fields.ToList())
                });
            }
            return result;
        }
        public async Task<List<PlaceholderDto>> GetEvaluationRequestFieldPlaceHolders(UnitOfWork scopedUow, List<PlaceHolder> placeHolders, 
            EvaluationRequest request, string lang, string wordFilePath = "")
        {
            var requestFields = scopedUow.GetRepository<ServiceRequestFieldsValue>().GetAllQueryFiltered()
                .Include(x => x.Field)
                .ThenInclude(f => f!.FieldType)
                .Include(x => x.Field)
                .ThenInclude(f => f!.FormGroupList)
                .ThenInclude(f => f!.Fields)
                .ThenInclude(f => f.FieldType)
                .Where(c => c.RefId == request.Id && placeHolders.Select(p => p.FieldId).Contains(c.FieldId))
                .ToList();

            var result = new List<PlaceholderDto>();
            result.AddRange(await GetStaticPlaceholders(request, string.Empty));

            foreach (var holder in placeHolders)
            {
                var fieldValue = requestFields.FirstOrDefault(f => f.FieldId == holder.FieldId);
                if (fieldValue == null) continue;

                var langToUse = GetLangFromPlaceHolderName(holder.PlaceHolderName!);
                if (string.IsNullOrEmpty(langToUse)) langToUse = lang;

                var placeholderType = PlaceholderType.Text;

                if (fieldValue.Field!.FieldType!.BackendName == ConstantKeys.FieldTypeConstant.list)
                {
                    var keyInsideBraces = holder.PlaceHolderName!.Trim('{', '}');
                    if (keyInsideBraces.Contains('.'))
                    {
                        var parts = keyInsideBraces.Split('.');
                        if (parts.Length >= 2 && parts[1].Equals("L", StringComparison.OrdinalIgnoreCase))
                        {
                            placeholderType = PlaceholderType.Table;
                        }
                    }
                    else
                    {
                        placeholderType = PlaceholderType.HTMLTable;
                    }
                }

                var firstChildFieldId= Guid.Parse(holder.ChildFieldIds?.Split(',').FirstOrDefault() ?? Guid.Empty.ToString());
                result.Add(new PlaceholderDto
                {
                    PlaceholderType = placeholderType,
                    Key = holder.PlaceHolderName!,
                    FieldListId = holder.FieldId,
                    ChildFieldId = holder.ChildFieldIds,
                    ChildField = fieldValue.Field!.FieldType!.BackendName == ConstantKeys.FieldTypeConstant.list
                                  ? fieldValue.Field!.FormGroupList?.Fields.FirstOrDefault(x => x.Id == firstChildFieldId) : null,
                    Value = placeholderType == PlaceholderType.HTMLTable
                            ? fieldValue.Value
                            : await RetrieveValueAsync(fieldValue.Field!.FieldType!.BackendName,
                                fieldValue.Field.DropDownTypeId, fieldValue.Value!, langToUse,
                                holder.PlaceHolderName,
                                holder.ChildFieldIds, fieldValue.Field!.FormGroupList?.Fields.ToList())
                });
            }
            return result;
        }

        public async Task<string> RetrieveValueAsync(string fieldType, Guid? dropDownTypeId, string? value, string lang, 
            string? placeholder, string? subFieldId = null, List<Field>? fieldList = null)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            switch (fieldType)
            {
                case ConstantKeys.FieldTypeConstant.dropdown or ConstantKeys.FieldTypeConstant.select2 when Guid.TryParse(value, out var parsedGuid):
                    return await RetrieveDropdownValue(parsedGuid, dropDownTypeId, lang);
                case ConstantKeys.FieldTypeConstant.dropdown or ConstantKeys.FieldTypeConstant.select2:
                    return placeholder ?? string.Empty;
                case ConstantKeys.FieldTypeConstant.date:
                    return value.Replace("T00:00:00", "");
                case ConstantKeys.FieldTypeConstant.list:
                    try
                    {
                        var dataList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(value);
                        if (dataList == null || dataList.Count == 0)
                            return string.Empty;

                        if (!string.IsNullOrEmpty(placeholder) && placeholder.EndsWith("}}"))
                        {
                            var keyInsideBraces = placeholder.Trim('{', '}');

                            var parts = keyInsideBraces.Split('.');

                            var mode = "F"; // default mode
                            if (parts.Length >= 2)
                                mode = parts[1].ToUpperInvariant();

                            var subFieldIdList = subFieldId?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(id => id.Trim())
                                .ToList() ?? [];
                            if (!subFieldIdList.Any())
                                return string.Empty;
                            var propertyId = Guid.Parse(subFieldIdList[0]);

                            switch (mode)
                            {
                                case "L":
                                {
                                    var items = dataList.Where(item => item.ContainsKey(propertyId.ToString()));
                                    foreach (var item in items)
                                    {
                                        var rawValue = item[propertyId.ToString()].ToString() ?? string.Empty;
                                        var fieldDef = fieldList?.FirstOrDefault(f => f.Id == propertyId);

                                        if (fieldDef == null) continue;
                                        var data = await RetrieveValueAsync(fieldDef.FieldType!.BackendName, fieldDef.DropDownTypeId, 
                                            rawValue, lang, placeholder, null, fieldList);

                                        item[propertyId.ToString()] = data;
                                    }
                                    return JsonConvert.SerializeObject(dataList);
                                }
                                case "F":
                                {
                                    var firstItem = dataList.FirstOrDefault();
                                    if (firstItem != null && firstItem.TryGetValue(propertyId.ToString(), out var firstValue))
                                    {
                                        var rawValue = firstValue.ToString() ?? string.Empty;

                                        // Lookup field definition using propertyName
                                        var fieldDef = fieldList?.FirstOrDefault(f => f.Id == propertyId);

                                        if (fieldDef != null)
                                        {
                                            return await RetrieveValueAsync(fieldDef.FieldType!.BackendName,
                                                fieldDef.DropDownTypeId, rawValue, lang, placeholder, null, fieldList);
                                        }
                                        return rawValue;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        return "Invalid List Format";
                    }

                    break;
            }

            return value;
        }

        private async Task<string> RetrieveDropdownValue(Guid? value, Guid? dropDownTypeId, string lang)
        {
            if (value == Guid.Empty)
                return string.Empty;

            var dropdownValue = await dropdownService.GetDropDownValue(lang, value, dropDownTypeId, Guid.Empty,null);
            if (dropdownValue == null)
                return string.Empty;

            return lang == "ar" ? dropdownValue.TitleAr : dropdownValue.TitleEn;
        }

    private async Task<List<PlaceholderDto>> GetStaticPlaceholders(ServiceRequest? dbreq, string remarks, Guid? partyTypeId = null)
    {
        var today = DateTime.Now;
        var result = new List<PlaceholderDto>
            {
                new() { Key = "{{Date}}", Value = today.ToString("dd-MM-yyyy") },
                new() { Key = "{{ActionNotes}}", Value = remarks },
                new() { Key = "{{todayDate}}", Value = today.ToString("dd-MM-yyyy") },
                new() { Key = "{{todayDateDay}}", Value = today.Day.ToString() },
                new() { Key = "{{todayDateMonth}}", Value = today.ToString("MMMM") },
                new() { Key = "{{todayDateYear}}", Value = today.Year.ToString() }
            };

        if (dbreq == null) return result;

        result.Add(new PlaceholderDto() { Key = "{{RequestDate}}", Value = dbreq.CreateDate.ToString("dd-MM-yyyy") });
        result.Add(new PlaceholderDto() { Key = "{{RequestNumber}}", Value = dbreq.RequestNumber });
        result.Add(new PlaceholderDto() { Key = "{{requestNumber}}", Value = dbreq.RequestNumber });
        result.Add(new PlaceholderDto() { Key = "{{ServiceNameAr}}", Value = dbreq.Service!.NameAr });
        result.Add(new PlaceholderDto() { Key = "{{ServiceNameEn}}", Value = dbreq.Service!.NameEn });

		//// ======================= OrgTree  Details =======================

		if (dbreq.OrgTree != null)
		{
			var orgBl = serviceProvider.GetRequiredService<OrgBL>();

			var orgDetails = await orgBl.GetOrgDetails(dbreq.OrgTree.Id, dbreq.Id);

			result.Add(new PlaceholderDto { Key = "{{SchoolNameAr}}", Value = orgDetails.NameAr ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{SchoolNameEn}}", Value = orgDetails.NameEn ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{SchoolName}}", Value = requestInfo.Lang == "ar" ? orgDetails.NameAr ?? "-" : orgDetails.NameEn ?? "-" });

			result.Add(new PlaceholderDto { Key = "{{SchoolManagerName}}", Value = orgDetails.ManagerName ?? "-" });


			result.Add(new PlaceholderDto { Key = "{{SchoolPhone}}", Value = orgDetails.Phone ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{SchoolAddress}}", Value = orgDetails.Address ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{SchoolEmail}}", Value = orgDetails.Email ?? "-" });

			result.Add(new PlaceholderDto { Key = "{{CurrentEvaluationResult}}", Value = orgDetails.CurrentEvaluationResult ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{CurrentEvaluationDate}}", Value = orgDetails.CurrentEvaluationDate ?? "-" });

			result.Add(new PlaceholderDto { Key = "{{LastEvaluationResult}}", Value = orgDetails.LastEvaluationResult ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{LastEvaluationDate}}", Value = orgDetails.LastEvaluationDate ?? "-" });

			result.Add(new PlaceholderDto { Key = "{{LastEvaluationResult2}}", Value = orgDetails.LastEvaluationResult2 ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{LastEvaluationDate2}}", Value = orgDetails.LastEvaluationDate2 ?? "-" });
		}



		// ======================= Request Status =======================
		var actionStatus = serviceProvider.CreateScopedUow()
            .GetRepository<ActionTransactionsLog>().GetAllQueryFiltered()
            .Where(c => c.RefId == dbreq.Id && c.NextStatusId == dbreq.StatusId)
            .OrderByDescending(c => c.CreateDate)
            .FirstOrDefault();

        if (actionStatus != null && partyTypeId != null)
        {
            result.Add(new PlaceholderDto() { Key = "{{CurrenStatusEn}}", Value = GetStatusDisplayName(actionStatus.NextStatusId, partyTypeId, "en") });
            result.Add(new PlaceholderDto() { Key = "{{CurrenStatusAr}}", Value = GetStatusDisplayName(actionStatus.NextStatusId, partyTypeId) });
            result.Add(new PlaceholderDto() { Key = "{{PreviousStatusEn}}", Value = GetStatusDisplayName(actionStatus.PreviousStatusId, partyTypeId, "en") });
            result.Add(new PlaceholderDto() { Key = "{{PreviousStatusAr}}", Value = GetStatusDisplayName(actionStatus.PreviousStatusId, partyTypeId) });
        }

        // ======================= Service Info =======================
        if (dbreq.Service != null)
        {
            result.Add(new PlaceholderDto() { Key = "{{ServiceNameAr}}", Value = dbreq.Service.NameAr });
            result.Add(new PlaceholderDto() { Key = "{{ServiceNameEn}}", Value = dbreq.Service.NameEn });
        }

        return result;
    }
    private async Task<List<PlaceholderDto>> GetStaticPlaceholders(EvaluationRequest? dbreq, string remarks, Guid? partyTypeId = null)
    {
        var today = DateTime.Now;
        var result = new List<PlaceholderDto>
            {
                new() { Key = "{{Date}}", Value = today.ToString("dd-MM-yyyy") },
                new() { Key = "{{ActionNotes}}", Value = remarks },
                new() { Key = "{{todayDate}}", Value = today.ToString("dd-MM-yyyy") },
                new() { Key = "{{todayDateDay}}", Value = today.Day.ToString() },
                new() { Key = "{{todayDateMonth}}", Value = today.ToString("MMMM") },
                new() { Key = "{{todayDateYear}}", Value = today.Year.ToString() }
            };

        if (dbreq == null) return result;

        result.Add(new PlaceholderDto() { Key = "{{RequestDate}}", Value = dbreq.CreateDate.ToString("dd-MM-yyyy") });
        result.Add(new PlaceholderDto() { Key = "{{RequestNumber}}", Value = dbreq.RequestNumber });
        result.Add(new PlaceholderDto() { Key = "{{requestNumber}}", Value = dbreq.RequestNumber });
        result.Add(new PlaceholderDto() { Key = "{{ServiceNameAr}}", Value = dbreq.Service!.NameAr });
        result.Add(new PlaceholderDto() { Key = "{{ServiceNameEn}}", Value = dbreq.Service!.NameEn });

		//// ======================= OrgTree Details =======================
		if (dbreq.OrgTree != null)
		{
			var orgBl = serviceProvider.GetRequiredService<OrgBL>();

			var orgDetails = await orgBl.GetOrgDetails(dbreq.OrgTree.Id, dbreq.Id);

			result.Add(new PlaceholderDto { Key = "{{SchoolNameAr}}", Value = orgDetails.NameAr ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{SchoolNameEn}}", Value = orgDetails.NameEn ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{SchoolName}}", Value = requestInfo.Lang == "ar" ? orgDetails.NameAr ?? "-" : orgDetails.NameEn ?? "-" });

			result.Add(new PlaceholderDto { Key = "{{SchoolPhone}}", Value = orgDetails.Phone ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{SchoolAddress}}", Value = orgDetails.Address ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{SchoolEmail}}", Value = orgDetails.Email ?? "-" });

			result.Add(new PlaceholderDto { Key = "{{SchoolManagerName}}", Value = orgDetails.ManagerName ?? "-" });

			result.Add(new PlaceholderDto { Key = "{{CurrentEvaluationResult}}", Value = orgDetails.CurrentEvaluationResult ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{CurrentEvaluationDate}}", Value = orgDetails.CurrentEvaluationDate ?? "-" });

			result.Add(new PlaceholderDto { Key = "{{LastEvaluationResult}}", Value = orgDetails.LastEvaluationResult ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{LastEvaluationDate}}", Value = orgDetails.LastEvaluationDate ?? "-" });

			result.Add(new PlaceholderDto { Key = "{{LastEvaluationResult2}}", Value = orgDetails.LastEvaluationResult2 ?? "-" });
			result.Add(new PlaceholderDto { Key = "{{LastEvaluationDate2}}", Value = orgDetails.LastEvaluationDate2 ?? "-" });
		}

		//// ======================= Final Form Items With Values =======================
		var formBL = serviceProvider.GetRequiredService<FormBL>();

		var formResult = await formBL.GetFinalFormItemsWithValues(dbreq.Id);

		if (formResult.IsSuccess && formResult.Value?.Tree != null)
		{
			using var scopeNoteUow = serviceProvider.CreateScopedUow();

			var scopeNotes = await scopeNoteUow
				.GetRepository<ScopeReportNote>()
				.GetAllActiveNonDeleted(x => x.EvaluationRequestId == dbreq.Id)
				.ToListAsync();

			var parentScopes = formResult.Value.Tree;

			var summary = string.Join(Environment.NewLine,
				parentScopes.Select(scope =>
				{
					var avg = GetScopeAverage(scope);
					return $"{scope.Name} | {GetJudgement(avg)} | {(avg * 20):0.##}%";
				}));

			var details = string.Join(Environment.NewLine + Environment.NewLine,
				parentScopes.Select(scope =>
				{
					var avg = GetScopeAverage(scope);
					var note = scopeNotes.FirstOrDefault(x => x.ScopeId == scope.Id);

					return
						$"{scope.Name} - {GetJudgement(avg)} ({(avg * 20):0.##}%)" + Environment.NewLine +
						$"أهم جوانب القوة: {note?.PositivePoint ?? "-"}" + Environment.NewLine +
						$"أهم الجوانب التي تحتاج إلى تحسين وتطوير: {note?.NegativePoint ?? "-"}";
				}));

			result.Add(new PlaceholderDto
			{
				Key = "{{ParentScopesSummary}}",
				Value = summary
			});

			result.Add(new PlaceholderDto
			{
				Key = "{{ParentScopesDetails}}",
				Value = details
			});
		}
		// ======================= Request Status =======================
		var actionStatus = serviceProvider.CreateScopedUow()
            .GetRepository<ActionTransactionsLog>().GetAllQueryFiltered()
            .Where(c => c.RefId == dbreq.Id && c.NextStatusId == dbreq.ServiceStatusId)
            .OrderByDescending(c => c.CreateDate)
            .FirstOrDefault();

        if (actionStatus != null && partyTypeId != null)
        {
            result.Add(new PlaceholderDto() { Key = "{{CurrenStatusEn}}", Value = GetStatusDisplayName(actionStatus.NextStatusId, partyTypeId, "en") });
            result.Add(new PlaceholderDto() { Key = "{{CurrenStatusAr}}", Value = GetStatusDisplayName(actionStatus.NextStatusId, partyTypeId) });
            result.Add(new PlaceholderDto() { Key = "{{PreviousStatusEn}}", Value = GetStatusDisplayName(actionStatus.PreviousStatusId, partyTypeId, "en") });
            result.Add(new PlaceholderDto() { Key = "{{PreviousStatusAr}}", Value = GetStatusDisplayName(actionStatus.PreviousStatusId, partyTypeId) });
        }

        // ======================= Service Info =======================
        if (dbreq.Service != null)
        {
            result.Add(new PlaceholderDto() { Key = "{{ServiceNameAr}}", Value = dbreq.Service.NameAr });
            result.Add(new PlaceholderDto() { Key = "{{ServiceNameEn}}", Value = dbreq.Service.NameEn });
        }
		// ======================= Visit Information =======================

		result.Add(new PlaceholderDto
		{
			Key = "{{VisitDate}}",
			Value = dbreq.EvaluationDate?.ToString("dd-MM-yyyy") ?? "-"
		});

		using var visitUow = serviceProvider.CreateScopedUow();

		var classroomObservationsCount = await visitUow
			.GetRepository<ServiceRequest>()
			.GetAllActiveNonDeleted(x => x.EvaluationRequestId == dbreq.Id)
			.Include(x => x.Service)
				.ThenInclude(x => x.EvaluationParty)
				.ThenInclude(x => x.EvalPartyCategory)
			.Include(x => x.Service)
				.ThenInclude(x => x.ServiceType)
			.CountAsync(x =>
				x.Service!.ServiceType!.BackendName == "ClassroomObservation" &&
				x.Service!.EvaluationParty!.EvalPartyCategory!.BackendName == "ClassroomObservation");

		result.Add(new PlaceholderDto
		{
			Key = "{{ClassroomObservationsCount}}",
			Value = classroomObservationsCount.ToString()
		});

		result.Add(new PlaceholderDto
		{
			Key = "{{InterviewsCount}}",
			Value = "-"
		});

		var assignments = await visitUow
			.GetRepository<EvaluationRequestAssignment>()
			.GetAllActiveNonDeleted(x => x.EvaluationRequestId == dbreq.Id)
			.Include(x => x.MinistryUser)
			.ToListAsync();

		var teamLeader = assignments.FirstOrDefault(x => x.IsLeader);

		result.Add(new PlaceholderDto
		{
			Key = "{{TeamLeader}}",
			Value = teamLeader?.MinistryUser == null
				? "-"
				: requestInfo.Lang == "ar"
					? teamLeader.MinistryUser.NameAr ?? "-"
					: teamLeader.MinistryUser.NameEn ?? "-"
		});

		var teamMembers = assignments
			.Where(x => !x.IsLeader && x.MinistryUser != null)
			.Select(x => requestInfo.Lang == "ar"
				? x.MinistryUser!.NameAr
				: x.MinistryUser!.NameEn)
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.ToList();

		result.Add(new PlaceholderDto
		{
			Key = "{{TeamMembers}}",
			Value = teamMembers.Any()
				? string.Join("، ", teamMembers)
				: "-"
		});

		return result;
	}
	
	private decimal GetScopeAverage(ScopeTreeDto scope)
	{
		var items = GetAllItems(scope);

		return items
			.Where(x => x.Value.HasValue)
			.Select(x => x.Value!.Value)
			.DefaultIfEmpty(0)
			.Average();
	}

	private List<FormItemDto> GetAllItems(ScopeTreeDto scope)
	{
		var result = new List<FormItemDto>();

		if (scope.Items != null)
			result.AddRange(scope.Items);

		if (scope.Children != null)
		{
			foreach (var child in scope.Children)
				result.AddRange(GetAllItems(child));
		}

		return result;
	}

	private string GetJudgement(decimal value)
	{
		return value switch
		{
			< 3m => "ضعيف",
			< 3.75m => "مقبول",
			< 4.25m => "جيد",
			< 4.75m => "جيد جداً",
			_ => "ممتاز"
		};
	}
	public async Task<List<PlaceholderDto>> GetDepartmentPlaceHoldersByTemplateId(Guid templateId, string lang)
    {

        var result = new List<PlaceholderDto>();
        var template = await serviceProvider.CreateScopedUow().GetRepository<TemplateDocument>().GetAllQueryFiltered(x => x.Id == templateId).FirstOrDefaultAsync();
        if (template != null)
        {
            var userPartyTypesIdsList = await serviceProvider.CreateScopedUow().GetRepository<UserPartyType>()
                .GetAllQueryFiltered()
                .Include(x => x.PartyType)
                .Where(x => x.SignaturePlaceHolder != null && x.PartyType.DepartmentId == template.DepartmentId)
                .Select(x => x.Id)
                .Distinct()
                .ToListAsync();

            var query = serviceProvider.CreateScopedUow().GetRepository<UserPartyTypeSignature>()
                .GetAllQueryFiltered(x => userPartyTypesIdsList.Contains(x.UserPartyTypeId))
                .Include(x => x.UserPartyType)
                .Where(x => x.UserPartyType.SignaturePlaceHolder != null)
                 .Select(x => new PlaceholderDto
                 {
                     Key = x.UserPartyType.SignaturePlaceHolder,
                     Value = Convert.ToBase64String(x.Signature),
                     PlaceholderType = PlaceholderType.Image,
                 });



            result = await query.ToListAsync();

        }
        return result;
    }
    private string GetStatusDisplayName(Guid statusId, Guid? partyTypeId = null, string lang = "ar")
        {
            partyTypeId ??= userInfo.PartyTypes.FirstOrDefault();
            var status = cacheDataProvider.GetStatusAllWithDeleted().Result.FirstOrDefault(s => s.Id == statusId);
            if (status == null) return string.Empty;

            var displayName = lang == "ar" ? status.NameAr : status.NameEn;
            var customDisplay = status.StatusPartyTypeDisplayNames
                .FirstOrDefault(d => d.PartyTypeId == partyTypeId && d.IsActive == true && !d.IsDeleted == true);

            if (customDisplay != null && !string.IsNullOrWhiteSpace(customDisplay.TitleAr))
            {
                displayName = lang == "ar" ? customDisplay.TitleAr : customDisplay.TitleEn;
            }

            return displayName;
        }

        private string GetLangFromPlaceHolderName(string? name) => name?.ToLower() switch
        {
            var x when x?.EndsWith("-ar}}") == true => "ar",
            var x when x?.EndsWith("-en}}") == true => "en",
            _ => string.Empty
        };
    }
