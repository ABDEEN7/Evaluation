namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class BaseDto
{
    public Guid Id { get; set; }
    public bool? IsActive { get; set; }
    public Guid? CreateById { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid? UpdateById { get; set; }
    public DateTime? UpdateDate { get; set; }
    public Guid? DeleteById { get; set; }
    public bool? IsDeleted { get; set; }
}
