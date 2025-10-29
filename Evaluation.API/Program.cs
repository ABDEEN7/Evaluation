using Evaluation.API.Middlewares;
using Evaluation.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Evaluation.SharedHelper;
using Evaluation.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
