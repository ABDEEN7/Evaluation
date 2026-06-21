using Evaluation.DAL.Dtos.Form;

namespace Evaluation.SharedHelper.Dtos.Form;

public class ScopeTreeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string ScopeTypeName { get; set; } = null!;
    public int OrderNo { get; set; }
    public Guid ScopeTypeId { get; set; }

    public string? ColorCode { get; set; }
    public List<ScopeTreeDto> Children { get; set; } = new();
    public List<FormItemDto> Items { get; set; } = new();
}

public class ScopeReportNoteDto
{
	public Guid ScopeId { get; set; }

	public string ScopeName { get; set; } = null!;

	public string PositivePoint { get; set; } = null!;

	public string NegativePoint { get; set; } = null!;

	public string? Note { get; set; }
}