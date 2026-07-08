using AutoMapper;
using Evaluation.DAL.Context;
using Evaluation.Services.Models.JWT;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Helper;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.Text.Json.Serialization;

namespace Evaluation.Admin.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureAzureADServices(this IServiceCollection services, IConfiguration configuration)
        {
            var initialScopes = configuration["DownstreamApi:Scopes"]?.Split(' ')
                                ?? configuration["MicrosoftGraph:Scopes"]?.Split(' ');

            services.AddDbContext<EvaluationDbContext>(options =>
            {
                options.UseSqlServer(ClsAppSetting.EvaluationConnectionString);
            });

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureADConfig"))
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                .AddMicrosoftGraph(configuration.GetSection("MicrosoftGraph"))
                .AddInMemoryTokenCaches();

            // Read RedirectUri from configuration
            services.Configure<OpenIdConnectOptions>(
     OpenIdConnectDefaults.AuthenticationScheme,
     options =>
     {
         options.Events.OnRedirectToIdentityProvider = context =>
         {
             context.ProtocolMessage.RedirectUri =
                 configuration["AzureADConfig:RedirectUri"];

             return Task.CompletedTask;
         };
     });

            services.AddTransient<AzureBlobStorageService>();

            services.AddAutoMapper(typeof(Profile));

            services.Configure<AzureADConfig>(
                configuration.GetSection("AzureADConfig"));

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            services.AddRazorPages()
                .AddMicrosoftIdentityUI();
        }
    }
}
