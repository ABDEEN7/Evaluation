using Evaluation.Web.Filters;
using Evaluation.Web.Middlewares;
using Evaluation.Web.Special;
using Microsoft.AspNetCore.StaticFiles;
using Evaluation.SharedHelper.Models;
using Evaluation.DAL.Helper;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CookieServices>();
builder.Services.AddScoped<RequestInfo>();
builder.Services.AddScoped<PopulateRequestInfoFilter>();

builder.Services.AddScoped<ResponseInfo>();
builder.Services.AddScoped<PopulateResponseInfoFilter>();


//builder.Services.AddScoped<HttpClient>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<PopulateRequestInfoFilter>(); 
    options.Filters.Add<PopulateResponseInfoFilter>(); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");
app.UseStaticFiles();


app.UseMiddleware<LanguageHandlerMiddleware>();


app.UseRouting();


app.UseMiddleware<CookiesProviderMiddleware>();

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        Mappings = { [".css"] = "text/css" }
    }
});

app.MapControllerRoute(
   "default",
   "{language=ar}/{controller=Home}/{action=Index}/{id?}");
app.Run();
