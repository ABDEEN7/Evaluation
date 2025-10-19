using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Authentication;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.DepartementEntites;
using Evaluation.DAL.Entities.FormsModules;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Entities.PermissionEntity;
using Evaluation.DAL.Entities.Planing;
using Evaluation.DAL.Entities.Template;
using Evaluation.DAL.Entities.UserEntiy;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Context;

public partial class EvaluationDbContext : DbContext
{

    public EvaluationDbContext(DbContextOptions<EvaluationDbContext> options)
        : base(options)
    {
    }
    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    builder.ApplyConfigurationsFromAssembly(typeof(EvaluationDbContext).Assembly);
    //}

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<ControlValidation> ControlValidations { get; set; }
    public virtual DbSet<Page> Pages { get; set; }
    public virtual DbSet<PagePermission> PagePermissions { get; set; }
    public virtual DbSet<PartyType> PartyTypes { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<RolePermission> RolePermissions { get; set; }
    public virtual DbSet<MinistryUser> MinistryUsers { get; set; }
    public virtual DbSet<UserPartyType> UserPartyTypes { get; set; }
    public virtual DbSet<UserPartyTypeSignature> UserPartyTypeSignatures { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<AcademicYear> AcademicYears { get; set; }
    public virtual DbSet<CalcMethod> CalcMethods { get; set; }
    public virtual DbSet<EvaluationParty> EvaluationParties { get; set; }
    public virtual DbSet<Form> Forms { get; set; }
    public virtual DbSet<FormScope> FormScopes { get; set; }
    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<ItemValue> ItemValues { get; set; }
    public virtual DbSet<Scope> Scopes { get; set; }
    public virtual DbSet<ScopeType> ScopeTypes { get; set; }
    public virtual DbSet<DepartmentOrgTree> DepartmentOrgTrees { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Organization> Organizations { get; set; }
    public virtual DbSet<OrgTree> OrgTree { get; set; }
    public virtual DbSet<School> Schools { get; set; }
    public virtual DbSet<SchoolType> SchoolTypes { get; set; }
    public virtual DbSet<Department> Departments { get; set; }
    public virtual DbSet<OrganizationType> OrganizationTypes { get; set; }
    public virtual DbSet<Plan> Plans { get; set; }
    public virtual DbSet<PlanSchedule> PlanSchedules { get; set; }
    public virtual DbSet<PlanStatus> PlanStatuses { get; set; }
    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
    public virtual DbSet<EmailTemplateDocument> EmailTemplateDocuments { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<NotificationTemplate> NotificationTemplates { get; set; }
    public virtual DbSet<SMSProfile> SMSProfiles { get; set; }
    public virtual DbSet<SMSTemplate> SMSTemplates { get; set; }
    public virtual DbSet<TemplateDocument> TemplateDocuments { get; set; }
    public virtual DbSet<TemplateGenrationType> TemplateGenrationTypies { get; set; }
    public virtual DbSet<ScopeAcademicYear> ScopeAcademicYears { get; set; }
    public virtual DbSet<OrgAcademicYear> OrgAcademicYears { get; set; }
    public virtual DbSet<UserLoginLog> UserLoginLogs { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DCDCSQL2DNET01;Database=Evaluation;Trust Server Certificate=true;User id=t-m.fatouh-dev;Integrated Security=SSPI;");
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
        .Properties<decimal>()
        .HavePrecision(18, 4);
    }
}
