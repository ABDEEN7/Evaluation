using Evaluation.DAL.Exceptions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper;
using Newtonsoft.Json;
using System.Net;
using System.Security;
using Evaluation.DAL.Models.Exception;

namespace Evaluation.API.Middlewares
{
    public class ExceptionHandlingMiddleware(
       ILogger<ExceptionHandlingMiddleware> logger,
       RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    await HandleNotFoundAsync(context);
                }
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);

            }
        }

        private static async Task HandleNotFoundAsync(HttpContext context)
        {
            var requestedUrl = context.Request.Path;
            var errorMessage = $"The requested resource was not found: {requestedUrl}";
            var error = new ErrorResponse(errorMessage, (int)HttpStatusCode.NotFound);
            var errorResponse = JsonConvert.SerializeObject(error);

            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(errorResponse);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {

            switch (ex)
            {
                case SecurityException securityException:
                    {
                        var error = new ErrorResponse(securityException.Message, context.Response.StatusCode);
                        var errorResponse = JsonConvert.SerializeObject(error);
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = "application/json";
                        SaveExceptionLog(ex);
                        await context.Response.WriteAsync(errorResponse);
                        break;
                    }
                case DatabaseException databaseException:
                    {
                        var error = new ErrorResponse(databaseException.UserFriendlyMessage, context.Response.StatusCode);
                        var errorResponse = JsonConvert.SerializeObject(error);
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";
                        var json = new
                        {
                            databaseException.UserFriendlyMessage,
                            databaseException.InnerExceptionMessage,
                            databaseException.InnerExceptionStackTrace,
                        };

                        SaveExceptionLog(ex, JsonConvert.SerializeObject(json));
                        await context.Response.WriteAsync(errorResponse);

                        break;
                    }
                case BusinessException businessException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";

                        // If FieldErrors exist, return them directly
                        if (businessException.FieldErrors != null && businessException.FieldErrors.Any())
                        {
                            var errorResponse = JsonConvert.SerializeObject(businessException.FieldErrors);
                            SaveExceptionLog(ex, errorResponse);
                            await context.Response.WriteAsync(errorResponse);
                        }
                        else
                        {
                            string message = await GetMessageText(businessException.Message, GetCurrentLanguage(context));
                            var error = new ErrorResponse(message, context.Response.StatusCode);
                            var errorResponse = JsonConvert.SerializeObject(error);
                            SaveExceptionLog(ex);
                            await context.Response.WriteAsync(errorResponse);
                        }

                        break;
                    }

                case UnauthorizedAccessException:
                    {
                        var error = new ErrorResponse(ex.Message, (int)HttpStatusCode.Unauthorized);
                        var errorResponse = JsonConvert.SerializeObject(error);
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.ContentType = "application/json";
                        SaveExceptionLog(ex);
                        await context.Response.WriteAsync(errorResponse);
                        break;
                    }

                case IntegrationServiceException integrationServiceException:
                    {
                        var error = new ErrorResponse(integrationServiceException.FriendlyErrorMessage, context.Response.StatusCode);
                        var errorResponse = JsonConvert.SerializeObject(error);
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";
                        var json = new
                        {
                            integrationServiceException.FriendlyErrorMessage,
                            integrationServiceException.ActualErrorMessage,
                            integrationServiceException.AdditionalErrorMessage,
                        };
                        SaveExceptionLog(ex, JsonConvert.SerializeObject(json));
                        await context.Response.WriteAsync(errorResponse);
                        break;
                    }


                case MissingConfigException missingConfigExeption:
                    {
                        var error = new ErrorResponse(missingConfigExeption.Message, context.Response.StatusCode);
                        var errorResponse = JsonConvert.SerializeObject(error);
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";
                        SaveExceptionLog(ex);
                        await context.Response.WriteAsync(errorResponse);
                        break;
                    }

                case HttpResponseException httpResponseException:
                    {
                        var error = new ErrorResponse(httpResponseException.Message, context.Response.StatusCode);
                        var errorResponse = JsonConvert.SerializeObject(error);
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";
                        var json = new
                        {
                            HttpResponseServerMessage = httpResponseException.Message,
                            HttpResponseContentMessage = await httpResponseException.HttpResponseMessage.Content.ReadAsStringAsync(),
                        };
                        SaveExceptionLog(ex, JsonConvert.SerializeObject(json));
                        await context.Response.WriteAsync(errorResponse);
                        break;
                    }
                case EmailServicesException emailServicesException:
                    {
                        var error = new ErrorResponse(emailServicesException.Message, context.Response.StatusCode);
                        var errorResponse = JsonConvert.SerializeObject(error);
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";
                        var json = new
                        {
                            InnerException = emailServicesException.InnerException,
                        };
                        SaveExceptionLog(ex, JsonConvert.SerializeObject(json));
                        await context.Response.WriteAsync(errorResponse);
                        break;
                    }
                default:
                    {
                        var error = new ErrorResponse(ex.Message, context.Response.StatusCode);
                        var errorResponse = JsonConvert.SerializeObject(error);
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";
                        SaveExceptionLog(ex);
                        await context.Response.WriteAsync(errorResponse);
                        break;
                    }

            }

        }
        private async Task<string> GetMessageText(string message, string lang)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var cacheDataProvider = scope.ServiceProvider.GetRequiredService<CacheDataProvider>();

                var result = await cacheDataProvider.GetExceptionMessage(message, lang);

                return result;
            }
        }

        private static string GetCurrentLanguage(HttpContext? context)
        {
            var lang = string.Empty;
            if (context != null)
            {
                lang = context.Request.Headers["lang"];
            }

            if (string.IsNullOrEmpty(lang))
                lang = AppSettings.DefaultLanguage;// en
            return lang;
        }

        private void SaveExceptionLog(Exception ex, string? json = null)
        {
            Task.Run(async () =>
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    using (var uow = scope.ServiceProvider.CreateScopedUow())
                    {

                        try
                        {
                            var log = new ExceptionLog
                            {
                                ExceptionType = ex.GetType().FullName ?? "",
                                Message = ex.Message,
                                StackTrace = ex.StackTrace ?? "",
                                Timestamp = DateTime.UtcNow,
                                JsonParameter = json,
                                CreateById = AppSettings.CreateById
                            };

                            uow.GetRepository<ExceptionLog>().Insert(log);
                            await uow.CommitAsync();

                        }
                        catch (Exception exception)
                        {
                            logger.LogError(exception, "Error while saving exception log");
                        }

                    }

                }
            });

        }

    }
}
