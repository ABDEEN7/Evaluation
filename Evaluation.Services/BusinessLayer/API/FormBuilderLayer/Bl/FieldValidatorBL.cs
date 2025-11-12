using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.FormBuilder;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scholarship.Services.BusinessLayer.API
{
#pragma warning disable S2325
    public class FieldValidatorBL(CacheDataProvider cacheDataProvider)
        {
		public IList<string> ValidateRequired(FieldValueDTO field, List<FieldAttributeValue> attributes, string lang, IList<FileFieldDTO> files = null!)
		{
			if (field.Type == ConstantKeys.FieldTypeConstant.file || field.Type == ConstantKeys.FieldTypeConstant.fileV2)
			{

				var requiredErrors = new List<string>();
				var value = files.FirstOrDefault(x => x.FieldId == field.FieldId);
				if (attributes.Any(attr => attr.AttributeKey.ToLower() == "required") && (field.Value == null && value == null))
				{
					requiredErrors.Add(attributes.Where(attr => attr.AttributeKey.ToLower() == "required").Select(c => lang == "ar" ? c.MessageAr : c.MessageEn).FirstOrDefault() + field.FieldId);
				}

				return requiredErrors;
			}
			else
			{
				var requiredErrors = new List<string>();
				if (attributes.Any(attr => attr.AttributeKey.ToLower() == "required") && (field.Value == null || field.Value == "[]" || string.IsNullOrWhiteSpace(field.Value)))
				{

					requiredErrors.Add(attributes.Where(attr => attr.AttributeKey!.ToLower() == "required").Select(c => lang == "ar" ? c.MessageAr : c.MessageEn).FirstOrDefault()!);
				}

				return requiredErrors;
			}

		}
		internal IList<string> ValidateNumber(FieldValueDTO field, List<FieldAttributeValue> attributes, string lang)
		{
			var numberErrors = new List<string>();
			if (!string.IsNullOrEmpty(field?.Value))
			{
				var numericValue = float.TryParse(field?.Value, out var result) ? result : float.NaN;
				var stringValue = field?.Value ?? string.Empty; // Get the textual representation of the number.

				if (!float.IsNaN(numericValue))
				{
					foreach (var attr in attributes)
					{
						var attrName = attr.AttributeKey.ToLower();
						var errorMessage = lang == "ar" ? attr.MessageAr : attr.MessageEn;
						switch (attrName)
						{
							case "min":
								if (numericValue < float.Parse(attr.AttributeValue!))
								{
									numberErrors.Add($"{field!.FieldName}: {errorMessage}");
								}
								break;
							case "max":
								if (numericValue > float.Parse(attr.AttributeValue!))
								{
									numberErrors.Add($"{field!.FieldName}: {errorMessage}");
								}
								break;
							case "minlength":
								if (stringValue.Length < int.Parse(attr.AttributeValue!))
								{
									numberErrors.Add($"{field!.FieldName}: {errorMessage}");
								}
								break;
							case "maxlength":
								if (stringValue.Length > int.Parse(attr.AttributeValue!))
								{
									numberErrors.Add($"{field!.FieldName}: {errorMessage}");
								}
								break;

						}
					}
				}
				else
				{
					numberErrors.Add($"{field?.FieldName}: Invalid number format.");
				}
			}
			return numberErrors;

		}
		internal IList<string> ValidateMaxLength(FieldValueDTO field, List<FieldAttributeValue> attributes, string lang)
		{
			var maxLengthAttribute = attributes.FirstOrDefault(attr => attr.AttributeKey.ToLower() == "maxlength");
			var maxLengthErrors = new List<string>();

			if (maxLengthAttribute != null && field?.Value?.Length > int.Parse(maxLengthAttribute.AttributeValue!))
			{
				var errorMessage = lang == "ar" ? maxLengthAttribute.MessageAr : maxLengthAttribute.MessageEn;
				maxLengthErrors.Add($"{field.FieldName}: {errorMessage}");
			}
			return maxLengthErrors;
		}
		internal IList<string> ValidateMinLength(FieldValueDTO field, List<FieldAttributeValue> attributes, string lang)
		{
			var minLengthAttribute = attributes.FirstOrDefault(attr => attr.AttributeKey.ToLower() == "minlength");
			var minLengthErrors = new List<string>();

			if (minLengthAttribute != null && field?.Value?.Length < int.Parse(minLengthAttribute.AttributeValue!))
			{
				var errorMessage = lang == "ar" ? minLengthAttribute.MessageAr : minLengthAttribute.MessageEn;
				minLengthErrors.Add($"{field.FieldName}: {errorMessage}");
			}

			return minLengthErrors;
		}
		internal IList<string> ValidateDate(FieldValueDTO field, List<FieldAttributeValue> attributes, string lang)
		{

			var dateErrors = new List<string>();
			if ((field?.Type == "date" || field?.Type == "datetime") && field?.Value != null)
			{
				DateTime? currentDate = null;

				if (field?.Type == "date")
				{
					DateTime.TryParseExact(field.Value, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result);
					currentDate = result;
				}

				if (field?.Type == "datetime")
				{
					DateTime.TryParseExact(field.Value, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime result);
					currentDate = result;
				}

				if (currentDate != null)
				{
					foreach (var attr in attributes)
					{
						var attrName = attr.AttributeKey.ToLower();
						var errorMessage = lang == "ar" ? attr.MessageAr : attr.MessageEn;
						switch (attrName)
						{
							case "mindate":
								if (currentDate < DateTime.Parse(attr.AttributeValue!))
								{
									dateErrors.Add($"{field!.FieldName}: {errorMessage}");
								}
								break;
							case "maxdate":
								if (currentDate > DateTime.Parse(attr.AttributeValue!))
								{
									dateErrors.Add($"{field!.FieldName}: {errorMessage}");
								}
								break;
							case "maxdaysfromnow":

								if (field!.IsEditable == false || field!.IsApproved == true || attributes.Any(x => x.AttributeKey == "disabled") || attributes.Any(x => x.AttributeKey == "ReadOnly"))
									continue;
								var maxDaysFromNow = int.Parse(attr.AttributeValue!);
								var maxDateFromNow = DateTime.Now.AddDays(maxDaysFromNow);

								if (currentDate > maxDateFromNow)
								{
									dateErrors.Add($"{field.FieldName}: {errorMessage}");
								}
								break;
							case "mindaysfromnow":
								if (field!.IsEditable == false || field!.IsApproved == true || attributes.Any(x => x.AttributeKey == "disabled") || attributes.Any(x => x.AttributeKey == "ReadOnly"))
									continue;

								var minDaysFromNow = int.Parse(attr.AttributeValue!);
								var minDateFromNow = DateTime.Now.AddDays(minDaysFromNow);

								if (currentDate < minDateFromNow)
								{
									dateErrors.Add($"{field.FieldName}: {errorMessage}");
								}
								break;
							case "minage":
								var minage = int.Parse(attr.AttributeValue!);
								var now = DateTime.Now.AddYears(-minage);

								now = now.Date;
								currentDate = currentDate.Value.Date;
								if (currentDate > now)
								{
									dateErrors.Add($"{field!.FieldName}: {errorMessage}");
								}
								break;
						}
					}

				}
			}
			return dateErrors;
		}
		internal IList<string> ValidateRegex(FieldValueDTO field, List<FieldAttributeValue> attributes, string lang)
		{
			var regexErrors = new List<string>();
			var regexAttribute = attributes.FirstOrDefault(attr => attr.AttributeKey.ToLower() == "regex" && attr.FieldId == field.FieldId);
			if (regexAttribute != null && !string.IsNullOrWhiteSpace(field?.Value))
			{
				var regexPattern = new Regex(regexAttribute.AttributeValue!);
				if (!regexPattern.IsMatch(field.Value))
				{
					var errorMessage = lang == "ar" ? regexAttribute.MessageAr : regexAttribute.MessageEn;
					regexErrors.Add($"{field.FieldName}: {errorMessage}");
				}
			}
			return regexErrors;
		}
		public IList<string> ValidateFile(FieldValueDTO field, IList<FileFieldDTO> files, List<FieldAttributeValue> attributes, string lang)
		{
			var errors = new List<string>();

			var acceptAttribute = attributes.FirstOrDefault(attr => attr.AttributeKey.ToLower() == "accept");
			var requiredAttribute = attributes.FirstOrDefault(attr => attr.AttributeKey.ToLower() == "required");
			var sizeAttribute = attributes.FirstOrDefault(attr => attr.AttributeKey.ToLower() == "data-max-size");
			var file = files.FirstOrDefault(x => x.FieldId == field.FieldId);
			if (file != null)
			{
				if (acceptAttribute != null && !string.IsNullOrEmpty(acceptAttribute.AttributeValue))
				{

					var allowed_extensions = acceptAttribute.AttributeValue.ToLower().Trim().Replace(".", "").Replace(" ", "").Trim().Split(new char[] { ',' }).ToList();

					var extension = System.IO.Path.GetExtension(file.File.FileName).Replace(".", "").ToLower();
					if (!allowed_extensions.Any(x => x == extension))
					{
						var errorMessage = lang == "ar" ? acceptAttribute.MessageAr : acceptAttribute.MessageEn;
						errors.Add($"{field.FieldName}: {errorMessage}");
					}
				}


				if (sizeAttribute != null && !string.IsNullOrEmpty(sizeAttribute.AttributeValue) && long.TryParse(sizeAttribute.AttributeValue, out long size))
				{
					var fileSize = file.File.Length;//bytes
					if (fileSize > size)
					{
						var errorMessage = lang == "ar" ? sizeAttribute.MessageAr : sizeAttribute.MessageEn;
						errors.Add($"{field.FieldName}: {errorMessage}");
					}
				}

			}
			else
			{
				if (requiredAttribute != null && field.Value == null)
				{
					var errorMessage = lang == "ar" ? requiredAttribute.MessageAr : requiredAttribute.MessageEn;
					errors.Add($"{field.FieldName}: {errorMessage}");
				}
			}

			return errors;
		}
		public List<string> ValidateFileForDraft(List<FileFieldDTO> fileFields, List<FieldAttributeValue> attributes, string lang)
		{
			var errors = new List<string>();

			foreach (var field in fileFields)
			{
				if (field.File.Length > 0)
				{

					var acceptAttribute = attributes.FirstOrDefault(attr => attr.FieldId == field.FieldId && attr.AttributeKey.ToLower() == "accept");
					var requiredAttribute = attributes.FirstOrDefault(attr => attr.FieldId == field.FieldId && attr.AttributeKey.ToLower() == "required");
					var sizeAttribute = attributes.FirstOrDefault(attr => attr.FieldId == field.FieldId && attr.AttributeKey.ToLower() == "data-max-size");

					var blockedExtensions = new List<string> { "exe", "txt" };
					var extension = System.IO.Path.GetExtension(field.File.FileName).Replace(".", "");
					if (blockedExtensions.Contains(extension))
					{
						var msg = lang == "ar"
							? "هذا النوع من الملفات غير مسموح به (EXE/TXT)."
							: "This file type is not allowed (EXE/TXT).";
						errors.Add($"{field.File.FileName}: {msg}");
						return errors;
					}
					if (acceptAttribute != null && !string.IsNullOrEmpty(acceptAttribute.AttributeValue))
					{

						var allowed_extensions = acceptAttribute.AttributeValue.Trim().Replace(".", "").Replace(" ", "").Trim().Split(new char[] { ',' }).ToList();
						if (!allowed_extensions.Any(x => x == extension))
						{
							var errorMessage = lang == "ar" ? acceptAttribute.MessageAr : acceptAttribute.MessageEn;
							errors.Add($"{field.File.FileName}: {errorMessage}");
						}
					}


					if (sizeAttribute != null && !string.IsNullOrEmpty(sizeAttribute.AttributeValue) && long.TryParse(sizeAttribute.AttributeValue, out long size))
					{
						var fileSize = field.File.Length;//bytes
						if (fileSize > size)
						{
							var errorMessage = lang == "ar" ? sizeAttribute.MessageAr : sizeAttribute.MessageEn;
							errors.Add($"{field.File.FileName}: {errorMessage}");
						}
					}


					if (requiredAttribute != null && field.File.Length == 0)
					{
						var errorMessage = lang == "ar" ? requiredAttribute.MessageAr : requiredAttribute.MessageEn;
						errors.Add($"{field.File.FileName}: {errorMessage}");
					}
				}
			}

			return errors;
		}
	
		internal async Task<IList<string>> ValidateList(FieldValueDTO field, IList<FileFieldDTO> _files, List<FieldAttributeValue> attributes, string lang)
		{
			var errors = new List<string>();

			if (string.IsNullOrWhiteSpace(field?.Value))
				return errors;

			var items = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(field.Value ?? "[]")
						?? new List<Dictionary<string, object>>();

			int totalCount = items.Count;
			int newCount = items.Count(d => !d.ContainsKey("IsOld") || d["IsOld"]?.ToString()?.ToLower() != "true");

			int? minCount = int.TryParse(attributes.FirstOrDefault(a => a.AttributeKey?.Equals("mincount", StringComparison.OrdinalIgnoreCase) == true)?.AttributeValue, out var mc) ? mc : (int?)null;
			int? maxCount = int.TryParse(attributes.FirstOrDefault(a => a.AttributeKey?.Equals("maxcount", StringComparison.OrdinalIgnoreCase) == true)?.AttributeValue, out var xc) ? xc : (int?)null;
			int? minCountNew = int.TryParse(attributes.FirstOrDefault(a => a.AttributeKey?.Equals("mincountnew", StringComparison.OrdinalIgnoreCase) == true)?.AttributeValue, out var mcn) ? mcn : (int?)null;
			int? maxCountNew = int.TryParse(attributes.FirstOrDefault(a => a.AttributeKey?.Equals("maxcountnew", StringComparison.OrdinalIgnoreCase) == true)?.AttributeValue, out var xcn) ? xcn : (int?)null;

			string lblMinNew = await cacheDataProvider.GetExceptionMessage(ConstantKeys.ExceptionMessage.lblMinimumNewRowsRequired, lang)
								?? (lang == "ar" ? "يجب إدخال الحد الأدنى لعدد الصفوف الجديدة." : "Minimum number of new rows required.");
			string lblMaxNew = await cacheDataProvider.GetExceptionMessage(ConstantKeys.ExceptionMessage.lblMaximumNewRowsExceeded, lang)
								?? (lang == "ar" ? "تم تجاوز الحد الأقصى لعدد الصفوف الجديدة." : "Maximum number of new rows exceeded.");
			string lblMinAll = await cacheDataProvider.GetExceptionMessage(ConstantKeys.ExceptionMessage.lblMinimumRowsRequired, lang)
								?? (lang == "ar" ? "يجب إدخال حد أدنى من الصفوف." : "Minimum total rows required.");
			string lblMaxAll = await cacheDataProvider.GetExceptionMessage(ConstantKeys.ExceptionMessage.lblMaximumRowsExceeded, lang)
								?? (lang == "ar" ? "تم تجاوز الحد الأقصى لإجمالي الصفوف." : "Maximum total rows exceeded.");

			if (minCount.HasValue && totalCount < minCount.Value)
				errors.Add($"{field.FieldName}: {lblMinAll} ({minCount.Value})");

			if (maxCount.HasValue && totalCount > maxCount.Value)
				errors.Add($"{field.FieldName}: {lblMaxAll} ({maxCount.Value})");

			if (minCountNew.HasValue && newCount < minCountNew.Value)
				errors.Add($"{field.FieldName}: {lblMinNew} ({minCountNew.Value})");

			if (maxCountNew.HasValue && newCount > maxCountNew.Value)
				errors.Add($"{field.FieldName}: {lblMaxNew} ({maxCountNew.Value})");

			return errors;
		}

	}


}
