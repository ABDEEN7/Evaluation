using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.PermissionEntity;


namespace Evaluation.DAL.Models.Website;

	public class SideBar : EntityBase, IAuditLogEntity
	{
		public string NameAr { get; set; } = null!;
		public string NameEn { get; set; } = null!;
		public Guid? PermissionId { get; set; } = null;
		public Permission? Permission { get; set; } = null;
		public Guid? ParentId { get; set; }
		public SideBar? Parent { get; set; }
		public string? Icon { get; set; }
		public string RoutingPath { get; set; } = null!;
		public int OrderNo { get; set; }
	}

