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

        public static readonly string ServiceRequestNotFound = "ServiceRequestNotFound";
        public static readonly string lblMajorOrDegreeIdMissing = "lblMajorOrDegreeIdMissing";
        public static readonly string lblInvalidGuidForMajorOrDegree = "lblInvalidGuidForMajorOrDegree";
        public static readonly string lbl_Request_is_not_Valid = "lbl_Request_is_not_Valid";
        public static readonly string lblRequestFieldsMissing = "lblRequestFieldsMissing";
        public static readonly string lblNoAvailableVacancySeat = "lblNoAvailableVacancySeat";
        public static readonly string lblUserGenderIDMissing = "lblUserGenderIDMissing";
        public static readonly string lblRequestNotValid = "lblRequestNotValid";
        public static readonly string lblNoPermissionForRequestStatus = "lblNoPermissionForRequestStatus";
        public static readonly string lblOnlyAuthorizedCanSeeHistory = "lblOnlyAuthorizedCanSeeHistory";
        public static readonly string lblNoDefaultStepFound = "lblNoDefaultStepFound";
        public static readonly string lblActionNotFound = "lblActionNotFound";
        public static readonly string lblQIDRequiredForNSIS = "lblQIDRequiredForNSIS";
        public static readonly string lblNoServiceStatusFound = "lblNoServiceStatusFound";
        public static readonly string lblActionConditionsNotMet = "lblActionConditionsNotMet";
        public static readonly string lblNoActionStatusConfiguration = "lblNoActionStatusConfiguration";
        public static readonly string lblMajorDegreeOrEntityIdMissing = "lblMajorDegreeOrEntityIdMissing";
        public static readonly string lblNoAccessToEvaluationRequest = "lblNoAccessToEvaluationRequest";
        public static readonly string lblRemarksRequired = "lblRemarksRequired";
        public static readonly string lblOtherAttachmentsRequired = "lblOtherAttachmentsRequired";
        public static readonly string lbl_you_are_not_authorized_to_perform_this_action_in_the_current_status = "lbl_you_are_not_authorized_to_perform_this_action_in_the_current_status";
        public static readonly string lblSomeFieldsCannotBePartOfAction = "lblSomeFieldsCannotBePartOfAction";
        public static readonly string lblRegistrationFailedOTPInterval = "lblRegistrationFailedOTPInterval";
        public static readonly string lblMaximumNewRowsExceeded = "lblMaximumNewRowsExceeded";
        public static readonly string lblMinimumNewRowsRequired = "lblMinimumNewRowsRequired";
        public static readonly string lblMinimumRowsRequired = "lblMinimumRowsRequired";
        public static readonly string lblMaximumRowsExceeded = "lblMaximumRowsExceeded";

        public static readonly string Attachment_NoFiles = "Attachment_NoFiles";
        public static readonly string Attachment_TooManyFiles = "Attachment_TooManyFiles";
        public static readonly string Attachment_InvalidExtension = "Attachment_InvalidExtension";
        public static readonly string Attachment_TooLarge = "Attachment_TooLarge";
        public static readonly string Attachment_InsertFailed = "Attachment_InsertFailed";
        public static readonly string IncompleteRequest = "IncompleteRequest";

        public static readonly string EmailtemplateNotFound = "EmailtemplateNotFound";
        public static readonly string SMSTemplateNotFound = "SMSTemplateNotFound";
        public static readonly string SMSProfileNotFound = "SMSProfileNotFound";

        public static readonly string ServiceNotFound = "ServiceNotFound";
        public static readonly string lblRequestAlreadyOpened = "lblRequestAlreadyOpened";
        public static readonly string UserInfoNotFound = "UserInfoNotFound";
        public static readonly string InActiveData = "InActiveData";
        public static readonly string UserPartyTypeNotFound = "UserPartyTypeNotFound";
        public static readonly string UserNotFound = "UserNotFound";
        public static readonly string lblNoPermissionForViewRequest = "lblNoPermissionForViewRequest";
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
        public static readonly string EvaluationRequestColumn = "EvaluationRequestColumn";
		public static readonly string SignatureUploadWidth = "SignatureUploadWidth";
		public static readonly string SignatureUploadHeight = "SignatureUploadHeight";
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

        #region EvaluationRequest
        public const string CanViewFieldHistory = "CanViewFieldHistory";
        public const string CanViewAllFieldHistory = "CanViewAllFieldHistory";
        public const string CanViewEvalFieldHistory = "CanViewEvalFieldHistory";
        #endregion

    }
    public static class WebAppSettings
    {
    }
    public static class WebAppCommon
    {
		public const string lblOk = "lblOk";
		public const string lblCancel = "lblCancel";
		public const string lblShowingEntries = "lblShowingEntries";
		public const string lblprevious = "lblprevious";
		public const string lblnext = "lblnext";
	}

	public static class EvaluationPlanRequests
	{
		public const string lblPlansListTitle = "lblPlansListTitle";
		public const string lblFilter = "lblFilter";
		public const string lblSearchPlan = "lblSearchPlan";
		public const string lblCreateNewPlan = "lblCreateNewPlan";

		public const string lblPlanEndDate = "lblPlanEndDate";
		public const string lblPlanStartDate = "lblPlanStartDate";
		public const string lblPlanCreatedOn = "lblPlanCreatedOn";
		public const string lblPlanSchoolsCount = "lblPlanSchoolsCount";
		public const string lblPlanStatus = "lblPlanStatus";
		public const string lblPlanName = "lblPlanName";

		public const string lblActions = "lblActions";
		public const string lblView = "lblView";
		public const string lblEdit = "lblEdit";
		public const string lblDelete = "lblDelete";

		public const string lblConfirmDelete = "lblConfirmDelete";
		public const string lblDeletePlanConfirm = "lblDeletePlanConfirm";

		public const string lblApplyFilter = "lblApplyFilter";
		public const string lblClear = "lblClear";
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

    public static class WebAppCacheTableName
    {

        public static readonly string ServiceStatus = "ServiceStatus";
        public static readonly string ServiceInitiatorPartyType = "ServiceInitiatorPartyType";
        public static readonly string SchServiceStatusConfiguration = "SchServiceStatusConfiguration";
        public static readonly string DropDown = "DropDown";
        public static readonly string ActionPartyType = "ActionPartyType";
        public static readonly string CACHE_DROPDOWNTYPE = "CACHE_DROPDOWNTYPE";
        public static readonly string CACHE_FIELDS = "CACHE_FIELDS";
        public static readonly string CACHE_ACTIONSTATUSCONFIG = "CACHE_ACTIONSTATUSCONFIG";
        public static readonly string CACHE_ALL_CLEAR = "CACHE_ALL_CLEAR";
        public static readonly string ActionPartyTypes = "ActionPartyTypes";
        public static readonly string SMSTemplates = "SMSTemplates";
        public static readonly string EmailTemplates = "EmailTemplates";
        public static readonly string SMSProfiles = "SMSProfiles";
        public static readonly string EmailProfiles = "EmailProfiles";
        public static readonly string CACHE_DROPDOWNVALUE = "CACHE_DROPDOWNVALUE";
        public static readonly string CACHE_ACADEMICYEAR = "CACHE_ACADEMICYEAR";
    }
    public static class LanguageConst
    {
        public static readonly string En = "En";
        public static readonly string Ar = "Ar";
    }

    public static class SystemSettings
    {
        public static readonly string ServiceSettings = "ServiceSettings";
        public static readonly string TimeFormat = "TimeFormat";
        public static readonly string ShortTimeFormat = "ShortTimeFormat";
        public static readonly string DateFormat = "DateFormat";
        public static readonly string DateTimeFormat = "DateTimeFormat";
        public static readonly string PageSize = "PageSize";
        public static readonly string ServiceRequestPageSize = "ServiceRequestPageSize";
        public static readonly string SchRestrictedStatuses = "SchRestrictedStatuses";
        public static readonly string ExceptionsPageSize = "ExceptionsPageSize";
        public static readonly string DropDownDataSourceAllowedTables = "DropDownDataSourceAllowedTables";
        public static readonly string DefaultMissingImage = "DefaultNullImage";
        public static readonly string AddAttachment_FILE_EXTENSION = "AddAttachment_FILE_EXTENSION";
        public static readonly string AddAttachment_FILE_SIZE = "AddAttachment_FILE_SIZE"; // in MB
        public static readonly string AddAttachment_FILE_COUNT = "AddAttachment_FILE_COUNT";
        public static readonly string useAsposeLib = "useAsposeLib";
    }


    public static class FieldTypeConstant
    {
        public const string label = "label";
        public const string text = "text";
        public const string textarea = "textarea";
        public const string jqte = "jqte";
        public const string Tiny = "Tiny";
        public const string number = "number";
        public const string date = "date";
        public const string datetime = "datetime";
        public const string checkbox = "checkbox";
        public const string radioButton = "radioButton";
        public const string dropdown = "dropdown";
        public const string select2 = "select2";
        public const string dual_select = "dual_select";
        public const string file = "file";
        public const string fileV2 = "fileV2";
        public const string dropzone = "dropzone";
        public const string table = "table";
        public const string list = "list";
    }
    public static class Module
    {
        public static readonly string AuthenticationModule = "AuthenticationModule";
        public static readonly string ConfigurationModule = "ConfigurationModule";
        public static readonly string Notification = "NotificationModule";
        public static readonly string Alert = "AlertModule";
        public static readonly string EvaluationModule = "EvaluationModule";
    }

    public static class ActionTypeKeys
    {
        public const string Approve = "APPROVE";
        public const string Reject = "REJECT";
        public const string Info = "INFO";
        public const string Assign = "ASSIGN";
        public const string CloseAndUpdate = "CLOSE_AND_UPDATE";
        public const string EDIT = "Edit";
        public const string RETURNBACK = "RETURNBACK";
        public const string EditDraft = "EditDraft";
        public const string Approve_And_Assign = "APPROVE_AND_ASSIGN";
        public const string SubmitMissingData = "SUBMIT_MISSING_DATA";
        public const string RequestDataChange = "REQUEST_DATA_CHANGE";
        public const string SaveAsDraft = "SaveAsDraft";
        public const string Close = "Close";
        public const string INFO_Override_Approve = "INFO_Override_Approve";
        public const string CreateEvaluationPlan = "CREATE_EVALUATION_PLAN";
        public const string UPDATE_ITEGRATION_FIELDS = "UPDATE_ITEGRATION_FIELDS";
        public const string INFO_WITH_DRAFT = "INFO_WITH_DRAFT";
    }


    public static class ServiceSettings
    {
        public static readonly string MaxCountOpen = "50";
    }
    public static class WebAppAccountConfigurations
    {
        public static readonly string ClearCacheDuration = "ClearCacheDuration";
    }

	public static class PlaceHolderTypes
	{

		public const string EvaluationField = "EvaluationField";
		public const string RequestField = "Request";
	}
	public static class EmailTemplateList
	{
		public static readonly string TestEmail = "TestEmail";
	}
}
