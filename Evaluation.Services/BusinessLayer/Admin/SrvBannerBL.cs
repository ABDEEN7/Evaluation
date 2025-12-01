using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.SystemSetting;
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
    public class SrvBannerBL : AdminBase
    {
        public SrvBannerBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices,
            IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory,
            RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo,
            serviceScopeFactory, requestInfo)
        {

        }

        public async Task<List<BannerDTO>> GetBannerList(int Page, int PageSize)
        {

            var mapper = await CreateMapperForAdmin<Banner, BannerDTO>();
            var list = await uow.GetRepository<Banner>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<BannerDTO>>(list);

            return result;

        }

        public async Task<BannerDTO> SaveBanner(BannerDTO message)
        {

            var mapper = await CreateMapperForAdmin<Banner, BannerDTO>();
            var objcount = uow.GetRepository<Banner>()
                .GetAllActiveNonDeleted()
                .Count();
            var settingcount = await uow.GetRepository<SystemSetting>()
                .GetAllNonDeleted()
                .Where(x => x.SettingKey == ConstantKeys.AdminSettings.NumberOfBanners)
                .Select(x => x.SettingValue)
                .FirstOrDefaultAsync();

            if (message.IsActive == true)
            {
                if (settingcount!=null && objcount + 1 > int.Parse(settingcount))
                {
                    message.ResponseStatus = DBResult.ExceedRecord;
                    return message;
                }

            }


            Banner obj = new Banner();

            obj.TitleAr = message.TitleAr;
            obj.TitleEn = message.TitleEn;
            obj.TitleColor = message.TitleColor;
            obj.SummaryAr = message.SummaryAr;
            obj.SummaryEn = message.SummaryEn;
            obj.SummaryColor = message.SummaryColor;
            obj.UrlAr = message.UrlAr;
            obj.UrlEn = message.UrlEn;
            obj.BtnNameAr = message.BtnNameAr;
            obj.BtnNameEn = message.BtnNameEn;
            obj.ImgNameAr = message.ImgNameAr;
            if (message.ImgNameAr_UiFileName != null && message.ImgNameAr_UiFileName.Length > 45)
            {
                obj.ImgNameAr_UiFileName = message.ImgNameAr_UiFileName != null
                    ? message.ImgNameAr_UiFileName.Substring(0, 40) +
                      message.ImgNameAr_UiFileName.Substring(message.ImgNameAr_UiFileName.Length - 5)
                    : null;
                ;
            }
            else
            {
                obj.ImgNameAr_UiFileName = message.ImgNameAr_UiFileName;

            }

            obj.ImgNameAr_BlobURL = message.ImgNameAr_BlobURL;
            obj.ImgNameAr_FileExt = message.ImgNameAr_FileExt;
            obj.ImgNameAr_Size = message.ImgNameAr_Size;
            obj.ImgNameEn = message.ImgNameEn;
            if (message.ImgNameEn_UiFileName != null && message.ImgNameEn_UiFileName.Length > 45)
            {
                obj.ImgNameEn_UiFileName = message.ImgNameEn_UiFileName != null
                    ? message.ImgNameEn_UiFileName.Substring(0, 40) +
                      message.ImgNameEn_UiFileName.Substring(message.ImgNameEn_UiFileName.Length - 5)
                    : null;
                ;
            }
            else
            {
                obj.ImgNameEn_UiFileName = message.ImgNameEn_UiFileName;

            }

            obj.ImgNameEn_BlobURL = message.ImgNameEn_BlobURL;
            obj.ImgNameEn_FileExt = message.ImgNameEn_FileExt;
            obj.ImgNameEn_Size = message.ImgNameEn_Size;
            obj.Target = message.Target;
            obj.ShowAr = message.ShowAr;
            obj.ShowEn = message.ShowEn;
            obj.EndDate = message.EndDate;
            obj.StartDate = message.StartDate;
            obj.IsActive = message.IsActive;

            uow.GetRepository<Banner>().Insert(obj);
            await uow.CommitAsync();
            var result = mapper.Map<BannerDTO>(obj);
            result.UpdateBy = userInfo.DBName;
            result.ResponseStatus = DBResult.Inserted;

            return result;

        }

        public async Task<BannerDTO> UpdateBanner(BannerDTO message)
        {




            var mapper = await CreateMapperForAdmin<Banner, BannerDTO>();
            var result = new BannerDTO();
            if (message.Id is not null)
            {
                var objcount = uow.GetRepository<Banner>()
                    .GetAllActiveNonDeleted()
                    .Where(x => x.Id != message.Id)
                    .Count();
                var settingcount = await uow.GetRepository<SystemSetting>()
                    .GetAllNonDeleted()
                    .Where(x => x.SettingKey == ConstantKeys.AdminSettings.NumberOfBanners)
                    .Select(x => x.SettingValue)
                    .FirstOrDefaultAsync();

                if (message.IsActive == true && settingcount!=null)
                {
                    if (objcount + 1 > int.Parse(settingcount))
                    {

                        message.ResponseStatus = DBResult.ExceedRecord;
                        return message;
                    }

                }

                Banner obj = await uow.GetRepository<Banner>()
                    .GetAllNonDeleted()
                    .Include(x => x.CreateBy)
                    .Where(x => x.Id == message.Id)
                    .FirstAsync();

                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.TitleColor = message.TitleColor;
                obj.SummaryAr = message.SummaryAr;
                obj.SummaryEn = message.SummaryEn;
                obj.SummaryColor = message.SummaryColor;
                obj.UrlAr = message.UrlEn;
                obj.UrlEn = message.UrlEn;
                obj.BtnNameAr = message.BtnNameAr;
                obj.BtnNameEn = message.BtnNameEn;

                if (message.ImgNameAr != null)
                    obj.ImgNameAr = message.ImgNameAr;

                if (message.ImgNameAr_UiFileName != null && message.ImgNameAr_UiFileName.Length > 45)
                {
                    obj.ImgNameAr_UiFileName = message.ImgNameAr_UiFileName != null
                        ? message.ImgNameAr_UiFileName.Substring(0, 40) +
                          message.ImgNameAr_UiFileName.Substring(message.ImgNameAr_UiFileName.Length - 5)
                        : null;
                    ;
                }
                else if (message.ImgNameAr_UiFileName != null && message.ImgNameAr_UiFileName.Length < 45)
                {
                    obj.ImgNameAr_UiFileName = message.ImgNameAr_UiFileName;

                }

                if (message.ImgNameAr_BlobURL != null)
                    obj.ImgNameAr_BlobURL = message.ImgNameAr_BlobURL;

                if (message.ImgNameEn != null)
                    obj.ImgNameEn = message.ImgNameEn;

                if (message.ImgNameEn_UiFileName != null && message.ImgNameEn_UiFileName.Length > 45)
                {
                    obj.ImgNameEn_UiFileName = message.ImgNameEn_UiFileName != null
                        ? message.ImgNameEn_UiFileName.Substring(0, 40) +
                          message.ImgNameEn_UiFileName.Substring(message.ImgNameEn_UiFileName.Length - 5)
                        : null;
                    ;
                }
                else if (message.ImgNameEn_UiFileName != null && message.ImgNameEn_UiFileName.Length < 45)
                {
                    obj.ImgNameEn_UiFileName = message.ImgNameEn_UiFileName;

                }

                if (message.ImgNameEn_BlobURL != null)
                    obj.ImgNameEn_BlobURL = message.ImgNameEn_BlobURL;
                obj.ImgNameAr_FileExt = message.ImgNameAr_FileExt;
                obj.ImgNameAr_Size = message.ImgNameAr_Size;
                obj.ImgNameEn_FileExt = message.ImgNameEn_FileExt;
                obj.ImgNameEn_Size = message.ImgNameEn_Size;
                obj.Target = message.Target;
                obj.ShowAr = message.ShowAr;
                obj.ShowEn = message.ShowEn;
                obj.EndDate = message.EndDate;
                obj.StartDate = message.StartDate;
                obj.IsActive = message.IsActive;



                uow.GetRepository<Banner>().Update(obj);
                await uow.CommitAsync();
                result = mapper.Map<BannerDTO>(obj);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;

            }

            return result;

        }

        public async Task<bool> UpdateBannerOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                join rowToUpdate in uow.GetRepository<Banner>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate
                    .Id
                select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }

        public async Task<BannerDTO> DeleteBanner(Guid? Id)
        {
            var mapper = await CreateMapperForAdmin<Banner, BannerDTO>();
            var result = new BannerDTO();
            if (Id is null) return result;
            Banner obj = await uow.GetRepository<Banner>()
                .GetAllNonDeleted()
                .Where(x => x.Id == Id)
                .FirstAsync();
            uow.GetRepository<Banner>().Delete(obj);
            await uow.CommitAsync();
            result = mapper.Map<BannerDTO>(obj);
            result.ResponseStatus = DBResult.Deleted;
            return result;


        }

    }
}
