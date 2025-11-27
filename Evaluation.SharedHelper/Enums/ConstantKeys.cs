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
        public static readonly string DepartmentExistsSystemModule = "DepartmentExistsSystemModule";
        public static readonly string DropDownTypeHasDataSource = "DropDownTypeHasDataSource";
        public static readonly string FieldExistsFieldVisabilityConfig = "FieldExistsFieldVisabilityConfig";
        public static readonly string FieldExistsFieldAttributeValue = "FieldExistsFieldAttributeValue";
        public static readonly string FieldExistsDropDownParentField = "FieldExistsDropDownParentField";
        public static readonly string FieldExistsFieldPartyType = "FieldExistsFieldPartyType";
        public static readonly string FieldExistsFieldViewCondition = "FieldExistsFieldViewCondition";
        public static readonly string FieldExistsPlaceHolder = "FieldExistsPlaceHolder";
        public static readonly string AcademicYearExistsScope = "AcademicYearExistsScope";
        public static readonly string DepartmentExistsAcademicYear = "DepartmentExistsAcademicYear";
        public static readonly string SameUserpartyTypeExists = "SameUserpartyTypeExists";
        public static readonly string UserPartyTypeExistsUserPartyTypeSignature = "UserPartyTypeExistsUserPartyTypeSignature";
        public static readonly string UserPartyTypeSignatureWidthError = "UserPartyTypeSignatureWidthError";
        public static readonly string UserPartyTypeSignatureHeightError = "UserPartyTypeSignatureHeightError";
        public static readonly string PartyTypeCannotDelete = "PartyTypeCannotDelete";
        public static readonly string PartyTypeExistsServiceInitiatorPartyType = "PartyTypeExistsServiceInitiatorPartyType";
        public static readonly string PartyTypeExistsServiceStatusPartyTypeDisplayName = "PartyTypeExistsServiceStatusPartyTypeDisplayName";
        public static readonly string PartyTypeExistsServiceRequestShowPartyType = "PartyTypeExistsServiceRequestShowPartyType";
        public static readonly string PartyTypeExistsServiceStatusPreventPartyType = "PartyTypeExistsServiceStatusPreventPartyType";
        public static readonly string PartyTypeExistsActionAssignPartyType = "PartyTypeExistsActionAssignPartyType";
        public static readonly string PartyTypeExistsActionPartyType = "PartyTypeExistsActionPartyType";
        public static readonly string PartyTypeExistsActionShowLogPartyType = "PartyTypeExistsActionShowLogPartyType";
        public static readonly string PartyTypeExistsFieldPartyType = "PartyTypeExistsFieldPartyType";
        public static readonly string PartyTypeExistsActionStatusConfigNotification = "PartyTypeExistsActionStatusConfigNotification";
        public static readonly string NavbarExistsSiteContent = "NavbarExistsSiteContent";
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
            public static readonly string EvaluationColumn = "EvaluationColumn";
            public static readonly string SessionExpireTime = "SessionExpireTime";
            public static readonly string PlaceHolderType = "PlaceHolderType";
        public static readonly string SignatureUploadWidth = "SignatureUploadWidth";
        public static readonly string SignatureUploadHeight = "SignatureUploadHeight";
        public static readonly string OPERATORS_LIST = "OPERATORS_LIST";
        public static readonly string ActionConditionType = "ActionConditionType";
        public static readonly string UserPartyTypeSignatureHeight = "UserPartyTypeSignatureHeight";
        public static readonly string UserPartyTypeSignatureWidth = "UserPartyTypeSignatureWidth";
        public static readonly string NumberOfBanners = "NumberOfBanners";
        public static readonly string TargetValue = "TargetValue";
        public static readonly string NumberOfNavbars = "NumberOfNavbars";
        public static readonly string NavBarPermissionList = "NavBarPermissionList";
        public static readonly string RoutingValue = "RoutingValue";
        public static readonly string WebAppSitePath = "WebAppSitePath";
        

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
        public static readonly string ADD_NEW_FORM_GROUP = "ADD_NEW_FORM_GROUP";
        public static readonly string FormGroupTabTitle = "FormGroupTabTitle";
        public static readonly string ListTabTitle = "ListTabTitle";
        public static readonly string SearchFormGroup = "SearchFormGroup";
        public static readonly string EvaluationActionDetails = "EvaluationActionDetails";
        public static readonly string EvaluationActionFieldDetails = "EvaluationActionFieldDetails";
        public static readonly string ActionConditionTab = "ActionConditionTab";
        public static readonly string EvaluationActionsField = "EvaluationActionsField";
        public static readonly string ActionTab = "ActionTab";
        public static readonly string EvaluationActionsService = "EvaluationActionsService";
        public static readonly string EvaluationActionsRoster = "EvaluationActionsRoster";
        public static readonly string EvaluationActionsFieldsRoster = "EvaluationActionsFieldsRoster";
        public static readonly string lblSearchTable = "lblSearchTable";
        public static readonly string lblSearchForNames = "lblSearchForNames";
        public static readonly string lblSearchTree = "lblSearchTree";
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
            public static readonly string AdminDropDown = "AdminDropDown";
            public static readonly string AdminDepartment = "AdminDepartment";
            public static readonly string AdminFormGroup = "AdminFormGroup";
            public static readonly string AdminField = "AdminField";
            public static readonly string AdminFieldAttribute = "AdminFieldAttribute";
            public static readonly string AdminFieldCondition = "AdminFieldCondition";
            public static readonly string AdminEvaluationAction = "AdminEvaluationAction";
            public static readonly string AdminActionFieldAttribute = "AdminActionFieldAttribute";
            public static readonly string AdminActionCondition = "AdminActionCondition";
        public static readonly string AdminAcademicYear = "AdminAcademicYear";
        public static readonly string AdminAcademicYearScope = "AdminAcademicYearScope";
        public static readonly string AdminScopeAcademicYear = "AdminScopeAcademicYear";
        public static readonly string AdminUserPartyType = "AdminUserPartyType";
        public static readonly string AdminUserPartyTypeSignature = "AdminUserPartyTypeSignature";
        public static readonly string AdminPartyType = "AdminPartyType";
        public static readonly string AdminBanner = "AdminBanner";
        public static readonly string AdminNavbar = "AdminNavbar";
        public static readonly string AdminSiteDocument = "AdminSiteDocument";
    }

    public static class AdminPermission
    {
        public const string VIEW_ADMIN_HOME = "VIEW_ADMIN_HOME";
        
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
        #region DROPDOWN

        public const string VIEW_ADMIN_DROPDOWN = "VIEW_ADMIN_DROPDOWN";
        public const string ADD_ADMIN_DROPDOWN = "ADD_ADMIN_DROPDOWN";
        public const string EDIT_ADMIN_DROPDOWN = "EDIT_ADMIN_DROPDOWN";
        public const string DELETE_ADMIN_DROPDOWN = "DELETE_ADMIN_DROPDOWN";

        #endregion

        #region DEPARTMENT

        public const string VIEW_ADMIN_DEPARTMENT = "VIEW_ADMIN_DEPARTMENT";
        public const string ADD_ADMIN_DEPARTMENT = "ADD_ADMIN_DEPARTMENT";
        public const string EDIT_ADMIN_DEPARTMENT = "EDIT_ADMIN_DEPARTMENT";
        public const string DELETE_ADMIN_DEPARTMENT = "DELETE_ADMIN_DEPARTMENT";

        #endregion

        #region EvaluationRequest
        public const string CanViewFieldHistory = "CanViewFieldHistory";
        public const string CanViewAllFieldHistory = "CanViewAllFieldHistory";
        public const string CanViewEvalFieldHistory = "CanViewEvalFieldHistory";
        #endregion

        #region FormGroup
        public const string VIEW_ADMIN_FORMGROUP = "VIEW_ADMIN_FORMGROUP";
        public const string ADD_ADMIN_FORMGROUP = "ADD_ADMIN_FORMGROUP";
        public const string EDIT_ADMIN_FORMGROUP = "EDIT_ADMIN_FORMGROUP";
        public const string DELETE_ADMIN_FORMGROUP = "DELETE_ADMIN_FORMGROUP";
        #endregion
        #region FIELD

        public const string VIEW_ADMIN_FIELD = "VIEW_ADMIN_FIELD";
        public const string ADD_ADMIN_FIELD = "ADD_ADMIN_FIELD";
        public const string EDIT_ADMIN_FIELD = "EDIT_ADMIN_FIELD";
        public const string DELETE_ADMIN_FIELD = "DELETE_ADMIN_FIELD";

        #endregion
        #region FIELD_ATTRIBUTE

        public const string VIEW_ADMIN_FIELD_ATTRIBUTE = "VIEW_ADMIN_FIELD_ATTRIBUTE";
        public const string ADD_ADMIN_FIELD_ATTRIBUTE = "ADD_ADMIN_FIELD_ATTRIBUTE";
        public const string EDIT_ADMIN_FIELD_ATTRIBUTE = "EDIT_ADMIN_FIELD_ATTRIBUTE";
        public const string DELETE_ADMIN_FIELD_ATTRIBUTE = "DELETE_ADMIN_FIELD_ATTRIBUTE";

        #endregion

        #region FIELD_CONDITION

        public const string VIEW_ADMIN_FIELD_CONDITION = "VIEW_ADMIN_FIELD_CONDITION";
        public const string ADD_ADMIN_FIELD_CONDITION = "ADD_ADMIN_FIELD_CONDITION";
        public const string EDIT_ADMIN_FIELD_CONDITION = "EDIT_ADMIN_FIELD_CONDITION";
        public const string DELETE_ADMIN_FIELD_CONDITION = "DELETE_ADMIN_FIELD_CONDITION";

        #endregion

        #region ACTION

        public const string VIEW_ADMIN_ACTION = "VIEW_ADMIN_ACTION";
        public const string ADD_ADMIN_ACTION = "ADD_ADMIN_ACTION";
        public const string EDIT_ADMIN_ACTION = "EDIT_ADMIN_ACTION";
        public const string DELETE_ADMIN_ACTION = "DELETE_ADMIN_ACTION";
        public const string VIEW_ADMIN_ACTION_FIELD = "VIEW_ADMIN_ACTION_FIELD";
        public const string UPDATE_ADMIN_ACTION_FIELD = "UPDATE_ADMIN_ACTION_FIELD";

        #endregion

        #region ACTION_FIELD_ATTRIBUTE

        public const string VIEW_ADMIN_ACTION_FIELD_ATTRIBUTE = "VIEW_ADMIN_ACTION_FIELD_ATTRIBUTE";
        public const string ADD_ADMIN_ACTION_FIELD_ATTRIBUTE = "ADD_ADMIN_ACTION_FIELD_ATTRIBUTE";
        public const string EDIT_ADMIN_ACTION_FIELD_ATTRIBUTE = "EDIT_ADMIN_ACTION_FIELD_ATTRIBUTE";
        public const string DELETE_ADMIN_ACTION_FIELD_ATTRIBUTE = "DELETE_ADMIN_ACTION_FIELD_ATTRIBUTE";

        #endregion

        #region ACTIONCONDITION

        public const string VIEW_ADMIN_ACTIONCONDITION = "VIEW_ADMIN_ACTIONCONDITION";
        public const string ADD_ADMIN_ACTIONCONDITION = "ADD_ADMIN_ACTIONCONDITION";
        public const string EDIT_ADMIN_ACTIONCONDITION = "EDIT_ADMIN_ACTIONCONDITION";
        public const string DELETE_ADMIN_ACTIONCONDITION = "DELETE_ADMIN_ACTIONCONDITION";

        #endregion

        #region ACADEMIC_YEAR

        public const string DELETE_ADMIN_ACADEMIC_YEAR = "DELETE_ADMIN_ACADEMIC_YEAR";
        public const string ADD_ADMIN_ACADEMIC_YEAR = "ADD_ADMIN_ACADEMIC_YEAR";
        public const string EDIT_ADMIN_ACADEMIC_YEAR = "EDIT_ADMIN_ACADEMIC_YEAR";
        public const string VIEW_ADMIN_ACADEMIC_YEAR = "VIEW_ADMIN_ACADEMIC_YEAR";

        #endregion

        #region ACADEMIC_YEAR_SCOPE

        public const string DELETE_ADMIN_ACADEMIC_YEAR_SCOPE = "DELETE_ADMIN_ACADEMIC_YEAR_SCOPE";
        public const string ADD_ADMIN_ACADEMIC_YEAR_SCOPE = "ADD_ADMIN_ACADEMIC_YEAR_SCOPE";
        public const string EDIT_ADMIN_ACADEMIC_YEAR_SCOPE = "EDIT_ADMIN_ACADEMIC_YEAR_SCOPE";
        public const string VIEW_ADMIN_ACADEMIC_YEAR_SCOPE = "VIEW_ADMIN_ACADEMIC_YEAR_SCOPE";

        #endregion

        #region SCOPE_ACADEMIC_YEAR

        public const string DELETE_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE = "DELETE_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE";
        public const string ADD_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE = "ADD_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE";
        public const string EDIT_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE = "EDIT_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE";
        public const string VIEW_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE = "VIEW_ADMIN_SCOPE_ACADEMIC_YEAR_SCOPE";

        #endregion

        #region USERPARTYTYPE

        public const string VIEW_ADMIN_USERPARTYTYPE = "VIEW_ADMIN_USERPARTYTYPE";
        public const string ADD_ADMIN_USERPARTYTYPE = "ADD_ADMIN_USERPARTYTYPE";
        public const string EDIT_ADMIN_USERPARTYTYPE = "EDIT_ADMIN_USERPARTYTYPE";
        public const string DELETE_ADMIN_USERPARTYTYPE = "DELETE_ADMIN_USERPARTYTYPE";

        #endregion

        #region SIGNATUREUSERPARTYTYPE

        public const string VIEW_ADMIN_USERPARTYTYPE_SIGNATURE = "VIEW_ADMIN_USERPARTYTYPE_SIGNATURE";
        public const string ADD_ADMIN_USERPARTYTYPE_SIGNATURE = "ADD_ADMIN_USERPARTYTYPE_SIGNATURE";
        public const string EDIT_ADMIN_USERPARTYTYPE_SIGNATURE = "EDIT_ADMIN_USERPARTYTYPE_SIGNATURE";
        public const string DELETE_ADMIN_USERPARTYTYPE_SIGNATURE = "DELETE_ADMIN_USERPARTYTYPE_SIGNATURE";

        #endregion

        #region PARTYTYPE

        public const string VIEW_ADMIN_PARTYTYPE = "VIEW_ADMIN_PARTYTYPE";
        public const string ADD_ADMIN_PARTYTYPE = "ADD_ADMIN_PARTYTYPE";
        public const string EDIT_ADMIN_PARTYTYPE = "EDIT_ADMIN_PARTYTYPE";
        public const string DELETE_ADMIN_PARTYTYPE = "DELETE_ADMIN_PARTYTYPE";

        #endregion
        #region BANNER
        public const string VIEW_ADMIN_BANNER = "VIEW_ADMIN_BANNER";
        public const string ADD_ADMIN_BANNER = "ADD_ADMIN_BANNER";
        public const string EDIT_ADMIN_BANNER = "EDIT_ADMIN_BANNER";
        public const string DELETE_ADMIN_BANNER = "DELETE_ADMIN_BANNER";
        #endregion
        #region NAVBAR

        public const string VIEW_ADMIN_NAVBAR = "VIEW_ADMIN_NAVBAR";
        public const string ADD_ADMIN_NAVBAR = "ADD_ADMIN_NAVBAR";
        public const string EDIT_ADMIN_NAVBAR = "EDIT_ADMIN_NAVBAR";
        public const string DELETE_ADMIN_NAVBAR = "DELETE_ADMIN_NAVBAR";

        #endregion
        #region SITEDOCUMENT

        public const string VIEW_ADMIN_SITEDOCUMENT = "VIEW_ADMIN_SITEDOCUMENT";
        public const string ADD_ADMIN_SITEDOCUMENT = "ADD_ADMIN_SITEDOCUMENT";
        public const string EDIT_ADMIN_SITEDOCUMENT = "EDIT_ADMIN_SITEDOCUMENT";
        public const string DELETE_ADMIN_SITEDOCUMENT = "DELETE_ADMIN_SITEDOCUMENT";

        #endregion

    }
    public static class CustomDataSource
    {
    }
    public static class WebAppSettings
    {
        public static readonly string PAGE_SIZE = "PAGE_SIZE";
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
