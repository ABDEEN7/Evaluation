using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.IntegrationEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using static Evaluation.DAL.ConstantKeys;
using static Evaluation.SharedHelper.Enums.ConstantKeys;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvField(SrvAttachments SrvAttachments,  IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
    {
	
		public async Task<Field?> GetFieldsByIdsAsync(Guid fieldId)
		{
			using var scope = serviceScopeFactory.CreateScopedUow();
		var fields = await scope
				.GetRepository<Field>()
				.GetAllActiveNonDeleted(x => x.Id == fieldId)
				.Include(x => x.FieldType)
				.Include(x => x.FormGroup)
				.Include(x => x.FieldViewConditions)
				.Include(x => x.FieldAttributeValues)
				.AsSplitQuery()
				.AsNoTracking()
				.FirstOrDefaultAsync();

			return fields;
		}

		public async Task<Field?> GetFieldsByBackendNameAsync(string BackendName)
		{
			using var scope = serviceScopeFactory.CreateScopedUow();

			var fields = await scope
				.GetRepository<Field>()
				.GetAllActiveNonDeleted(x => x.BackendName == BackendName)
				.Include(x => x.FieldType)
				.AsSplitQuery()
				.AsNoTracking()
				.FirstOrDefaultAsync();

			return fields;
		}
		public async Task<Field?> GetSysFieldsByIdsAsync(Guid SysfieldId)
		{
			using var scope = serviceScopeFactory.CreateScopedUow();

			var fields = await scope
				.GetRepository<Field>()
				.GetAllActiveNonDeleted(x => x.Id == SysfieldId)
				.Include(x => x.FieldType)
				.AsNoTracking()
				.FirstOrDefaultAsync();

			return fields;
		}
		public async Task<IList<CssClassesDTO>> GetCssClasses()
		{
			using var scope = serviceScopeFactory.CreateScopedUow();

			var result = await scope.GetRepository<CssClass>().GetAllQueryFiltered().Include(c => c.ApplyType).Select(c => new CssClassesDTO()
			{
				ClassName = c.ClassName,
				Styles = c.Styles,
				Type = c.ApplyType!.BackendName
			}).ToListAsync();
			return result;
		}
		public async Task<List<Guid>> GetHiddenFields(Guid serviceId)
		{
			using var scope = serviceScopeFactory.CreateScopedUow();

			userInfo.UserId = Guid.Parse("C2536611-576B-4EB8-84F4-747F4ECE9A23");
			if (userInfo.UserId == null)
			{
				throw new BusinessException(SharedHelper.Enums.ConstantKeys.ExceptionMessage.UserInfoNotFound);
			}
			if(userInfo.PartyTypes != null)
			{ 
			var userPartyTypeIds = new List<Guid>(userInfo.PartyTypes);

			var hiddenFieldsIds = await scope
				.GetRepository<Field>()
				.GetAllActiveNonDeleted()
				.Include(f => f.FieldPartyTypes)
				.Where(f => f.ServiceId == serviceId && f.FieldPartyTypes!.Any(x => x.IsActive == true && x.IsDeleted == false))
				.Where(f => !f.FieldPartyTypes!
							  .Select(pt => pt.PartyTypeId)
							  .Intersect(userPartyTypeIds)
							  .Any())
				.Select(f => f.Id)
				.ToListAsync();
			
			return hiddenFieldsIds;
			}
			return new List<Guid>();

		}
		public async Task<List<Field>> GetFieldsByActionId(Guid actionId, Guid ServiceId)
		{
			// 1. Try to get the full list from cache
			var data =new List<Field>(); //cacheDataProvider.GetFromCache<List<Field>>(WebAppCacheTableName.CACHE_FIELDS);

			// 2. If cache miss, fetch from DB and cache the result
			//if (data == null)
			//{
				using var scopedUow = serviceScopeFactory.CreateScopedUow();
				var repository = scopedUow.GetRepository<Field>();

				data = await repository.GetAllQueryFiltered()
									.Include(f => f.ActionField)
									.Include(f => f.Service)
				.Include(f => f.FieldType)
				.Include(f => f.FormGroup)
				.Include(f => f.FormGroupList)
				.Include(f => f.FieldViewConditions)
				.Include(f => f.FieldAttributeValues)
				.Include(f => f.MappingField)
				.Where(x => x.ServiceId == ServiceId )
				.Where(X => X.ActionField!.Any(X => X.ServiceActionId == actionId))
				.AsSplitQuery()
				.AsNoTracking()
				.ToListAsync();

				await cacheDataProvider.SetToCache(WebAppCacheTableName.CACHE_FIELDS, data);
			//}

			return data;

				//.Where(f => f.ActionsStepsField.Any(x => x.ServiceActionId == actionId) && f.FormGroup!.FormGroupTypeId != FormGroupTypeKeyIds.List)
				//.ToList();
		}
		public async Task<List<Field>> GetFieldsListByActionIdAsync(Guid? serviceId, Guid? SystemModuleId = null)
		{
			// 1. Try to get the full list from cache
			var data = cacheDataProvider.GetFromCache<List<Field>>(WebAppCacheTableName.CACHE_FIELDS);

			// 2. If cache miss, fetch from DB and cache the result
			if (data == null)
			{
				using var scopedUow = serviceScopeFactory.CreateScopedUow();
				var repository = scopedUow.GetRepository<Field>();

				data = await repository.GetAllQueryFiltered()
									.Include(f => f.ActionField)
									.Include(f => f.Service)
				.Include(f => f.FieldType)
				.Include(f => f.FormGroup)
				.Include(f => f.FormGroupList)
				.Include(f => f.FieldViewConditions)
				.Include(f => f.FieldAttributeValues)
				.Include(f => f.MappingField)
				.AsSplitQuery()
				.AsNoTracking()
				.ToListAsync();

				await cacheDataProvider.SetToCache(WebAppCacheTableName.CACHE_FIELDS, data);
			}
			var FieldsList = data
				 .Where(f => f.ServiceId == serviceId || f.Service!.SystemModuleId == SystemModuleId)
				 .Where(f => f.FormGroup!.FormGroupTypeId == FormGroupTypeKeyIds.List)
				 .ToList();
			return FieldsList;
		}
		public async Task<string?> GenerateJsonSchemaForFormGroupList(Guid? formGroupListId, List<Field> FieldsList, string RenderType = null!, bool schfield = false)
		{
			string lang = _requestInfo.Lang;
			if (formGroupListId is null) return null;


			if (FieldsList.Any())
			{
				var formGroup = FieldsList!.Where(x => x.FormGroupId == formGroupListId).FirstOrDefault()?.FormGroup;

				var schema = new
				{
					Name = lang == "ar" ? formGroup?.TitleAr : formGroup?.TitleEn,
					formGroupName = lang == "ar" ? formGroup?.TitleAr : formGroup?.TitleEn,
					NameAr = formGroup?.TitleAr,
					NameEn = formGroup?.TitleEn,
					Order = formGroup?.Order ?? 9999,
					fields = FieldsList.Where(x => x.FormGroupId == formGroupListId).Select(f => new
					{
						fieldId = (schfield == true ? f.MappingFieldId : f.Id),
						Value = "",
						fieldName = lang == "ar" ? f?.TitleAr : f?.TitleEn,
						fieldBackendName = f!.BackendName,
						fieldNameAr = f.TitleAr,
						fieldNameEn = f.TitleEn,
						fieldTooltipAr = f.InfoAr,
						fieldTooltipEn = f.InfoEn,
						fieldTooltip = "",
						row = f.Row,
						column = f.Column,
						type = (RenderType == "Preview" && f.DropDownTypeId != null) ? "text" : f.FieldType?.BackendName,
						formGroupId = f.FormGroupId,
						formGroupName = "",
						formGroupNameAr = f.FormGroup!.TitleAr,
						formGroupNameEn = f.FormGroup.TitleEn,
						formGroupOrderNo = f.FormGroup.Order,
						dropDownTypeId = f.DropDownTypeId,
						dropDownParentFieldId = f.DropDownParentFieldId,
						Attributes = f.FieldAttributeValues!.Select(attr => new
						{
							Name = attr.AttributeKey,
							Value = attr.AttributeValue,
							message = lang == "ar" ? attr.MessageAr : attr.MessageEn,

						}),
						Conditions = f.FieldViewConditions?.Select(m => new FieldViewConditionDTO
						{
							operators = m.operators,
							FieldValue = m.FieldValue,
							IsSufficient = m.IsSufficient,
							ParentFieldId = m.ParentFieldId
						}).ToList() ?? new List<FieldViewConditionDTO>(),
					}).ToList()
				};

				return JsonConvert.SerializeObject(schema, Formatting.None);
			}
			else
				return null;
		}
		public async Task<List<Field>> GetFieldListByFieldId(Guid FieldId)
		{
			using var _uow = serviceScopeFactory.CreateScopedUow();

			var field = await _uow.GetRepository<Field>()
								 .GetByIDActiveNonDeleted(FieldId);

			if (field == null) return new List<Field>();

			var fieldList = await _uow.GetRepository<Field>()
									 .GetAllQueryFiltered(x => x.FormGroupId == field.FormGroupListId)
									 .Include(x => x.FieldType)
									 .Include(x => x.FieldAttributeValues)
									 .Include(x => x.FieldViewConditions)
									 .Include(x => x.MappingField)
									 .Include(x => x.ReadField)
									 .AsSplitQuery()
									 .ToListAsync();

			return fieldList;
		}
		public List<ExtractedObjectDto> ExtractFieldValues(List<Field> fieldList, string jsonData)
		{
			if (string.IsNullOrEmpty(jsonData) || fieldList == null || !fieldList.Any())
				return new List<ExtractedObjectDto>();

			var jsonDataList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData)
								?? new List<Dictionary<string, object>>();

			var extractedObjects = new List<ExtractedObjectDto>();

			foreach (var entry in jsonDataList)
			{
				var extractedObject = new ExtractedObjectDto();

				foreach (var item in entry)
				{
					if (item.Key.Trim() == "Index")
					{
						// Always include Index
						extractedObject.Fields!.Add(new FieldListValueDto
						{
							FieldBackendName = item.Key,
							Value = item.Value?.ToString() ?? ""
						});
						continue;
					}

					if (!Guid.TryParse(item.Key.ToString(), out var fieldId))
						continue;

					var matchingField = fieldList.FirstOrDefault(f => f.Id == fieldId);
					if (matchingField == null)
						continue;

					string value = item.Value?.ToString() ?? "";

					extractedObject.Fields!.Add(new FieldListValueDto
					{
						FieldBackendName = matchingField.MappingField?.BackendName!,
						Value = value
					});
				}

				extractedObjects.Add(extractedObject);
			}

			return extractedObjects;
		}

		public async Task<List<TEntity>> ReturnEntityListAsync<TEntity>(List<Field> fieldList, string FieldValue, Guid FieldId, string prefix) where TEntity : EntityBase, new()
		{
			//var fieldList = await GetFieldListByFieldId(FieldId);
			var fieldValues = ExtractFieldValues(fieldList, FieldValue);

			var entities = fieldValues
				.AsParallel()
				.Select(dto =>
				{
					if (dto.Fields == null || !dto.Fields.Any()) return null;

					var entity = new TEntity();

					foreach (var field in dto.Fields)
					{
						var backendName = field.FieldBackendName;
						if (backendName != null)
						{
							if (backendName.StartsWith(prefix))
							{
								backendName = backendName.Substring(prefix.Length);
							}

							var property = typeof(TEntity).GetProperty(backendName);
							if (property != null)
							{
								object value = ConvertValueToPropertyType(property.PropertyType, field.Value)!;
								property.SetValue(entity, value);
							}
						}

					}

					var indexField = dto.Fields.FirstOrDefault(f => f.FieldBackendName != null && f.FieldBackendName.Equals("Index", StringComparison.OrdinalIgnoreCase));
					if (indexField != null && !string.IsNullOrEmpty(indexField.Value))
					{
						if (Guid.TryParse(indexField.Value, out var id))
						{
							entity.Id = id;
						}
					}

					return entity;
				})
				.Where(entity => entity != null)
				.ToList();

			return entities!;
		}
		public object? ConvertValueToPropertyType(Type propertyType, string value)
		{
			if (string.IsNullOrEmpty(value)) return null;

			try
			{
				if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
				{
					return DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue)
						? dateValue
						: null;
				}

				if (propertyType == typeof(DateOnly) || propertyType == typeof(DateOnly?))
				{
					return DateOnly.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOnlyValue)
						? dateOnlyValue
						: null;
				}

				if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
					return Guid.TryParse(value, out var guidValue) ? guidValue : null;

				if (propertyType == typeof(bool) || propertyType == typeof(bool?))
					return bool.TryParse(value, out var boolValue) ? boolValue : null;

				if (propertyType == typeof(int) || propertyType == typeof(int?))
					return int.TryParse(value, out var intValue) ? intValue : null;

				if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
				{
					return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue)
						? decimalValue
						: null;
				}
				return value;
			}
			catch
			{
				return null;
			}
		}

		public async Task<List<FieldValueDTO>> ProcessIntegrationFieldsAsync(List<FieldValueDTO> integrationFieldsToProcess,string studentQID,Guid? RequestId,bool isDownload = false)
		{
			if (integrationFieldsToProcess == null || integrationFieldsToProcess.Count == 0)
				return integrationFieldsToProcess!;

			var fieldsByIntegration = new Dictionary<string, List<(FieldValueDTO field, string responseProperty)>>();

			foreach (var field in integrationFieldsToProcess)
			{
				if (field.Attributes == null) continue;

				foreach (var attr in field.Attributes)
				{
					if (attr?.Name?.StartsWith("Integration_") == true)
					{
						string integrationName = attr.Name.Substring("Integration_".Length);
						string responseProperty = attr.Value!;

						if (string.IsNullOrWhiteSpace(responseProperty))
							continue;

						if (!fieldsByIntegration.ContainsKey(integrationName))
							fieldsByIntegration[integrationName] = new List<(FieldValueDTO, string)>();

						fieldsByIntegration[integrationName].Add((field, responseProperty));
					}
				}
			}

			using var httpClient = new HttpClient();

			foreach (var integrationName in fieldsByIntegration.Keys)
			{
				IntegrationPoint? config = await GetIntegrationPointByNameAsync(integrationName);
				if (config == null)
					continue;

				object? integrationResult = null;
				string? jsonResponse = null;

				try
				{
					if (config.IsInternal)
					{
						switch (integrationName)
						{
							// case "NSISServices":
							//     integrationResult = await INSISServices.GetSchoolEnrollmentAsync(studentQID);
							//     break;
							// case "NSISAcademicCertificate":
							//     integrationResult = await INSISServices.GetStudentDetails(studentQID, isDownload);
							//     break;
							// case "NSISFullStudentDetails":
							//     integrationResult = await INSISServices.GetFullStudentDetails(studentQID, isDownload);
							//     break;
							default:
								break;
						}

						jsonResponse = integrationResult != null
							? JsonConvert.SerializeObject(integrationResult)
							: null;
					}
					else
					{
						string url = config.EndPoint!;

						if (!string.IsNullOrEmpty(config.URLParameter))
						{
							string paramName = config.URLParameter;
							string paramValue = studentQID;

							url += url.Contains("?")
								? $"&{paramName}={Uri.EscapeDataString(paramValue)}"
								: $"?{paramName}={Uri.EscapeDataString(paramValue)}";
						}

						var response = await httpClient.GetAsync(url);
						if (!response.IsSuccessStatusCode)
							continue;

						jsonResponse = await response.Content.ReadAsStringAsync();
					}

					if (string.IsNullOrWhiteSpace(jsonResponse))
						continue;

					var jToken = JToken.Parse(jsonResponse);

					foreach (var (field, responseProperty) in fieldsByIntegration[integrationName])
					{
						try
						{
							
							if (field.FormGroupListId != null && field.FormGroupListId != Guid.Empty)
							{
								var arrayToken = jToken.SelectToken(responseProperty, false) as JArray;
								var fieldList = await GetFieldListByFieldId(field.FieldId!.Value);

								if (fieldList.Any() && arrayToken != null && arrayToken.Count > 0)
								{
									var resultList = new List<Dictionary<string, object>>();

									foreach (var item in arrayToken)
									{
										var row = new Dictionary<string, object>();
										var index = Guid.NewGuid();

										foreach (var subField in fieldList)
										{
											var integrationAttr = subField.FieldAttributeValues?
												.FirstOrDefault(attr => attr.AttributeKey.StartsWith("Integration_"));

											if (integrationAttr == null || string.IsNullOrWhiteSpace(integrationAttr.AttributeValue))
												continue;

											var subValue = item.SelectToken(integrationAttr.AttributeValue, false);

											if ((subField.FieldType?.BackendName == FieldTypeConstant.file ||
												 subField.FieldType?.BackendName == FieldTypeConstant.fileV2) && isDownload)
											{
												var base64String = subValue?.ToString();

												if (!string.IsNullOrWhiteSpace(base64String))
												{
													try
													{
														_ = Convert.FromBase64String(base64String);

														var obj = new JObject
														{
															["DocumentBase64"] = base64String,
															["DocumentPath"] = "Document.pdf",
															["ContentType"] = "application/pdf"
														};

														var attachmentId = await SrvAttachments.UploadIntegrationFileAsync(
															field.FieldId, subField.Id, index, obj, RequestId);

														row[subField.Id.ToString()] = attachmentId!;
													}
													catch
													{
														row[subField.Id.ToString()] = null!;
													}
												}
												else
												{
													row[subField.Id.ToString()] = null!;
												}
											}
											else
											{
												row[subField.Id.ToString()] = subValue?.ToString()!;
											}
										}

										row["Index"] = index.ToString();
										resultList.Add(row);
									}

									var serialized = JsonConvert.SerializeObject(resultList);
									field.Value = serialized;
									field.IsApproved = resultList.Any();
								}
								else
								{
									field.Value = "[]";
									field.IsApproved = false;
								}
							}
							else
							{
							
								var scalarToken = jToken.SelectToken(responseProperty, false);

								if ((field.Type == FieldTypeConstant.file || field.Type == FieldTypeConstant.fileV2) && isDownload)
								{
									var base64String = scalarToken?.ToString();

									if (!string.IsNullOrWhiteSpace(base64String))
									{
										try
										{
											_ = Convert.FromBase64String(base64String);

											var obj = new JObject
											{
												["DocumentBase64"] = base64String,
												["DocumentPath"] = "Document.pdf",
												["ContentType"] = "application/pdf"
											};

											var attachmentId = await SrvAttachments.UploadIntegrationFileAsync(
												field.FieldId, null, null, obj, RequestId);

											field.Value = attachmentId;
											field.IsApproved = !string.IsNullOrWhiteSpace(attachmentId);
										}
										catch
										{
											field.Value = null;
											field.IsApproved = false;
										}
									}
									else
									{
										field.Value = null;
										field.IsApproved = false;
									}
								}
								else
								{
									field.Value = scalarToken?.ToString();
									field.IsApproved = !string.IsNullOrWhiteSpace(field.Value);
								}
							}
						}
						catch
						{
							field.IsApproved = false;
						}
					}
				}
				catch
				{
					foreach (var item in fieldsByIntegration[integrationName])
					{
						item.field.IsApproved = false;
					}
				}
			}

			return integrationFieldsToProcess;
		}
		private async Task<IntegrationPoint?> GetIntegrationPointByNameAsync(string integrationName)
		{
			using var scope = serviceScopeFactory.CreateScopedUow();

			return await scope
				.GetRepository<IntegrationPoint>()
				.GetAllActiveNonDeleted(x => x.BackendName == integrationName)
				.FirstOrDefaultAsync();
		}



	}
}
