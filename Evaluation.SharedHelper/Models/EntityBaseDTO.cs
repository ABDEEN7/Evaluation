using Evaluation.SharedHelper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models
{
    public abstract class EntityTableDTO
    {
        public string? CreateById { get; set; }
        public string? CreateBy { get; set; }
        public string? CreateDate { get; set; }

        public string? UpdateById { get; set; }
        public string? UpdateBy { get; set; }
        public string? UpdateDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DBResult ResponseStatus { get; set; }
        public string? ResponseMessage { get; set; }
        public bool? ResponseState { get; set; }
        public Guid ServiceTypeId { get; set; }
    }
    public class EntityBaseDTO : EntityTableDTO
    {
        public Guid? Id { get; set; }
    }
}
