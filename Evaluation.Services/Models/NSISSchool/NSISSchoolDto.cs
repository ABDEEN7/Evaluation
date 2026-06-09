using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Models.NSISSchool
{
	public class NSISSchoolDto
	{
		public string Institution { get; set; }       // INSTITUTION
		public string Location { get; set; }          // LOCATION
		public string NameEn { get; set; }            // SCHOOL_NAME_ENG
		public string NameAr { get; set; }            // SCHOOL_NAME_ARA
		public string Campus { get; set; }            // CAMPUS
		public string AreaCode { get; set; }          // AREA
		public string AreaNameAr { get; set; }        // AREA_NAME_ARA
		public string AreaNameEn { get; set; }        // AREA_NAME_ENG
		public string SubAreaCode { get; set; }       // SUB_AREA
		public string SubAreaNameAr { get; set; }     // SUB_AREA_NAME_ARA
		public string SubAreaNameEn { get; set; }     // SUB_AREA_NAME_ENG
		public string Address { get; set; }           // ADDRESS_LINE1
		public string Phone { get; set; }             // PHONE
		public string Fax { get; set; }               // FAX
		public string Email { get; set; }             // EMAIL_ADDR
		public string SchoolCategoryCode { get; set; } // SCHOOL_CATEGORY
		public string SchoolCategoryAr { get; set; }  // SCHOOL_CATEGORY_ARA
		public string SchoolCategoryEn { get; set; }  // SCHOOL_CATEGORY_ENG
		public string GenderCode { get; set; }        // SCHL_GENDER_CD
		public string GenderName { get; set; }        // SCHL_GENDER_DN
		public bool HasKG { get; set; }               // KG = 'Y'
		public bool HasPrimary { get; set; }          // PRIMARY = 'Y'
		public bool HasPreparatory { get; set; }      // PREPARATORY = 'Y'
		public bool HasSecondary { get; set; }        // SECONDARY = 'Y'
		public string ModelCode { get; set; }         // SCHL_MODEL_CD
		public string ModelName { get; set; }         // SCHL_MODEL_DN
		public string CohortCode { get; set; }        // SCHL_COHORT_CD
		public string CohortName { get; set; }        // SCHL_COHORT_DN
		public string EmpGenderCode { get; set; }     // SCHL_EMP_GEN_CD
		public string EmpGenderName { get; set; }     // SCHL_EMP_GEN_DN
		public string TypeCode { get; set; }          // SCHL_TYPE_CD
		public string TypeName { get; set; }          // SCHL_TYPE_DN
		public string Latitude { get; set; }          // LATITUDE
		public string Longitude { get; set; }         // LONGITUDE
		public string Url { get; set; }               // URL
		public string OutsCode { get; set; }          // SCHL_OUTS_CD
		public string OutsName { get; set; }          // SCHL_OUTS_DN
		public string ProgramCode { get; set; }       // SCHL_PROGRAM_CD
		public string ProgramName { get; set; }       // SCHL_PROGRAM_DN
		public int SchoolCapacity { get; set; }       // SCHOOL_CAPACITY
		public string PinNumber { get; set; }         // SCHL_PIN_NUMBER
	}

	// NSISScheduleDto.cs
	public class NSISScheduleDto
	{
		public string Institution { get; set; }
		public string EmplId { get; set; }
		public string Subject { get; set; }
		public string AcadPlan { get; set; }
		public string PlanSection { get; set; }
		public string GroupId { get; set; }
		public int Periods { get; set; }
		public int DayCd { get; set; }
		public DateTime PeriodBegin { get; set; }
		public DateTime PeriodEnd { get; set; }
		public string LocationCode { get; set; }
	}

	// NSISHomeroomDto.cs
	public class NSISHomeroomDto
	{
		public string Strm { get; set; }
		public string Institution { get; set; }
		public string Location { get; set; }
		public string AcadLevel { get; set; }
		public string AcadPlan { get; set; }
		public string PlanSection { get; set; }
	}

	// NSISAcadPlanDto.cs
	public class NSISAcadPlanDto
	{
		public string AcadPlan { get; set; }
		public string AcadPlanAr { get; set; }
		public string AcadPlanEn { get; set; }
		public string AcadLevel { get; set; }
		public string AcadProg { get; set; }
		public string AcadProgAr { get; set; }
		public string AcadProgEn { get; set; }
	}

	public class LookupDto
	{
		public string? Code { get; set; }
		public string? NameAr { get; set; }
		public string? NameEn { get; set; }
	}

	public class NSISCourseDto
	{
		public string? Year { get; set; }
		public string? Institution { get; set; }
		public string? AcadPlan { get; set; }
		public string? SubjectCode { get; set; }
		public string? ShortSubjectNameEn { get; set; }
		public string? SubjectNameEn { get; set; }
		public string? SubjectNameAr { get; set; }
	}
}
