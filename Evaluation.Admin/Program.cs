using AutoMapper;
using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Middlewares;
using Evaluation.DAL.Context;
using Evaluation.DAL.Helper;
using Evaluation.Services.Extensions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Middlewares;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;

        ClsAppSetting.TenantId = config.GetSection("AzureADConfig:TenantId").Value ?? "";
        ClsAppSetting.ClientId = config.GetSection("AzureADConfig:ClientId").Value ?? "";
        ClsAppSetting.ClientSecret = config.GetSection("AzureADConfig:ClientSecret").Value ?? "";
        ClsAppSetting.EvaluationConnectionString = config.GetSection("ConnectionStrings:EvaluationDBConn").Value ?? "";



        string KeyVaultURL = config.GetSection("BLKeyVault").Value ?? "";
        bool IsKeyVault = Convert.ToBoolean(config.GetSection("IsKeyVault").Value ?? "true");

        if (IsKeyVault)
        {
            ClsAppSetting.AzureBlobConnectionString = config["AzureBlobStorageConnectionString"] ?? "";

            ClsAppSetting.BlobSasUrl = builder.Configuration["AzureBlobStorage"] ?? "";

            ClsAppSetting.AllowAdminCorsOnly = builder.Configuration["baseAdminUrl"] ?? "";

        }
        else
        {
            //if (isProduction)
            //    ClsAppSetting.ScholarshipConnectionString = config.GetSection("ConnectionStrings:ScholarshipDBConnDev").Value ?? "";
            //else
            //    ClsAppSetting.ScholarshipConnectionString = config.GetSection("ConnectionStrings:ScholarshipDBConnStg").Value ?? "";

            ClsAppSetting.AzureBlobConnectionString = builder.Configuration.GetSection("AzureBlobStorageConnectionString").Value ?? "";
            ClsAppSetting.BlobSasUrl = builder.Configuration.GetSection("ConnectionStrings:AzureBlobStorage").Value ?? "";


            ClsAppSetting.AllowAdminCorsOnly = builder.Configuration.GetSection("AppSettings:baseAdminUrl").Value ?? "";

        }
        builder.Services.AddDbContext<EvaluationDbContext>(options =>
        {
            options.UseSqlServer(ClsAppSetting.EvaluationConnectionString);
        });



        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AdminOnly",
                policy =>
                {
                    policy
                    //.AllowAnyOrigin()
                    .WithOrigins(ClsAppSetting.AllowAdminCorsOnly.Replace("/{lang}", "")) // Allows requests from any origin
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });



        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<RequestInfo>();
        builder.Services.AddScoped<PopulateRequestInfoFilter>();


        builder.Services.AddScoped<ResponseInfo>();
        builder.Services.AddScoped<PopulateResponseInfoFilter>();

        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<PopulateRequestInfoFilter>(); // Register globally
            options.Filters.Add<PopulateResponseInfoFilter>(); // Register globally
        });
        builder.Services.ConfigureSession(config, 30);
        builder.Services.ConfigureAzureADServices(config);
        builder.Services.ConfigureUserInfo(config);
        builder.Services.ConfigureRequestInfo(config);
        //builder.Services.PopulateAppSettings(config);
        builder.Services.ConfigureMasterBL(config, builder.Environment.IsDevelopment());
        builder.Services.AddAutoMapper(typeof(Profile));


        var app = builder.Build();

        app.UseCors("AdminOnly");

        app.UseMiddleware<ExceptionHandlingMiddleware>();


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        //app.UseMiddleware<AuthenticationCheckMiddleware>(); // Register the authentication check middleware
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<LanguageHandlerMiddleware>();
        app.UseMiddleware<PopulateUserInfoMiddleware>();
        app.UseMiddleware<PopulateRequestInfoMiddleware>();

        app.MapControllerRoute(
            name: "default",
            pattern: "{language=ar}/{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
