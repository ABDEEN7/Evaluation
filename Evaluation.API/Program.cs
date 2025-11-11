using Evaluation.API.Filters;
using Evaluation.API.Middlewares;
using Evaluation.DAL.Context;
using Evaluation.Services.Extensions;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Helper;
using Evaluation.Web.Middlewares;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;

        // -------------------------------------
        // 1️⃣ Load configuration & AppSettings
        // -------------------------------------
        bool isKeyVault = Convert.ToBoolean(config.GetSection("IsKeyVault").Value ?? "true");

        if (isKeyVault)
        {
            ClsAppSetting.AzureBlobConnectionString = config["AzureBlobStorageConnectionString"] ?? "";
            ClsAppSetting.BlobSasUrl = config["AzureBlobStorage"] ?? "";
            ClsAppSetting.AllowWebCorsOnly = config["baseAppUrl"] ?? "";
        }
        else
        {
            ClsAppSetting.AzureBlobConnectionString = config.GetSection("AzureBlobStorageConnectionString").Value ?? "";
            ClsAppSetting.BlobSasUrl = config.GetSection("ConnectionStrings:AzureBlobStorage").Value ?? "";
           
            ClsAppSetting.AllowWebCorsOnly = config.GetSection("AppSettings:baseAppUrl").Value!.Replace("/{lang}", "") ?? "";
            ClsAppSetting.BaseApiUrl = config.GetSection("AppSettings:baseApiUrl").Value!.Replace("/{lang}", "") ?? "";

            // -------------------------------------
            // 2️⃣ Add Core Services
            // -------------------------------------
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDistributedMemoryCache();
            //builder.Services.AddDataProtection(); // ✅ Required for session encryption

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.ConfigureSession(config, 30);
            builder.Services.ConfigureUserInfo(config);
            builder.Services.ConfigureRequestInfo(config);
            builder.Services.PopulateAppSettings(config);
            builder.Services.ConfigureMasterBL(config);
            builder.Services.AddScoped<TokenValidationFilter>();

            builder.Services.AddScoped<TokenValidationFilter>();

            // -------------------------------------
            // 3️⃣ Register DbContext
            // -------------------------------------
            builder.Services.AddDbContext<EvaluationDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("EvaluationDBConn")));

            // -------------------------------------
            // 4️⃣ Mapster Mapper Registration
            // -------------------------------------
            var mapsterConfig = TypeAdapterConfig.GlobalSettings;

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Define the BearerAuth security scheme
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                    });
            });

            //// -------------------------------------
            //// 5️⃣ CORS and Auth Policies
            //// -------------------------------------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowWebAndAdmin", policy =>
                {
                    policy.WithOrigins(ClsAppSetting.AllowWebCorsOnly, ClsAppSetting.AllowAdminCorsOnly)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .WithExposedHeaders("Content-Disposition", "newToken", "expiryDateTime", "expirationTime");
                });
            });

            builder.Services.AddAuthorization(options =>
            {
                //options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            // -------------------------------------
            // 6️⃣ Build the App
            // -------------------------------------
            var app = builder.Build();

            // -------------------------------------
            // 7️⃣ Middlewares (correct order)
            // -------------------------------------
            app.UseMiddleware<SecurityLayerMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.OAuthUsePkce();
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider
                {
                    Mappings = { [".css"] = "text/css" }
                }
            });


            app.UseSession(); // ✅ Must be after UseRouting

            app.UseCors("AllowWebAndAdmin");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<PopulateUserInfoMiddleware>();
            app.UseMiddleware<PopulateRequestInfoMiddleware>();

            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "api/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}