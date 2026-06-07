using Evaluation.DAL.Models.DepartementEntites;

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
        public static readonly string MissingPlan = "MissingPlan";
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
        public static readonly string InvalidPlan = "InvalidPlan";
        public static readonly string InvalidEvaluationDate = "InvalidEvaluationDate";
        public static readonly string PlanInThePastIsNotAllowed = "PlanInThePastIsNotAllowed";
        public static readonly string InvalidDraftPlan = "InvalidDraftPlan";
        public static readonly string InvalidAssignment = "InvalidAssignment";
        public static readonly string OneLeader = "OneLeader";
        public static readonly string AtLeastHaveOneScopes = "AtLeastHaveOneScopes";

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
        public static readonly string Requiredfield = "Requiredfield";
        public static readonly string ExceedMaxlength = "ExceedMaxlength";
        public static readonly string BelowMinlength = "BelowMinlength";
        public static readonly string InvalidRegex = "InvalidRegex";
        public static readonly string InvalidJson = "InvalidJson";
        public static readonly string NotoficationTemplateExistsActionStatusConfigNotification = "NotoficationTemplateExistsActionStatusConfigNotification";
        public static readonly string SmsTemplateExistsActionStatusConfigNotification = "SmsTemplateExistsActionStatusConfigNotification";
        public static readonly string EmailTemplateExistsEmailTemplateDocument = "EmailTemplateExistsEmailTemplateDocument";
        public static readonly string EmailTemplateExistsActionStatusConfigNotification = "EmailTemplateExistsActionStatusConfigNotification";
        public static readonly string TemplateDocExistsEmailTemplateDocument = "TemplateDocExistsEmailTemplateDocument";
        public static readonly string TemplateDocExistsActionTemplateDoc = "TemplateDocExistsActionTemplateDoc";
        public static readonly string BackendNameAlreadyExists = "BackendNameAlreadyExists";
        public static readonly string EmailAlreadyExists = "EmailAlreadyExists";
        public static readonly string EmailProfileExistsEmailTemplate = "EmailProfileExistsEmailTemplate";
        public static readonly string InvalidJsonParam = "InvalidJsonParam";
        public static readonly string WebGroupExistsDepWebGroup = "WebGroupExistsDepWebGroup";
        public static readonly string ScopeExistsScopeAcademicYear = "ScopeExistsScopeAcademicYear";
        public static readonly string FormEvalMatrixExistsFormEval = "FormEvalMatrixExistsFormEval";
        public static readonly string DepEvalMatrixExistsFormItemValue = "DepEvalMatrixExistsFormItemValue";
        public static readonly string ParentScopeTypeExistsScopeType = "ParentScopeTypeExistsScopeType";
        public static readonly string DepEvalMatrixExistsOrgEvalResults = "DepEvalMatrixExistsOrgEvalResults";
        public static readonly string ScopeExistsAcademicYearScope = "ScopeExistsAcademicYearScope";
        public static readonly string SchoolTypesExistsOrganization = "SchoolTypesExistsOrganization";
        public static readonly string SchoolTypesExistsSchool = "SchoolTypesExistsSchool";
        public static readonly string PlanTypeDepExistsPlan = "PlanTypeDepExistsPlan";
        public static readonly string PlanStatusExistsPlanHistory = "PlanStatusExistsPlanHistory";
        public static readonly string PlanStatusExistsPlan = "PlanStatusExistsPlan";
        public static readonly string OrgTypeExistsOrgTree = "OrgTypeExistsOrgTree";
        public static readonly string OrgTypeExistsOrganization = "OrgTypeExistsOrganization";
        public static readonly string SchoolsExistsSchoolLevel = "OrgTypeExistsOrganization";
        public static readonly string OrgTypeDoesNotExists = "OrgTypeDoesNotExists";
        public static readonly string OrgTreeExistsDepartmentOrgTree = "OrgTreeExistsDepartmentOrgTree";
        public static readonly string OrgTreeExistsOrgEvalResult = "OrgTreeExistsOrgEvalResult";
        public static readonly string OrgTreeExistsEvalAttachment = "OrgTreeExistsEvalAttachment";
        public static readonly string OrgTreeExistsServiceRequest = "OrgTreeExistsServiceRequest";
        public static readonly string OrgTreeExistsEvaluationRequestHistory = "OrgTreeExistsEvaluationRequestHistory";
        public static readonly string OrgTreeExistsParentOrgTree = "OrgTreeExistsParentOrgTree";
        public static readonly string OrgTreeExistsEmployee = "OrgTreeExistsEmployee";
        public static readonly string OrgTreeExistsOrganization = "OrgTreeExistsOrganization";
        public static readonly string OrgTreeExistsSchool = "OrgTreeExistsSchool";
        public static readonly string OrgTreeExistsDepartment = "OrgTreeExistsDepartment";
        public static readonly string OrgTreeExistsEvaluationRequest = "OrgTreeExistsEvaluationRequest";
        public static readonly string OrgTreeExistsOrgAcademicYear = "OrgTreeExistsOrgAcademicYear";
        public static readonly string JobTitleCannotDelete = "JobTitleCannotDelete";
        public static readonly string DepartmentEvaluationParty = "DepartmentEvaluationParty";
        public static readonly string EvalFormExistsFormItem = "EvalFormExistsFormItem";
        public static readonly string EvalFormExistsFormScope = "EvalFormExistsFormScope";
        public static readonly string FormItemExistsFormItemValues = "FormItemExistsFormItemValues";
        public static readonly string FormItemExistsSubFormItem = "FormItemExistsSubFormItem";
        public static readonly string EvalFormTypeExistsEvalForms = "EvalFormTypeExistsEvalForms";
        public static readonly string EducationLevelExistsSchoolLevel = "EducationLevelExistsSchoolLevel";
        public static readonly string EvaluationPartiesExistsService = "EvaluationPartiesExistsService";
        public static readonly string EvaluationPartiesExistsServiceRequest = "EvaluationPartiesExistsServiceRequest";
        public static readonly string EvaluationPartiesExistsEvalForm = "EvaluationPartiesExistsEvalForm";
        public static readonly string EvaluationTypeExistsEvaluationRequestHistory = "EvaluationTypeExistsEvaluationRequestHistory";
        public static readonly string DropdownTypeExistsField = "DropdownTypeExistsField";
        public static readonly string FormItemExistsFormItemRelated = "FormItemExistsFormItemRelated";
        public static readonly string CurrentAcademiUser = "CurrentAcademiUser";
        public static readonly string NdaStatusCannotDelete = "NdaStatusCannotDelete";
        public static readonly string EvaluationRequestAssignment = "EvaluationRequestAssignment";
        public static readonly string NdaStatusNotFound = "NdaStatusNotFound";
        public static readonly string ScopeNotExistsFormAssignment = "ScopeNotExistsFormAssignment";
        public static readonly string TheSelectedValueIsNotRecognized = "TheSelectedValueIsNotRecognized";
        public static readonly string FormDataIsNotValid = "FormDataIsNotValid";
        public static readonly string FormDataIsNull = "FormDataIsNull";
        public static readonly string CompareDateException = "CompareDateException";
        public static readonly string StartSchoolPlanDateException = "StartSchoolPlanDateException";
        public static readonly string EndSchoolPlanDateException = "EndSchoolPlanDateException";

        public static readonly string EvalRequestNotExsit = "EvalRequestNotExsit";
        public static readonly string OneOfUserNotExsit = "OneOfUserNotExsit";
        public static readonly string UserNotExist = "UserNotExist";
        public static readonly string RequestEvaluationNotExist = "RequestEvaluationNotExist";

        public static readonly string TeamExistsUserTeam = "TeamExistsUserTeam";
        public static readonly string msgInvalidEvaluationPlan = "msgInvalidEvaluationPlan"; //TO DO INSERTED
        public static readonly string msgInvalidEvaluationForm = "msgInvalidEvaluationForm"; //TO DO INSERTED
        public static readonly string msgInvalidAttachmentId = "msgInvalidAttachmentId"; //TO DO INSERTED
        public static readonly string FormItemConfigPercentageMax = "FormItemConfigPercentageMax";
        public static readonly string Exception_No_Data_Provided = "Exception_No_Data_Provided";
        public static readonly string DuplicateRecordsinRequest = "DuplicateRecordsinRequest";
        public static readonly string RECORD_NOT_FOUND = "RECORD_NOT_FOUND";
        public static readonly string Exception_Invalid_Total_Percentage_After_Delete = "Exception_Invalid_Total_Percentage_After_Delete";
        public static readonly string Final_Evaluation_Form_Limit = "Final_Evaluation_Form_Limit";
        public static readonly string Max_Final_Evaluation_Forms_Exceeded = "Max_Final_Evaluation_Forms_Exceeded";
        public static readonly string UserNotExsistInThisEvaluationRequest = "UserNotExsistInThisEvaluationRequest";
        public static readonly string UnsupportedDepartmentCategory = "UnsupportedDepartmentCategory";
        public static readonly string lblNoPartyTypeFound = "lblNoPartyTypeFound";
        public static readonly string NoEvaluationRequestsFoundForTheSelectedUser = "NoEvaluationRequestsFoundForTheSelectedUser";
        public static readonly string NoneOfTheSelectedEvaluationRequestsWereFoundForThisUser = "NoneOfTheSelectedEvaluationRequestsWereFoundForThisUser";
        public static readonly string UnExpectedException = "UnExpectedException";
        public static readonly string NoEvaluationRequestsWereSelected = "NoEvaluationRequestsWereSelected";
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
        public static readonly string SubSiteContentMaxCount = "SubSiteContentMaxCount";
        public static readonly string SchoolOrgType = "SchoolOrgType";
        public static readonly string EmployeeOrgType = "EmployeeOrgType";
        public static readonly string OrgClassList = "OrgClassList";
        public static readonly string EmployeeOrgClassList = "EmployeeOrgClassList";


    }
    public static class WebsiteSettings
    {
        public static readonly string PictureProfileSizeAllow = "PictureProfileSizeAllow";
        public static readonly string PictureProfileExtensions = "PictureProfileExtensions";
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
        public static readonly string SELECTALL = "SELECTALL";
        public static readonly string EXPANDALL = "EXPANDALL";
        public static readonly string COLLAPSEALL = "COLLAPSEALL";
        public static readonly string lblDepartment = "lblDepartment";
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
        public static readonly string AdminOrgAcademicYear = "AdminOrgAcademicYear";
        public static readonly string AdminFormEvalMatrix = "AdminFormEvalMatrix";
        public static readonly string AdminDepEvalMatrix = "AdminDepEvalMatrix";
        public static readonly string AdminAcademicYearScope = "AdminAcademicYearScope";
        public static readonly string AdminScopeAcademicYear = "AdminScopeAcademicYear";
        public static readonly string AdminUserPartyType = "AdminUserPartyType";
        public static readonly string AdminUserPartyTypeSignature = "AdminUserPartyTypeSignature";
        public static readonly string AdminPartyType = "AdminPartyType";
        public static readonly string AdminBanner = "AdminBanner";
        public static readonly string AdminNavbar = "AdminNavbar";
        public static readonly string AdminSiteDocument = "AdminSiteDocument";
        public static readonly string AdminSiteContent = "AdminSiteContent";
        public static readonly string AdminNotificationTemplate = "AdminNotificationTemplate";
        public static readonly string AdminSMSTemplate = "AdminSMSTemplate";
        public static readonly string AdminEmailTemplate = "AdminEmailTemplate";
        public static readonly string AdminTemplateDocument = "AdminTemplateDocument";
        public static readonly string AdminEmailProfile = "AdminEmailProfile";
        public static readonly string AdminUserPermission = "AdminUserPermission";
        public static readonly string AdminPermission = "AdminPermission";
        public static readonly string AdminRole = "AdminRole";
        public static readonly string AdminWebGroups = "AdminWebGroups";
        public static readonly string AdminScopes = "AdminScopes";
        public static readonly string AdminScopeType = "AdminScopeType";
        public static readonly string AdminSchoolTypes = "AdminSchoolTypes";
        public static readonly string AdminPlanTypeDep = "AdminPlanTypeDep";
        public static readonly string AdminPlanStatuses = "AdminPlanStatuses";
        public static readonly string AdminOrgTypes = "AdminOrgTypes";
        public static readonly string AdminJobTitle = "AdminJobTitle";
        public static readonly string AdminDepEvaluationType = "AdminDepEvaluationType";
        public static readonly string AdminSchools = "AdminSchools";
        public static readonly string AdminOrgTree = "AdminOrgTree";
        public static readonly string AdminEvalFormType = "AdminEvalFormType";
        public static readonly string AdminEducationLevel = "AdminEducationLevel";
        public static readonly string AdminEmployees = "AdminEmployees";
        public static readonly string AdminEvaluationParties = "AdminEvaluationParties";
        public static readonly string AdminEvaluationType = "AdminEvaluationType";
        public static readonly string AdminFormEvalMarixValue = "AdminFormEvalMarixValue";
        public static readonly string AdminCssClass = "AdminCssClass";
        public static readonly string AdminDropDownType = "AdminDropDownType";
        public static readonly string AdminTeam = "AdminTeam";
        public static readonly string AdminSchoolLevel = "AdminSchoolLevel";
        public static readonly string AdminSystemSetting = "AdminSystemSetting";
        public static readonly string AdminUiControl = "AdminUiControl";
        public static readonly string AdminNDAStatus = "AdminNDAStatus";
        public static readonly string AdminNDAStatusDepartment = "AdminNDAStatusDepartment";
        public static readonly string AdminUserTeamScope = "AdminUserTeamScope";
        public static readonly string AdminDepTargetOrgTree = "AdminDepTargetOrgTree";
        public static readonly string AdminPartyTypeEvalPartyStatus = "AdminPartyTypeEvalPartyStatus";
        public static readonly string AdminServiceStatusConfiguration = "AdminServiceStatusConfiguration";
    }
    public static class WebAppPages
    {
        public static readonly string WebEvaluationForm = "WebEvaluationForm";
        public static readonly string WebEvaluationFormItem = "WebEvaluationFormItem";
        public static readonly string WebEvaluationSubFormItem = "WebEvaluationSubFormItem";
        public static readonly string WebEvaluationFormScopes = "WebEvaluationFormScopes";
        public static readonly string WebDepartmentHoliday = "WebDepartmentHoliday";
        public static readonly string WebAppRequest = "WebAppRequest";
        public static readonly string WebAppCommon = "WebAppCommon";
        public static readonly string AssignmentUserTeam = "AssignmentUserTeam";
        public static readonly string EducationalEntitiesDescription = "EducationalEntitiesDescription";
        public static readonly string FormEducationalEntitiesTitle = "FormEducationalEntitiesTitle";
        public static readonly string ReassignAssignment = "WebReassignAssignment";
    }

    public static class WebAppOrgDetails
    {
        public static readonly string lblOrgDetailsCurrentEvaluation = "lblOrgDetailsCurrentEvaluation";
        public static readonly string lblOrgDetailsCurrentSituation = "lblOrgDetailsCurrentSituation";
        public static readonly string lblOrgDetailsLastEvaluation = "lblOrgDetailsLastEvaluation";
        public static readonly string lblOrgDetailsInProgressEvaluation = "lblOrgDetailsInProgressEvaluation";
        public static readonly string lblOrgDetailsBasicInformation = "lblOrgDetailsBasicInformation";
        public static readonly string lblOrgDetailsManager = "lblOrgDetailsManager";
        public static readonly string lblOrgDetailsEstablishmentDate = "lblOrgDetailsEstablishmentDate";
        public static readonly string lblOrgDetailsLevels= "lblOrgDetailsLevels";
    }
    public static class WebAppRequest
    {
        public static readonly string lblDownload = "lblDownload";
        public static readonly string lblMinLengthNotReached = "lblMinLengthNotReached";
        public static readonly string lblMissingFormIdForEvaluationForm = "lblMissingFormIdForEvaluationForm";
        public static readonly string lblLoadingEvaluationForm = "lblLoadingEvaluationForm";
        public static readonly string lblMaximumRowsExceeded = "lblMaximumRowsExceeded";
        public static readonly string lblValueMustBeGreaterOrEqual = "lblValueMustBeGreaterOrEqual";
        public static readonly string lblMaxLengthExceeded = "lblMaxLengthExceeded";
        public const string actionTransactionsdetailsLabel = "actionTransactionsdetailsLabel";
        public static readonly string lblFileSizeExceeded = "lblFileSizeExceeded";
        public static readonly string lblMaximumNewRowsExceeded = "lblMaximumNewRowsExceeded";
        public static readonly string lblLoadingEvaluationPlan = "lblLoadingEvaluationPlan";
        public static readonly string lblRenderFormGroupsNotDefined = "lblRenderFormGroupsNotDefined";
        public static readonly string lblNotEqualValidation = "lblNotEqualValidation";
        public static readonly string lblFailedToLoadEvaluationPlan = "lblFailedToLoadEvaluationPlan";
        public static readonly string lblRemarkISRequired = "lblRemarkISRequired";
        public static readonly string lblPlanUtilityNotFound = "lblPlanUtilityNotFound";
        public static readonly string lblFileTypeNotAllowed = "lblFileTypeNotAllowed";
        public static readonly string lblFormSubmittedSuccessfully = "lblFormSubmittedSuccessfully";
        public static readonly string lblDefaultValidationMessage = "lblDefaultValidationMessage";
        public static readonly string lblUnexpectedErrorOccurred = "lblUnexpectedErrorOccurred";
        public static readonly string lblInvalidNumberFormat = "lblInvalidNumberFormat";
        public static readonly string lblValueMustBeLessOrEqual = "lblValueMustBeLessOrEqual";
        public static readonly string lblMinimumNewRowsRequired = "lblMinimumNewRowsRequired";
        public static readonly string lblMinimumRowsRequired = "lblMinimumRowsRequired";
        public static readonly string lblFailedToLoadEvaluationForm = "lblFailedToLoadEvaluationForm";
        public static readonly string lblInvalidBase64Content = "lblInvalidBase64Content";
        public static readonly string lblRequestCreatedSuccessfully = "lblRequestCreatedSuccessfully";
        public static readonly string lblSaveChanges = "lblSaveChanges";
        public static readonly string lblSaveAsDraft = "lblSaveAsDraft";
        public static readonly string lblFailedToLoadFile = "lblFailedToLoadFile";
        public static readonly string lblRequestSavedAsDraftSuccessfully = "lblRequestSavedAsDraftSuccessfully";
        public static readonly string lblFailedToRenderPlanWrapper = "lblFailedToRenderPlanWrapper";
        public static readonly string lblUnnamedGroup = "lblUnnamedGroup";
        public static readonly string lblBase64PdfFileName = "lblBase64PdfFileName";
        public static readonly string lblUnexpectedEmptyResponse = "lblUnexpectedEmptyResponse";
        public static readonly string lblInvalidFormat = "lblInvalidFormat";
        public static readonly string lblViewHistory = "lblViewHistory";
        public static readonly string lblProcedures = "lblProcedures";
        public static readonly string lblDateRangeInvalid = "lblDateRangeInvalid";
        public static readonly string lblRemarks = "lblRemarks";
        public static readonly string lblRequiredField = "lblRequiredField";
        public static readonly string lblUnnamedFile = "lblUnnamedFile";
        public static readonly string lblInvalidFileExtension = "lblInvalidFileExtension";
        public static readonly string lblCloseModal = "lblCloseModal";
        public static readonly string lblEvaluationFileScope = "lblEvaluationFileScope";
        public static readonly string lblEvaluationFileHeader = "lblEvaluationFileHeader";
        public static readonly string lblRequestNumber = "lblRequestNumber";
        public static readonly string lblSchoolEvalRequests = "lblSchoolEvalRequests";
        public static readonly string lblFromDate = "lblFromDate";
        public static readonly string lblToDate = "lblToDate";
        public static readonly string lblEvaluationDate = "lblEvaluationDate";
        public static readonly string lblNextEvaluationDate = "lblNextEvaluationDate";
        public static readonly string lblEvaluationResult = "lblEvaluationResult";
        public static readonly string lblStatus = "lblStatus";
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

        #region actionstatusconfiguration

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

        #region ORG_ACADEMIC_YEAR

        public const string DELETE_ADMIN_ORG_ACADEMIC_YEAR = "DELETE_ADMIN_ORG_ACADEMIC_YEAR";
        public const string ADD_ADMIN_ORG_ACADEMIC_YEAR = "ADD_ADMIN_ORG_ACADEMIC_YEAR";
        public const string EDIT_ADMIN_ORG_ACADEMIC_YEAR = "EDIT_ADMIN_ORG_ACADEMIC_YEAR";
        public const string VIEW_ADMIN_ORG_ACADEMIC_YEAR = "VIEW_ADMIN_ORG_ACADEMIC_YEAR";

        #endregion

        #region FORM_EVAL_MATRIX

        public const string DELETE_ADMIN_FORM_EVAL_MATRIX = "DELETE_ADMIN_FORM_EVAL_MATRIX";
        public const string ADD_ADMIN_FORM_EVAL_MATRIX = "ADD_ADMIN_FORM_EVAL_MATRIX";
        public const string EDIT_ADMIN_FORM_EVAL_MATRIX = "EDIT_ADMIN_FORM_EVAL_MATRIX";
        public const string VIEW_ADMIN_FORM_EVAL_MATRIX = "VIEW_ADMIN_FORM_EVAL_MATRIX";

        #endregion

        #region DEP_EVAL_MATRIX

        public const string DELETE_ADMIN_DEP_EVAL_MATRIX = "DELETE_ADMIN_DEP_EVAL_MATRIX";
        public const string ADD_ADMIN_DEP_EVAL_MATRIX = "ADD_ADMIN_DEP_EVAL_MATRIX";
        public const string EDIT_ADMIN_DEP_EVAL_MATRIX = "EDIT_ADMIN_DEP_EVAL_MATRIX";
        public const string VIEW_ADMIN_DEP_EVAL_MATRIX = "VIEW_ADMIN_DEP_EVAL_MATRIX";

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

        #region SITECONTENT

        public const string VIEW_ADMIN_SITECONTENT = "VIEW_ADMIN_SITECONTENT";
        public const string ADD_ADMIN_SITECONTENT = "ADD_ADMIN_SITECONTENT";
        public const string EDIT_ADMIN_SITECONTENT = "EDIT_ADMIN_SITECONTENT";
        public const string DELETE_ADMIN_SITECONTENT = "DELETE_ADMIN_SITECONTENT";

        #endregion

        #region NOTIFICATION_TEMPLATE

        public const string VIEW_ADMIN_NOTIFICATION_TEMPLATE = "VIEW_ADMIN_NOTIFICATION_TEMPLATE";
        public const string ADD_ADMIN_NOTIFICATION_TEMPLATE = "ADD_ADMIN_NOTIFICATION_TEMPLATE";
        public const string EDIT_ADMIN_NOTIFICATION_TEMPLATE = "EDIT_ADMIN_NOTIFICATION_TEMPLATE";
        public const string DELETE_ADMIN_NOTIFICATION_TEMPLATE = "DELETE_ADMIN_NOTIFICATION_TEMPLATE";

        #endregion

        #region SMSTEMPLATE

        public const string VIEW_ADMIN_SMSTEMPLATE = "VIEW_ADMIN_SMSTEMPLATE";
        public const string ADD_ADMIN_SMSTEMPLATE = "ADD_ADMIN_SMSTEMPLATE";
        public const string EDIT_ADMIN_SMSTEMPLATE = "EDIT_ADMIN_SMSTEMPLATE";
        public const string DELETE_ADMIN_SMSTEMPLATE = "DELETE_ADMIN_SMSTEMPLATE";

        #endregion
        #region EMAILTEMPLATE

        public const string VIEW_ADMIN_EMAILTEMPLATE = "VIEW_ADMIN_EMAILTEMPLATE";
        public const string ADD_ADMIN_EMAILTEMPLATE = "ADD_ADMIN_EMAILTEMPLATE";
        public const string EDIT_ADMIN_EMAILTEMPLATE = "EDIT_ADMIN_EMAILTEMPLATE";
        public const string DELETE_ADMIN_EMAILTEMPLATE = "DELETE_ADMIN_EMAILTEMPLATE";

        #endregion

        #region TEMPLATEDOC

        public const string VIEW_ADMIN_TEMPLATEDOC = "VIEW_ADMIN_TEMPLATEDOC";
        public const string ADD_ADMIN_TEMPLATEDOC = "ADD_ADMIN_TEMPLATEDOC";
        public const string EDIT_ADMIN_TEMPLATEDOC = "EDIT_ADMIN_TEMPLATEDOC";
        public const string DELETE_ADMIN_TEMPLATEDOC = "DELETE_ADMIN_TEMPLATEDOC";

        #endregion

        #region EMAILPROFILE
        public const string VIEW_ADMIN_EMAILPROFILE = "VIEW_ADMIN_EMAILPROFILE";
        public const string ADD_ADMIN_EMAILPROFILE = "ADD_ADMIN_EMAILPROFILE";
        public const string EDIT_ADMIN_EMAILPROFILE = "EDIT_ADMIN_EMAILPROFILE";
        public const string DELETE_ADMIN_EMAILPROFILE = "DELETE_ADMIN_EMAILPROFILE";
        #endregion
        #region USER_PERMISSION
        public const string VIEW_ADMIN_USER_PERMISSION = "VIEW_ADMIN_USER_PERMISSION";
        public const string ADD_ADMIN_USER_PERMISSION = "ADD_ADMIN_USER_PERMISSION";
        public const string EDIT_ADMIN_USER_PERMISSION = "EDIT_ADMIN_USER_PERMISSION";
        #endregion

        #region ROLE_PERMISSION
        public const string VIEW_ADMIN_ROLE_PERMISSION = "VIEW_ADMIN_ROLE_PERMISSION";
        public const string ADD_ADMIN_ROLE_PERMISSION = "ADD_ADMIN_ROLE_PERMISSION";
        #endregion

        #region ROLE
        public const string VIEW_ADMIN_ROLE = "VIEW_ADMIN_ROLE";
        public const string ADD_ADMIN_ROLE = "ADD_ADMIN_ROLE";
        public const string EDIT_ADMIN_ROLE = "EDIT_ADMIN_ROLE";
        public const string DELETE_ADMIN_ROLE = "DELETE_ADMIN_ROLE";
        #endregion

        #region WEBGROUPS
        public const string VIEW_ADMIN_WEBGROUPS = "VIEW_ADMIN_WEBGROUPS";
        public const string ADD_ADMIN_WEBGROUPS = "ADD_ADMIN_WEBGROUPS";
        public const string EDIT_ADMIN_WEBGROUPS = "EDIT_ADMIN_WEBGROUPS";
        public const string DELETE_ADMIN_WEBGROUPS = "DELETE_ADMIN_WEBGROUPS";
        #endregion

        #region SCOPES
        public const string VIEW_ADMIN_SCOPES = "VIEW_ADMIN_SCOPES";
        public const string ADD_ADMIN_SCOPES = "ADD_ADMIN_SCOPES";
        public const string EDIT_ADMIN_SCOPES = "EDIT_ADMIN_SCOPES";
        public const string DELETE_ADMIN_SCOPES = "DELETE_ADMIN_SCOPES";
        #endregion

        #region SCOPE_TYPE
        public const string VIEW_ADMIN_SCOPE_TYPE = "VIEW_ADMIN_SCOPE_TYPE";
        public const string ADD_ADMIN_SCOPE_TYPE = "ADD_ADMIN_SCOPE_TYPE";
        public const string EDIT_ADMIN_SCOPE_TYPE = "EDIT_ADMIN_SCOPE_TYPE";
        public const string DELETE_ADMIN_SCOPE_TYPE = "DELETE_ADMIN_SCOPE_TYPE";
        #endregion

        #region SCHOOL_TYPES
        public const string VIEW_ADMIN_SCHOOL_TYPES = "VIEW_ADMIN_SCHOOL_TYPES";
        public const string ADD_ADMIN_SCHOOL_TYPES = "ADD_ADMIN_SCHOOL_TYPES";
        public const string EDIT_ADMIN_SCHOOL_TYPES = "EDIT_ADMIN_SCHOOL_TYPES";
        public const string DELETE_ADMIN_SCHOOL_TYPES = "DELETE_ADMIN_SCHOOL_TYPES";
        #endregion

        #region PLANTYPEDEP
        public const string VIEW_ADMIN_PLANTYPEDEP = "VIEW_ADMIN_PLANTYPEDEP";
        public const string ADD_ADMIN_PLANTYPEDEP = "ADD_ADMIN_PLANTYPEDEP";
        public const string EDIT_ADMIN_PLANTYPEDEP = "EDIT_ADMIN_PLANTYPEDEP";
        public const string DELETE_ADMIN_PLANTYPEDEP = "DELETE_ADMIN_PLANTYPEDEP";
        #endregion

        #region PLANSTATUS
        public const string VIEW_ADMIN_PLANSTATUS = "VIEW_ADMIN_PLANSTATUS";
        public const string ADD_ADMIN_PLANSTATUS = "ADD_ADMIN_PLANSTATUS";
        public const string EDIT_ADMIN_PLANSTATUS = "EDIT_ADMIN_PLANSTATUS";
        public const string DELETE_ADMIN_PLANSTATUS = "DELETE_ADMIN_PLANSTATUS";
        #endregion

        #region ORGTYPES
        public const string VIEW_ADMIN_ORGTYPES = "VIEW_ADMIN_ORGTYPES";
        public const string ADD_ADMIN_ORGTYPES = "ADD_ADMIN_ORGTYPES";
        public const string EDIT_ADMIN_ORGTYPES = "EDIT_ADMIN_ORGTYPES";
        public const string DELETE_ADMIN_ORGTYPES = "DELETE_ADMIN_ORGTYPES";
        #endregion
        #region JOBTITLE
        public const string VIEW_ADMIN_JOBTITLE = "VIEW_ADMIN_JOBTITLE";
        public const string ADD_ADMIN_JOBTITLE = "ADD_ADMIN_JOBTITLE";
        public const string EDIT_ADMIN_JOBTITLE = "EDIT_ADMIN_JOBTITLE";
        public const string DELETE_ADMIN_JOBTITLE = "DELETE_ADMIN_JOBTITLE";
        public const string UPDATE_ORDER_ADMIN_JOBTITLE = "UPDATE_ORDER_ADMIN_JOBTITLE";
        #endregion

        #region DepEvaluationType
        public const string VIEW_ADMIN_DEPEVALUATIONTYPE = "VIEW_ADMIN_DEPEVALUATIONTYPE";
        public const string ADD_ADMIN_DEPEVALUATIONTYPE = "ADD_ADMIN_DEPEVALUATIONTYPE";
        public const string EDIT_ADMIN_DEPEVALUATIONTYPE = "EDIT_ADMIN_DEPEVALUATIONTYPE";
        public const string DELETE_ADMIN_DEPEVALUATIONTYPE = "DELETE_ADMIN_DEPEVALUATIONTYPE";
        public const string EDIT_ORDER_ADMIN_DEPEVALUATIONTYPE = "EDIT_ORDER_ADMIN_DEPEVALUATIONTYPE";
        #endregion

        #region SCHOOL

        public const string VIEW_ADMIN_SCHOOL = "VIEW_ADMIN_SCHOOL";
        public const string ADD_ADMIN_SCHOOL = "ADD_ADMIN_SCHOOL";
        public const string EDIT_ADMIN_SCHOOL = "EDIT_ADMIN_SCHOOL";
        public const string DELETE_ADMIN_SCHOOL = "DELETE_ADMIN_SCHOOL";

        #endregion

        #region ORGTREE

        public const string VIEW_ADMIN_ORGTREE = "VIEW_ADMIN_ORGTREE";
        public const string ADD_ADMIN_ORGTREE = "ADD_ADMIN_ORGTREE";
        public const string EDIT_ADMIN_ORGTREE = "EDIT_ADMIN_ORGTREE";
        public const string DELETE_ADMIN_ORGTREE = "DELETE_ADMIN_ORGTREE";

        #endregion

        #region EVALFORMTYPE

        public const string VIEW_ADMIN_EVALFORMTYPE = "VIEW_ADMIN_EVALFORMTYPE";
        public const string ADD_ADMIN_EVALFORMTYPE = "ADD_ADMIN_EVALFORMTYPE";
        public const string EDIT_ADMIN_EVALFORMTYPE = "EDIT_ADMIN_EVALFORMTYPE";
        public const string DELETE_ADMIN_EVALFORMTYPE = "DELETE_ADMIN_EVALFORMTYPE";

        #endregion

        #region EDUCATIONLEVEL

        public const string VIEW_ADMIN_EDUCATIONLEVEL = "VIEW_ADMIN_EDUCATIONLEVEL";
        public const string ADD_ADMIN_EDUCATIONLEVEL = "ADD_ADMIN_EDUCATIONLEVEL";
        public const string EDIT_ADMIN_EDUCATIONLEVEL = "EDIT_ADMIN_EDUCATIONLEVEL";
        public const string DELETE_ADMIN_EDUCATIONLEVEL = "DELETE_ADMIN_EDUCATIONLEVEL";

        #endregion

        #region EMPLOYEE

        public const string VIEW_ADMIN_EMPLOYEE = "VIEW_ADMIN_EMPLOYEE";
        public const string ADD_ADMIN_EMPLOYEE = "ADD_ADMIN_EMPLOYEE";
        public const string EDIT_ADMIN_EMPLOYEE = "EDIT_ADMIN_EMPLOYEE";
        public const string DELETE_ADMIN_EMPLOYEE = "DELETE_ADMIN_EMPLOYEE";

        #endregion

        #region EVALUATIONPARTIES

        public const string VIEW_ADMIN_EVALUATIONPARTIES = "VIEW_ADMIN_EVALUATIONPARTIES";
        public const string ADD_ADMIN_EVALUATIONPARTIES = "ADD_ADMIN_EVALUATIONPARTIES";
        public const string EDIT_ADMIN_EVALUATIONPARTIES = "EDIT_ADMIN_EVALUATIONPARTIES";
        public const string DELETE_ADMIN_EVALUATIONPARTIES = "DELETE_ADMIN_EVALUATIONPARTIES";

        #endregion

        #region EVALUATIONTYPE

        public const string VIEW_ADMIN_EVALUATIONTYPE = "VIEW_ADMIN_EVALUATIONTYPE";
        public const string ADD_ADMIN_EVALUATIONTYPE = "ADD_ADMIN_EVALUATIONTYPE";
        public const string EDIT_ADMIN_EVALUATIONTYPE = "EDIT_ADMIN_EVALUATIONTYPE";
        public const string DELETE_ADMIN_EVALUATIONTYPE = "DELETE_ADMIN_EVALUATIONTYPE";

        #endregion

        #region FORMEVAL_MATRIX_VALUE

        public const string VIEW_ADMIN_FORMEVAL_MATRIX_VALUE = "VIEW_ADMIN_FORMEVAL_MATRIX_VALUE";
        public const string ADD_ADMIN_FORMEVAL_MATRIX_VALUE = "ADD_ADMIN_FORMEVAL_MATRIX_VALUE";
        public const string EDIT_ADMIN_FORMEVAL_MATRIX_VALUE = "EDIT_ADMIN_FORMEVAL_MATRIX_VALUE";
        public const string DELETE_ADMIN_FORMEVAL_MATRIX_VALUE = "DELETE_ADMIN_FORMEVAL_MATRIX_VALUE";

        #endregion

        #region CSSCLASS

        public const string VIEW_ADMIN_CSSCLASS = "VIEW_ADMIN_CSSCLASS";
        public const string ADD_ADMIN_CSSCLASS = "ADD_ADMIN_CSSCLASS";
        public const string EDIT_ADMIN_CSSCLASS = "EDIT_ADMIN_CSSCLASS";
        public const string DELETE_ADMIN_CSSCLASS = "DELETE_ADMIN_CSSCLASS";

        #endregion
        #region DROPDOWNTYPE

        public const string VIEW_ADMIN_DROPDOWNTYPE = "VIEW_ADMIN_DROPDOWNTYPE";
        public const string ADD_ADMIN_DROPDOWNTYPE = "ADD_ADMIN_DROPDOWNTYPE";
        public const string EDIT_ADMIN_DROPDOWNTYPE = "EDIT_ADMIN_DROPDOWNTYPE";
        public const string DELETE_ADMIN_DROPDOWNTYPE = "DELETE_ADMIN_DROPDOWNTYPE";

        #endregion

        #region SCHOOLLEVEL

        public const string VIEW_ADMIN_SCHOOLLEVEL = "VIEW_ADMIN_SCHOOLLEVEL";
        public const string ADD_ADMIN_SCHOOLLEVEL = "ADD_ADMIN_SCHOOLLEVEL";
        public const string EDIT_ADMIN_SCHOOLLEVEL = "EDIT_ADMIN_SCHOOLLEVEL";
        public const string DELETE_ADMIN_SCHOOLLEVEL = "DELETE_ADMIN_SCHOOLLEVEL";

        #endregion
        #region TEAM

        public const string VIEW_ADMIN_TEAM = "VIEW_ADMIN_TEAM";
        public const string ADD_ADMIN_TEAM = "ADD_ADMIN_TEAM";
        public const string EDIT_ADMIN_TEAM = "EDIT_ADMIN_TEAM";
        public const string DELETE_ADMIN_TEAM = "DELETE_ADMIN_TEAM";

        #endregion
        #region SYSTEMSETTING

        public const string VIEW_ADMIN_SYSTEMSETTING = "VIEW_ADMIN_SYSTEMSETTING";
        public const string EDIT_ADMIN_SYSTEMSETTING = "EDIT_ADMIN_SYSTEMSETTING";

        #endregion
        #region UICONTROL

        public const string VIEW_ADMIN_UICONTROL = "VIEW_ADMIN_UICONTROL";
        public const string EDIT_ADMIN_UICONTROL = "EDIT_ADMIN_UICONTROL";

        #endregion
        #region NDAStatus
        public const string VIEW_ADMIN_NDAStatus = "VIEW_ADMIN_NDASTATUS";
        public const string ADD_ADMIN_NDAStatus = "ADD_ADMIN_NDASTATUS";
        public const string EDIT_ADMIN_NDAStatus = "EDIT_ADMIN_NDASTATUS";
        public const string DELETE_ADMIN_NDAStatus = "DELETE_ADMIN_NDASTATUS";
        public const string UPDATE_ORDER_ADMIN_NDAStatus = "UPDATE_ORDER_ADMIN_NDASTATUS";
        #endregion
        #region NDAStatusDepartment
        public const string VIEW_ADMIN_NDA_STATUS_DEPARTMENT = "VIEW_ADMIN_NDA_STATUS_DEPARTMENT";
        public const string ADD_ADMIN_NDA_STATUS_DEPARTMENT = "ADD_ADMIN_NDA_STATUS_DEPARTMENT";
        public const string EDIT_ADMIN_NDA_STATUS_DEPARTMENT = "EDIT_ADMIN_NDA_STATUS_DEPARTMENT";
        public const string DELETE_ADMIN_NDA_STATUS_DEPARTMENT = "DELETE_ADMIN_NDA_STATUS_DEPARTMENT";
        public const string UPDATE_ORDER_ADMIN_NDAStatus_Department = "EDIT_ADMIN_NDA_STATUS_DEPARTMENT";
        #endregion

        #region USERTEAMSCOPE

        public const string VIEW_ADMIN_USERTEAMSCOPE = "VIEW_ADMIN_USERTEAMSCOPE";
        public const string ADD_ADMIN_USERTEAMSCOPE = "ADD_ADMIN_USERTEAMSCOPE";
        public const string EDIT_ADMIN_USERTEAMSCOPE = "EDIT_ADMIN_USERTEAMSCOPE";
        public const string DELETE_ADMIN_USERTEAMSCOPE = "DELETE_ADMIN_USERTEAMSCOPE";

        #endregion

        #region deptargetorgtree

        public const string VIEW_ADMIN_DEPTARGETORGTREE = "VIEW_ADMIN_DEPTARGETORGTREE";
        public const string ADD_ADMIN_DEPTARGETORGTREE = "ADD_ADMIN_DEPTARGETORGTREE";
        public const string EDIT_ADMIN_DEPTARGETORGTREE = "EDIT_ADMIN_DEPTARGETORGTREE";
        public const string DELETE_ADMIN_DEPTARGETORGTREE = "DELETE_ADMIN_DEPTARGETORGTREE";

        #endregion

        #region USERTEAMSCOPE

        public const string VIEW_ADMIN_PartyTypeEvalPartyStatus = "VIEW_ADMIN_PartyTypeEvalPartyStatus";
        public const string ADD_ADMIN_PartyTypeEvalPartyStatus = "ADD_ADMIN_PartyTypeEvalPartyStatus";
        public const string EDIT_ADMIN_PartyTypeEvalPartyStatus = "EDIT_ADMIN_PartyTypeEvalPartyStatus";
        public const string DELETE_ADMIN_PartyTypeEvalPartyStatus = "DELETE_ADMIN_PartyTypeEvalPartyStatus";

        #endregion
        #region ServiceStatusConfiguration

        public const string VIEW_ADMIN_ServiceStatusConfiguration = "VIEW_ADMIN_ServiceStatusConfiguration";
        public const string ADD_ADMIN_ServiceStatusConfiguration = "ADD_ADMIN_ServiceStatusConfiguration";
        public const string EDIT_ADMIN_ServiceStatusConfiguration = "EDIT_ADMIN_ServiceStatusConfiguration";
        public const string DELETE_ADMIN_ServiceStatusConfiguration = "DELETE_ADMIN_ServiceStatusConfiguration";

        #endregion

    }

    public static class CustomDataSource
    {
    }
    public static class WebPermissions
    {
        public const string GET_FORM_ITEMS = "GET_FORM_ITEMS";
        public const string SAVE_EVALUATION_FORM = "SAVE_EVALUATION_FORM";
        public const string UPDATE_EVALUATION_FORM = "UPDATE_EVALUATION_FORM";
        public const string RENAME_EVALUATION_FORM = "RENAME_EVALUATION_FORM";
        public const string VALIDATE_EVALUATION_FORM = "VALIDATE_EVALUATION_FORM";
        public const string CALCULATE_EVALUATION_FORM = "CALCULATE_EVALUATION_FORM";

        #region EVALFORMS

        public const string VIEW_WEB_EVALFORMS = "VIEW_WEB_EVALFORMS";
        public const string ADD_WEB_EVALFORMS = "ADD_WEB_EVALFORMS";
        public const string EDIT_WEB_EVALFORMS = "EDIT_WEB_EVALFORMS";
        public const string DELETE_WEB_EVALFORMS = "DELETE_WEB_EVALFORMS";

        #endregion

        #region DEPARTMENTHOLIDAY

        public const string VIEW_WEB_DEPARTMENT_HOLIDAY = "VIEW_WEB_DEPARTMENT_HOLIDAY";
        public const string ADD_WEB_DEPARTMENT_HOLIDAY = "ADD_WEB_DEPARTMENT_HOLIDAY";
        public const string EDIT_WEB_DEPARTMENT_HOLIDAY = "EDIT_WEB_DEPARTMENT_HOLIDAY";
        public const string DELETE_WEB_DEPARTMENT_HOLIDAY = "DELETE_WEB_DEPARTMENT_HOLIDAY";

        #endregion

        #region FORMITEMS

        public const string VIEW_WEB_FORMITEMS = "VIEW_WEB_FORMITEMS";
        public const string ADD_WEB_FORMITEMS = "ADD_WEB_FORMITEMS";
        public const string EDIT_WEB_FORMITEMS = "EDIT_WEB_FORMITEMS";
        public const string DELETE_WEB_FORMITEMS = "DELETE_WEB_FORMITEMS";

        #endregion

        #region SUBFORMITEMS

        public const string VIEW_WEB_SUBFORMITEMS = "VIEW_WEB_SUBFORMITEMS";
        public const string ADD_WEB_SUBFORMITEMS = "ADD_WEB_SUBFORMITEMS";
        public const string EDIT_WEB_SUBFORMITEMS = "EDIT_WEB_SUBFORMITEMS";
        public const string DELETE_WEB_SUBFORMITEMS = "DELETE_WEB_SUBFORMITEMS";

        #endregion
        #region FORMITEMCONFIG
        public const string VIEW_WEB_FORMITEMCONFIG = "VIEW_WEB_FORMITEMCONFIG";
        public const string ADD_WEB_FORMITEMCONFIG = "ADD_WEB_FORMITEMCONFIG";
        public const string EDIT_WEB_FORMITEMCONFIG = "EDIT_WEB_FORMITEMCONFIG";
        public const string DELETE_WEB_FORMITEMCONFIG = "DELETE_WEB_FORMITEMCONFIG";
        #endregion
        #region TEAM

        public const string VIEW_WEB_TEAM = "VIEW_WEB_TEAM";
        public const string ADD_WEB_TEAM = "ADD_WEB_TEAM";
        public const string EDIT_WEB_TEAM = "EDIT_WEB_TEAM";
        public const string DELETE_WEB_TEAM = "DELETE_WEB_TEAM";

        #endregion

        public const string IS_SEND_USERTEAM_MAIL = "IS_SEND_USERTEAM_MAIL";

        #region FORMSCOPES

        public const string VIEW_WEB_FORMSCOPES = "VIEW_WEB_FORMSCOPES";
        public const string ADD_WEB_FORMSCOPES = "ADD_WEB_FORMSCOPES";
        public const string EDIT_WEB_FORMSCOPES = "EDIT_WEB_FORMSCOPES";
        public const string DELETE_WEB_FORMSCOPES = "DELETE_WEB_FORMSCOPES";

        #endregion

        #region Plan Module
        //PlanType
        public const string GET_WEB_PLAN_TYPE_REQUEST = "GET_WEB_PLAN_TYPE_REQUEST";
        //Plan 
        public const string ADD_WEB_PLAN = "ADD_WEB_PLAN";
        public const string EDIT_WEB_PLAN = "EDIT_WEB_PLAN";
        public const string VIEW_WEB_PLAN = "VIEW_WEB_PLAN";
        public const string DELETE_WEB_PLAN = "DELETE_WEB_PLAN";
        #endregion
        #region Reassign
        public const string VIEW_WEB_ReassignAssignment = "VIEW_WEB_ReassignAssignment";
        public const string ADD_WEB_ReassignAssignment = "ADD_WEB_ReassignAssignment";
        public const string EDIT_WEB_ReassignAssignment = "EDIT_WEB_ReassignAssignment";
        public const string DELETE_WEB_ReassignAssignment = "DELETE_WEB_ReassignAssignment";
        #endregion

        #region EvaluationRequest
        public const string VIEW_EVALUATION_REQUEST = "VIEW_EVALUATION_REQUEST";
        public const string UPDATE_EVALUATION_REQUEST = "UPDATE_EVALUATION_REQUEST";
        #endregion

        #region School
        public const string VIEW_WEB_SCHOOL = "VIEW_WEB_SCHOOL";
        #endregion
    }
    public static class WebAppSettings
    {
        public static readonly string PAGE_SIZE_FOR_SCHOOL_EVALUATION_REQUESTS = "PageSizeForSchoolEvaluationRequests";
        public static readonly string PAGE_SIZE = "PAGE_SIZE";
        public static readonly string DefaultWebsiteBannerImage = "DefaultWebsiteBannerImage";
    }

    public static class WebHomePage
    {
    }
	public static class WebAppUserProfilePage
	{
		// TO DO ADD IN UI CONTROLS
		public static readonly string lblEmail = "lblEmail";
		public static readonly string lblQID = "lblQID";
		public static readonly string lblFullNameEn = "lblFullNameEn";
		public static readonly string lblFullNameAr = "lblFullNameAr";
		public static readonly string lblLastLoginDate = "lblLastLoginDate";
		public static readonly string lblMobile = "lblMobile";
		public static readonly string lblSecondMobile = "lblSecondMobile";
		public static readonly string lblOccupation = "lblOccupation";
		public static readonly string lblNationality = "lblNationality";
		public static readonly string lblSwitchLanguage = "lblSwitchLanguage";
		public static readonly string lblPrefferedlang = "lblPrefferedlang";
		public static readonly string lblVerfiyMobile = "lblVerfiyMobile";
		public static readonly string lblOTP = "lblOTP";
		public static readonly string lblRefreshOTP = "lblRefreshOTP";
		public static readonly string lblValidateOTP = "lblValidateOTP";
		public static readonly string lblSendOTP = "lblSendOTP";
	}
	public static class WebAppCommon
    {
        public const string lblOk = "lblOk";
        public const string lblCancel = "lblCancel";
        public const string lblShowingEntries = "lblShowingEntries";
        public const string lblprevious = "lblprevious";
        public const string lblnext = "lblnext";
        public const string ACTION = "ACTION";
        public const string LASTUPDATEDBY = "LASTUPDATEDBY";
        public const string LASTUPDATEDDATE = "LASTUPDATEDDATE";
        public const string lblDisplayAsCards = "lblDisplayAsCards";
        public const string lblDisplayAsTable = "lblDisplayAsTable";

    }
    public static class CreatePlan
    {
        public const string lblAddPlan = "lblAddPlan";
        public const string lblChoosePlanType = "lblChoosePlanType";
        public const string lblCreateNewPlan = "lblCreateNewPlan";
        public const string lblDeletePlanConfirm = "lblDeletePlanConfirm";
        public const string lblEvaluationPlan = "lblEvaluationPlan";
        public const string lblEvaluationPlanRequests = "lblEvaluationPlanRequests";
        public const string lblInsertPlan = "lblInsertPlan";
        public const string lblPlanCreatedOn = "lblPlanCreatedOn";
        public const string lblPlanEndDate = "lblPlanEndDate";
        public const string lblPlanName = "lblPlanName";
        public const string lblPlanRequests = "lblPlanRequests";
        public const string lblPlanSchoolsCount = "lblPlanSchoolsCount";
        public const string lblPlansListTitle = "lblPlansListTitle";
        public const string lblPlanStartDate = "lblPlanStartDate";
        public const string lblPlanStatus = "lblPlanStatus";
        public const string lblPlanType = "lblPlanType";
        public const string lblPleaseChoosePlanType = "lblPleaseChoosePlanType";
        public const string lblPleaseEnterPlanTitle = "lblPleaseEnterPlanTitle";
        public const string lblSearchPlan = "lblSearchPlan";

        public const string PlanStatusesIsActive = "PlanStatusesIsActive";
        public const string PlanStatusesNameAr = "PlanStatusesNameAr";
        public const string PlanStatusesNameEn = "PlanStatusesNameEn";
        public const string PlanStatusExistsPlan = "PlanStatusExistsPlan";
        public const string PlanStatusExistsPlanHistory = "PlanStatusExistsPlanHistory";

        public const string PlanTypeDepDepartmentId = "PlanTypeDepDepartmentId";
        public const string PlanTypeDepExistsPlan = "PlanTypeDepExistsPlan";
        public const string PlanTypeDepIsActive = "PlanTypeDepIsActive";
        public const string PlanTypeDepNameAr = "PlanTypeDepNameAr";
        public const string PlanTypeDepNameEn = "PlanTypeDepNameEn";
        public const string PlanTypeDepPlanTypeId = "PlanTypeDepPlanTypeId";
        public const string lblSelectSchools = "lblSelectSchools";
        public const string lblVisitDate = "lblVisitDate";
        public const string lblLastEvaluation = "lblLastEvaluation";
        public const string lblVisitType = "lblVisitType";
        public const string lblAcademicYear = "lblAcademicYear";
    }
    public static class Assignment
    {
        public const string lblNoSelectedMembers = "lblNoSelectedMembers";
        public const string lblAllTeams = "lblAllTeams";
        public const string msgMembersAddedSuccessfully = "msgMembersAddedSuccessfully";
        public const string lblTeamLeader = "lblTeamLeader";
        public const string phSearchHere = "phSearchHere";
        public const string lblSelectedVisitTeam = "lblSelectedVisitTeam";
        public const string lblJobTitle = "lblJobTitle";
        public const string lblMembers = "lblMembers";
        public const string lblMemberName = "lblMemberName";
        public const string btnFilter = "btnFilter";
        public const string lblDomain = "lblDomain";
        public const string btnSaveTeam = "btnSaveTeam";
        public const string btnDeleteSelected = "btnDeleteSelected";
        public const string lblNDA = "lblNDA";
        public const string lblNoMembers = "lblNoMembers";
        public const string lblLoading = "lblLoading";
        public const string lblPartyType = "lblPartyType";
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
        public const string lblRequestNo = "lblRequestNo";
        public const string lblRequestStatus = "lblRequestStatus";
        public const string lblRequestService = "lblRequestService";
        public const string lblScholarships = "lblScholarships";
        public const string lblRequestCreatedDate = "lblRequestCreatedDate";
        public const string lblRequestCreatedTime = "lblRequestCreatedTime";


        public const string lblRequestDetails = "lblRequestDetails";

        public const string lblStatistics = "lblStatistics";
        public const string lblSchoolData = "lblSchoolData";
        public const string lblSupportingFiles = "lblSupportingFiles";
        public const string lblInterviewsSchedule = "lblInterviewsSchedule";
        public const string lblEvidenceCollection = "lblEvidenceCollection";
        public const string lblClassroomObservations = "lblClassroomObservations";
        public const string lblSchoolRounds = "lblSchoolRounds";
        public const string lblLabsVisits = "lblLabsVisits";
        public const string lblResultsAnalysis = "lblResultsAnalysis";
        public const string lblRealtimeEvaluationSystem = "lblRealtimeEvaluationSystem";
        public const string lblPeriodicReport = "lblPeriodicReport";

        public const string lblCloseModal = "lblCloseModal";
        public const string lblAddPlan = "lblAddPlan";
        public const string lblPlanRequests = "lblPlanRequests";
    }
    public static class EvaluationOperations
    {
        private const string Prefix = "lblEvaluationOperations";

        // Titles
        public const string PlansListTitle = Prefix + nameof(PlansListTitle);
        public const string PlanRequests = Prefix + nameof(PlanRequests);
        public const string RequestDetails = Prefix + nameof(RequestDetails);

        // Filters
        public const string AdvancedSearch = Prefix + nameof(AdvancedSearch);
        public const string Filter = Prefix + nameof(Filter);
        public const string Clear = Prefix + nameof(Clear);

        public const string RequestNo = Prefix + nameof(RequestNo);
        public const string Status = Prefix + nameof(Status);
        public const string RequestService = Prefix + nameof(RequestService);

        public const string CreatedDate = Prefix + nameof(CreatedDate);
        public const string CreatedTime = Prefix + nameof(CreatedTime);

        // Plan Info
        public const string PlanName = "lblEvaluationOperationsPlanName";
        public const string PlanStatus = Prefix + nameof(PlanStatus);
        public const string PlanStartDate = Prefix + nameof(PlanStartDate);
        public const string PlanEndDate = Prefix + nameof(PlanEndDate);
        public const string PlanCreatedOn = Prefix + nameof(PlanCreatedOn);
        public const string PlanSchoolsCount = Prefix + nameof(PlanSchoolsCount);

        // Actions
        public const string Actions = Prefix + nameof(Actions);
        public const string View = Prefix + nameof(View);
        public const string Edit = Prefix + nameof(Edit);
        public const string Delete = Prefix + nameof(Delete);

        // Dialogs
        public const string ConfirmDelete = Prefix + nameof(ConfirmDelete);
        public const string DeletePlanConfirm = Prefix + nameof(DeletePlanConfirm);

        // Sections
        public const string Statistics = Prefix + nameof(Statistics);
        public const string School = Prefix + nameof(School);
        public const string SupportingFiles = Prefix + nameof(SupportingFiles);
        public const string InterviewsSchedule = Prefix + nameof(InterviewsSchedule);
        public const string EvidenceCollection = Prefix + nameof(EvidenceCollection);
        public const string ClassroomObservations = Prefix + nameof(ClassroomObservations);
        public const string SchoolRounds = Prefix + nameof(SchoolRounds);
        public const string LabsVisits = Prefix + nameof(LabsVisits);
        public const string ResultsAnalysis = Prefix + nameof(ResultsAnalysis);
        public const string RealtimeEvaluationSystem = Prefix + nameof(RealtimeEvaluationSystem);
        public const string PeriodicReport = Prefix + nameof(PeriodicReport);

        // Buttons
        public const string CloseModal = Prefix + nameof(CloseModal);
        public const string AddPlan = Prefix + nameof(AddPlan);
        public const string CardView = Prefix + nameof(CardView);
        public const string TableView = Prefix + nameof(TableView);
        public const string All = Prefix + nameof(All);
    }
    public static class ReassignAssignment
    {
        // Page Header
        public const string ReassignAssignmentTitle = "ReassignAssignmentTitle";
        public const string ReassignAssignmentSubTitle = "ReassignAssignmentSubTitle";

        // Sections
        public const string SelectUsersSectionTitle = "SelectUsersSectionTitle";
        public const string AssignmentsTableTitle = "AssignmentsTableTitle";

        // Labels
        public const string FromUserLabel = "FromUserLabel";
        public const string ToUserLabel = "ToUserLabel";

        // Placeholders
        public const string SelectUserPlaceholder = "SelectUserPlaceholder";

        // Buttons
        public const string LoadButton = "LoadButton";
        public const string SelectAllButton = "SelectAllButton";
        public const string ClearAllButton = "ClearAllButton";
        public const string ResetButton = "ResetButton";
        public const string SaveReassignButton = "SaveReassignButton";

        // Table Headers
        public const string RowNumberHeader = "RowNumberHeader";
        public const string RequestNumberHeader = "RequestNumberHeader";
        public const string ServiceNameArHeader = "ServiceNameArHeader";
        public const string ServiceNameEnHeader = "ServiceNameEnHeader";

        // Selection Info
        public const string SelectedCountText = "SelectedCountText";
        public const string TotalCountText = "TotalCountText";

        // Messages
        public const string NoAssignmentsMessage = "NoAssignmentsMessage";
        public const string ReassignSuccessMessage = "ReassignSuccessMessage";
        public const string ReassignFailedMessage = "ReassignFailedMessage";
        public const string ConfirmReassignMessage = "ConfirmReassignMessage";

        // Validation Messages
        public const string FromUserRequiredMessage = "FromUserRequiredMessage";
        public const string ToUserRequiredMessage = "ToUserRequiredMessage";
        public const string SelectAssignmentRequiredMessage = "SelectAssignmentRequiredMessage";

        // Loading
        public const string LoadingMessage = "LoadingMessage";
        public const string SavingMessage = "SavingMessage";
    }
    public static class EvaluationPlans
    {
        private const string Prefix = "lblEvaluationPlans";

        // Filters
        public const string AcademicYear = Prefix + "AcademicYear";
        public const string SchoolName = Prefix + "SchoolName";
        public const string Search = Prefix + "Search";
        public const string Clear = Prefix + "Clear";
        public const string All = Prefix + "All";

        // View Mode
        public const string CardView = Prefix + "CardView";
        public const string TableView = Prefix + "TableView";

        // Table
        public const string PlanName = Prefix + "PlanName";
        public const string SchoolsCount = Prefix + "SchoolsCount";
        public const string Period = Prefix + "Period";
        public const string Status = Prefix + "Status";
        public const string View = Prefix + "View";
        public const string Actions = Prefix + "Actions";
    }
    public static class EvaluationForm
    {
        private const string Prefix = "lblEvaluationForm"; 

        public const string PageTitle = Prefix + "PageTitle";
        public const string PageDescription = Prefix + "PageDescription";

        public const string TabEvaluationPlans = Prefix + "TabEvaluationPlans";
        public const string TabEvaluationOperations = "TabEvaluationOperations";
        public const string TabSchools = Prefix + "TabSchools";

        public const string SubTabPlanRequests = Prefix + "SubTabPlanRequests";
        public const string SubTabPlanDetails = Prefix + "SubTabPlanDetails";

        public const string BreadcrumbSchool = Prefix + "BreadcrumbSchool";
        public const string BreadcrumbRequestNo = Prefix + "BreadcrumbRequestNo";

        public const string AccordionSchoolBasicData = Prefix + "AccordionSchoolBasicData";
        public const string AccordionSchoolData = Prefix + "AccordionSchoolData";

        public const string PlanDetailsTitle = Prefix + "PlanDetailsTitle";
        public const string PlanDetailsClose = Prefix + "PlanDetailsClose";

        public const string BreadcrumbPlans = Prefix + "BreadcrumbPlans";
        public const string BreadcrumbRequestDetails = Prefix + "BreadcrumbRequestDetails";

        public const string CreateRequestTitle = Prefix + "CreateRequestTitle";
        public const string CreateRequestBreadcrumb = Prefix + "CreateRequestBreadcrumb";

        public const string ActionDetailsHeader = Prefix + "ActionDetailsHeader";
        public const string ActionCloseBtn = Prefix + "ActionCloseBtn";

        public const string AccordionRequestDetails = Prefix + "AccordionRequestDetails";
        public const string AccordionActionLog = Prefix + "AccordionActionLog";
        public const string lblAllOpenRequests = Prefix + "lblAllOpenRequests";

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
        public static readonly string lblLoginMenu = "lblLoginMenu";
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
    //public static class AssignmentUserTeam
    //{

    //}

    public static class LanguageConst
    {
        public static readonly string En = "en";
        public static readonly string Ar = "ar";
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
        public static readonly string TemplateSendReminderToUser = "TemplateSendReminderToUser";
    }
    public static class EvalFormSettings
    {
        public static readonly string Eval_WEB_From_PageSize = "Eval_WEB_From_PageSize";
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
        public const string EvaluationPlan = "EvaluationPlan";
        public const string Evl_Form = "Evl_Form";
        public const string Time = "Time";
    }

    public static class Module
    {
        public static readonly string AuthenticationModule = "AuthenticationModule";
        public static readonly string ConfigurationModule = "ConfigurationModule";
        public static readonly string Notification = "NotificationModule";
        public static readonly string Alert = "AlertModule";
        public static readonly string EvaluationModule = "EvaluationModule";
    }
    public static class ActionTypeIds
    {
        public static readonly Guid Draft = Guid.Parse("f328d31b-86ca-4994-b2ae-4c2d6b3dfec3");
        public static readonly Guid Info = Guid.Parse("7227bc28-d11b-4885-a6c0-ad402471d9e3");
        public static readonly Guid INFO_Override_Approve = Guid.Parse("2a9e6657-2122-46da-8790-7dd41a391276");
        public static readonly Guid Edit = Guid.Parse("b8c6d1b0-d915-442f-a7b4-18f9cd8d6a10");
        public static readonly Guid Approve = Guid.Parse("e4c923d1-b9fa-4f59-94bc-dae7c90bcf4f");
        public static readonly Guid Reject = Guid.Parse("9c7a6f74-bb19-41d1-929d-15a12372ff1d");
        public static readonly Guid Assign = Guid.Parse("b163ef2d-fdf4-43c3-8488-22544f5643ae");
        public static readonly Guid ASSIGNT_TEAM = Guid.Parse("b163ef2d-fdf4-43c3-8488-22544f564311");
        public static readonly Guid ApproveAndAssign = Guid.Parse("8ab3257e-748f-4207-89f7-407e4175b9a7");
        public static readonly Guid RETURNBACK = Guid.Parse("d71bba64-9e7f-4e19-948f-03f08d0243f7");
        public static readonly Guid SubmitMissingData = Guid.Parse("6d697ec3-cc8c-485a-bdf3-ef2e9c24c512");
        public static readonly Guid RequestDataChange = Guid.Parse("cfeb4c71-d1a7-4d35-9a77-5f90e1f8768b");
        public static readonly Guid CreatePlan = Guid.Parse("3e6d8fd3-76f9-479f-94f9-14b8b4a7d589");
        public static readonly Guid ReserveVacancy = Guid.Parse("423dbe7b-3e9d-4f10-bcf4-6d232de5ec39");
        public static readonly Guid INFO_WITH_DRAFT = Guid.Parse("d71bba64-9e7f-4e19-948f-03f08d024311");

    }
    public static class ActionTypeKeys
    {
        public const string Approve = "APPROVE";
        public const string Reject = "REJECT";
        public const string Info = "INFO";
        public const string Assign = "ASSIGN";
        public const string ASSIGNT_TEAM = "ASSIGNT_TEAM";
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
        public const string CreateEvaluationPlan = "CREATE_PLAN";
        public const string UPDATE_ITEGRATION_FIELDS = "UPDATE_ITEGRATION_FIELDS";
        public const string INFO_WITH_DRAFT = "INFO_WITH_DRAFT";
        public const string CLOSE_AND_DELETE_PLAN = "CLOSE_AND_DELETE_PLAN";
        public const string CLOSE_AND_UPDATE_PLAN = "CLOSE_AND_UPDATE_PLAN";
        public const string CLOSE_AND_UPDATE_FORM = "CLOSE_AND_UPDATE_FORM";
    }
	public static class FieldInfoTypeKeys
	{

		public const string VisitDateFrom = "VisitDateFrom";
		public const string VisitDateTo = "VisitDateTo";
		public const string VisitName = "VisitName";


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


    public static class ModuleType
    {
        public const string EvaluationPlan = "EvaluationPlan";
        public const string EvaluationRequest = "EvaluationRequest";
        public const string EvaluationParty = "EvaluationParty";

    }
    public static class ModuleTypeIds
    {
        public static readonly Guid EvaluationPlan = Guid.Parse("FFFBF420-AED3-4A73-B25E-B7B9E67337A7");

        public static readonly Guid EvaluationRequest = Guid.Parse("6677F0C5-2531-4123-98F8-8E0A7C5B0ADA");

        public static readonly Guid EvaluationParty = Guid.Parse("281E98ED-9ED5-4AF3-8C08-4A659BE99DC8");
    }
	public static class NDAStatic
	{
		public static string Pending = "PENDING";
		public static string Approved = "APPROVED";
		public static string Objection = "OBJECTION";
		public static string Suspended = "SUSPENDED";
		public static string Forced = "FORCED";
	}
	public static class NDAStatusIds
	{
		public static readonly Guid Objection =Guid.Parse("466D8F62-F8F9-4A3F-B364-82C77D7DE72D");

		public static readonly Guid Approve =Guid.Parse("E679F265-2BDA-4A36-8C4C-8911EE26BB7B");

		public static readonly Guid Suspended =Guid.Parse("8F017EEF-84C1-46B9-9E89-9698BAB996CF");

		public static readonly Guid Pending =Guid.Parse("B08F866B-52E7-4B98-966B-B16F0C4FCCE7");

		public static readonly Guid Approved =Guid.Parse("2E30F24C-A86D-48A5-B922-DB59859AB617");

		public static readonly Guid Forced =Guid.Parse("D3CCAB02-789D-4D44-A8BC-EBEFABCCF2C4");
	}
	public static class EvaluationDetailsModal
    {
        private const string Prefix = "EvaluationDetailsModal.";

        public const string Title = Prefix + "Title";
        public const string Statistics = Prefix + "Statistics";
    }
    public static class WebSchools
    {
        private const string Prefix = "lblWebSchools";

        // Header
        public const string Schools = Prefix + "Schools";

        // Filters
        public const string SchoolName = Prefix + "SchoolName";
        public const string SchoolCode = Prefix + "SchoolCode";
        public const string Phase = Prefix + "Phase";
        public const string Type = Prefix + "Type";
        public const string Region = Prefix + "Region";
        public const string CurrentPlanStatus = Prefix + "CurrentPlanStatus";

        // Buttons
        public const string Search = Prefix + "Search";
        public const string ClearFilter = Prefix + "ClearFilter";

        // Table
        public const string Name = Prefix + "Name";
        public const string Code = Prefix + "Code";
        public const string TypeCol = Prefix + "TypeCol";
        public const string LevelsCol = Prefix + "LevelsCol";
        public const string RegionCol = Prefix + "RegionCol";
        public const string StatusCol = Prefix + "StatusCol";

        // Modal
        public const string SchoolDetails = Prefix + "SchoolDetails";
    }
    public static class UserGenderIds
    {
        public static readonly Guid MALE = Guid.Parse("550e8400-e29b-41d4-a716-446655440001");
        public static readonly Guid FEMALE = Guid.Parse("550e8400-e29b-41d4-a716-446655440002");
    }
    public static class FormGroupTypeKeyIds
    {
        public static readonly Guid FormGroup = Guid.Parse("b8c6d1b0-d915-442f-a7b4-18f9cd8d6a10");
        public static readonly Guid List = Guid.Parse("e4c923d1-b9fa-4f59-94bc-dae7c90bcf4f");
    }
    public static class TransactionTypeGuidIds
    {
        public static readonly Guid INSERT = Guid.Parse("550e8400-e29b-41d4-a716-446655440001");
        public static readonly Guid APPROVE = Guid.Parse("550e8400-e29b-41d4-a716-446655440002");
        public static readonly Guid REJECT = Guid.Parse("550e8400-e29b-41d4-a716-446655440003");
        public static readonly Guid UPDATE = Guid.Parse("550e8400-e29b-41d4-a716-446655440004");
        public static readonly Guid REQUESTMISSING = Guid.Parse("550e8400-e29b-41d4-a716-446655440005");
    }
    public static class ServiceStatusTypeBackend
    {
        public static string Open = "Open";
        public static string Completed = "Completed";
        public static string Closed = "Closed";
    }
     public static class SystemModuleBackend
	{
        public static string EvaluationRequest = "EvaluationRequest";
    }


}
