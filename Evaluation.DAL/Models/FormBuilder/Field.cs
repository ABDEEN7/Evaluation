using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.ServiceEnities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormBuilder
{
    public class Field : EntityBase, IAuditLogEntity
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public Guid ServiceId { get; set; }
        public Service? Service { get; set; } = null!;
        public string? InfoAr { get; set; }
        public string? InfoEn { get; set; }
        public Guid FieldTypeId { get; set; }
        public FieldType? FieldType { get; set; } = null!;
        public string? Description { get; set; }

        public int Column { get; set; } = 1;
        public int Row { get; set; } = 1;

        // Relationship with FormGroup
        public Guid FormGroupId { get; set; }
        public FormGroup? FormGroup { get; set; } = null!;

        // Alternative relationship with FormGroupList
        public Guid? FormGroupListId { get; set; }
        public FormGroup? FormGroupList { get; set; }

        public string? ClassName { get; set; }
        public Guid? MappingFieldId { get; set; }
        public Field? MappingField { get; set; }
        public Guid? ReadFieldId { get; set; }
        public Field? ReadField { get; set; }

        public Guid? DropDownParentFieldId { get; set; }
        public Field? DropDownParentField { get; set; }
        public Guid? DropDownTypeId { get; set; }
        public DropDownType? DropDownType { get; set; }

		public Guid? FormGroupCustomListId { get; set; }
		public FormGroupCustomList? FormGroupCustomList { get; set; }

		public virtual ICollection<ActionField>? ActionsStepsField { get; set; } = new List<ActionField>();
        public virtual ICollection<FieldAttributeValue>? FieldAttributeValues { get; set; }
        public virtual ICollection<FieldPartyType>? FieldPartyTypes { get; set; }
        public virtual ICollection<FieldViewCondition>? FieldViewConditions { get; set; }
    }
}
