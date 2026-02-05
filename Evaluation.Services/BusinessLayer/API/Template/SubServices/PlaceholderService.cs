using System.Diagnostics;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.Template;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


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

       
        result.AddRange(await GetScholarshipFieldPlaceHolders(scopedUow, 
                placeholders.Where(p => p.TypeDisplay == ConstantKeys.PlaceHolderTypes.EvaluationField).ToList(), 
                request, lang));

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

        public async Task<List<PlaceholderDto>> GetScholarshipFieldPlaceHolders(UnitOfWork scopedUow, List<PlaceHolder>? placeHolders, 
            ServiceRequest request, string lang)
        {
            //if (request.ScholarshipId == null || placeHolders == null || !placeHolders.Any())
            //    return new List<PlaceholderDto>();

            //var scholarshipFields = scopedUow.GetRepository<SchFieldValue>().GetAll()
            //    .Include(x => x.SystemField)
            //    .ThenInclude(f => f!.FieldType)
            //    .Where(f => f.ScholarshipId == request.ScholarshipId && placeHolders.Select(ph => ph.FieldId).Contains(f.SystemFieldId))
            //    .ToList();

            var result = new List<PlaceholderDto>();

            //foreach (var holder in placeHolders)
            //{
            //    var fieldValue = scholarshipFields.FirstOrDefault(f => f.SystemFieldId == holder.FieldId);
            //    if (fieldValue == null) continue;

            //    var langToUse = GetLangFromPlaceHolderName(holder.PlaceHolderName);
            //    if (string.IsNullOrEmpty(langToUse)) langToUse = lang;

            //    result.Add(new PlaceholderDto
            //    {
            //        Key = holder.PlaceHolderName,
            //        Value = await RetrieveValueAsync(fieldValue.SystemField!.FieldType.BackendName, 
            //            fieldValue.SystemField.DropDownTypeId, 
            //            fieldValue.Value,  langToUse, holder.PlaceHolderName)
            //    });
            //}

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

        //// ======================= Student Details =======================
        //if (dbreq.Scholarship?.StudentUser != null)
        //{
        //    var student = dbreq.Scholarship.StudentUser;
        //    result.Add(new PlaceholderDto() { Key = "{{DbStudentQID}}", Value = student.QID });
        //    result.Add(new PlaceholderDto() { Key = "{{DbStudentNameEN}}", Value = student.FullNameEn });
        //    result.Add(new PlaceholderDto() { Key = "{{DbStudentNameAr}}", Value = student.FullNameAr });

        //    var nationalityCode = student.NationalityCode;
        //    var nationality = await dropdownService.GetCountryBycode(nationalityCode);
        //    var nationalityName = requestInfo.Lang == "ar" ? nationality?.NameAr : nationality?.NameEn;
        //    result.Add(new PlaceholderDto() { Key = "{{DbStudentNationality}}", Value = nationalityName });
        //}
        //else if (dbreq.Student != null)
        //{
        //    var student = dbreq.Student;
        //    result.Add(new PlaceholderDto() { Key = "{{DbStudentQID}}", Value = student.QID });
        //    result.Add(new PlaceholderDto() { Key = "{{DbStudentNameEN}}", Value = student.FullNameEn });
        //    result.Add(new PlaceholderDto() { Key = "{{DbStudentNameAr}}", Value = student.FullNameAr });

        //    var nationalityCode = student.NationalityCode;
        //    var nationality = await dropdownService.GetCountryBycode(nationalityCode);
        //    var nationalityName = requestInfo.Lang == "ar" ? nationality?.NameAr : nationality?.NameEn;
        //    result.Add(new PlaceholderDto() { Key = "{{DbStudentNationality}}", Value = nationalityName });
        //}

        //// ======================= Scholarship Financial Data =======================
        //if (dbreq.ScholarshipId.HasValue)
        //{
        //    var schId = dbreq.ScholarshipId.Value;

        //    var salary = await srvFinShared.GetStudentSalaryAsync(schId);
        //    var netPay = await srvFinShared.GetStudentNetPayAsync(schId);
        //    var totalInstalment = await srvFinShared.GetTotalInstalmentAsync(schId);
        //    var totalPaidInstalment = await srvFinShared.GetTotalPaidInstalmentAsync(schId);
        //    var remainingInstalment = await srvFinShared.GetRemainingInstalmentAsync(schId);
        //    var totalDeduction = await srvFinShared.GetTotalScheduleDeductionAsync(schId);
        //    var totalPaidDeduction = await srvFinShared.GetTotalPaidScheduleDeductionAsync(schId);
        //    var remainingDeduction = await srvFinShared.GetRemainingScheduleDeductionAsync(schId);

        //    result.AddRange(new List<PlaceholderDto>
        //{
        //    new() { Key = "{{StudentSalary}}", Value = salary },
        //    new() { Key = "{{StudentNetPay}}", Value = netPay.ToString("N2") },
        //    new() { Key = "{{TotalInstalment}}", Value = totalInstalment.ToString("N2") },
        //    new() { Key = "{{TotalPaidInstalment}}", Value = totalPaidInstalment.ToString("N2") },
        //    new() { Key = "{{RemainingInstalment}}", Value = remainingInstalment.ToString("N2") },
        //    new() { Key = "{{totalSchaduleDeduction}}", Value = totalDeduction.ToString("N2") },
        //    new() { Key = "{{totalPaidSchaduleDeduction}}", Value = totalPaidDeduction.ToString("N2") },
        //    new() { Key = "{{remainingSchaduleDeduction}}", Value = remainingDeduction.ToString("N2") }
        //});



        //    // ======================= Request Status =======================
        //    var actionStatus = serviceProvider.CreateScopedUow()
        //        .GetRepository<ActionTransactionsLog>().GetAllQueryFiltered()
        //        .Where(c => c.ServiceRequestId == dbreq.Id && c.NextStatusId == dbreq.StatusId)
        //        .OrderByDescending(c => c.CreateDate)
        //        .FirstOrDefault();

        //    if (actionStatus != null && partyTypeId != null)
        //    {
        //        result.Add(new PlaceholderDto() { Key = "{{CurrenStatusEn}}", Value = GetStatusDisplayName(actionStatus.NextStatusId, partyTypeId, "en") });
        //        result.Add(new PlaceholderDto() { Key = "{{CurrenStatusAr}}", Value = GetStatusDisplayName(actionStatus.NextStatusId, partyTypeId) });
        //        result.Add(new PlaceholderDto() { Key = "{{PreviousStatusEn}}", Value = GetStatusDisplayName(actionStatus.PreviousStatusId, partyTypeId, "en") });
        //        result.Add(new PlaceholderDto() { Key = "{{PreviousStatusAr}}", Value = GetStatusDisplayName(actionStatus.PreviousStatusId, partyTypeId) });
        //    }

        //    // ======================= Service Info =======================
        //    if (dbreq.Service != null)
        //    {
        //        result.Add(new PlaceholderDto() { Key = "{{ServiceNameAr}}", Value = dbreq.Service.NameAr });
        //        result.Add(new PlaceholderDto() { Key = "{{ServiceNameEn}}", Value = dbreq.Service.NameEn });
        //    }

           
        //}
        return result;
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
