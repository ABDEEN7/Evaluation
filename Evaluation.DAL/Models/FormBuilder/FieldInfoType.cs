using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.FormBuilder
{
	public class FieldInfoType : EntityBase, IAuditLogEntity
	{
		public string NameAr { get; set; } = null!;
		public string NameEn { get; set; } = null!;
		public string BackendName { get; set; } = null!;
		public int? OrderNo { get; set; }
		public Guid SystemModuleId { get; set; }
		public SystemModule? SystemModule { get; set; }
	}
}
