using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.WebsiteDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Evaluation.Services.BusinessLayer.API
{
    public class WebsiteBL : ApiBase
    {
        private WebGroupService _webGroupService;
        public WebsiteBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo, WebGroupService webGroupService)
            : base(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
        {
            _webGroupService = webGroupService;
        }
        public async Task<List<NavbarDTO>> GetNavbarList(string? deprouting)
        {
            string lang = requestInfo.Lang?? "ar";
            var loggedIn = userInfo.UserId is not null;

            var query = serviceProvider.CreateScopedUow()
        .GetRepository<Navbar>()
        .GetAllQueryFiltered()
        .Include(x => x.Permission)
        .AsNoTracking();

            if (!loggedIn)
            {
                query = query.Where(c => !c.IsAuthorized);
            }
            else
            {
                query = query.Where(x => x.DepartmentId == null || deprouting == x.Department.RoutingPath);
            }

            var flatNavbars = await query
        .OrderBy(c => c.OrderNo)
        .ThenByDescending(c => c.CreateDate)
        .Select(c => new NavbarDTO
        {
            Id = c.Id,
            Title = Lang == "ar" ? c.TitleAr : c.TitleEn,
            Url = Lang == "ar" ? c.UrlAr : c.UrlEn,
            ParentId = c.ParentId,
            IsInternal = c.IsInternal,
            OrderNo = c.OrderNo,
            Target = c.Target,
            IsAuthorized = c.IsAuthorized,
            PermissionBackendName = c.Permission != null ? c.Permission.BackendName : null
        })
        .ToListAsync();

            var permissionList = loggedIn ? userInfo.PermissionList ?? new List<string>() : new List<string>();

            return BuildTreeWithPermissions(flatNavbars, null, permissionList, loggedIn);
        }

        private List<NavbarDTO> BuildTreeWithPermissions(
            List<NavbarDTO> navbars,
            Guid? parentId,
            List<string> permissionList,
            bool loggedIn)
        {
            var result = new List<NavbarDTO>();

            foreach (var nav in navbars.Where(x => x.ParentId == parentId).OrderBy(x => x.OrderNo))
            {
                // Recursively build children
                var children = BuildTreeWithPermissions(navbars, nav.Id, permissionList, loggedIn);

                // Check if node is accessible
                bool hasOwnPermission = !nav.IsAuthorized || !loggedIn ||
                                (nav.PermissionBackendName != null && permissionList.Contains(nav.PermissionBackendName));

                // Keep node if it has permission OR any descendant is accessible
                bool shouldInclude = hasOwnPermission || children.Any();

                if (shouldInclude)
                {
                    result.Add(new NavbarDTO
                    {
                        Id = nav.Id,
                        Title = nav.Title,
                        ParentId = nav.ParentId,
                        IsInternal = nav.IsInternal,
                        OrderNo = nav.OrderNo,
                        Target = hasOwnPermission ? nav.Target : null,
                        Url = hasOwnPermission ? nav.Url : null,
                        IsAuthorized = nav.IsAuthorized,
                        PermissionBackendName = nav.PermissionBackendName,
                        IsAccessible = hasOwnPermission,
                        Children = children
                    });
                }
            }

            return result;
        }

        public async Task<List<BannerDTO>> GetBanners(string webGroupPath, string Lang = "ar")
        {
            var webGroup = await _webGroupService.GetWebGroupByPath(webGroupPath);

            if (webGroup == null)
                return null;

            string MissingImage = await cacheDataProvider.GetSystemSettingValue(ConstantKeys.WebAppSettings.DefaultWebsiteBannerImage);
            DateTime today = DateTime.Now;
            var qry = serviceProvider.CreateScopedUow().GetRepository<Banner>()
                    .GetAllQueryFiltered()
                .AsNoTracking()
                .OrderBy(c => c.OrderNo)
                .Where(c => today >= c.StartDate && (null == c.EndDate || c.EndDate.Value.AddDays(1) >= today));
            if (Lang == "ar")
            {
                qry = qry.Where(c => c.ShowAr);
            }
            else
            {
                qry = qry.Where(c => c.ShowEn);

            }

            qry = qry.Where(c => c.WebGroupId == webGroup.Id);

            var rslt = await qry.Select(c => new BannerDTO
            {
                Summary = Lang == "ar" ? c.SummaryAr : c.SummaryEn,
                SummaryColor = c.SummaryColor,
                Title = Lang == "ar" ? c.TitleAr : c.TitleEn,
                TitleColor = c.TitleColor,
                ImgName = Lang == "ar" ? c.ImgNameAr_UiFileName : c.ImgNameEn_UiFileName,
                ImgURL = Lang == "ar" ? c.ImgNameAr_BlobURL ?? MissingImage : c.ImgNameEn_BlobURL ?? MissingImage,
                ImgExt = Lang == "ar" ? c.ImgNameAr_FileExt : c.ImgNameEn_FileExt,
                Url = Lang == "ar" ? c.UrlAr : c.UrlEn,
                Target = c.Target,
                BtnName = Lang == "ar" ? c.BtnNameAr : c.BtnNameEn,
            }).ToListAsync();

            return rslt;
        }


    }
}