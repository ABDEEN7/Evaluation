

using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Models;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Helper;

namespace Evaluation.Services.Special
{
    public class MapperConfigServices
    {
        private readonly UnitOfWork uow;
        private readonly RequestInfo requestInfo;

        public MapperConfigServices(UnitOfWork uow, RequestInfo requestInfo)
        {
            this.uow = uow;
            this.requestInfo = requestInfo;
        }

    

        public async Task<MapperConfig> GetMapperConfigAsync()
        {
            // Fetch the date format from the database
            //var setting = await _dbContext.Settings.FirstOrDefaultAsync(s => s.Key == "DateFormat"); // Adjust based on your database structure
            //var dateFormat = setting?.Value ?? "yyyy-MM-dd"; // Default if not found

            var dateFormat = AppSettings.DateFormat; //"yyyy-MM-dd HH:mm:ss";

            //// Get the language from the service (e.g., from cookies)
            //var language = _languageService.GetLanguage(); // Assuming GetLanguage() returns a string
            var language = string.IsNullOrEmpty(requestInfo.Lang) ? "ar" : requestInfo.Lang;

            return new MapperConfig
            {
                DateFormat = dateFormat,
                Lang = language
            };
        }
    }
}
