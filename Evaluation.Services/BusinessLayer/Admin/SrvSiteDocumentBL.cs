using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvSiteDocumentBL : AdminBase
    {
        private readonly AzureBlobStorageService _blobService;
        public SrvSiteDocumentBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo, AzureBlobStorageService blobService) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            _blobService=blobService;
        }

        public async Task<List<SiteDocumentDTO>> GetSiteDocumentList(int Page, int PageSize)
        {
          
            var mapper = await CreateMapperForAdmin<SiteDocument, SiteDocumentDTO>();

            var list = await uow.GetRepository<SiteDocument>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderByDescending(x=>x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<SiteDocumentDTO>>(list);

            return result;
            
        }
        public async Task<SiteDocumentDTO> SaveSiteDocument(SiteDocumentDTO message)
        {
           
            var mapper = await CreateMapperForAdmin<SiteDocument, SiteDocumentDTO>();

           
                
                SiteDocument obj = new SiteDocument();

                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.Skey = Guid.NewGuid().ToString();
                obj.FileName = message.FileName;
                obj.FileName_UiFileName = message.FileName_UiFileName;
                obj.FileName_BlobURL = message.FileName_BlobURL;
                obj.FileName_FileExt = message.FileName_FileExt;
                obj.FileName_Size = message.FileName_Size;
                obj.EndDate = message.EndDate;
                obj.StartDate = message.StartDate;
                obj.IsActive = message.IsActive;

                uow.GetRepository<SiteDocument>().Insert(obj);
                await uow.CommitAsync();
                var result = mapper.Map<SiteDocumentDTO>(obj);
            result.UpdateBy = userInfo.DBName;
            result.ResponseStatus = DBResult.Inserted;
                return result;
           
        }
        public async Task<SiteDocumentDTO> UpdateSiteDocument(SiteDocumentDTO message)
        {
           
            
           
                var mapper = await CreateMapperForAdmin<SiteDocument, SiteDocumentDTO>();
                var result = new SiteDocumentDTO();

                if (message.Id is not null)
                {
                    

                    SiteDocument obj = await uow.GetRepository<SiteDocument>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();

                    obj.TitleAr = message.TitleAr;
                    obj.TitleEn = message.TitleEn;
                    obj.Skey = Guid.NewGuid().ToString();
                    if (message.FileName != null)
                    {
                        obj.FileName = message.FileName;
                        obj.FileName_UiFileName = message.FileName_UiFileName;
                        obj.FileName_BlobURL = message.FileName_BlobURL;
                        obj.FileName_FileExt = message.FileName_FileExt;
                        obj.FileName_Size = message.FileName_Size;
                    }
                    else

                    {
                        if (obj.EndDate != message.EndDate)
                        {
                            int totalMinutes = 0;
                            if (message.EndDate != null)
                            {
                                TimeSpan timeSpan = (TimeSpan)(message.EndDate - DateTime.Now);
                                totalMinutes = (int)timeSpan.TotalMinutes;
                            }
                            string BlobUrl =  _blobService.GenerateSasToken(obj.FileName, totalMinutes, obj.FileName_UiFileName, false, StorageContainerType.website);
                        obj.FileName_BlobURL = BlobUrl;

                        }
                    }
                    obj.EndDate = message.EndDate;
                    obj.StartDate = message.StartDate;
                    obj.IsActive = message.IsActive;
                    uow.GetRepository<SiteDocument>().Update(obj);
                    await uow.CommitAsync();
                    result = mapper.Map<SiteDocumentDTO>(obj);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
                }

                return result;
           
        }
        public async Task<SiteDocumentDTO> DeleteSiteDocument(Guid? Id)
        {

           
                var mapper = await CreateMapperForAdmin<SiteDocument, SiteDocumentDTO>();
                var result = new SiteDocumentDTO();
                if (Id is not null)
                {
                    SiteDocument obj = await uow.GetRepository<SiteDocument>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                    uow.GetRepository<SiteDocument>().Delete(obj);
                    await uow.CommitAsync();
                    result = mapper.Map<SiteDocumentDTO>(obj);
                    result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
       
    }
}
