using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Masters
{
    public class EvaluationType : EntityBase
    {
        public string BackendName { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public int OrderNo { get; set; } 

    }
}
