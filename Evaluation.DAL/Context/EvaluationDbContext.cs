using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.FormsModules;
using Evaluation.DAL.Entities.OrganizationTrees;
using Evaluation.DAL.Entities.Planing;
using Evaluation.DAL.Entities.Template;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.DAL.Context;

public class EvaluationDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(EvaluationDbContext).Assembly);
    }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<ControlValidation> ControlValidations { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<PagePermission> PagePermissions { get; set; }
    public DbSet<PartyType> PartyTypes { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserPartyType> UserPartyTypes { get; set; }
    public DbSet<UserPartyTypeSignature> UserPartyTypeSignatures { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<AcademicYear> AcademicYears { get; set; }
    public DbSet<CalcMethod> CalcMethods { get; set; }
    public DbSet<EvaluationParty> EvaluationParties { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<FormScope> FormScopes { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemValue> ItemValues { get; set; }
    public DbSet<Scope> Scopes { get; set; }
    public DbSet<ScopeType> ScopeTypes { get; set; }
    public DbSet<DepartmentOrganizationTree> DepartmentOrganizationTrees { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationTree> OrganizationTrees { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<SchoolType> SchoolTypes { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<OrganizationType> OrganizationTypes { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<PlanSchedule> PlanSchedules { get; set; }
    public DbSet<PlanStatus> PlanStatuses { get; set; }
    public DbSet<EmailTemplate> EmailTemplates { get; set; }
    public DbSet<EmailTemplateDocument> EmailTemplateDocuments { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
    public DbSet<SMSProfile> SMSProfiles { get; set; }
    public DbSet<SMSTemplate> SMSTemplates { get; set; }
    public DbSet<TemplateDocument> TemplateDocuments { get; set; }
    public DbSet<TemplateGenrationType> TemplateGenrationTypies { get; set; }

}
