using Evaluation.API.Filters;
using Evaluation.API.Middlewares;
using Evaluation.DAL.Context;
using Evaluation.Services.Extensions;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Helper;
using Evaluation.Web.Middlewares;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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
            ClsAppSetting.OracleDBConnection = config["OracleDBConnection"] ?? "";
        }
        else
        {
            ClsAppSetting.AzureBlobConnectionString = config.GetSection("AzureBlobStorageConnectionString").Value ?? "";
            ClsAppSetting.BlobSasUrl = config.GetSection("ConnectionStrings:AzureBlobStorage").Value ?? "";


            ClsAppSetting.OracleDBConnection = config["OracleDBConnection"] ?? "";

            // Form JWT
            ClsAppSetting.FormJwtConfigKey = (isKeyVault ? config["NSISStudentInfoURL"] : config["FormJwtConfig:Key"]) ?? "";
            ClsAppSetting.FormJwtExpirationTime = (isKeyVault ? config["FormJwtExpirationTime"] : config["FormJwtConfig:ExpirationTime"]) ?? "";
            ClsAppSetting.BaseApiUrl = (isKeyVault ? config["BaseApiUrl"] : config["AppSettings:baseApiUrl"]) ?? "";
            ClsAppSetting.AllowWebCorsOnly = (isKeyVault ? config["baseAppUrl"] : config["AppSettings:baseAppUrl"]) ?? "";
            ClsAppSetting.AllowAdminCorsOnly = (isKeyVault ? config["baseAdminUrl"] : config["AppSettings:baseAdminUrl"]) ?? "";


            ClsAppSetting.NsisBaseURL = config["NsisSettings:NsisBaseURL"] ?? "";
            ClsAppSetting.NsisAuthenticationURL = config["NsisSettings:NsisAuthenticationURL"] ?? "";
            ClsAppSetting.NsisUsername = config["NsisSettings:NsisUsername"] ?? "";
            ClsAppSetting.NsisPassword = config["NsisSettings:NsisPassword"] ?? "";
            ClsAppSetting.NsisGrantType = config["NsisSettings:NsisGrantType"] ?? "";
            ClsAppSetting.NsisSchoolsApi = config["NsisSettings:NsisSchoolsApi"] ?? "";
            ClsAppSetting.NsisClassesApi = config["NsisSettings:NsisClassesApi"] ?? "";
            ClsAppSetting.NsisTeachersApi = config["NsisSettings:NsisTeachersApi"] ?? "";
            ClsAppSetting.NsisStaffApi = config["NsisSettings:NsisStaffApi"] ?? "";
            ClsAppSetting.NsisEnrollmentApi = config["NsisSettings:NsisEnrollmentApi"] ?? "";
            ClsAppSetting.NsisLimit = config.GetValue<int>("NsisSettings:NsisLimit");
            ClsAppSetting.NsisStatus = config["NsisSettings:NsisStatus"] ?? "";
            ClsAppSetting.CountPage = config.GetValue<int>("PageSettings:CountPage");
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
            builder.Services.ConfigureMasterBL(config, builder.Environment.IsDevelopment());
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

            // Add services to the container
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = config.GetValue<string>("FormJwtConfig:Issuer"), // Replace with your issuer
                    //ValidAudience = config.GetValue<string>("FormJwtConfig:Audience"), // Replace with your audience
                    ValidIssuer = ClsAppSetting.BaseApiUrl,
                    ValidAudiences = new[]
                    {
                    //ClsAppSetting.AllowWebCorsOnly,
                    //ClsAppSetting.AllowAdminCorsOnly,
                    ClsAppSetting.BaseApiUrl,
                    },
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ClsAppSetting.FormJwtConfigKey)), // Replace with your secret key

                    // Use custom lifetime validation
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                    {
                        // Custom logic here. For example, allow tokens that are within 5 minutes of expiration.
                        if (expires.HasValue)
                        {
                            var expirationDate = expires.Value.ToLocalTime();
                            var currentDate = DateTime.Now;

                            return expirationDate > currentDate;
                        }
                        return false;
                    }
                };

            });

            builder.Services.AddEndpointsApiExplorer();
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

                c.CustomSchemaIds(x => x.FullName);
            });

            //// -------------------------------------
            //// 5️⃣ CORS and Auth Policies
            //// -------------------------------------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowWebAndAdmin",
                    policy =>
                    {
                        policy
                        //.AllowAnyOrigin()  
                        .WithOrigins(ClsAppSetting.AllowWebCorsOnly.Replace("/{lang}", ""), ClsAppSetting.AllowAdminCorsOnly.Replace("/{lang}", ""))
                        .AllowAnyHeader()
                        .AllowAnyMethod() // Allows any HTTP method (GET, POST, etc.)
                        .WithExposedHeaders("Content-Disposition", "newToken", "expiryDateTime", "expirationTime"); // Exposes specific headers to the client
                    });
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            // Register AutoMapper and scan all assemblies
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // -------------------------------------
            // 6️⃣ Build the App
            // -------------------------------------
            var app = builder.Build();

            app.UseRouting();
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

            app.UseMiddleware<PopulateUserInfoMiddleware>();
            app.UseMiddleware<PopulateRequestInfoMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.MapControllerRoute(
            name: "default",
            pattern: "api/{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}