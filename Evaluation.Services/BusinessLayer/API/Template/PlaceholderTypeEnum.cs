namespace Evaluation.Services.BusinessLayer.API.Template;

public enum PlaceholderType
{
    Untyped = 0,
    Text = 1,
    Image = 2,
    Table = 3,
    HTMLTable = 4,
    SchoolPeriodicEvaluationCriteria = 5 //Createed to handle {{ParentScopesDetails}} placeholder under Periodic Evaluation Report only
}