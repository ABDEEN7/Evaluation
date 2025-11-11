namespace Evaluation.SharedHelper.Enums;

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
            public static readonly string ExistsActionStatusConfigNotification = "ExistsActionStatusConfigNotification";
            public static readonly string ParentDoesNotExist = "ParentDoesNotExist";
            public static readonly string CannotDeleteItsParent = "CannotDeleteItsParent";
            public static readonly string InvalidRequest = "InvalidRequest";
            public static readonly string NoDataFound = "NoDataFound";
            public static readonly string ServiceActionUsed = "ServiceActionUsed";
            public static readonly string ActionExistsActionAssignPartyType = "ActionExistsActionAssignPartyType";
            public static readonly string ActionExistsActionCondition = "ActionExistsActionCondition";
            public static readonly string ActionExistsActionPartyType = "ActionExistsActionPartyType";
            public static readonly string ActionExistsActionShowLogPartyType = "ActionExistsActionShowLogPartyType";
            public static readonly string ActionExistsActionTemplateDoc = "ActionExistsActionTemplateDoc";
            public static readonly string ServiceStatusUsed = "ServiceStatusUsed";
            public static readonly string StatusExistsServiceStatusPreventPartyType = "StatusExistsServiceStatusPreventPartyType";
            public static readonly string DraftPlanWithSameAcademicYearAlreadyExists = "DraftPlanWithSameAcademicYearAlreadyExists";
            public static readonly string PlanIsNotFound = "PlanIsNotFound";
            public static readonly string InvalidApprovePlan = "InvalidApprovePlan";
            public static readonly string PlanInThePastIsNotAllowed = "PlanInThePastIsNotAllowed";
            public static readonly string InvalidDraftPlan = "InvalidDraftPlan";
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
            public static readonly string SessionExpireTime = "SessionExpireTime";

            
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
            public static readonly string DELETE_BUTTON = "DELETE_BUTTON";
            public static readonly string EDIT_BUTTON = "EDIT_BUTTON";
            public static readonly string ADMIN_CANCEL = "ADMIN_CANCEL";
            public static readonly string ServiceAlreadyExists = "ServiceAlreadyExists";
            public static readonly string lblSystemModule = "lblSystemModule";
            public static readonly string lblService = "lblService";
            public static readonly string lblSearch = "lblSearch";
            public static readonly string lblSelect = "lblSelect";
            public static readonly string lblStatusService = "lblStatusService";
            public static readonly string lblStatusesRoster = "lblStatusesRoster";
            public static readonly string lblStatusSearchForNames = "lblStatusSearchForNames";
            public static readonly string lblStatusDetails = "lblStatusDetails";
            public static readonly string StatusTab = "StatusTab";
            public static readonly string ActionStatusConfigTab = "ActionStatusConfigTab";
            public static readonly string lblStatusPartyTypeDisplayList = "lblStatusPartyTypeDisplayList";
        }

        public static class AdminPages
        {
            public static readonly string AdminCommon = "AdminCommon";
            public static readonly string AdminService = "AdminService";
            public static readonly string AdminPlaceHolder = "AdminPlaceHolder";
            public static readonly string AdminServiceFreeze = "AdminServiceFreeze";
            public static readonly string AdminActionStatusConfiguration = "AdminActionStatusConfiguration";
            public static readonly string AdminActionStatusConfigurationNotification = "AdminActionStatusConfigurationNotification";
            public static readonly string AdminServiceStatus = "AdminServiceStatus";
        }

        public static class AdminPermission
        {
            #region  Service
            public const string ADD_ADMIN_SERVICE = "ADD_ADMIN_SERVICE";
            public const string EDIT_ADMIN_SERVICE = "EDIT_ADMIN_SERVICE";
            public const string DELETE_ADMIN_SERVICE = "DELETE_ADMIN_SERVICE";
            public const string VIEW_ADMIN_SERVICE = "VIEW_ADMIN_SERVICE";
            public const string FREEZE_ADMIN_SERVICE = "FREEZE_ADMIN_SERVICE";
            #endregion

            #region  Placeholder
            public const string ADD_ADMIN_SERVICE_PLACEHOLDER = "ADD_ADMIN_SERVICE_PLACEHOLDER";
            public const string EDIT_ADMIN_SERVICE_PLACEHOLDER = "EDIT_ADMIN_SERVICE_PLACEHOLDER";
            public const string DELETE_ADMIN_SERVICE_PLACEHOLDER = "DELETE_ADMIN_SERVICE_PLACEHOLDER";
            public const string VIEW_ADMIN_SERVICE_PLACEHOLDER = "VIEW_ADMIN_SERVICE_PLACEHOLDER";
            #endregion

            #region ACTIONSTATUSCONFIGURATION

            public const string VIEW_ADMIN_ACTIONSTATUSCONFIGURATION = "VIEW_ADMIN_ACTIONSTATUSCONFIGURATION";
            public const string ADD_ADMIN_ACTIONSTATUSCONFIGURATION = "ADD_ADMIN_ACTIONSTATUSCONFIGURATION";
            public const string EDIT_ADMIN_ACTIONSTATUSCONFIGURATION = "EDIT_ADMIN_ACTIONSTATUSCONFIGURATION";
            public const string DELETE_ADMIN_ACTIONSTATUSCONFIGURATION = "DELETE_ADMIN_ACTIONSTATUSCONFIGURATION";

            #endregion

            #region ACTIONSTATUSCONFIGURATION_NOTIFICATION_NOTIFICATION

            public const string VIEW_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION =
                "VIEW_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION";

            public const string ADD_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION =
                "ADD_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION";

            public const string EDIT_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION =
                "EDIT_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION";

            public const string DELETE_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION =
                "DELETE_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION";

            #endregion

            #region SERVICE_STATUS

            public const string VIEW_ADMIN_SERVICE_STATUS = "VIEW_ADMIN_SERVICE_STATUS";
            public const string ADD_ADMIN_SERVICE_STATUS = "ADD_ADMIN_SERVICE_STATUS";
            public const string EDIT_ADMIN_SERVICE_STATUS = "EDIT_ADMIN_SERVICE_STATUS";
            public const string DELETE_ADMIN_SERVICE_STATUS = "DELETE_ADMIN_SERVICE_STATUS";

            public const string UPDATE_STATUS_PARTY_TYPE_DISPLAY_NAME_STATUS =
                "UPDATE_STATUS_PARTY_TYPE_DISPLAY_NAME_STATUS";

            #endregion

        }
        public static class WebAppSettings
        {
        }
    public static class WebAppCommon
        {
        }
	public static class WebAppLoginPage
		{
			public static readonly string lblEmail = "lblEmail";
			public static readonly string lblPassword = "lblPassword";
			public static readonly string lblContinue = "lblContinue";
			public static readonly string lblBackToEmail = "lblBackToEmail";
			public static readonly string lblLoginBtn = "lblLoginBtn";
			public static readonly string lblLoginTitle = "lblLoginTitle";
			public static readonly string lblForgotPasswordQuestion = "lblForgotPasswordQuestion";
			public static readonly string lblMobileResetPassword = "lblMobileResetPassword";
			public static readonly string lblRegisterNewAccount = "lblRegisterNewAccount";
		}

    public static class LanguageConst
    {
        public static readonly string En = "En";
        public static readonly string Ar = "Ar";
    }
}
