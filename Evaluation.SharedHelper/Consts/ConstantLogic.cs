using Evaluation.SharedHelper.Dtos.AcademicYearDto;

namespace Evaluation.SharedHelper.Consts;

public static class ConstantLogic
{
    public static int? ResolveAcademicYear(DateOnly? nextEvaluationDate, List<AcademicYearLite> years)
    {
        if (nextEvaluationDate == null)
            return null;
        var match = years.FirstOrDefault(a => nextEvaluationDate >= a.StartDate
        && nextEvaluationDate <= a.EndDate);
        return match?.Year;
    }
}