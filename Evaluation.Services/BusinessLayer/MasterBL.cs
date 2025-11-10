
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer
{
    public class MasterBL
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ISmsServices smsServices;
        private readonly LoggingServices loggingServices;
        private readonly CacheDataProvider cacheDataProvider;
        private readonly MSJsonWT mSJsonWT;
        private readonly AzureBlobStorageService _AzureStorage;

        public MasterBL(IServiceProvider serviceProvider,
            ISmsServices smsServices,
            LoggingServices loggingServices, 
            CacheDataProvider cacheDataProvider,
            MSJsonWT mSJsonWT, AzureBlobStorageService _storage)
        {
            this.serviceProvider = serviceProvider;
            this.smsServices = smsServices;
           
            this.loggingServices = loggingServices;
            this.cacheDataProvider = cacheDataProvider;
            this.mSJsonWT = mSJsonWT;
            _AzureStorage = _storage;
        }

        public ISmsServices SmsServices { get { return smsServices; } }
        public LoggingServices LoggingServices { get { return loggingServices; } }
        public CacheDataProvider CacheDataProvider { get { return cacheDataProvider; } }
        public MSJsonWT MSJsonWT { get { return mSJsonWT; } }
        public AzureBlobStorageService StorageService { get { return _AzureStorage; } }

        public T GetAdminService<T>() where T : AdminBase
        {
            return serviceProvider.GetRequiredService<T>();
        }

        public T GetApiService<T>() where T : ApiBase
        {
            return serviceProvider.GetRequiredService<T>();
        }
    }
}
