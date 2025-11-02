using System.Text.Json;
using Evaluation.API.Middlewares;
using Evaluation.DAL.Context;
using Evaluation.Services.Extensions;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Helper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.
bool IsKeyVault = Convert.ToBoolean(builder.Configuration.GetSection("IsKeyVault").Value ?? "true");

if (IsKeyVault)
{
    ClsAppSetting.AzureBlobConnectionString = builder.Configuration["AzureBlobStorageConnectionString"] ?? "";

    ClsAppSetting.BlobSasUrl = builder.Configuration["AzureBlobStorage"] ?? "";

    ClsAppSetting.AllowWebCorsOnly = builder.Configuration["baseAppUrl"] ?? "";

}
else
{
    ClsAppSetting.AzureBlobConnectionString = builder.Configuration.GetSection("AzureBlobStorageConnectionString").Value ?? "";
    ClsAppSetting.BlobSasUrl = builder.Configuration.GetSection("ConnectionStrings:AzureBlobStorage").Value ?? "";

    ClsAppSetting.MOPHUserName = builder.Configuration.GetSection("MOPHConfig:UserName").Value ?? "";
    ClsAppSetting.MOPHPassword = builder.Configuration.GetSection("MOPHConfig:Password").Value ?? "";
    ClsAppSetting.MOPHApiURL = builder.Configuration.GetSection("MOPHConfig:ApiURL").Value ?? "";
    ClsAppSetting.NSISApiURL = builder.Configuration.GetSection("NSISConfig:ApiURL").Value ?? "";
    ClsAppSetting.NSISAppID = builder.Configuration.GetSection("NSISConfig:AppID").Value ?? "";
    ClsAppSetting.StudentCertificateUserName = builder.Configuration.GetSection("NSISConfig:StudentCertificateUserName").Value ?? "";
    ClsAppSetting.StudentCertificatePassword = builder.Configuration.GetSection("NSISConfig:StudentCertificatePassword").Value ?? "";
    ClsAppSetting.ValidationURL = builder.Configuration.GetSection("NSISConfig:ValidationURL").Value ?? "";
    ClsAppSetting.StudentInfoURL = builder.Configuration.GetSection("NSISConfig:StudentInfoURL").Value ?? "";


    ClsAppSetting.FormJwtConfigKey = builder.Configuration.GetSection("FormJwtConfig:Key").Value ?? "";


    ClsAppSetting.AllowWebCorsOnly = builder.Configuration.GetSection("AppSettings:baseAppUrl").Value!.Replace("/{lang}", "") ?? "";
    ClsAppSetting.BaseApiUrl = builder.Configuration.GetSection("AppSettings:baseApiUrl").Value!.Replace("/{lang}", "") ?? "";
    ClsAppSetting.AllowAdminCorsOnly = builder.Configuration.GetSection("AppSettings:baseAdminUrl").Value!.Replace("/{lang}", "") ?? "";

}
builder.Services.AddControllers()
      .AddJsonOptions(options =>
      {
          options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
      });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureMasterBL(config, builder.Environment.IsDevelopment());

builder.Services.ConfigureUserInfo(config);
builder.Services.ConfigureRequestInfo(config);
builder.Services.ConfigureMasterBL(config, builder.Environment.IsDevelopment());



builder.Services.AddDbContext<EvaluationDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("EvaluationDBConn")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<PopulateUserInfoMiddleware>();
app.UseMiddleware<PopulateRequestInfoMiddleware>();



app.MapControllerRoute(
                 name: "default",
                 pattern: "api/{controller=Home}/{action=Index}/{id?}");
app.Run();
