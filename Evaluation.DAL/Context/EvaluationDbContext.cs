using Evaluation.DAL.Extensions;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.Authentication;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.EvalResult;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.IntegrationEntity;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.PermissionEntity;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.Planing.TeamsModule;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Models.Template;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Models.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
    public virtual DbSet<EvalForm> EvalForms { get; set; }
    public virtual DbSet<FormScope> FormScopes { get; set; }
    public virtual DbSet<FormItem> FormItem { get; set; }
    public virtual DbSet<FormItemValue> FormItemValues { get; set; }
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
    public virtual DbSet<UiControl> UiControls { get; set; }
    public virtual DbSet<OrgClass> OrgClass { get; set; }
    public virtual DbSet<ServiceRequestFieldsValue> ServiceRequestFieldsValue { get; set; }
    public virtual DbSet<EvaluationRequestAssignment> RequestAssignment { get; set; }
    public virtual DbSet<EvaluationRequest> EvaluationRequest { get; set; }
    public virtual DbSet<EvaluationRequestHistory> EvaluationRequestHistory { get; set; }
    public virtual DbSet<EvaluationRequestHistoryFieldsValue> EvaluationRequestHistoryFieldsValue { get; set; }
    public virtual DbSet<JobTitle> JobTitle { get; set; }
    public virtual DbSet<UserGender> UserGender { get; set; }
    public virtual DbSet<EvaluationType> EvaluationType { get; set; }
    public virtual DbSet<DepartmentHoliday> DepartmentHolidays { get; set; }
    public virtual DbSet<Semester> Semesters { get; set; }
    public virtual DbSet<EvalFormType> EvalFormType { get; set; }
    public virtual DbSet<SubFormItem> SubFormItems { get; set; }
    public virtual DbSet<SubFormItemValue> SubFormItemValues { get; set; }
    public virtual DbSet<DepEvalMatrix> DepEvalMatrixs { get; set; }
    public virtual DbSet<OrgEvalResult> OrgEvalResults { get; set; }
    public virtual DbSet<UserTeam> UserTeams { get; set; }
    public virtual DbSet<RequestAssignmentScope> RequestAssignmentScopes { get; set; }
    public virtual DbSet<EvalAttachment> EvalAttachments { get; set; }
    public virtual DbSet<PlanTypeDep> PlanTypeDep { get; set; }
    public virtual DbSet<WebGroup> WebGroups { get; set; }
    public virtual DbSet<DepWebGroup> DepWebGroup { get; set; }
    public virtual DbSet<DepEvaluationType> DepEvaluationType { get; set; }
    public virtual DbSet<EvaluationRequestAssignment> EvaluationRequestAssignment { get; set; }
    public virtual DbSet<EvalRequestAssignmentScope> EvalRequestAssignmentScope { get; set; }
    public virtual DbSet<SystemModuleType> SystemModuleTypes { get; set; }
    public virtual DbSet<SystemAttribute> SystemAttributes { get; set; }
    public virtual DbSet<DepartmentRoleAttributeValue> DepartmentRoleAttributeValue { get; set; }
    public virtual DbSet<FormItemRelated> FormItemRelated { get; set; }
    public virtual DbSet<NdaStatus> NdaStatus { get; set; }
    public virtual DbSet<NdaStatusDepartment> NdaStatusDepartments { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<DepTargetOrgTree> DepTargetOrgTree { get; set; }
    public virtual DbSet<CssClass> CssClass { get; set; }
    public virtual DbSet<PartyTypeEvalParty> PartyTypeEvalParty { get; set; }
    public virtual DbSet<PartyTypeEvalPartyStatus> PartyTypeEvalPartyStatus { get; set; }
    public virtual DbSet<ServiceStatusType> ServiceStatusType { get; set; }
    public virtual DbSet<FormItemConfig> FormItemConfig { get; set; }
    public virtual DbSet<FieldInfoType> FieldInfoType { get; set; }
    public virtual DbSet<IntegrationPointLog> IntegrationPointLog { get; set; }
    public virtual DbSet<IntegrationPointDataLog> IntegrationPointDataLog { get; set; }
    public virtual DbSet<SchoolStatus> SchoolStatus { get; set; }
    public virtual DbSet<SchoolModel> SchoolModel { get; set; }
    public virtual DbSet<SchoolGender> SchoolGender { get; set; }
    public virtual DbSet<SchoolGrade> SchoolGrade { get; set; }
    public virtual DbSet<SchoolGradeSection> SchoolGradeSction { get; set; }
    public virtual DbSet<SchoolGradeSectionCourse> SchoolGradeSectionCourse { get; set; }
    public virtual DbSet<SchoolProgram> SchoolProgram { get; set; }
    public virtual DbSet<EvalPartyCategory> EvalPartyCategory { get; set; }


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
        modelBuilder.Entity<EvaluationRequest>(entity =>
        {
            entity.Property(e => e.Sequence)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore); // seed: 1, increment: 1
        });
        modelBuilder.Entity<ServiceRequest>(entity =>
        {
            entity.Property(e => e.Sequence)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore); // seed: 1, increment: 1
        });
        modelBuilder.Entity<OrgTree>()
           .HasOne(x => x.OrgType)
                   .WithMany()
                   .HasForeignKey(x => x.OrgTypeId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

        // Employee relations
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.UserGender)
            .WithMany()
            .HasForeignKey(e => e.UserGenderId);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.JobTitle)
            .WithMany()
            .HasForeignKey(e => e.JobTitleId);

        // Organization relations
        modelBuilder.Entity<Organization>()
            .HasOne(o => o.SchoolType)
            .WithMany()
            .HasForeignKey(o => o.TypeId);

        // School relations
        modelBuilder.Entity<School>()
            .HasOne(s => s.SchoolType)
            .WithMany()
            .HasForeignKey(s => s.TypeId);

        var baseType = typeof(OrgTree);
        var derivedTypes =
            Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType));

        modelBuilder.Entity<FormItemRelated>(entity =>
        {
            entity.HasKey(x => new { x.FormItemId, x.RelatedItemId });

            entity.HasOne(x => x.FormItem)
                .WithMany(f => f.RelatedFrom)
                .HasForeignKey(x => x.FormItemId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.RelatedItem)
                .WithMany(f => f.RelatedTo)
                .HasForeignKey(x => x.RelatedItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        foreach (var type in derivedTypes)
        {
            modelBuilder.Entity(type).ToTable(type.Name + "s");
        }

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
                .Property<bool>(nameof(EntityBase.IsActive))
                .HasDefaultValue(1);
            modelBuilder.Entity(entityType)
                .Property<bool>(nameof(EntityBase.IsDeleted))
                .HasDefaultValue(0);
            modelBuilder.Entity(entityType)
                .Property<Guid>(nameof(EntityBase.CreateById));
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
