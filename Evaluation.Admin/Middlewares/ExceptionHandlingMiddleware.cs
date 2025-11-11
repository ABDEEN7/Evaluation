using Evaluation.DAL.Entities.Exception;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Exceptions;
using Newtonsoft.Json;
using System.Net;
using System.Security;

namespace Evaluation.Admin.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;
        private readonly IServiceProvider serviceProvider;

        //private readonly IUnitOfWork _uow;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IServiceProvider serviceProvider)
        {
            _next = next;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                // Log the exception if necessary
                switch (ex)
                {
                    case SecurityException securityException:
                        {
                            var error = new ErrorResponse(securityException.Message, context.Response.StatusCode);
                            var errorResponse = JsonConvert.SerializeObject(error);
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.Response.ContentType = "application/json";
                            //await SaveException(new ExceptionLog(ex));

                            await context.Response.WriteAsync(errorResponse);
                            //var exceptionlog = new ExceptionLog(ex);

                            break;
                        }
                    case BusinessException businessException:
                        {

                            string message = await GetMessageText("en", businessException.Message);
                            var error = new ErrorResponse(message, context.Response.StatusCode);
                            var errorResponse = JsonConvert.SerializeObject(error);
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            context.Response.ContentType = "application/json";
                            //await SaveException(new ExceptionLog(ex));

                            await context.Response.WriteAsync(errorResponse);
                            break;
                        }
                    case UnauthorizedAccessException unauthorizedAccessException:
                        {
                            var error = new ErrorResponse(unauthorizedAccessException.Message, context.Response.StatusCode);
                            var errorResponse = JsonConvert.SerializeObject(error);
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";
                            //await SaveException(new ExceptionLog(ex));

                            await context.Response.WriteAsync(errorResponse);
                            //var exceptionlog = new ExceptionLog(ex);

                            break;
                        }
                    default:
                        {
                            var formattedMessages = FormatErrorMessage(context.Request);
                            logger.LogCritical(ex, formattedMessages);
                            var error = new ErrorResponse(formattedMessages, context.Response.StatusCode);
                            var errorResponse = JsonConvert.SerializeObject(error);
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            context.Response.ContentType = "application/json";
                            //await SaveException(new ExceptionLog(ex));

                            await context.Response.WriteAsync(errorResponse);

                            break;
                        }

                }


            }
        }

        private async Task<string> GetMessageText(string lang, string message)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var generalSettingsProvider = scope.ServiceProvider.GetRequiredService<CacheDataProvider>();
                var _message = await generalSettingsProvider.GetExceptionMessage(message, lang);
                if (string.IsNullOrEmpty(_message)) return message;
                return _message;
            }
        }

        private async Task SaveException(ExceptionLog exceptionlog)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var uow = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
                uow.GetRepository<ExceptionLog>().Insert(exceptionlog);
                await uow.CommitAsync();
            }


        }
        private string FormatErrorMessage(HttpRequest request)
        {
            var message = $"Exception occur while processing request : {request.Path} with request method {request.Method} at Time {DateTime.Now}";
            return message;
        }
    }
}
