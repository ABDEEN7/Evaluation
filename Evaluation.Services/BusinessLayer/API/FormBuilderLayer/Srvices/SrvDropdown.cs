using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Extensions;
using Evaluation.Services.Integration;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text.Json;
using static Evaluation.SharedHelper.Enums.ConstantKeys;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvDropdown(IServiceScopeFactory serviceScopeFactory, OrgBL OrgBL,NSISService NSISService, CacheDataProvider cacheDataProvider, UnitOfWork uow,SrvPartyType SrvPartyType,  LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo)
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
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var dropDownsFields = await scopedUow.GetRepository<Field>()
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

        public async Task<List<DropDownValueDTO>> GetDropDownValues(string lang, Guid? EvalId, Guid? SchoolId, List<Guid?>? dropDownTypeIds = null)
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


                        var rawData = await GetDataFromTable(tableName, EvalId, SchoolId, lang);
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



        public async Task<List<DropDownValueDTO>> GetDropDownValuesByDropDownTypeId(Guid dropDownTypeId, Guid? EvalId, Guid? SchoolId, Guid? parentDropDownId = null, Guid? fieldValueId = null)
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
                var rawData = await GetDataFromTable(tableName, EvalId, SchoolId, lang, fieldValueId);

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
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var dropdownValue = await scopedUow
									.GetRepository<FieldDropDownValue>()
                                    .GetByIDNonDeleted(Id);

            return dropdownValue;
        }

        public async Task<List<DropDownValueDTO?>> GetDropDownValuesForAction(Guid? PlanId, Guid? EvalId, Guid actionId, string? dropDownTypeIds, string lang, Guid? requestId)
        {
            var dropDownTypeIdsList = ParseDropDownTypeIds(dropDownTypeIds);

            var allFields = await GetActionFieldsAsync( actionId);

            var tasks = new List<Task<List<DropDownValueDTO>>>
                                {
                                    LoadRegularDropDowns(allFields, dropDownTypeIdsList, EvalId,PlanId, lang),
                                };

            if (requestId.HasValue)
            {
                tasks.Add(LoadLazyDropDownsWithValues(allFields, requestId.Value, EvalId));
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
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			return await scopedUow
				.GetRepository<ActionField>()
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


        private async Task<List<DropDownValueDTO>> LoadRegularDropDowns(List<ActionFieldInfo> fields, List<Guid> excludeDropDownTypeIds, Guid? EvalId, Guid? PlanId, string lang)
        {
            var ids = fields
                .Where(f => !f.IsLazy)
                .SelectMany(f => f.GroupDropDownTypeIds.Concat(f.DropDownTypeId != null ? new[] { f.DropDownTypeId.Value } : Array.Empty<Guid>()))
                .Where(id => !excludeDropDownTypeIds.Contains(id))
                .Distinct()
                .ToList();

            return ids.Any()
                ? await GetDropDownValues(lang, EvalId,  PlanId, ids.Cast<Guid?>().ToList()) ?? new List<DropDownValueDTO>()
                : new List<DropDownValueDTO>();
        }
        private async Task<List<DropDownValueDTO>> LoadLazyDropDownsWithValues(List<ActionFieldInfo> fields, Guid requestId, Guid? EvalId)
        {
            using var uow = serviceScopeFactory.CreateScopedUow();
            var requestValueRepo = uow.GetRepository<ServiceRequestFieldsValue>();

            var lazyFields = fields
                .Where(f => f.IsLazy && f.DropDownTypeId.HasValue)
                .Select(f => new { f.FieldId, DropDownTypeId = f.DropDownTypeId!.Value, DropDownParentFieldId=f.DropDownParentFieldId })
                .ToList();

            var lazyFieldIds = lazyFields.Select(f => f.FieldId).ToList();

            var values = await requestValueRepo.GetAllQueryFiltered()
                .Where(v => v.RefId == requestId && lazyFieldIds.Contains(v.FieldId) && v.Value != null)
                .ToListAsync();

            var result = new List<DropDownValueDTO>();
            foreach (var field in lazyFields)
            {
                var valueEntry = values.FirstOrDefault(x => x.FieldId == field.FieldId);
                var ParentvalueEntry = values.FirstOrDefault(x => x.FieldId == field.DropDownParentFieldId);
                if (valueEntry != null && ParentvalueEntry != null && ParentvalueEntry.Value != null)
                {
                    var valuesList = await GetDropDownValuesByDropDownTypeId(field.DropDownTypeId, EvalId, Guid.Empty,Guid.Parse(ParentvalueEntry!.Value!));
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


        public async Task<DropDownValueDTO?> GetDropDownValue(string lang, Guid? fieldValueId, Guid? dropDownTypeId, Guid? EvalId, Guid? PlanId)
        {
            using var scopedUow = serviceScopeFactory.CreateScopedUow();
            var fieldDropdownRepo = cacheDataProvider.GetFromCache<List<FieldDropDownValue>>(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNVALUE);

            if (fieldDropdownRepo == null)
            {
                var repository = scopedUow.GetRepository<FieldDropDownValue>();

                fieldDropdownRepo = await repository.GetAllQueryFiltered().ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNVALUE, fieldDropdownRepo);
            }
            var dropdownTypeRepo = cacheDataProvider.GetFromCache<List<DropDownType>>(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNTYPE);
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
                var rawData = await GetDataFromTable(tableName, EvalId,PlanId, lang, fieldValueId);

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
        private async Task<List<Dictionary<string, object>>> GetDataFromTable(string tableName, Guid? EvalId, Guid? SchoolId, string lang, Guid? fieldValueId = null)
        {
            
            var allowedTablesSettingValue = await cacheDataProvider.GetSystemSettingValue(SystemSettings.DropDownDataSourceAllowedTables);
            var allowedTables = System.Text.Json.JsonSerializer.Deserialize<List<string>>( string.IsNullOrWhiteSpace(allowedTablesSettingValue) ? "[]" : allowedTablesSettingValue) ?? new();
            using var scopedUow = serviceScopeFactory.CreateScopedUow();

            List<Dictionary<string, object>> result;

            switch (tableName)
            {



				case "CurrentAcademicYear":
					result = await GetCurrentAcademicYearData(fieldValueId);
					break;

				case "LastTwoAcademicYears":
					result = await GetLastTwoAcademicYearsData(fieldValueId);
					break;
				//case "AcademicYearsByYears":
    //                result = await GetAcademicYearsByYears(fieldValueId);
    //                break;
				case "AcademicYear":
					result = await GetAllAcademicYearsData(fieldValueId);
					break;
				case "schoolLevels":
					{
						result = await GetSchoolLevelsData(SchoolId, fieldValueId);
						break;
					}

				case "schoolGrades":
					{
						result = await GetSchoolGradesData(SchoolId, fieldValueId);
						break;
					}

				case "schoolSections":
					{
						result = await GetSchoolSectionsData(SchoolId, fieldValueId);
						break;
					}

				case "schoolGradeSectionSubjects":
					{
						result = await GetSchoolGradeSectionSubjectsData(SchoolId, fieldValueId);
						break;
					}

				case "schoolGradeSectionSubjectTeachers":
					{
						result = await GetSchoolGradeSectionSubjectTeachersData(SchoolId, fieldValueId);
						break;
					}

				case "schoolEmployee":
					{
						SchoolId = Guid.Parse("a297a912-2e70-453c-befc-5dd502cd4894");

						var schoolEmployee = await OrgBL.GetEmployeesBySchoolId(SchoolId.Value);

						result = schoolEmployee?
							.Where(x => fieldValueId == null || x.Id == fieldValueId)
							.Select(x => new Dictionary<string, object>
							{
								["Id"] = x.Id,
								["NameEn"] = string.IsNullOrWhiteSpace(x.JobTitle)
									? x.Name ?? string.Empty
									: $"{x.Name} - {x.JobTitle}",

								["NameAr"] = string.IsNullOrWhiteSpace(x.JobTitle)
									? x.Name ?? string.Empty
									: $"{x.Name} - {x.JobTitle}",

								["OrderNo"] = 0
							})
							.ToList()
							?? new List<Dictionary<string, object>>();

						break;
					}
				case "scops":
					{
						result = await GetScopesData(requestInfo.DepId!.Value,fieldValueId);
						break;
					}
				case "teamMemberScops":
					{
						result = await GetTeamMemberScopesData(fieldValueId);
						break;
					}
				case "teamMember":
					{
						result = await GetTeamMembersData(EvalId.Value, fieldValueId);
						break;
					}
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
		private async Task<List<Dictionary<string, object>>> GetAllAcademicYearsData(Guid? fieldValueId = null)
		{
			var data = cacheDataProvider.GetFromCache<List<AcademicYear>>(
				ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICYEAR);

			using var scopedUow = serviceScopeFactory.CreateScopedUow();
			var repository = scopedUow.GetRepository<AcademicYear>();

			if (data == null)
			{
				data = await repository.GetAllQueryFiltered().ToListAsync();

				await cacheDataProvider.SetToCache(
					ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICYEAR,
					data);
			}

			
				data = data
					.Where(x => x.DepartmentId == requestInfo.DepId!.Value)
					.ToList();
		

			if (fieldValueId.HasValue && fieldValueId != Guid.Empty)
			{
				data = data
					.Where(x => x.Id == fieldValueId.Value)
					.ToList();
			}

			return data
				.OrderByDescending(x => x.StartDate)
				.Select(item => new Dictionary<string, object>
				{
					["Id"] = item.Id,
					["NameAr"] = item.NameAr ?? string.Empty,
					["NameEn"] = item.NameEn ?? string.Empty,
					["StartDate"] = item.StartDate.ToString("yyyy"),
					["EndDate"] = item.EndDate.ToString("yyyy")
				})
				.ToList();
		}
		private async Task<List<Dictionary<string, object>>> GetLastTwoAcademicYearsData(Guid? fieldValueId = null)
		{
			var data = cacheDataProvider.GetFromCache<List<AcademicYear>>(
				ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICYEAR);

			using var scopedUow = serviceScopeFactory.CreateScopedUow();
			var repository = scopedUow.GetRepository<AcademicYear>();

			if (data == null)
			{
				data = await repository.GetAllQueryFiltered().ToListAsync();

				await cacheDataProvider.SetToCache(
					ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICYEAR,
					data);
			}

			
				data = data
					.Where(x => x.DepartmentId == requestInfo.DepId!.Value)
					.ToList();
			

			if (fieldValueId.HasValue && fieldValueId != Guid.Empty)
			{
				data = data
					.Where(x => x.Id == fieldValueId.Value)
					.ToList();
			}
			else
			{
				data = data
					.OrderByDescending(x => x.StartDate)
					.Take(2)
					.ToList();
			}

			return data
				.OrderByDescending(x => x.StartDate)
				.Select(item => new Dictionary<string, object>
				{
					["Id"] = item.Id,
					["NameAr"] = item.NameAr ?? string.Empty,
					["NameEn"] = item.NameEn ?? string.Empty,
					["StartDate"] = item.StartDate.ToString("yyyy"),
					["EndDate"] = item.EndDate.ToString("yyyy")
				})
				.ToList();
		}
		private async Task<List<Dictionary<string, object>>> GetAcademicYearsByYears(List<int> years , Guid? fieldValueId = null)
		{
			if (years == null || !years.Any())
				return new List<Dictionary<string, object>>();

			var data = cacheDataProvider.GetFromCache<List<AcademicYear>>(
				ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICYEAR);

			using var scopedUow = serviceScopeFactory.CreateScopedUow();
			var repository = scopedUow.GetRepository<AcademicYear>();

			if (data == null)
			{
				data = await repository.GetAllQueryFiltered().ToListAsync();

				await cacheDataProvider.SetToCache(
					ConstantKeys.WebAppCacheTableName.CACHE_ACADEMICYEAR,
					data);
			}

			
				data = data
					.Where(x => x.DepartmentId == requestInfo.DepId.Value)
					.ToList();


			if (fieldValueId.HasValue && fieldValueId != Guid.Empty)
			{
				data = data.Where(x => x.Id == fieldValueId).ToList();
			}
			else
			{
				data = data
					.Where(x =>
						years.Contains(x.StartDate.Year) ||
						years.Contains(x.EndDate.Year))
					.ToList();
			}

			

			return data
				.OrderByDescending(x => x.StartDate)
				.Select(item => new Dictionary<string, object>
				{
					["Id"] = item.Id,
					["NameAr"] = item.NameAr ?? string.Empty,
					["NameEn"] = item.NameEn ?? string.Empty,
					["StartDate"] = item.StartDate.ToString("yyyy"),
					["EndDate"] = item.EndDate.ToString("yyyy")
				})
				.ToList();
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

             data = data.Where(a => a.IsCurrent && a.DepartmentId==requestInfo.DepId).ToList();

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
		private async Task<List<Dictionary<string, object>>> GetScopesData(Guid DepId,Guid? fieldValueId = null)
		{
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var repository = scopedUow.GetRepository<ScopeAcademicYear>();

		
			var currentAcademicYear = await scopedUow.GetRepository<AcademicYear>()
				.GetAllQueryFiltered()
				.Where(x => x.IsCurrent && x.DepartmentId== DepId)
				.Select(x => x.Id)
				.FirstOrDefaultAsync();

			if (currentAcademicYear == Guid.Empty)
				return new List<Dictionary<string, object>>();

			IQueryable<ScopeAcademicYear> query = repository.GetAllQueryFiltered()
				.Include(x => x.Scope);


			if (fieldValueId.HasValue && fieldValueId != Guid.Empty)
			{
				query = query.Where(x => x.ScopeId == fieldValueId);
			}
			else
			{
				query = query.Where(x => x.AcademicYearId == currentAcademicYear);
			}

			var data = await query.ToListAsync();

			if (data == null || !data.Any())
				return new List<Dictionary<string, object>>();

			return data
				.Where(x => x.Scope != null)
				.OrderBy(x => x.Scope.OrderNo)
				.Select(x => new Dictionary<string, object>
				{
					["Id"] = x.Scope!.Id,
					["NameAr"] = x.Scope.NameAr ?? string.Empty,
					["NameEn"] = x.Scope.NameEn ?? string.Empty,
					["OrderNo"] = x.Scope.OrderNo,
					["ColorCode"] = x.Scope.ColorCode ?? ""
				})
				.ToList();
		}

		private async Task<List<Dictionary<string, object>>> GetTeamMemberScopesData(Guid? fieldValueId = null)
		{
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var query = scopedUow.GetRepository<EvaluationRequestAssignment>()
				.GetAllQueryFiltered()
				.Where(x => x.MinistryUserId == userInfo.UserId)
				.SelectMany(x => x.EvalRequestAssignmentScopies!)
				.Where(x => x.Scope != null);

			if (fieldValueId.HasValue && fieldValueId.Value != Guid.Empty)
			{
				query = query.Where(x => x.ScopeId == fieldValueId.Value);
			}

			var scopes = await query
	                        .Select(x => x.Scope!)
	                        .Distinct()
	                        .OrderBy(x => x.OrderNo)
	                        .ToListAsync();

			var data = scopes
				.Select(x => new Dictionary<string, object>
				{
					["Id"] = x.Id,
					["NameAr"] = x.NameAr ?? string.Empty,
					["NameEn"] = x.NameEn ?? string.Empty,
					["OrderNo"] = x.OrderNo,
					["ColorCode"] = x.ColorCode ?? string.Empty
				})
				.ToList();

			return data;
		}
		private async Task<List<Dictionary<string, object>>> GetTeamMembersData(Guid? evaluationRequestId,Guid? fieldValueId = null)
		{
			if (evaluationRequestId == Guid.Empty)
				return new List<Dictionary<string, object>>();

			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var repository = scopedUow.GetRepository<EvaluationRequestAssignment>();

			IQueryable<EvaluationRequestAssignment> query = repository
				.GetAllQueryFiltered()
				.Include(x => x.MinistryUser)
				.Where(x => x.EvaluationRequestId == evaluationRequestId );

			if (fieldValueId.HasValue && fieldValueId != Guid.Empty)
			{
				query = query.Where(x => x.MinistryUserId == fieldValueId.Value);
			}

			var data = await query
				.OrderByDescending(x => x.IsLeader)
				.ThenBy(x => x.MinistryUser!.NameAr)
				.Select(x => new
				{
					Id = x.MinistryUser!.Id,
					NameAr = x.MinistryUser.NameAr ?? string.Empty,
					NameEn = x.MinistryUser.NameEn ?? string.Empty,
					IsLeader = x.IsLeader,
					IsNDA = x.IsNDA,
					PartyTypeId = x.PartyTypeId,
					OrderNo = x.IsLeader ? 0 : 1
				})
				.ToListAsync();

			return data.Select(x => new Dictionary<string, object>
			{
				["Id"] = x.Id,
				["NameAr"] = x.NameAr,
				["NameEn"] = x.NameEn,
				["IsLeader"] = x.IsLeader,
				["IsNDA"] = x.IsNDA,
				["PartyTypeId"] = x.PartyTypeId,
				["OrderNo"] = x.OrderNo
			}).ToList();
		}

		public async Task<string> ResolveDropDownTextAsync(string lang, string rawValue, Guid dropDownTypeId, Guid? EvalId, Guid? PlanId)
		{
			if (Guid.TryParse(rawValue, out var singleId) && singleId != Guid.Empty)
			{
				var v = await GetDropDownValue(lang, singleId, dropDownTypeId, EvalId, PlanId);
				if (v != null) return lang == "ar" ? v.TitleAr ?? "" : v.TitleEn ?? "";
				return rawValue;
			}
			var ids = TryParseGuidList(rawValue);
			if (ids.Count == 0) return rawValue ?? string.Empty;

			var titles = new List<string>(ids.Count);
			foreach (var id in ids)
			{
				var v = await GetDropDownValue(lang, id, dropDownTypeId, EvalId, PlanId);
				titles.Add(v != null ? (lang == "ar" ? v.TitleAr ?? "" : v.TitleEn ?? "") : id.ToString());
			}
			var sep = lang == "ar" ? "، " : ", ";
			return string.Join(sep, titles.Where(t => !string.IsNullOrWhiteSpace(t)));
		}
		private static List<Guid> TryParseGuidList(string? raw)
		{
			var result = new List<Guid>();
			if (string.IsNullOrWhiteSpace(raw)) return result;
			try
			{
				var asArray = JsonConvert.DeserializeObject<List<string>>(raw!);
				if (asArray != null && asArray.Count > 0)
				{
					foreach (var s in asArray)
						if (Guid.TryParse(s, out var g)) result.Add(g);
					if (result.Count > 0) return result;
				}
			}
			catch { }

			var parts = raw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			foreach (var p in parts)
				if (Guid.TryParse(p, out var g)) result.Add(g);

			return result;
		}
		private async Task<List<Dictionary<string, object>>> GetSchoolLevelsData(Guid? schoolId,Guid? fieldValueId = null)
		{
			if (!schoolId.HasValue || schoolId == Guid.Empty)
				return new();

			using var uow = serviceScopeFactory.CreateScopedUow();

			var query = uow.GetRepository<SchoolLevel>()
				.GetAllQueryFiltered()
				.Include(x => x.EducationLevel)
				.Where(x => x.SchoolId == schoolId.Value);

			if (fieldValueId.HasValue && fieldValueId != Guid.Empty)
				query = query.Where(x => x.Id == fieldValueId.Value);

			var data = await query.ToListAsync();

			return data
				.Where(x => x.EducationLevel != null)
				.Select(x => new Dictionary<string, object>
				{
					["Id"] = x.Id,
					["NameAr"] = x.EducationLevel!.NameAr,
					["NameEn"] = x.EducationLevel!.NameEn,
					["OrderNo"] = x.EducationLevel.OrderNo
				})
				.ToList();
		}
		private async Task<List<Dictionary<string, object>>> GetSchoolGradesData(Guid? schoolId,Guid? fieldValueId = null)
		{
			if (!schoolId.HasValue || schoolId == Guid.Empty)
				return new();

			using var uow = serviceScopeFactory.CreateScopedUow();

			var query = uow.GetRepository<SchoolGrade>()
				.GetAllQueryFiltered()
				.Include(x => x.GradeLevel)
				.Include(x => x.SchoolLevel)
				.Where(x => x.SchoolLevel!.SchoolId == schoolId.Value);

			if (fieldValueId.HasValue && fieldValueId != Guid.Empty)
				query = query.Where(x => x.Id == fieldValueId.Value);

			var data = await query.ToListAsync();

			return data
				.Where(x => x.GradeLevel != null)
				.Select(x => new Dictionary<string, object>
				{
					["Id"] = x.Id,
					["NameAr"] = x.GradeLevel!.NameAr,
					["NameEn"] = x.GradeLevel!.NameEn,
					["OrderNo"] = x.GradeLevel.orderNo,
					["ParentDropDownId"] = x.SchoolLevelId
				})
				.ToList();
		}
		private async Task<List<Dictionary<string, object>>> GetSchoolSectionsData(Guid? schoolId,Guid? fieldValueId = null)
		{
			if (!schoolId.HasValue || schoolId == Guid.Empty)
				return new();

			using var uow = serviceScopeFactory.CreateScopedUow();

			var query = uow.GetRepository<SchoolGradeSection>()
				.GetAllQueryFiltered()
				.Include(x => x.SchoolGrade)
					.ThenInclude(x => x.SchoolLevel)
				.Where(x => x.SchoolGrade!.SchoolLevel!.SchoolId == schoolId.Value);

			if (fieldValueId.HasValue && fieldValueId != Guid.Empty)
				query = query.Where(x => x.Id == fieldValueId.Value);

			var data = await query.ToListAsync();

			return data.Select(x => new Dictionary<string, object>
			{
				["Id"] = x.Id,
				["NameAr"] = x.SectionAr,
				["NameEn"] = x.SectionEn,
				["OrderNo"] = 0,
				["ParentDropDownId"] = x.SchoolGradeId
			}).ToList();
		}
		private async Task<List<Dictionary<string, object>>> GetSchoolGradeSectionSubjectsData(Guid? schoolId,Guid? fieldValueId = null)
		{
			if (!schoolId.HasValue || schoolId == Guid.Empty)
				return new();

			using var uow = serviceScopeFactory.CreateScopedUow();

			var query = uow.GetRepository<SchoolGradeSectionCourse>()
				.GetAllQueryFiltered()
				.Include(x => x.SchoolCourse)
				.Include(x => x.SchoolGradeSction)
					.ThenInclude(x => x.SchoolGrade)
						.ThenInclude(x => x.SchoolLevel)
				.Where(x => x.SchoolGradeSction!.SchoolGrade!.SchoolLevel!.SchoolId == schoolId.Value);

			var data = await query.ToListAsync();

			return data
				.Where(x => x.SchoolCourse != null)
				.GroupBy(x => x.SchoolCourseId)
				.Select(g =>
				{
					var item = g.First();

					return new Dictionary<string, object>
					{
						["Id"] = item.SchoolCourseId,
						["NameAr"] = item.SchoolCourse!.NameAr,
						["NameEn"] = item.SchoolCourse!.NameEn,
						["OrderNo"] = item.SchoolCourse.OrderNo,
						["ParentDropDownId"] = item.SchoolGradeSctionId,

					};
				})
				.ToList();
		}

		private async Task<List<Dictionary<string, object>>> GetSchoolGradeSectionSubjectTeachersData(Guid? schoolId,Guid? schoolCourseId,Guid? fieldValueId = null)
		{
			if (!schoolId.HasValue || schoolId == Guid.Empty)
				return new();

			if (!schoolCourseId.HasValue || schoolCourseId == Guid.Empty)
				return new();

			using var uow = serviceScopeFactory.CreateScopedUow();

			var query = uow.GetRepository<SchoolGradeSectionCourse>()
				.GetAllQueryFiltered()
				.Where(x => x.SchoolCourseId == schoolCourseId.Value);

			if (fieldValueId.HasValue && fieldValueId != Guid.Empty)
				query = query.Where(x => x.Id == fieldValueId.Value);

			var sectionCourses = await query.ToListAsync();

			var schoolEmployees = await OrgBL.GetEmployeesBySchoolId(schoolId.Value);

			var employeeMap = schoolEmployees
				.Where(x => !string.IsNullOrWhiteSpace(x.QID))
				.GroupBy(x => x.QID!.Trim())
				.ToDictionary(g => g.Key, g => g.First());

			return sectionCourses
				.Where(x => !string.IsNullOrWhiteSpace(x.QID))
				.Where(x => employeeMap.ContainsKey(x.QID.Trim()))
				.GroupBy(x => x.QID.Trim())
				.Select(g =>
				{
					var item = g.First();
					var emp = employeeMap[item.QID.Trim()];

					return new Dictionary<string, object>
					{
						["Id"] = item.Id,
						["NameAr"] = emp.NameAr ?? item.QID,
						["NameEn"] = emp.NameEn ?? item.QID,
						["OrderNo"] = 0,
						["ParentDropDownId"] = item.SchoolCourseId
					};
				})
				.ToList();
		}
	}
}
