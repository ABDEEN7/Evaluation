using Evaluation.DAL.Models.Calendars;

namespace Evaluation.DAL.Dtos;

public class SemesterDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<SemesterDto> ConvertSemesterToDto(List<Semester> model)
    {
        return model.Select(e => new SemesterDto
        {
            Id = e.Id,
            Name = e.NameEn,
            StartDate = e.StartDate,
            EndDate = e.EndDate
        }).ToList();
    }
}
