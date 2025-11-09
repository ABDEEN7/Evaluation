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
        public static readonly string InvalidDraftPlan = "InvalidDraftPlan";
        public static readonly string DraftPlanWithSameAcademicYearAlreadyExists = "DraftPlanWithSameAcademicYearAlreadyExists";
        public static readonly string PlanInThePastIsNotAllowed = "PlanInThePastIsNotAllowed";
        public static readonly string EmailIsRequired = "EmailIsRequired";
        public static readonly string UserDataNotFound = "UserDataNotFound";
        public static readonly string PlanIsNotFound = "PlanIsNotFound";
        public static readonly string InvalidRequest = "InvalidRequest";
        public static readonly string InvalidApprovePlan = "InvalidApprovePlan";
    }
    public static class AdminSettings
    {
        public static readonly string EnableCaching = "EnableCaching";
        public static readonly string ClearCacheDuration = "ClearCacheDuration";

            
        }
    public static class AdminPages
    {
        public static readonly string AdminCommon = "AdminCommon";

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
