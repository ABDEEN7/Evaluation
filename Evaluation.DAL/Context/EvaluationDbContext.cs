using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Authentication;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.DepartementEntites;
using Evaluation.DAL.Entities.FormsModules;
using Evaluation.DAL.Entities.Masters;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Entities.PermissionEntity;
using Evaluation.DAL.Entities.Planing;
using Evaluation.DAL.Entities.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Entities.ServiceRequestEntities;
using Evaluation.DAL.Entities.ServicesEntities;
using Evaluation.DAL.Entities.SystemModulesEntities;
using Evaluation.DAL.Entities.Template;
using Evaluation.DAL.Entities.UserEntiy;
using Evaluation.DAL.Extensions;
using Evaluation.DAL.SystemSetting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Evaluation.DAL.Context;

public partial class EvaluationDbContext : DbContext
{
    public EvaluationDbContext() { }
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
    public virtual DbSet<OrgType> OrgTypes { get; set; }
    public virtual DbSet<Plan> Plans { get; set; }
    public virtual DbSet<PlanHistory> PlanHistory { get; set; }
    public virtual DbSet<EvaluationRequest> EvaluationRequests { get; set; }
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
    public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }
    public virtual DbSet<Service> Services { get; set; }
    public virtual DbSet<SystemModule> SystemModules { get; set; }
    public virtual DbSet<ModuleType> ModuleTypes { get; set; }
    public virtual DbSet<UiControl> UiControls { get; set; }
    public virtual DbSet<OrgClass> OrgClass { get; set; }
    public virtual DbSet<ServiceRequestFieldsValue> ServiceRequestFieldsValue { get; set; }
    public virtual DbSet<RequestAssignment> RequestAssignment { get; set; }
    public virtual DbSet<EvaluationRequest> EvaluationRequest { get; set; }
    public virtual DbSet<EvaluationRequestFieldsValue> EvaluationRequestFieldsValue { get; set; }
    public virtual DbSet<EvaluationRequestHistory> EvaluationRequestHistory { get; set; }
    public virtual DbSet<EvaluationRequestHistoryFieldsValue> EvaluationRequestHistoryFieldsValue { get; set; }
    public virtual DbSet<JobTitle> JobTitle { get; set; }
    public virtual DbSet<UserGender> UserGender { get; set; }
    public virtual DbSet<EvaluationType> EvaluationType { get; set; }
    public virtual DbSet<DepartmentHoliday> DepartmentHolidays { get; set; }
    public virtual DbSet<Semester> Semesters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       // optionsBuilder.UseSqlServer("Server=DCDCSQL2DNET01;Database=Evaluation;Trust Server Certificate=true;User id=t-m.fatouh-dev;Integrated Security=SSPI;");
        //optionsBuilder.UseSqlServer("Server=DCDCSQL2DNET01;Database=Evaluation;Trust Server Certificate=true;User id=Eval_User; Password=Abc@1234;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        base.OnModelCreating(modelBuilder);
        var typesToRegister = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in typesToRegister)
        {
            if (type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
        ApplyGeneralConfigurations(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }



    private void ApplyGeneralConfigurations(ModelBuilder modelBuilder)
    {

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        var entityBaseType = typeof(EntityBase);
        var entityTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => entityBaseType.IsAssignableFrom(t) && t != entityBaseType);


        var excludedTypeFromGlobalQuery = new List<Type>
        {
            typeof(Organization),
            typeof(Employee),
            typeof(School),
            
        };

        foreach (var entityType in entityTypes)
        {

            // Set the default value for "CreateDate" property
            modelBuilder.Entity(entityType)
                .Property<DateTime>(nameof(EntityBase.CreateDate))
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity(entityType)
                .Property<bool?>(nameof(EntityBase.IsActive))
                .HasDefaultValueSql("1");
            modelBuilder.Entity(entityType)
                .Property<bool?>(nameof(EntityBase.IsDeleted))
                .HasDefaultValueSql("0");
            modelBuilder.Entity(entityType)
                .Property<Guid?>(nameof(EntityBase.CreateById));
            //.HasDefaultValueSql("'1'");

            if (!excludedTypeFromGlobalQuery.Contains(entityType))
            {
                var method = typeof(ModelBuilderExtensions).GetMethod(nameof(ModelBuilderExtensions.AddGlobalQueryFilter));
                var genericMethod = method.MakeGenericMethod(entityType);
                genericMethod.Invoke(null, [modelBuilder, entityType]);
            }
        }
        modelBuilder.Ignore<EntityBase>();

        // Use reflection to find all the entity types derived from EntityBase


    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);



    //protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    //{
    //    configurationBuilder
    //    .Properties<decimal>()
    //    .HavePrecision(18, 4);
    //}
}
