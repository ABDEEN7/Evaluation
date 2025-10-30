using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Enums
{
#pragma warning disable S3218, S101
    public static class ConstantKeys
    {
        public static class ExceptionMessage
        {
            public static readonly string InvalidEmailRequest = "InvalidEmailRequest";
            public static readonly string InvalidRequestEmptyCode = "InvalidRequestEmptyCode";
            public static readonly string NoEmailFound = "NoEmailFound";
            public static readonly string NoMinistryUserFound = "NoMinistryUserFound";
            public static readonly string NoTokenFound = "NoTokenFound";
            public static readonly string EmailIsRequired = "EmailIsRequired";
            public static readonly string UserDataNotFound = "UserDataNotFound";

            //Admin Exception Messages
            public static readonly string TITLE_CANNOT_BE_UNICODE = "TITLE_CANNOT_BE_UNICODE";
            public static readonly string SERVICE_FREEZED_CANNOT_EDIT = "SERVICE_FREEZED_CANNOT_EDIT";
            public static readonly string SERVICE_FREEZED_CANNOT_DELETE = "SERVICE_FREEZED_CANNOT_DELETE";
            public static readonly string ServiceExistsFormGroup = "ServiceExistsFormGroup";
            public static readonly string ServiceExistsServiceRequest = "ServiceExistsServiceRequest";
            public static readonly string ServiceExistsField = "ServiceExistsField";
            public static readonly string ServiceExistsServiceStatus = "ServiceExistsServiceStatus";
            public static readonly string ServiceExistsServiceAction = "ServiceExistsServiceAction";
            public static readonly string ServiceExistsEmailTemplate = "ServiceExistsEmailTemplate";
            public static readonly string SERVICE_FREEZED_CANNOT_ADD = "SERVICE_FREEZED_CANNOT_ADD";
        }

        public static class AdminSettings
        {
            public static readonly string EnableCaching = "EnableCaching";
            public static readonly string ClearCacheDuration = "ClearCacheDuration";
            public static readonly string ADMIN_FILE_SIZE = "ADMIN_FILE_SIZE";
            public static readonly string ADMIN_FILE_EXTENSION = "ADMIN_FILE_EXTENSION";
            public static readonly string ADMIN_FILE_COUNT = "ADMIN_FILE_COUNT";
            public static readonly string ADMIN_PAGE_SIZE = "ADMIN_PAGE_SIZE";
            public static readonly string AdminLogoAr = "AdminLogoAr";
            public static readonly string AdminLogoEn = "AdminLogoEn";
            public static readonly string WebLogoAr = "WebLogoAr";
            public static readonly string WebLogoEn = "WebLogoEn";
            public static readonly string Favicon = "Favicon";
            public static readonly string RequestColumn = "RequestColumn";
            public static readonly string ScholarshipColumn = "ScholarshipColumn";

            
        }

        public static class AdminBackendUI
        {
            public static readonly string ADMIN_MSG_FILE_SIZE = "ADMIN_MSG_FILE_SIZE";
            public static readonly string VALID_UPLOAD_TYPE = "VALID_UPLOAD_TYPE";
            public static readonly string SysLogoWebLogoAr = "SysLogoWebLogoAr";
            public static readonly string SysLogoWebLogoEn = "SysLogoWebLogoEn";
            public static readonly string SysLogoAdminLogoAr = "SysLogoAdminLogoAr";
            public static readonly string SysLogoAdminLogoEn = "SysLogoAdminLogoEn";
            public static readonly string SysLogoFavicon = "SysLogoFavicon";
            public static readonly string ADD_RECORD = "ADD_RECORD";
            public static readonly string BACK_BUTTON = "BACK_BUTTON";
            public static readonly string SAVE_BUTTON = "SAVE_BUTTON";
            public static readonly string ServiceAlreadyExists = "ServiceAlreadyExists";
        }

        public static class AdminPages
        {
            public static readonly string AdminCommon = "AdminCommon";
            public static readonly string AdminService = "AdminService";
            public static readonly string AdminPlaceHolder = "AdminPlaceHolder";
            public static readonly string AdminServiceFreeze = "AdminServiceFreeze";
        }

        public static class AdminPermission
        {
            //Permission for Service
            public const string ADD_ADMIN_SERVICE = "ADD_ADMIN_SERVICE";
            public const string EDIT_ADMIN_SERVICE = "EDIT_ADMIN_SERVICE";
            public const string DELETE_ADMIN_SERVICE = "DELETE_ADMIN_SERVICE";
            public const string VIEW_ADMIN_SERVICE = "VIEW_ADMIN_SERVICE";
            public const string FREEZE_ADMIN_SERVICE = "FREEZE_ADMIN_SERVICE";

            //Permission for Placeholder
            public const string ADD_ADMIN_SERVICE_PLACEHOLDER = "ADD_ADMIN_SERVICE_PLACEHOLDER";
            public const string EDIT_ADMIN_SERVICE_PLACEHOLDER = "EDIT_ADMIN_SERVICE_PLACEHOLDER";
            public const string DELETE_ADMIN_SERVICE_PLACEHOLDER = "DELETE_ADMIN_SERVICE_PLACEHOLDER";
            public const string VIEW_ADMIN_SERVICE_PLACEHOLDER = "VIEW_ADMIN_SERVICE_PLACEHOLDER";
        }
        public static class WebAppSettings
        {
        }

    }
}
