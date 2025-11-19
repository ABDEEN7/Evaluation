using Evaluation.DAL.Models.FormBuilder;

namespace Evaluation.Services.BusinessLayer.API.Template;

public record RowChildData(Guid? ChildFieldId, Field? ChildField, List<Dictionary<string, string>> Rows);