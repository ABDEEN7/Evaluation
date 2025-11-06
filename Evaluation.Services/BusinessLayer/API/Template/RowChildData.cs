using Scholarship.DAL.Models.FormBuilder;

namespace Scholarship.Services.BusinessLayer.API.Template;

public record RowChildData(Guid? ChildFieldId, Field? ChildField, List<Dictionary<string, string>> Rows);