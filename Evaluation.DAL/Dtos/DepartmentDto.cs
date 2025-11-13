
namespace Evaluation.DAL.Dtos;

public class DepartmentDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? DepIcon { get; set; }
    public string? Desc { get; set; }
    public Guid TypeId { get; set; }
    public string? TypeName { get; set; }
}