
using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.RegularExpressions;



namespace Evaluation.Services.Models.Admin
{
    public class SrvBaseBL : AdminBase
    {

        private readonly AzureBlobStorageService _blobService;
        private readonly SrvSystemSettingBL srvSystemSettingBL;

        public SrvBaseBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo, AzureBlobStorageService blobService, SrvSystemSettingBL srvSystemSettingBL)
            : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

            _blobService = blobService;
            this.srvSystemSettingBL = srvSystemSettingBL;
        }

        public async Task<List<UiControlDTO>> GetUiControls(List<string> pageNames, string lang, bool forceChangeLang = false, List<string>? backEndKeys = null)
        {
            var uiControls = await LoadUiControlsInCache(pageNames, lang);

            if (backEndKeys != null && backEndKeys.Count > 0)
            {
                uiControls = uiControls.Where(x => backEndKeys.Contains(x.BackEndName!)).ToList();
            }

            foreach (var item in uiControls)
            {
                item.txtValue = lang == "ar" ? item.ArValue : item.EnValue;
            }

            return uiControls;
        }

        public async Task<List<UiControlDTO>> LoadUiControlsInCache(List<string> PageName, string Lang)
        {
            //this.cacheService.RemoveCahe(PageName);
            if (PageName.Any())
            {
                UnitOfWork newuow = serviceProvider.CreateScopedUow();
                var _UiControles = await newuow.GetRepository<UiControl>().GetAllNonDeleted()
                                               .Where(x => PageName.Contains(x.PageName))
                       .Select(c => new UiControlDTO
                       {
                           Id = c.Id,
                           BackEndName = c.BackendName,
                           PageName = c.PageName,
                           ControlName = c.ControlName,
                           ArValue = c.ValueAr != null ? c.ValueAr.Trim() : string.Empty,
                           EnValue = c.ValueEn != null ? c.ValueEn.Trim() : string.Empty,
                           txtValue = Lang == "ar" ? (c.ValueAr != null ? c.ValueAr.Trim() : string.Empty): (c.ValueEn != null ? c.ValueEn.Trim() : string.Empty),
                           Url = c.Url
                       }).ToListAsync();
                return _UiControles;
            }
            else
            {
                return new List<UiControlDTO>();
            }


        }

        public bool CheckUploadExt(string FileExt, string UploadType)
        {
            string[] extarry = UploadType.ToLower().Split(',');
            bool check = extarry.Any(c => c.Contains(FileExt.ToLower()));
            return check;
        }
        public static bool IsValidJson(string jsonString)
        {
            try
            {
                JsonDocument.Parse(jsonString); // Try parsing the JSON
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
        public async Task<bool> ValidateObject(object obj, string permission)
        {
            var constraintList = await srvSystemSettingBL.GetAppConstraints(permission);

            if (constraintList != null && obj != null)
            {
                foreach (var propInfo in obj.GetType().GetProperties())
                {
                    ControlValidationDTO? validation = constraintList.FirstOrDefault(x => x.ControlName == propInfo.Name);

                    if (validation != null)
                    {
                        var data = propInfo.GetType();
                        var propValue = propInfo.GetValue(obj);
                        if (validation.IsRequired && (validation.Dbrequired ?? false) && (propValue == null || string.IsNullOrEmpty(propValue.ToString()) || string.IsNullOrWhiteSpace(propValue.ToString())))
                        {
                            return false;
                        }


                        if (validation.MaxLength != null && propValue is string MaxstringValue)
                        {
                            MaxstringValue = MaxstringValue.Trim();
                            if (!string.IsNullOrEmpty(MaxstringValue))
                            {
                                if (MaxstringValue.Length > validation.MaxLength)
                                {
                                    return false;
                                }
                            }
                        }
                        if (validation.MinLength != null && propValue is string MinstringValue)
                        {
                            MinstringValue = MinstringValue.Trim();
                            if (!string.IsNullOrEmpty(MinstringValue))
                            {

                                if (MinstringValue.Length < validation.MinLength)
                                    return false;
                            }
                        }

                        if (!string.IsNullOrEmpty(validation.Regex) && propValue is string RegexstringValue)
                        {
                            bool isValid = Regex.IsMatch(RegexstringValue, validation.Regex);
                            if (!isValid)
                            {
                                return false;
                            }
                        }
                        if (validation.ControlType == "JSON_AREA" && propValue is string jsonstringValue)
                        {
                            bool isValid = IsValidJson(jsonstringValue);
                            if (!isValid)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            return true;
        }

        public async Task<FileResponse> ValidateFiles(object model, IEnumerable<ControlValidationDTO> constraintList, List<IFormFile> FinalFiles, bool uploadFiles = true)
        {
            var response = new FileResponse();
            response.ResponseStatus = true;
            var fileControlList = constraintList.Where(x => x.ControlType == "FILEUPLOAD" || x.ControlType == "DROPZONE").ToList();
            foreach (var item in FinalFiles)
            {
                var controlsetting = fileControlList.Where(x => (x.ControlType == "FILEUPLOAD" ? x.UibackendName == item.Name : x.ControlName == item.Name)).FirstOrDefault();
                if (controlsetting != null)
                {
                    var MaxFileSize = controlsetting.FileSize ?? long.Parse(srvSystemSettingBL.GetSetting(ConstantKeys.AdminSettings.ADMIN_FILE_SIZE));
                    var AttachmentType = controlsetting.FileExtention ?? srvSystemSettingBL.GetSetting(ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION);

                    long Filesize = item.Length;
                    if (Filesize > 0 && Filesize > MaxFileSize * 1024)
                    {
                        var msg = await GetUiMessage(ConstantKeys.AdminBackendUI.ADMIN_MSG_FILE_SIZE);
                        response.ResponseMessage = string.Format(msg, MaxFileSize / 1024);

                        response.ResponseStatus = false;


                    }

                    string EXT = Path.GetExtension(item.FileName).Replace(".", "").ToLower();

                    if (!CheckUploadExt(EXT, AttachmentType))
                    {
                        var message = await GetUiMessage(ConstantKeys.AdminBackendUI.VALID_UPLOAD_TYPE);
                        response.ResponseMessage = string.Format(message, AttachmentType);
                        response.ResponseStatus = false;


                    }
                }
                if (uploadFiles)
                {
                    FileVm uploads = await _blobService.UploadFileAsync(item,StorageContainerType.website);
                    if (uploads != null)
                    {
                        long fileLength = uploads.FileLength ?? 0;
                        string BlobUrl = _blobService.GenerateSasToken(uploads.CustomFileName, 0,uploads.FileName,false,StorageContainerType.website);
                        SetPropertyValue(model, controlsetting!.ControlName, uploads.CustomFileName);
                        SetPropertyValue(model, controlsetting.ControlName + "_UiFileName", uploads.FileName);
                        SetPropertyValue(model, controlsetting.ControlName + "_BlobURL", BlobUrl);
                        SetPropertyValue(model, controlsetting.ControlName + "_FileExt", uploads.FileType.Replace(".", "").ToLower());
                        SetPropertyValue(model, controlsetting.ControlName + "_Size", fileLength);
                    }
                }
            }
            response.Data = model;
            return response;

        }

       

        public async Task<CustomFileResponse> ValidateAttachment(IEnumerable<ControlValidationDTO> constraintList, List<IFormFile> FinalFiles)
        {
            var response = new CustomFileResponse();
            response.ResponseStatus = true;
            var fileControlList = constraintList.Where(x => x.ControlType == "FILEUPLOAD" || x.ControlType == "DROPZONE").ToList();
            List<AttachmentDTO> model = new List<AttachmentDTO>();
            foreach (var item in FinalFiles)
            {
                AttachmentDTO attachment = new AttachmentDTO();
                var controlsetting = fileControlList.Where(x => (x.ControlType == "FILEUPLOAD" ? x.UibackendName == item.Name : x.ControlName == item.Name)).FirstOrDefault();
                if (controlsetting != null)
                {
                    var MaxFileSize = controlsetting.FileSize ?? long.Parse(srvSystemSettingBL.GetSetting(ConstantKeys.AdminSettings.ADMIN_FILE_SIZE));
                    var AttachmentType = controlsetting.FileExtention ?? srvSystemSettingBL.GetSetting(ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION);

                    long Filesize = item.Length;
                    if (Filesize > 0 && Filesize > MaxFileSize * 1024)
                    {
                        var msg = await GetUiMessage(ConstantKeys.AdminBackendUI.ADMIN_MSG_FILE_SIZE);
                        response.ResponseMessage = string.Format(msg, MaxFileSize / 1024);

                        response.ResponseStatus = false;


                    }

                    string EXT = Path.GetExtension(item.FileName).Replace(".", "").ToLower();

                    if (!CheckUploadExt(EXT, AttachmentType))
                    {
                        var message = await GetUiMessage(ConstantKeys.AdminBackendUI.VALID_UPLOAD_TYPE);
                        response.ResponseMessage = string.Format(message, AttachmentType);
                        response.ResponseStatus = false;


                    }
                }

                FileVm uploads = await _blobService.UploadFileAsync(item);
                if (uploads != null)
                {
                    long fileLength = uploads.FileLength ?? 0;

                    string BlobUrl = _blobService.GenerateSasToken(uploads.CustomFileName, 0);
                    attachment.FileName = uploads.CustomFileName;
                    attachment.UiFileName = uploads.FileName;
                    attachment.BlobUrl = BlobUrl;
                    attachment.FileExtension = uploads.FileType.Replace(".", "").ToLower();
                    attachment.FileSize = fileLength;
                    attachment.ControlFileName = item.Name;
                    model.Add(attachment);

                }
            }
            response.Data = model;
            return response;

        }
       
       
      

        public async Task<FileResponse> ValidateSysLogo(SysLogoDTO model, IEnumerable<ControlValidationDTO> constraintList, List<IFormFile> FinalFiles)
        {
            var response = new FileResponse();
            response.ResponseStatus = true;
            var fileControlList = constraintList.Where(x => x.ControlType == "FILEUPLOAD" || x.ControlType == "DROPZONE").ToList();
            foreach (var item in FinalFiles)
            {
                var controlsetting = fileControlList.Where(x => (x.ControlType == "FILEUPLOAD" ? x.UibackendName == item.Name : x.ControlName == item.Name)).FirstOrDefault();
                if (controlsetting != null)
                {
                    var MaxFileSize = controlsetting.FileSize ?? long.Parse(srvSystemSettingBL.GetSetting(ConstantKeys.AdminSettings.ADMIN_FILE_SIZE));
                    var AttachmentType = controlsetting.FileExtention ?? srvSystemSettingBL.GetSetting(ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION);

                    long Filesize = item.Length;
                    if (Filesize > 0 && Filesize > MaxFileSize * 1024)
                    {
                        var msg = await GetUiMessage(ConstantKeys.AdminBackendUI.ADMIN_MSG_FILE_SIZE);
                        response.ResponseMessage = string.Format(msg, MaxFileSize / 1024);

                        response.ResponseStatus = false;


                    }

                    string EXT = Path.GetExtension(item.FileName).Replace(".", "").ToLower();

                    if (!CheckUploadExt(EXT, AttachmentType))
                    {
                        var message = await GetUiMessage(ConstantKeys.AdminBackendUI.VALID_UPLOAD_TYPE);
                        response.ResponseMessage = string.Format(message, AttachmentType);
                        response.ResponseStatus = false;


                    }
                }
                FileVm uploads = await _blobService.UploadFileAsync(item,StorageContainerType.website);
                if (uploads != null)
                {
                    string BlobUrl = _blobService.GenerateSasToken(uploads.CustomFileName, 0,uploads.FileName,false,StorageContainerType.website);
                    if (item.Name == ConstantKeys.AdminBackendUI.SysLogoWebLogoAr)
                    {
                        model.WebLogoAr = BlobUrl;
                    }
                    else if (item.Name == ConstantKeys.AdminBackendUI.SysLogoWebLogoEn)
                    {
                        model.WebLogoEn = BlobUrl;
                    }
                    else if (item.Name == ConstantKeys.AdminBackendUI.SysLogoAdminLogoAr)
                    {
                        model.AdminLogoAr = BlobUrl;
                    }
                    else if (item.Name == ConstantKeys.AdminBackendUI.SysLogoAdminLogoEn)
                    {
                        model.AdminLogoEn = BlobUrl;
                    }
                    else if (item.Name == ConstantKeys.AdminBackendUI.SysLogoFavicon)
                    {
                        model.Favicon = BlobUrl;
                    }
                }
            }
            response.Data = model;
            return response;

        }
        private void SetPropertyValue<T>(T model, string propertyName, object value)
        {

            var property = model?.GetType().GetProperty(propertyName);
            if (property != null)
            {

                property.SetValue(model, value);
            }
        }
        public async Task<List<SystemModuleDTO>> GetAllSystemModules()
        {
            var result = await uow.GetRepository<SystemModule>()
                                  .GetAllNonDeleted()
                                  .OrderBy(x => x.OrderNo)
                                  .ThenByDescending(x => x.CreateDate)
                                  .Select(x => new SystemModuleDTO
                                  {
                                      Id = x.Id,
                                      Name = _requestInfo.Lang == "ar" ? x.NameAr : x.NameEn,

                                  }).ToListAsync();


            return result;

        }


        public async Task<List<ServiceDTO>> GetAllServices()
        {
            var result = await uow.GetRepository<Service>()
                                  .GetAllNonDeleted()
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                .Select(x => new ServiceDTO
                {
                    Id = x.Id,
                    Name = _requestInfo.Lang == "ar" ? x.NameAr : x.NameEn,
                    SystemModuleId = x.SystemModuleId,
                    IsFreez = x.IsFreez
                }).ToListAsync();

            return result;
        }

       
    }
}
