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
            public static readonly string InvalidDraftPlan = "InvalidDraftPlan";
            public static readonly string DraftPlanWithSameAcademicYearAlreadyExists = "DraftPlanWithSameAcademicYearAlreadyExists";
            public static readonly string PlanInThePastIsNotAllowed = "PlanInThePastIsNotAllowed";
            public static readonly string EmailIsRequired = "EmailIsRequired";
            public static readonly string UserDataNotFound = "UserDataNotFound";
        }

        public static class AdminSettings
        {
            public static readonly string EnableCaching = "EnableCaching";
            public static readonly string ClearCacheDuration = "ClearCacheDuration";

            
        }
        public static class WebAppSettings
        {
        }

    }
}
