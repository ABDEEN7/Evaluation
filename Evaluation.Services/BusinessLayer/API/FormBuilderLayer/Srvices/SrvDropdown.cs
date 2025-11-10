using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scholarship.DAL.Framework;
using Scholarship.DAL.Models.Base;
using Scholarship.DAL.Models.FinancialRegulations;
using Scholarship.DAL.Models.FormBuilder;
using Scholarship.DAL.Models.Masters;
using Scholarship.DAL.Models.ScholarshipEntity;
using Scholarship.DAL.Models.ScholarshipPlanEntity;
using Scholarship.DAL.Models.ServiceRequestEntities;
using Scholarship.DAL.Models.VacanciesPlans;
using Scholarship.Services.BusinessLayer.API.Scholarship;
using Scholarship.Services.Extensions;
using Scholarship.Services.Models.API;
using Scholarship.Services.Special;
using Scholarship.SharedHelper.Enums;
using Scholarship.SharedHelper.Exceptions;
using Scholarship.SharedHelper.Models;
using Scholarship.SharedHelper.Models.Api;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Scholarship.SharedHelper.Models.Api.MastersDTO;
using Scholarship.SharedHelper.Models.Api.ScholarshipDTOs;
using System.Text.Json;
using static Scholarship.SharedHelper.Enums.ConstantKeys;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvDropdown(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, SrvSchSemester srvSchSemester, SrvEntityContract SrvEntityContract,SrvPartyType SrvPartyType, SrvAccreditedUniversity SrvAccreditedUniversity, SrvScholarshipPlan srvScholarshipPlan, SrvProgram SrvProgram, SrvMajor SrvMajor, SrvAcademicDegree SrvAcademicDegree, SrvTrack SrvTrack, SrvVacancy SrvVacancy, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
    {

        public async Task<List<Guid>> GetDropdownFieldsGroup(List<Guid> list)
        {
            var result = new List<Guid>();

            var processedIds = new List<Guid>();

            await GetRelatedDropdownFields(list, result, processedIds);

            return result.Distinct().ToList();
        }

        public async Task GetRelatedDropdownFields(List<Guid> list, List<Guid> result, List<Guid> processedIds)
        {
            if (!list.Any())
                return;

            var unprocessedIds = list.Where(id => !processedIds.Contains(id)).ToList();

            if (!unprocessedIds.Any())
                return;

            foreach (var id in unprocessedIds)
            {
                processedIds.Add(id);
            }

            var dropDownsFields = await serviceScopeFactory.CreateScopedUow()
                .GetRepository<Field>()
                .GetAllQueryFiltered()
                .Where(x => unprocessedIds.Contains(x.Id)
                            || unprocessedIds.Contains(x.DropDownParentFieldId!.Value))  // Fetch both current fields and their parents
                .Where(x => x.FieldType!.BackendName == ConstantKeys.FieldTypeConstant.dropdown || x.FieldType.BackendName == ConstantKeys.FieldTypeConstant.select2)
                .Select(x => new { x.Id, x.DropDownParentFieldId })
                .Distinct()
                .ToListAsync();

            var dropdownIds = dropDownsFields.Select(x => x.Id).ToList();
            var parentIds = dropDownsFields
                .Where(x => x.DropDownParentFieldId.HasValue)
                .Select(x => x.DropDownParentFieldId!.Value)
                .ToList();

            result.AddRange(dropdownIds);

            var allRelatedIds = dropdownIds.Concat(parentIds).Distinct().ToList();

            if (allRelatedIds.Any())
            {
                await GetRelatedDropdownFields(allRelatedIds, result, processedIds);
            }
        }

        public async Task<List<DropDownValueDTO>> GetDropDownValues(string lang, Guid StudentId, Guid? SchId, List<Guid?>? dropDownTypeIds = null)
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            // 1. Try to get the full list from cache
            var fieldDropdownRepo = cacheDataProvider.GetFromCache<List<FieldDropDownValue>>(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNVALUE);

            // 2. If cache miss, fetch from DB and cache the result
            if (fieldDropdownRepo == null)
            {

                var repository = scopedUow.GetRepository<FieldDropDownValue>();

                fieldDropdownRepo = await repository.GetAllQueryFiltered()
                    .Include(x => x.dropDownType)
                                                    .Include(x => x.ParentDropDown).ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNVALUE, fieldDropdownRepo);
            }
            // 1. Try to get the full list from cache
            var dropdownTypeRepo = cacheDataProvider.GetFromCache<List<DropDownType>>(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNTYPE);
            // 2. If cache miss, fetch from DB and cache the result
            if (dropdownTypeRepo == null)
            {

                var repository = scopedUow.GetRepository<DropDownType>();

                dropdownTypeRepo = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNTYPE, dropdownTypeRepo);
            }

            if (dropDownTypeIds != null && dropDownTypeIds.Any())
            {
                var idSet = new HashSet<Guid?>(dropDownTypeIds);
                dropdownTypeRepo = dropdownTypeRepo.Where(dt => idSet.Contains(dt.Id)).ToList();
            }

            if (dropdownTypeRepo.Any())
            {
                var dropdownTypeWithValuesList = new List<DropDownValueDTO>();
                foreach (var dropdownType in dropdownTypeRepo)
                {
                    List<DropDownValueDTO> values;

                    if (!string.IsNullOrEmpty(dropdownType.DataSourceTable))
                    {

                        var tableName = dropdownType.DataSourceTable;


                        var rawData = await GetDataFromTable(tableName, StudentId, SchId, lang);
                        if (rawData != null)
                        {
                            values = rawData.Select(c => new DropDownValueDTO
                            {
                                TitleEn = lang == "ar" ? (c["NameAr"].ToString()??""): (c["NameEn"].ToString()??""),
                                TitleAr = lang == "ar" ? (c["NameAr"].ToString() ?? "") : (c["NameEn"].ToString() ?? ""),
                                DropDownTypeId = dropdownType.Id,
                                OrderNo = c.TryGetValue("Order", out var orderVal) && int.TryParse(orderVal?.ToString(), out var orderNo) ? orderNo : 0,
                                Id = Guid.Parse(c["Id"].ToString()??""),
                                ParentDropDownId = c.TryGetValue("ParentDropDownId", out var parentValue) && parentValue != null
                                                       ? Guid.Parse(parentValue.ToString()??"")
                                                       : null,
                                GPA = c.TryGetValue("GPA", out var gpaVal) ? Convert.ToDecimal(gpaVal) : null,
                                TotalHours = c.TryGetValue("TotalHours", out var thVal) ? Convert.ToInt32(thVal) : null

                            }).ToList();

                            dropdownTypeWithValuesList.AddRange(values);

                        }


                    }
                    else
                    {
                        var dropdownValues =  fieldDropdownRepo
                                                    
                                                    .Where(fdv => fdv.DropDownTypeId == dropdownType.Id)
                                                    .OrderBy(fdv => fdv.OrderNo)
                                                    .ToList();

                        values = dropdownValues!.Select(fdv => new DropDownValueDTO
                        {
                            TitleEn = lang == "ar" ? fdv.TitleAr : fdv.TitleEn,
                            TitleAr = lang == "ar" ? fdv.TitleAr : fdv.TitleEn,
                            DropDownTypeId = fdv.DropDownTypeId,
                            OrderNo = fdv.OrderNo ?? 0,
                            Id = fdv.Id,
                            ParentDropDownId = fdv.ParentDropDownId != null ? fdv.ParentDropDownId.Value : null,
                        }).ToList();
                        dropdownTypeWithValuesList.AddRange(values);
                    }

                }

                return dropdownTypeWithValuesList;
            }
            else
                return new List<DropDownValueDTO>();
        }



        public async Task<List<DropDownValueDTO>> GetDropDownValuesByDropDownTypeId(Guid dropDownTypeId, Guid StudentId,Guid? schId, Guid? parentDropDownId = null, Guid? fieldValueId = null)
        {
            string lang = _requestInfo.Lang;
            using var scopedUow = serviceScopeFactory.CreateScopedUow();

            // 1. Try to get the full list from cache
            var fieldDropdownRepo = cacheDataProvider.GetFromCache<List<FieldDropDownValue>>(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNVALUE);

            // 2. If cache miss, fetch from DB and cache the result
            if (fieldDropdownRepo == null)
            {

                var repository = scopedUow.GetRepository<FieldDropDownValue>();

                fieldDropdownRepo = await repository.GetAllQueryFiltered()
                    .Include(x => x.dropDownType)
                                                    .Include(x => x.ParentDropDown).ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNVALUE, fieldDropdownRepo);
            }
            // 1. Try to get the full list from cache
            var dropdownTypeRepo = cacheDataProvider.GetFromCache<List<DropDownType>>(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNTYPE);
            // 2. If cache miss, fetch from DB and cache the result
            if (dropdownTypeRepo == null)
            {

                var repository = scopedUow.GetRepository<DropDownType>();

                dropdownTypeRepo = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNTYPE, dropdownTypeRepo);
            }

            var dropdownType =  dropdownTypeRepo.Where(x => x.Id == dropDownTypeId).FirstOrDefault();

            if (dropdownType == null)
            {
                return new List<DropDownValueDTO>(); // Return empty list if dropdownType not found
            }

            var dropdownTypeWithValuesList = new List<DropDownValueDTO>();

            List<DropDownValueDTO> values;

            if (!string.IsNullOrEmpty(dropdownType.DataSourceTable))
            {
                var tableName = dropdownType.DataSourceTable;
                var rawData = await GetDataFromTable(tableName, StudentId, schId, lang, fieldValueId);

                if (rawData != null)
                {
                    values = rawData
                        .Where(c => parentDropDownId == null || c.ContainsKey("ParentDropDownId") && c["ParentDropDownId"].ToString() == parentDropDownId.ToString()) // Filter by parent ID
                        .Select(c => new DropDownValueDTO
                        {
                            TitleEn = c["NameEn"].ToString()??"" ,
                            TitleAr =  c["NameAr"].ToString()??"" ,
                            DropDownTypeId = dropdownType.Id,
                            OrderNo = c.TryGetValue("Order", out var orderVal) && int.TryParse(orderVal?.ToString(), out var orderNo) ? orderNo : 0,
                            Id = Guid.Parse(c["Id"].ToString()??""),
                            ParentDropDownId = c.ContainsKey("ParentDropDownId") ? Guid.Parse(c["ParentDropDownId"].ToString()??"") : null,
                            GPA = c.TryGetValue("GPA", out var gpaVal) ? Convert.ToDecimal(gpaVal) : null,
                            TotalHours = c.TryGetValue("TotalHours", out var thVal) ? Convert.ToInt32(thVal) : null
                        }).ToList();

                    dropdownTypeWithValuesList.AddRange(values);
                }
            }
            else
            {
                var dropdownValuesQuery = fieldDropdownRepo
                    .Where(fdv => fdv.DropDownTypeId == dropdownType.Id);

                if (parentDropDownId is not null)
                {
                    dropdownValuesQuery = dropdownValuesQuery.Where(fdv => fdv.ParentDropDownId == parentDropDownId);
                }

                var dropdownValues =  dropdownValuesQuery
                    .OrderBy(fdv => fdv.OrderNo)
                    .ToList();
                if (dropdownValues != null)
                {
                    values = dropdownValues.Select(fdv => new DropDownValueDTO
                    {
                        TitleEn = fdv.TitleEn,
                        TitleAr = fdv.TitleAr,
                        DropDownTypeId = fdv.DropDownTypeId,
                        OrderNo = fdv.OrderNo ?? 0,
                        Id = fdv.Id,
                        ParentDropDownId = fdv.ParentDropDownId
                    }).ToList();

                    dropdownTypeWithValuesList.AddRange(values);
                }
            }

            return dropdownTypeWithValuesList;
        }


        public async Task<FieldDropDownValue?> GetDropDownValuesById(Guid Id)
        {
            var dropdownValue = await serviceScopeFactory.CreateScopedUow()
                                    .GetRepository<FieldDropDownValue>()
                                    .GetByIDNonDeleted(Id);

            return dropdownValue;
        }

        public async Task<List<DropDownValueDTO?>> GetDropDownValuesForAction(Guid? SchId, Guid studentId, Guid actionId, string? dropDownTypeIds, string lang, Guid? requestId)
        {
            var dropDownTypeIdsList = ParseDropDownTypeIds(dropDownTypeIds);

            var allFields = await GetActionFieldsAsync( actionId);

            var tasks = new List<Task<List<DropDownValueDTO>>>
                                {
                                    LoadRegularDropDowns(allFields, dropDownTypeIdsList, studentId,SchId, lang),
                                };

            if (requestId.HasValue)
            {
                tasks.Add(LoadLazyDropDownsWithValues(allFields, requestId.Value, studentId));
            }

            var results = await Task.WhenAll(tasks);
            return results.SelectMany(x => x).Cast<DropDownValueDTO?>().ToList();
        }

        private List<Guid> ParseDropDownTypeIds(string? dropDownTypeIds)
        {
            return dropDownTypeIds?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => Guid.TryParse(id, out var guid) ? guid : (Guid?)null)
                .Where(g => g.HasValue)
                .Select(g => g!.Value)
                .ToList() ?? new List<Guid>();
        }
        private async Task<List<ActionFieldInfo>> GetActionFieldsAsync(Guid actionId)
        {
            return await serviceScopeFactory.CreateScopedUow()
                .GetRepository<ActionStepField>()
                .GetAllQueryFiltered()
                .Include(x => x.Field)
                .Include(x => x.Field!.FieldAttributeValues)
                .Where(x => x.ServiceActionId == actionId && x.Field != null)
                .Select(x => new ActionFieldInfo
                {
                    FieldId = x.FieldId,
                    DropDownParentFieldId = x.Field!.DropDownParentFieldId,
                    DropDownTypeId = x.Field!.DropDownTypeId,
                    IsLazy = x.Field!.FieldAttributeValues!
                                .Any(fa => fa.AttributeKey == "lazyload"),
                    GroupDropDownTypeIds = x.Field.FormGroupList != null && x.Field.FormGroupList.Fields != null
                        ? x.Field.FormGroupList.Fields
                            .Where(f => f.DropDownTypeId != null)
                            .Select(f => f.DropDownTypeId!.Value)
                            .ToList()
                        : new List<Guid>()
                })
                .ToListAsync();
        }


        private async Task<List<DropDownValueDTO>> LoadRegularDropDowns(List<ActionFieldInfo> fields, List<Guid> excludeDropDownTypeIds, Guid studentId, Guid? SchId, string lang)
        {
            var ids = fields
                .Where(f => !f.IsLazy)
                .SelectMany(f => f.GroupDropDownTypeIds.Concat(f.DropDownTypeId != null ? new[] { f.DropDownTypeId.Value } : Array.Empty<Guid>()))
                .Where(id => !excludeDropDownTypeIds.Contains(id))
                .Distinct()
                .ToList();

            return ids.Any()
                ? await GetDropDownValues(lang, studentId,  SchId, ids.Cast<Guid?>().ToList()) ?? new List<DropDownValueDTO>()
                : new List<DropDownValueDTO>();
        }
        private async Task<List<DropDownValueDTO>> LoadLazyDropDownsWithValues(List<ActionFieldInfo> fields, Guid requestId, Guid studentId)
        {
            using var uow = serviceScopeFactory.CreateScopedUow();
            var requestValueRepo = uow.GetRepository<ServiceRequestFieldsValue>();

            var lazyFields = fields
                .Where(f => f.IsLazy && f.DropDownTypeId.HasValue)
                .Select(f => new { f.FieldId, DropDownTypeId = f.DropDownTypeId!.Value, DropDownParentFieldId=f.DropDownParentFieldId })
                .ToList();

            var lazyFieldIds = lazyFields.Select(f => f.FieldId).ToList();

            var values = await requestValueRepo.GetAllQueryFiltered()
                .Where(v => v.ServiceRequestId == requestId && lazyFieldIds.Contains(v.FieldId) && v.Value != null)
                .ToListAsync();

            var result = new List<DropDownValueDTO>();
            foreach (var field in lazyFields)
            {
                var valueEntry = values.FirstOrDefault(x => x.FieldId == field.FieldId);
                var ParentvalueEntry = values.FirstOrDefault(x => x.FieldId == field.DropDownParentFieldId);
                if (valueEntry != null && ParentvalueEntry != null && ParentvalueEntry.Value != null)
                {
                    var valuesList = await GetDropDownValuesByDropDownTypeId(field.DropDownTypeId, studentId,Guid.Empty,Guid.Parse(ParentvalueEntry!.Value!));
                    if (valuesList != null)
                        result.AddRange(valuesList);
                }
            }

            return result;
        }

        private record ActionFieldInfo
        {
            public Guid FieldId { get; set; }
            public Guid? DropDownTypeId { get; set; }
            public Guid? DropDownParentFieldId { get; set; }
            public bool IsLazy { get; set; }
            public List<Guid> GroupDropDownTypeIds { get; set; } = new();
        }


        public async Task<DropDownValueDTO?> GetDropDownValue(string lang, Guid? fieldValueId, Guid? dropDownTypeId, Guid? StudentId, Guid? SchId)
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            // 1. Try to get the full list from cache
            var fieldDropdownRepo = cacheDataProvider.GetFromCache<List<FieldDropDownValue>>(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNVALUE);

            // 2. If cache miss, fetch from DB and cache the result
            if (fieldDropdownRepo == null)
            {
               
                var repository = scopedUow.GetRepository<FieldDropDownValue>();

                fieldDropdownRepo = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNVALUE, fieldDropdownRepo);
            }
            // 1. Try to get the full list from cache
            var dropdownTypeRepo = cacheDataProvider.GetFromCache<List<DropDownType>>(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNTYPE);
            // 2. If cache miss, fetch from DB and cache the result
            if (dropdownTypeRepo == null)
            {

                var repository = scopedUow.GetRepository<DropDownType>();

                dropdownTypeRepo = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNTYPE, dropdownTypeRepo);
            }

            var dropdownType = dropdownTypeRepo
                .Where(dt => dt.Id == dropDownTypeId).FirstOrDefault();

            if (dropdownType == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(dropdownType.DataSourceTable))
            {
                var tableName = dropdownType.DataSourceTable;
                var rawData = await GetDataFromTable(tableName, StudentId,SchId, lang, fieldValueId);

                if (rawData != null && rawData.Any())
                {
                    var matchedValue = rawData.First();

                    return new DropDownValueDTO
                    {
                        TitleEn = matchedValue["NameEn"].ToString()??"",
                        TitleAr = matchedValue["NameAr"].ToString()??"",
                        DropDownTypeId = dropdownType.Id,
                        OrderNo = matchedValue.TryGetValue("Order", out var orderVal) && int.TryParse(orderVal?.ToString(), out var orderNo)? orderNo: 0,
                        Id = Guid.Parse(matchedValue["Id"].ToString()??""),
                        ParentDropDownId = matchedValue.TryGetValue("ParentDropDownId", out var parentValue) && parentValue != null
                            ? Guid.Parse(parentValue.ToString()??"")
                            : null,
                            GPA = matchedValue.TryGetValue("GPA", out var gpaVal) ? Convert.ToDecimal(gpaVal) : null,
                        TotalHours = matchedValue.TryGetValue("TotalHours", out var thVal) ? Convert.ToInt32(thVal) : null
                    };
                }
            }
            else
            {
                var dropdownValue = scopedUow.GetRepository<FieldDropDownValue>().GetAll()
                    .Where(fdv => fdv.DropDownTypeId == dropDownTypeId && fdv.Id == fieldValueId)
                    .FirstOrDefault();

                if (dropdownValue != null)
                {
                    return new DropDownValueDTO
                    {
                        TitleEn = dropdownValue.TitleEn,
                        TitleAr = dropdownValue.TitleAr,
                        DropDownTypeId = dropdownValue.DropDownTypeId,
                        OrderNo = dropdownValue.OrderNo ?? 0,
                        Id = dropdownValue.Id,
                        ParentDropDownId = dropdownValue.ParentDropDownId
                    };
                }
            }

            return null;
        }
        private async Task<List<Dictionary<string, object>>> GetDataFromTable(string tableName, Guid? studentId,Guid? SchId, string lang, Guid? fieldValueId = null)
        {
            //var allowedTables = new[] { "AcademicYear", "Sector", "vp.EntityContract", "Currency", "Semester",
                                  //"SP.ProgramType", "RelationshipType", "SP.EnrollmentType", "CompanionType"};
            var allowedTablesSettingValue = await cacheDataProvider.GetSystemSettingValue(SystemSettings.DropDownDataSourceAllowedTables);
            var allowedTables = JsonSerializer.Deserialize<List<string>>( string.IsNullOrWhiteSpace(allowedTablesSettingValue) ? "[]" : allowedTablesSettingValue) ?? new();
            using var scopedUow = serviceScopeFactory.CreateScopedUow();

            List<Dictionary<string, object>> result;

            switch (tableName)
            {
                case "Bank":
                    result = await GetBankData(fieldValueId);
                    break;

                case "BankBranch":
                    result = await GetBankBranchData(fieldValueId);
                    break;

                case "UserGender":
                    result = await GetUserGenderData(fieldValueId);
                    break;

                case "MainDegree":
                    result = await GetMainDegreeData(fieldValueId);
                    break;

                case "AcademicDegree":
                    result = await GetSubDegreeData(fieldValueId);
                    break;

                case "FinancialRegulations":
                    result = await GetFinancialRegulationsData(fieldValueId);
                    break;

                case "Programs":
                    result = await GetProgramsData(fieldValueId);
                    break;
                case "AccreditedCountry":
                    result = await GetAccreditedCountryData(null, fieldValueId);
                    break;
                case "Country":
                    result = await GetCountriesData(fieldValueId);
                    break;
                case "City":
                    result = await GetCityData(fieldValueId);
                    break;
                case "Institutions":
                    result = await GetInstitutionData(fieldValueId);
                    break;
                case "Vacancy":
                    result = await GetVacancyData(fieldValueId, null, null, null);
                    break;
                case "PreferredEntity":
                    result = await GetPreferredEntityData(fieldValueId);
                    break;

                case "MainMajor":
                    result = await GetMainMajorData(fieldValueId);
                    break;

                case "Major":
                    result = await GetSubMajorData(fieldValueId);
                    break;
                case "SP.Track":
                    result = await GetTrackData(fieldValueId);
                    break;



                case "CurrentAcademicYear":
                    result = await GetCurrentAcademicYearData(fieldValueId);
                    break;

                case "AccreditedUniversity":
                    result = await GetAccreditedUniversity(null, fieldValueId);
                    break;


                case "SchSemester":
                    result = await GetSchSemester(null,SchId);
                    break;
                default:
                    if (!allowedTables.Any(t => t.Equals(tableName, StringComparison.OrdinalIgnoreCase)))
                        return new List<Dictionary<string, object>>();
                    var query = fieldValueId == null
                        ? $"SELECT Id, NameEn, NameAr ,OrderNo FROM {tableName} WHERE IsActive = 1 AND IsDeleted = 0"
                        : $"SELECT Id, NameEn, NameAr ,OrderNo FROM {tableName} WHERE IsActive = 1 AND IsDeleted = 0 AND Id = @FieldValueId";

                    var rawData = await scopedUow.ExecuteRawQueryAsync(query, fieldValueId == null ? new{ } : new { FieldValueId = fieldValueId });
                    result = rawData.Select(item => new Dictionary<string, object>(item)).ToList();
                    break;
            }

            return result;
        }

        public async Task<List<Dictionary<string, object>>> GetMainDegreeData(Guid? fieldValueId = null, FormRequestSearchDTO? requestSearchDTO = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<AcademicDegree>>(ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICDEGREE);
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
               
                var repository = scopedUow.GetRepository<AcademicDegree>();

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICDEGREE, data);
            }

            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var degree =  scopedUow.GetRepository<AcademicDegree>().GetAll().Where(b => b.Id == fieldValueId.Value).FirstOrDefault();
                if (degree != null)
                {
                    return new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "Id", degree.Id },
                                { "NameAr", degree.NameAr },
                                { "NameEn", degree.NameEn },
                                { "OrderNo", degree.OrderNo! }
                            }
                        };
                }
                return new List<Dictionary<string, object>>();
            }
            if (requestSearchDTO != null)
            {
                return await SrvAcademicDegree.GetMainAcademicDegreesByScholarshipPlanId(requestSearchDTO);
            }
            return new List<Dictionary<string, object>>();

        }

        public async Task<List<Dictionary<string, object>>> GetSubDegreeData(Guid? fieldValueId = null, FormRequestSearchDTO? requestSearchDTO = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<AcademicDegree>>(ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICDEGREE);
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
              
                var repository = scopedUow.GetRepository<AcademicDegree>();

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICDEGREE, data);
            }

            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var degree = scopedUow.GetRepository<AcademicDegree>().GetAll().Where(b => b.Id == fieldValueId.Value).FirstOrDefault();
                if (degree != null)
                {
                    return new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "Id", degree.Id },
                                { "NameAr", degree.NameAr },
                                { "NameEn", degree.NameEn },
                                { "OrderNo", degree.OrderNo! },
                                { "ParentDropDownId", degree.ParentDegreeId! }
                            }
                        };
                }
                return new List<Dictionary<string, object>>();
            }
            if (requestSearchDTO != null)
            {
                return await SrvAcademicDegree.GetSubAcademicDegreesByScholarshipPlanId(requestSearchDTO);
            }
            return new List<Dictionary<string, object>>();

        }

        private async Task<List<Dictionary<string, object>>> GetUserGenderData(Guid? fieldValueId = null)
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var repository = scopedUow.GetRepository<UserGender>();

            var data = await repository.GetAllQueryFiltered().ToListAsync();

            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                data = repository.GetAll().Where(b => b.Id == fieldValueId.Value).ToList();
            }

            return data.Select(item => new Dictionary<string, object>
                {
                    { "Id", item.Id },
                    { "NameAr", item.TitleAr },
                    { "NameEn", item.TitleEn }
                }).ToList();
        }
        public async Task<List<Dictionary<string, object>>> GetProgramsData(Guid? fieldValueId = null, SchRequestDetails? schRequestDetails = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<SchProgram>>(ConstantKeys.WebAppCacheTableName.CACHE_PROGRAM);
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
               
                var repository = scopedUow.GetRepository<SchProgram>();

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_PROGRAM, data);
            }
            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var program = scopedUow.GetRepository<SchProgram>().GetAll().Where(b => b.Id == fieldValueId.Value).FirstOrDefault();
                if (program != null)
                {
                    return new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "Id", program.Id },
                            { "NameAr", program.NameAr },
                            { "NameEn", program.NameEn }
                        }
                    };
                }
                return new List<Dictionary<string, object>>();
            }

            if (schRequestDetails != null)
            {
                var list = await srvScholarshipPlan.ValidateScholarshipRequest(schRequestDetails);
                return list.AllowedPrograms.Select(item => new Dictionary<string, object>
                    {
                        { "Id", item.Id },
                        { "NameAr", item.NameAr },
                        { "NameEn", item.NameEn }
                    }).ToList();
            }
            return new List<Dictionary<string, object>>();
        }

        public async Task<List<Dictionary<string, object>>> GetVacancyData(Guid? fieldValueId, Guid? GenderId, Guid? AcademicDegreeId, Guid? MajorIdPlaceholder)
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var repository = scopedUow.GetRepository<VacancySeat>();

           var data = await repository.GetAllQueryFiltered()
        .Include(vs => vs.VacancyGenderPlan)
            .ThenInclude(vgp => vgp!.Vacancy)
                .ThenInclude(v => v.Major)
        .Include(vs => vs.VacancyGenderPlan)
            .ThenInclude(vgp => vgp!.Vacancy)
                .ThenInclude(v => v.AcademicDegree)
        .Include(vs => vs.VacancyGenderPlan)
            .ThenInclude(vgp => vgp!.Vacancy)
                .ThenInclude(v => v.EntityContract)
                .ToListAsync();


            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var seatDetails = repository.GetAll()
                                .Include(vs => vs.VacancyGenderPlan)
                                 .ThenInclude(vgp => vgp!.Vacancy)
                                        .ThenInclude(v => v.Major)
                                .Include(vs => vs.VacancyGenderPlan)
                                    .ThenInclude(vgp => vgp!.Vacancy)
                                        .ThenInclude(v => v.AcademicDegree)
                                .Include(vs => vs.VacancyGenderPlan)
                                    .ThenInclude(vgp => vgp!.Vacancy)
                                        .ThenInclude(v => v.EntityContract)
                .Where(b => b.Id == fieldValueId.Value).Select(seat=>new VacancySearchResultDTO
                {
                    seatId = seat.Id,
                    EntityContractId = seat.VacancyGenderPlan!.Vacancy.EntityContractId,
                    EntityContractNameEn = seat.VacancyGenderPlan.Vacancy.EntityContract!.NameEn,
                    EntityContractNameAr = seat.VacancyGenderPlan.Vacancy.EntityContract!.NameAr,
                    MajorId = seat.VacancyGenderPlan.Vacancy.MajorId,
                    MajorNameEn = seat.VacancyGenderPlan.Vacancy.Major!.NameEn,
                    MajorNameAr = seat.VacancyGenderPlan.Vacancy.Major!.NameAr,
                    AcademicDegreeId = seat.VacancyGenderPlan.Vacancy.AcademicDegreeId,
                    AcademicDegreeNameEn = seat.VacancyGenderPlan.Vacancy.AcademicDegree!.NameEn,
                    AcademicDegreeNameAr = seat.VacancyGenderPlan.Vacancy.AcademicDegree!.NameAr,
                }).FirstOrDefault();

                if (seatDetails != null)
                {
                    
                    return new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "Id", seatDetails.seatId },
                                { "NameAr", seatDetails.MajorNameAr + " - "+ seatDetails.EntityContractNameAr + " - "+ seatDetails.EntityContractNameAr },
                                { "NameEn", seatDetails.MajorNameEn  + " - "+ seatDetails.EntityContractNameEn +"" },
                                { "EntityContractId", seatDetails.EntityContractId },

                            }
                        };
                }
            }
            return new List<Dictionary<string, object>>();
            
        }
        public async Task<List<DropDownValueDTO>> GetVacancyData(Guid genderId, Guid academicDegreeId, Guid majorIdPlaceholder, Guid? fieldValueId)
        {
            string lang = _requestInfo.Lang;

            var result = new List<DropDownValueDTO>();

            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var seatDetails = await SrvVacancy.GetSeatDetails(fieldValueId.Value);
                if (seatDetails != null)
                {
                    result.Add(new DropDownValueDTO
                    {
                        Id = seatDetails.seatId,
                        TitleAr = $"{seatDetails.MajorNameAr} + \" - \"+ {seatDetails.EntityContractNameAr}".Trim(),
                        TitleEn = $"{seatDetails.MajorNameEn} + \" - \"+  {seatDetails.EntityContractNameEn}".Trim(),
                        ParentDropDownId = seatDetails.MajorId
                    });

                    return result;
                }
            }

            var vacancies = await SrvVacancy.SearchVacanciesHaveSeatAvailable(genderId, academicDegreeId, majorIdPlaceholder);

            if (vacancies == null || !vacancies.Any())
                return result;

            result = vacancies.Select(item => new DropDownValueDTO
            {
                Id = item.EntityContractId,
                TitleAr = $"{item.MajorNameAr} {item.EntityContractNameAr}".Trim(),
                TitleEn = $"{item.MajorNameEn} {item.EntityContractNameEn}".Trim(),
                ParentDropDownId = item.MajorId
            }).ToList();

            return result;
        }
        public async Task<List<Dictionary<string, object>>> GetPreferredEntityData(Guid? fieldValueId)
        {
            string lang = _requestInfo.Lang;

            var result = new List<DropDownValueDTO>();
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<EntityContract>>(ConstantKeys.WebAppCacheTableName.CACHE_ENTITYCONTRACT);
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var repository = scopedUow.GetRepository<EntityContract>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
              

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_ENTITYCONTRACT, data);
            }
            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var EntityDetails = repository.GetAll().Where(b => b.Id == fieldValueId.Value).FirstOrDefault();
                if (EntityDetails != null)
                {
                    return new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "Id", EntityDetails.Id },
                                { "NameAr", EntityDetails.NameAr },
                                { "NameEn", EntityDetails.NameEn  },

                            }
                        };
                }
            }

            return new List<Dictionary<string, object>>();


        }
        public async Task<List<DropDownValueDTO>> GetPreferredEntityData(Guid genderId, Guid academicDegreeId, Guid majorIdPlaceholder, Guid? fieldValueId)
        {
            string lang = _requestInfo.Lang;

            var result = new List<DropDownValueDTO>();

            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var EntityDetails = await SrvEntityContract.GetEntityContractById(fieldValueId.Value);
                if (EntityDetails != null)
                {
                    result.Add(new DropDownValueDTO
                    {
                        Id = EntityDetails.Id,
                        TitleAr = $" {EntityDetails.NameEn}".Trim(),
                        TitleEn = $"{EntityDetails.NameAr}".Trim(),

                    });

                    return result;
                }
            }

            var vacancies = await SrvVacancy.SearchVacanciesHaveSeatAvailableOrHold(genderId, academicDegreeId, majorIdPlaceholder);

            if (vacancies == null || !vacancies.Any())
                return result;

            result = vacancies.Select(item => new DropDownValueDTO
            {
                Id = item.EntityContractId,
                TitleAr = $"{item.EntityContractNameAr}".Trim(),
                TitleEn = $"{item.EntityContractNameEn}".Trim(),
                ParentDropDownId = item.MajorId
            }).ToList();

            return result;
        }

        public async Task<List<Dictionary<string, object>>> GetMainMajorData(Guid? fieldValueId = null, FormRequestSearchDTO? requestSearchDTO = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<Major>>(ConstantKeys.WebAppCacheTableName.CACHE_MAJOR);

            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var repository = scopedUow.GetRepository<Major>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_MAJOR, data);
            }
            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var major = repository.GetAll().Where(b => b.Id == fieldValueId.Value).FirstOrDefault();
                if (major != null)
                {
                    return new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "Id", major.Id },
                            { "NameAr", major.NameAr },
                            { "NameEn", major.NameEn },
                            { "OrderNo", major.OrderNo! }
                        }
                    };
                }
                return new List<Dictionary<string, object>>();
            }
            
            if (requestSearchDTO != null)
            {
                return await SrvMajor.GetParentMajorsUsedInYearlyPlan(requestSearchDTO);
            }
            return new List<Dictionary<string, object>>();

        }

        public async Task<List<Dictionary<string, object>>> GetSubMajorData(Guid? fieldValueId = null, FormRequestSearchDTO? requestSearchDTO = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<Major>>(ConstantKeys.WebAppCacheTableName.CACHE_MAJOR);
                using var scopedUow = serviceScopeFactory.CreateScopedUow();
                var repository = scopedUow.GetRepository<Major>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_MAJOR, data);
            }
            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var major = repository.GetAll().Where(b => b.Id == fieldValueId.Value).FirstOrDefault();
                if (major != null)
                {
                    return new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "Id", major.Id },
                            { "NameAr", major.NameAr },
                            { "NameEn", major.NameEn },
                            { "OrderNo", major.OrderNo! },
                            { "ParentDropDownId", major.ParentMajorId! }
                        }
                    };
                }

            }
            if (requestSearchDTO != null)
            {
                return await SrvMajor.GetMajorsUsedInYearlyPlan(requestSearchDTO);
            }
            return new List<Dictionary<string, object>>();


        }

        public async Task<List<Dictionary<string, object>>> GetTrackData(Guid? fieldValueId = null, FormRequestSearchDTO? requestSearchDTO = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<Track>>(ConstantKeys.WebAppCacheTableName.CACHE_TRACK);
               using var scopedUow = serviceScopeFactory.CreateScopedUow();
                var repository = scopedUow.GetRepository<Track>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
                

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_TRACK, data);
            }
            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var Track = repository.GetAll().Where(b => b.Id == fieldValueId.Value).FirstOrDefault();
                if (Track != null)
                {
                    return new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "Id", Track.Id! },
                                { "NameAr", Track.NameAr },
                                { "NameEn", Track.NameEn   },
                                { "OrderNo", Track.OrderNo!   },

                            }
                        };
                }
                return new List<Dictionary<string, object>>();
            }
            if (requestSearchDTO != null)
            {
                return await SrvTrack.GetTracksByScholarshipPlanId(requestSearchDTO);
            }
            return new List<Dictionary<string, object>>();

        }
        private async Task<List<Dictionary<string, object>>> GetCurrentAcademicYearData(Guid? fieldValueId = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<AcademicYear>>(ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICYEAR);

                using var scopedUow = serviceScopeFactory.CreateScopedUow();
                var repository = scopedUow.GetRepository<AcademicYear>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            { 

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICYEAR, data);
            }

             data = data.Where(a => a.IsCurrent).ToList();

            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                data = repository.GetAll().Where(x => x.Id == fieldValueId).ToList();
            }
            else
            {
                data = data.Where(a => a.IsCurrent).ToList();

            }

            return data.Select(item => new Dictionary<string, object>
            {
                { "Id", item.Id },
                { "NameAr", item.NameAr },
                { "NameEn", item.NameEn },
                { "StartDate", item.StartDate.ToString("yyyy") },
                { "EndDate", item.EndDate.ToString("yyyy") }
            }).ToList();
        }

        public async Task<List<Dictionary<string, object>>> GetAccreditedUniversity(FormRequestSearchDTO? formRequestSearchDTO, Guid? fieldValueId = null, Guid? academicYearId = null)
        {
            // 1. Try to get the full list from cache
            using var scopedUow = serviceScopeFactory.CreateScopedUow();

            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var university = scopedUow.GetRepository<AccreditedUniversity>()
                    .GetAll()
                    .Include(u => u.University)
                    .ThenInclude(u => u!.City)
                    .ThenInclude(u => u!.Country)
                    .Where(b => b.UniversityId == fieldValueId.Value).FirstOrDefault();
                if (university?.University != null)
                {
                    return new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                { "Id", university.UniversityId },
                                { "NameAr", university.University.NameAr },
                                { "NameEn", university.University.NameEn },
                                { "ParentDropDownId", university.University.City?.CountryId ?? Guid.Empty }
                            }
                        };
                }
                return new List<Dictionary<string, object>>();
            }
            else
            {
                var currentyear = await scopedUow.GetRepository<AcademicYear>().GetAllNonDeleted().Where(x => x.IsCurrent == true).Select(x => x.Id).FirstOrDefaultAsync();

                currentyear = academicYearId ?? currentyear;

                var data = cacheDataProvider.GetFromCache<List<AccreditedUniversity>>(ConstantKeys.WebAppCacheTableName.CACHE_ACCREDITEDUNIVERSITY + "_" + currentyear);

                // 2. If cache miss, fetch from DB and cache the result
                if (data == null)
                {

                    var repository = scopedUow.GetRepository<AccreditedUniversity>();

                    data = await repository.GetAllQueryFiltered()
                        .Where(x => x.AcademicYearId == currentyear)
                        .Include(u => u.University)
                        .ThenInclude(u => u!.City)
                        .ThenInclude(u => u!.Country)
                        .ToListAsync();

                    await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_ACCREDITEDUNIVERSITY + "_" + currentyear, data);
                }


                if (formRequestSearchDTO != null)
                {
                    return await SrvAccreditedUniversity.GetAccreditedUniversitiesByCountryId(formRequestSearchDTO);
                }
                return new List<Dictionary<string, object>>();
            }


        }
       
        public async Task<List<Dictionary<string, object>>> GetAccreditedCountryData(FormRequestSearchDTO? formRequestSearchDTO, Guid? fieldValueId = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<Country>>(ConstantKeys.WebAppCacheTableName.CACHE_COUNTRY);
             using var scopedUow = serviceScopeFactory.CreateScopedUow();
                var repository = scopedUow.GetRepository<Country>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
               

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_COUNTRY, data);
            }
            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var Country = repository.GetAll().Where(b => b.Id == fieldValueId.Value).FirstOrDefault();
                if (Country != null)
                {
                    return new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "Id", Country.Id! },
                            { "NameAr", Country.NameAr },
                            { "NameEn", Country.NameEn },

                        }
                    };
                }
                return new List<Dictionary<string, object>>();
            }
            if (formRequestSearchDTO != null)
            {
                
                return await SrvAccreditedUniversity.GetAllCountriesByAccreditedUniversities(formRequestSearchDTO);
            }
            return new List<Dictionary<string, object>>();

        }
        public async Task<List<Dictionary<string, object>>> GetCountriesData(Guid? fieldValueId = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<Country>>(ConstantKeys.WebAppCacheTableName.CACHE_COUNTRY);
  using var scopedUow = serviceScopeFactory.CreateScopedUow();
                var repository = scopedUow.GetRepository<Country>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
              

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_COUNTRY, data);
            }
            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                var Country = repository.GetAll().Where(b => b.Id == fieldValueId.Value).FirstOrDefault();
                if (Country != null)
                {
                    return new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "Id", Country.Id! },
                            { "NameAr", Country.NameAr },
                            { "NameEn", Country.NameEn },

                        }
                    };
                }
                return new List<Dictionary<string, object>>();
            }

            var list = await GetAllCountries();
            return list.Select(item => new Dictionary<string, object>
            {
                { "Id", item.Id },
                { "NameAr", item.NameAr },
                { "NameEn", item.NameEn },
            }).ToList();


        }
        public async Task<List<Dictionary<string, object>>> GetFinancialRegulationsData(Guid? fieldValueId = null, Guid? countryId = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<FinRegulation>>(ConstantKeys.WebAppCacheTableName.CACHE_FINREGULATION);
using var scopedUow = serviceScopeFactory.CreateScopedUow();
                var repository = scopedUow.GetRepository<FinRegulation>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
                

                data = await repository.GetAllQueryFiltered().Include(x=>x.FinRegulationAllocations).ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_FINREGULATION, data);
            }
            if (fieldValueId == null && countryId == null)
            {
                return new List<Dictionary<string, object>>();
            }
            if (countryId != null && countryId != Guid.Empty)
            {
                data = data.Where(x => x.FinRegulationAllocations!.Any(c => c.CountryId == countryId)).ToList();
            }
            // 3. Filter the result if fieldValueId is provided
            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {

                data = repository.GetAll().Where(x => x.Id == fieldValueId).ToList();
            }

            
            return data.Select(item => new Dictionary<string, object>
                {
                    { "Id", item.Id },
                    { "NameAr", item.NameAr },
                    { "NameEn", item.NameEn },
                    { "StartAcademicYear", item.StartAcademicYear.ToString("yyyy-MM-dd") },
                    { "EndAcademicYear", item.EndAcademicYear?.ToString("yyyy-MM-dd")??"" },
                    { "FinRegulationStatusId", item.FinRegulationStatusId }
                }).ToList();
        }

        private async Task<List<Dictionary<string, object>>> GetBankData(Guid? fieldValueId = null)
        {

            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<Bank>>(ConstantKeys.WebAppCacheTableName.CACHE_BANK);
 using var scopedUow = serviceScopeFactory.CreateScopedUow();
                var repository = scopedUow.GetRepository<Bank>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
               

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_BANK, data);
            }

            // 3. Filter the result if fieldValueId is provided
            if (fieldValueId != null && fieldValueId != Guid.Empty)
            {
                data = repository.GetAll().Where(b => b.Id == fieldValueId.Value).ToList();
            }

            return data.Select(item => new Dictionary<string, object>
                                     {
                                        { "Id", item.Id },
                                        { "NameAr", item.NameAr },
                                        { "NameEn", item.NameEn },
                                        { "OrderNo", item.OrderNo! },
                                        { "ParentDropDownId", item.CountryId }
                                     }).ToList();

        }


        private async Task<List<Dictionary<string, object>>> GetBankBranchData(Guid? fieldValueId = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<BankBranch>>(ConstantKeys.WebAppCacheTableName.CACHE_BANKBRANCH);
    using var scopedUow = serviceScopeFactory.CreateScopedUow();
                var repository = scopedUow.GetRepository<BankBranch>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
            

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_BANKBRANCH, data);
            }

            // 3. Filter the result if fieldValueId is provided
            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                data = repository.GetAll().Where(b => b.Id == fieldValueId.Value).ToList();
            }


            return data.Select(item => new Dictionary<string, object>
            {
                { "Id", item.Id },
                { "NameAr", item.NameAr },
                { "NameEn", item.NameEn },
                { "OrderNo", item.OrderNo! },
                { "ParentDropDownId", item.BankId }
            }).ToList();
            
        }
        private async Task<List<Dictionary<string, object>>> GetCityData(Guid? fieldValueId = null)
        {
            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<City>>(ConstantKeys.WebAppCacheTableName.CACHE_CITY);
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var repository = scopedUow.GetRepository<City>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {


                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_CITY, data);
            }

            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                data = repository.GetAll().Where(x => x.Id == fieldValueId).ToList();
            }

            return data.Select(item => new Dictionary<string, object>
            {
                { "Id", item.Id },
                { "NameAr", item.NameAr },
                { "NameEn", item.NameEn },
                { "ParentDropDownId", item.CountryId }
            }).ToList();
        }
        private async Task<List<Dictionary<string, object>>> GetInstitutionData(Guid? fieldValueId = null)
        {

            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<SchInstitution>>(ConstantKeys.WebAppCacheTableName.CACHE_INSTITUTION);
 using var scopedUow = serviceScopeFactory.CreateScopedUow();
                var repository = scopedUow.GetRepository<SchInstitution>();
            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
               

                data = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_INSTITUTION, data);
            }

            if (fieldValueId is not null && fieldValueId != Guid.Empty)
            {
                data = repository.GetAll().Where(x => x.Id == fieldValueId).ToList();
            }
            return data.Select(item => new Dictionary<string, object>
            {
                { "Id", item.Id },
                { "NameAr", item.NameAr },
                { "NameEn", item.NameEn },
                { "ParentDropDownId", item.CountryId }
            }).ToList();
        }
        public async Task<List<Country>> GetCityData()
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var repository = scopedUow.GetRepository<Country>();

            return await repository
                .GetAllQueryFiltered()
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Country>> GetAllCountries()
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var repository = scopedUow.GetRepository<Country>();

            return await repository
                .GetAllQueryFiltered()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Country?> GetCountryById(string countryId)
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var repository = scopedUow.GetRepository<Country>();

            return await repository
                .GetAllQueryFiltered()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Code == countryId);
        }
        public async Task<CountryDTO?> GetCountryBycode(string? countrycode)
        {
            var country = await serviceScopeFactory.CreateScopedUow().GetRepository<Country>()
                .GetAllQueryFiltered()
                .FirstOrDefaultAsync(c => c.Code == countrycode);

            return country != null ? mapper.Map<CountryDTO>(country, opts => opts.Items["Language"] = _requestInfo.Lang) : null;
        }
        public async Task<FormRequestSearchDTO> GetUserDataByuserId(Guid userId, Guid? EnrollmentTypeId = null, Guid? CountryId = null, Guid? UniversityId = null, Guid? TrackId = null, Guid? MainDegreeId = null, Guid? SubDegreeId = null, Guid? mainMajorId = null)
        {
            var uow = serviceScopeFactory.CreateScopedUow();
            var userProfile = await uow.GetRepository<StudentUser>().GetByIDActiveNonDeleted(userId);

            if (userProfile == null)
                throw new BusinessException(ConstantKeys.ExceptionMessage.UserInfoNotFound);

            Guid? nationalityId = null;
            if (!string.IsNullOrEmpty(userProfile.NationalityCode))
            {
                var nationality = await GetCountryBycode(userProfile.NationalityCode);
                nationalityId = nationality?.Id;
            }

            var dto = new FormRequestSearchDTO
            {
                GenderId = userProfile.UserGenderId,
                NationalityId = nationalityId,
                QID = userProfile.QID,
                Age = userProfile.DOB.HasValue ? CalculateAge(userProfile.DOB.Value) : (int?)null,
                TrackId= TrackId,
                MainDegreeId= MainDegreeId,
                SubDegreeId= SubDegreeId,
                CountryId= CountryId,
                EnrollmentTypeId= EnrollmentTypeId,
                UniversityId= UniversityId,
                MainMajorId = mainMajorId
            };

            return dto;
        }

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
        public async Task<List<Dictionary<string, object>>> GetSchSemester( string? semesterIds = null,  Guid? schId = null)
        {
            if (!string.IsNullOrWhiteSpace(semesterIds))
            {
                var semesters = await srvSchSemester.GetSemestersByIdsAsync(semesterIds);
                return semesters.Select(s => new Dictionary<string, object>
                {
                    ["Id"] = s.Id,  
                    ["NameAr"] = $"{s.SemesterNameAr ?? string.Empty} - TotalHours: {s.TotalHours} - GPA: {s.GPA}",
                    ["NameEn"] = $"{s.SemesterNameEn ?? string.Empty} - TotalHours: {s.TotalHours} - GPA: {s.GPA}",
                    ["ParentDropDownId"] = s.AcademicYear!,
                    ["TotalHours"] = s.TotalHours ?? 0,
                    ["GPA"] = s.GPA ?? 0,
                }).ToList();
            }

            if (schId.HasValue)
            {
                var semesters = await srvSchSemester.GetSrvSchSemesterList( schId.Value,true);

                return semesters.Select(s => new Dictionary<string, object>
                {
                    ["Id"] = s.SemesterId, 
                    ["NameAr"] = $"{s.SemesterNameAr ?? string.Empty} - TotalHours: {s.TotalHours} - GPA: {s.GPA}",
                    ["NameEn"] = $"{s.SemesterNameEn ?? string.Empty} - TotalHours: {s.TotalHours} - GPA: {s.GPA}",
                    ["ParentDropDownId"] = s.AcademicYear ?? Guid.Empty,
                    ["TotalHours"] = s.TotalHours ?? 0,
                    ["GPA"] = s.GPA ?? 0,
                }).ToList();
            }

            return new List<Dictionary<string, object>>();
        }


        #region Advanced search function

        public async Task<List<SelectListItemDTO>> GetDistinctCountriesFromRequestsAsync(Guid moduleId)
        {
            string lang = _requestInfo.Lang;
            var userId = userInfo.UserId;
            var uow = serviceScopeFactory.CreateScopedUow();
            var fieldRepo = uow.GetRepository<ServiceRequest>();

            var isAllowedToViewAllRequestsWithoutFilter = await SrvPartyType.IsAllowedToViewAllRequestsWitoutFilterationAsync(userId, moduleId);
            var userPartyData = await SrvPartyType.GetUserPartyTypeData(userId, moduleId);

            List<Guid> allowedCountryIds = new List<Guid>();

            if (!isAllowedToViewAllRequestsWithoutFilter)
            {
                allowedCountryIds = userPartyData
                    .SelectMany(p => p.PartyTypeCountyUniversity)
                    .Select(x => x.CountryId)
                    .Distinct()
                    .ToList();

                if (!allowedCountryIds.Any())
                    return new List<SelectListItemDTO>();
            }

            var countryFieldValues = await fieldRepo.GetAllQueryFiltered()
                .Select(x => x.CountryId)
                .Distinct()
                .ToListAsync();

            var parsedIds = countryFieldValues
                .Where(id => id.HasValue && (isAllowedToViewAllRequestsWithoutFilter || allowedCountryIds.Contains(id.Value)))
                .Select(id => id!.Value)
                .ToList();

            var countryRepo = uow.GetRepository<Country>();
            var countries = await countryRepo.GetAllQueryFiltered(x => parsedIds.Contains(x.Id)).ToListAsync();

            return countries
                .OrderBy(x => lang == "ar" ? x.NameAr : x.NameEn)
                .Select(x => new SelectListItemDTO
                {
                    Value = x.Id.ToString(),
                    Text = lang == "ar" ? x.NameAr : x.NameEn
                })
                .ToList();
        }

        public async Task<List<SelectListItemDTO>> GetDistinctUniversitiesFromRequestsWithAccessAsync(Guid moduleId)
        {
            string lang = _requestInfo.Lang;
            var userId = userInfo.UserId;
            var uow = serviceScopeFactory.CreateScopedUow();
            var fieldRepo = uow.GetRepository<ServiceRequest>();

            var isAllowedToViewAllRequestsWithoutFilter = await SrvPartyType.IsAllowedToViewAllRequestsWitoutFilterationAsync(userId, moduleId);
            var userPartyData = await SrvPartyType.GetUserPartyTypeData(userId, moduleId);

            List<Guid> allowedUniversityIds = new List<Guid>();

            if (!isAllowedToViewAllRequestsWithoutFilter)
            {
                allowedUniversityIds = userPartyData
                    .SelectMany(p => p.PartyTypeCountyUniversity)
                    .Where(x => x.UniversityId.HasValue)
                    .Select(x => x.UniversityId!.Value)
                    .Distinct()
                    .ToList();
            }

            var universityFieldValues = await fieldRepo.GetAllQueryFiltered()
                .Select(x => x.UniversityId)
                .Distinct()
                .ToListAsync();

            var parsedIds = universityFieldValues
                .Where(id => id.HasValue && (isAllowedToViewAllRequestsWithoutFilter  || !allowedUniversityIds.Any() || allowedUniversityIds.Contains(id.Value)))
                .Select(id => id!.Value)
                .ToList();

            var universityRepo = uow.GetRepository<University>();
            var universities = await universityRepo.GetAllQueryFiltered(x => parsedIds.Contains(x.Id)).ToListAsync();

            return universities
                .OrderBy(x => lang == "ar" ? x.NameAr : x.NameEn)
                .Select(x => new SelectListItemDTO
                {
                    Value = x.Id.ToString(),
                    Text = lang == "ar" ? x.NameAr : x.NameEn
                })
                .ToList();
        }

        public async Task<List<SelectListItemDTO>> GetDistinctCountriesFromScholarshipsAsync(Guid moduleId)
        {
            string lang = _requestInfo.Lang;
            var userId = userInfo.UserId;
            var uow = serviceScopeFactory.CreateScopedUow();

            var isAllowedToViewAll = await SrvPartyType.IsAllowedToViewAllScholarshipWitoutFilterationAsync(userId, moduleId);
            var userPartyData = await SrvPartyType.GetUserPartyTypeData(userId, moduleId);

            List<Guid> allowedCountryIds = new List<Guid>();

            if (!isAllowedToViewAll)
            {
                allowedCountryIds = userPartyData
                    .SelectMany(p => p.PartyTypeCountyUniversity)
                    .Select(x => x.CountryId)
                    .Distinct()
                    .ToList();

                if (!allowedCountryIds.Any())
                    return new List<SelectListItemDTO>();
            }

            var query = uow.GetRepository<ScholarshipData>()
                .GetAllQueryFiltered()
                .Include(x => x.Country)
                .Where(s => s.Country != null);

            if (!isAllowedToViewAll)
            {
                query = query.Where(s => allowedCountryIds.Contains(s.CountryId));
            }

            var countries = await query
                .Select(s => new
                {
                    s.Country!.Id,
                    Name = lang == "ar" ? s.Country.NameAr : s.Country.NameEn,
                    Order = s.Country.OrderNo
                })
                .Distinct()
                .OrderBy(c => c.Order)
                .ToListAsync();

            return countries.Select(c => new SelectListItemDTO
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }

        public async Task<List<SelectListItemDTO>> GetNationalitiesAsync(string lang)
        {
            var userRepo = serviceScopeFactory.CreateScopedUow().GetRepository<UserProfile>();
            var countryRepo = serviceScopeFactory.CreateScopedUow().GetRepository<Country>();

            // Get distinct nationality codes from user profiles
            var usedNationalityCodes = await userRepo
                .GetAllQueryFiltered(x => x.NationalityCode != null)
                .Select(x => x.NationalityCode)
                .Distinct()
                .ToListAsync();

            // Get countries that match the used nationality codes
            var countries = await countryRepo
                .GetAllQueryFiltered(x => usedNationalityCodes.Contains(x.Code))
                .ToListAsync();

            return countries
                .OrderBy(x => lang == "ar" ? x.NameAr : x.NameEn)
                .Select(x => new SelectListItemDTO
                {
                    Value = x.Code,
                    Text = lang == "ar" ? x.NameAr : x.NameEn
                })
                .ToList();
        }

        public async Task<List<SelectListItemDTO>> GetDistinctUniversitiesFromScholarshipsAsync(Guid moduleId)
        {
            string lang = _requestInfo.Lang;
            var userId = userInfo.UserId;
            var uow = serviceScopeFactory.CreateScopedUow();

            var isAllowedToViewAll = await SrvPartyType.IsAllowedToViewAllScholarshipWitoutFilterationAsync(userId, moduleId);
            var userPartyData = await SrvPartyType.GetUserPartyTypeData(userId, moduleId);

            List<Guid?> allowedUniversityIds = new();

            if (!isAllowedToViewAll)
            {
                allowedUniversityIds = userPartyData
                    .SelectMany(p => p.PartyTypeCountyUniversity)
                    .Where(x => x.UniversityId.HasValue)
                    .Select(x => x.UniversityId)
                    .Distinct()
                    .ToList();

            }

            var query = uow.GetRepository<ScholarshipData>()
                .GetAllQueryFiltered()
                .Include(x => x.University)
                .Where(s => s.University != null);

            if (!isAllowedToViewAll)
            {
                query = query.Where(s => !allowedUniversityIds.Any() || allowedUniversityIds.Contains(s.UniversityId));
            }

            var universities = await query
                .Select(s => new
                {
                    s.University!.Id,
                    Name = lang == "ar" ? s.University.NameAr : s.University.NameEn,
                    Order = s.University.OrderNo
                })
                .Distinct()
                .OrderBy(u => u.Order)
                .ToListAsync();

            return universities.Select(u => new SelectListItemDTO
            {
                Value = u.Id.ToString(),
                Text = u.Name
            }).ToList();
        }


        public async Task<List<SelectListItemDTO>> GetDistinctMajorsFromScholarshipsAsync(string lang)
        {

            var majors = await serviceScopeFactory.CreateScopedUow()
                                .GetRepository<ScholarshipData>()
                                .GetAllQueryFiltered()
                                .Include(x=>x.Major)
                                .Where(s => s.Major != null)
                                .Select(s => new
                                {
                                    s.Major!.Id,
                                    Name = lang == "ar" ? s.Major.NameAr : s.Major.NameEn, 
                                    order=s.Major.OrderNo,
                                })
                                .Distinct()
                                .OrderBy(m => m.order)
                                .ToListAsync();

            return majors.Select(m => new SelectListItemDTO
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }).ToList();
        }

        public async Task<List<SelectListItemDTO>> GetDistinctParentAcademicDegreesFromScholarshipsAsync(string lang)
        {

            var parentDegrees = await serviceScopeFactory.CreateScopedUow()
                                .GetRepository<ScholarshipData>()
                                .GetAllQueryFiltered()
                                .Include(x => x.ParentAcademicDegree)
                .Where(s => s.ParentAcademicDegree != null && s.ParentAcademicDegree.ParentDegreeId == null)
                .Select(s => new
                {
                    s.ParentAcademicDegree!.Id,
                    Name = lang == "ar" ? s.ParentAcademicDegree.NameAr : s.ParentAcademicDegree.NameEn,
                    order = s.ParentAcademicDegree.OrderNo,
                })
                .Distinct()
                .OrderBy(d => d.order)
                .ToListAsync();

            return parentDegrees.Select(d => new SelectListItemDTO
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToList();
        }

        public async Task<List<SelectListItemDTO>> GetDistinctSubAcademicDegreesFromScholarshipsAsync(string lang)
        {

            var degrees = await serviceScopeFactory.CreateScopedUow()
                                .GetRepository<ScholarshipData>()
                                .GetAllQueryFiltered()
                                .Include(x => x.AcademicDegree)
                .Where(s => s.AcademicDegree != null && s.AcademicDegree.ParentDegreeId != null)
                .Select(s => new
                {
                    s.AcademicDegree!.Id,
                    Name = lang == "ar" ? s.AcademicDegree.NameAr : s.AcademicDegree.NameEn,
                    order=s.AcademicDegree.OrderNo,
                    parent=s.AcademicDegree.ParentDegreeId,

                })
                .Distinct()
                .OrderBy(d => d.Name)
                .ToListAsync();

            return degrees.Select(d => new SelectListItemDTO
            {
                Value = d.Id.ToString(),
                Text = d.Name,
                Parent = d.parent,
            }).ToList();
        }

        public async Task<List<SelectListItemDTO>> GetEnrollmentTypesAsync(string lang)
        {
            var repo = serviceScopeFactory.CreateScopedUow().GetRepository<EnrollmentType>();
            var data = await repo.GetAllQueryFiltered().ToListAsync();

            return data
                .OrderBy(x => lang == "ar" ? x.NameAr : x.NameEn)
                .Select(x => new SelectListItemDTO
                {
                    Value = x.Id.ToString(),
                    Text = lang == "ar" ? x.NameAr : x.NameEn
                })
                .ToList();
        }

        public async Task<List<SelectListItemDTO>> GetDistinctScholarshipProgramsFromScholarshipsAsync(string lang)
        {
            var programs = await serviceScopeFactory.CreateScopedUow()
                                .GetRepository<ScholarshipData>()
                                .GetAllQueryFiltered()
                                .Include(x => x.SchProgram)
                .Where(s => s.SchProgram != null)
                .Select(s => new
                {
                    s.SchProgram!.Id,
                    Name = lang == "ar" ? s.SchProgram.NameAr : s.SchProgram.NameEn,
                 
                })
                .Distinct()
                .OrderBy(p => p.Name)
                .ToListAsync();

            return programs.Select(p => new SelectListItemDTO
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
        }

        public async Task<List<SelectListItemDTO>> GetDistinctFinancialRegulationsFromScholarshipsAsync(string lang)
        {
            var regulations = await serviceScopeFactory.CreateScopedUow()
                                .GetRepository<ScholarshipData>()
                                .GetAllQueryFiltered()
                                .Include(x => x.FinRegulation)
                                .Where(s => s.FinRegulation != null)
                                .Select(s => new
                                {
                                    s.FinRegulation!.Id,
                                    Name = lang == "ar" ? s.FinRegulation.NameAr : s.FinRegulation.NameEn,
                                    
                                })
                                .Distinct()
                                .OrderBy(r => r.Name)
                                .ToListAsync();

            return regulations.Select(r => new SelectListItemDTO
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();
        }

        public async Task<List<SelectListItemDTO>> GetDistinctEntityContractsFromScholarshipsAsync(string lang)
        {

            var contracts = await serviceScopeFactory.CreateScopedUow()
                                .GetRepository<ScholarshipData>()
                                .GetAllQueryFiltered()
                                .Include(x => x.EntityContract)
                .Where(s => s.EntityContract != null)
                .Select(s => new
                {
                    s.EntityContract!.Id,
                    Name = lang == "ar" ? s.EntityContract.NameAr : s.EntityContract.NameEn
                })
                .Distinct()
                .OrderBy(c => c.Name)
                .ToListAsync();

            return contracts.Select(c => new SelectListItemDTO
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }




        #endregion

    }
}
