using AutoMapper;
using Dapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.IntegrationEntity;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.OutputAnalysis;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Models.NSISSchool;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.QNEDsDto;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.FormAnalysisDtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Evaluation.Services.Integration;

public class QNEDSService : ApiBase
{
	private readonly IntegrationLogger _integrationLogger;

	public QNEDSService(
		IServiceScopeFactory serviceScopeFactory,
		CacheDataProvider cacheDataProvider,
		UnitOfWork uow,
		LoggingServices loggingServices,
		IMapper mapper,
		UserInfo userInfo,
		IServiceProvider serviceProvider,
		RequestInfo requestInfo,
		IntegrationLogger integrationLogger)
		: base(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
	{
		_integrationLogger = integrationLogger;
	}

	// EV_Percent_of_Achvmnt_By_Grade_and_Subject_G1_10
	public async Task<List<AchievementDto>> GetAchievementByInstitutionAndYearAsync(string institutionId, int year)
	{
		using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		const string sql = @"
            SELECT
                STRM,
                Timespan,
                Institution,
                Program,
                Grade,
                CourseCode,
                CourseTitle,
                Grade_Achvment_NoSev3_And_Present_NoActivity,
                No_of_Student_Grade_Without_Activity
            FROM dbo.EV_Percent_of_Achvmnt_By_Grade_and_Subject_G1_10
            WHERE Institution = @Institution
              AND STRM = @Year";

		var result = await con.QueryAsync<AchievementDto>(sql, new { Institution = institutionId, Year = year });
		return result.ToList();
	}

	// EV_Percent_of_Achvmnt_By_Grade_and_Subject_G1_10_Sev3
	public async Task<List<AchievementSev3Dto>> GetAchievementSev3ByInstitutionAndYearAsync(string institutionId, int year)
	{
		var result = new List<AchievementSev3Dto>();
		const string sql = @"
            SELECT
                STRM,
                Timespan,
                Institution,
                Program,
                Grade,
                CourseCode,
                CourseTitle,
                Grade_Achvment_Sev3_And_Present_NoActivity,
                No_of_Student_Grade_Without_Activity
            FROM dbo.EV_Percent_of_Achvmnt_By_Grade_and_Subject_G1_10_Sev3
            WHERE Institution = @Institution
              AND STRM = @Year";

		await using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		await con.OpenAsync();
		await using var cmd = new SqlCommand(sql, con);
		cmd.Parameters.Add("@Institution", System.Data.SqlDbType.Int).Value = institutionId;
		cmd.Parameters.Add("@Year", System.Data.SqlDbType.Int).Value = year;

		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			result.Add(new AchievementSev3Dto
			{
				STRM = Convert.ToInt32(reader["STRM"]),
				Timespan = reader["Timespan"]?.ToString(),
				Institution = reader["Institution"]?.ToString(),
				Program = Convert.ToInt32(reader["Program"]),
				Grade = reader["Grade"]?.ToString(),
				CourseCode = reader["CourseCode"]?.ToString(),
				CourseTitle = reader["CourseTitle"]?.ToString(),
				Grade_Achvment_Sev3_And_Present_NoActivity =
					Convert.ToDecimal(reader["Grade_Achvment_Sev3_And_Present_NoActivity"]),
				No_of_Student_Grade_Without_Activity =
					Convert.ToInt32(reader["No_of_Student_Grade_Without_Activity"])
			});
		}
		return result;
	}

	// EV_Percent_of_Achvmnt_By_GRADE_No_Subject_G12
	public async Task<List<AchievementG12NoSubjectDto>> GetAchievementG12ByInstitutionAndYearAsync(string institutionId, int year)
	{
		using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		const string sql = @"
            SELECT
                STRM,
                Timespan,
                Institution,
                Program,
                Grade,
                Grade_NumberOf_Students_NoSev3_And_Present,
                Track_Achvment_NoSev3_And_Present
            FROM dbo.EV_Percent_of_Achvmnt_By_GRADE_No_Subject_G12
            WHERE Institution = @Institution
              AND STRM = @Year";

		var result = await con.QueryAsync<AchievementG12NoSubjectDto>(sql, new { Institution = institutionId, Year = year });
		return result.ToList();
	}

	public async Task<List<AchievementTrackSubjectG11G12Dto>> GetAchievementTrackSubjectG11G12ByInstitutionAndYearAsync(string institutionId, int year)
	{
		using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		const string sql = @"
        SELECT
            STRM,
            Timespan,
            Institution,
            Program,
            Grade,
            Track,
            CourseCode,
            CourseTitle,
            Grade_NumberOf_Students_NoSev3_And_Present,
            Grade_Achvment_NoSev3_And_Present,
            Grade_Success_NoSev3_And_Present
        FROM dbo.EV_Percent_of_Achvmnt_By_Track_and_Subject_G11_12
        WHERE Institution = @Institution
          AND STRM = @Year";

		var result = await con.QueryAsync<AchievementTrackSubjectG11G12Dto>(sql, new { Institution = institutionId, Year = year });
		return result.ToList();
	}
	public async Task<List<SuccessRateByGradeDto>> GetSuccessRateByGradeByInstitutionAndYearAsync(string institutionId, int year)
	{
		using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		const string sql = @"
        SELECT
            STRM,
            Timespan,
            Institution,
            Program,
            Grade,
            NotSucceededPercentOfStudent,
            SucceededPercentOfStudent
        FROM dbo.EV_Percent_of_Success_By_GRADE
        WHERE Institution = @Institution
          AND STRM = @Year";

		var result = await con.QueryAsync<SuccessRateByGradeDto>(sql, new { Institution = institutionId, Year = year });
		return result.ToList();
	}
	// EV_Yearly_Percent_of_Student_Below_70_Track
	public async Task<List<YearlyStudentBelow70TrackDto>> GetYearlyStudentBelow70TrackByInstitutionAndYearAsync(string institutionId, int year)
	{
		using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		const string sql = @"
        SELECT
            strm,
            Institution,
            Grade,
            CourseCode,
            TermCode,
            Program,
            Track,
            No_of_Student_Grading_Assignment,
            No_of_Student_Grading_Assignment_Below_70
        FROM dbo.EV_Yearly_Percent_of_Student_Below_70_Track
        WHERE Institution = @Institution
          ";

		var result = await con.QueryAsync<YearlyStudentBelow70TrackDto>(sql, new { Institution = institutionId, Year = year });
		return result.ToList();
	}
	public async Task<List<YearlyStudentBelow70GradeDto>> GetYearlyStudentBelow70GradeByInstitutionAndYearAsync(string institutionId, int year)
	{
		using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		const string sql = @"
    SELECT
        strm,
        Institution,
        Grade,
        CourseCode,
        TermCode,
        Program,
        No_of_Student_Grading_Assignment,
        No_of_Student_Grading_Assignment_Below_70
    FROM dbo.EV_Yearly_Percent_of_Student_Below_70_Grade
    WHERE Institution = @Institution
      AND strm = @Year";

		var result = await con.QueryAsync<YearlyStudentBelow70GradeDto>(sql, new { Institution = institutionId, Year = year });
		return result.ToList();
	}
	// EV_Percent_of_Success_By_Track
	public async Task<List<SuccessRateByTrackDto>> GetSuccessRateByTrackByInstitutionAndYearAsync(string institutionId, int year)
	{
		using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		const string sql = @"
    SELECT
        STRM,
        Timespan,
        Institution,
        Program,
        Grade,
        Track,
        NotSucceededPercentOfStudent,
        SucceededPercentOfStudent
    FROM dbo.EV_Percent_of_Success_By_Track
    WHERE Institution = @Institution
      AND STRM = @Year";

		var result = await con.QueryAsync<SuccessRateByTrackDto>(sql, new { Institution = institutionId, Year = year });
		return result.ToList();
	}
	// EV_Percent_of_Achvmnt_By_Track_No_Subject_G11_12
	public async Task<List<AchievementTrackNoSubjectG11G12Dto>> GetAchievementTrackNoSubjectG11G12ByInstitutionAndYearAsync(string institutionId, int year)
	{
		using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		const string sql = @"
    SELECT
        STRM,
        Timespan,
        Institution,
        Program,
        Grade,
        Track,
        Grade_NumberOf_Students_NoSev3_And_Present,
        Track_Achvment_NoSev3_And_Present
    FROM dbo.EV_Percent_of_Achvmnt_By_Track_No_Subject_G11_12
    WHERE Institution = @Institution
      AND STRM = @Year";

		var result = await con.QueryAsync<AchievementTrackNoSubjectG11G12Dto>(sql, new { Institution = institutionId, Year = year });
		return result.ToList();
	}
	public async Task<List<AchievementG1G11Dto>> GetAchievementG1G11ByInstitutionAndYearAsync(string institutionId, int year)
	{
		using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		const string sql = @"
    SELECT
        STRM,
        Timespan,
        Institution,
        Program,
        Grade,
        CourseCode,
        CourseTitle,
        Grade_Achvment_NoSev3_And_Present_NoActivity,
        No_of_Student_Grade_Without_Activity
    FROM dbo.EV_Percent_of_Achvmnt_By_Grade_and_Subject_G1_11
    WHERE Institution = @Institution
      AND STRM = @Year";

		var result = await con.QueryAsync<AchievementG1G11Dto>(sql, new { Institution = institutionId, Year = year });
		return result.ToList();
	}
	// vw_Ev_TeacherSchedule
	public async Task<List<TeacherScheduleDto>> GetTeacherScheduleByInstitutionAndYearAsync(string institutionId, int year)
	{
		using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		const string sql = @"
    SELECT
        STRM,
        SC_Report_ID,
        INSTITUTION,
        AcedemicPlanEn,
        PLAN_SECTION,
        SubjectNameEn,
        SC_FULL_NAME_ARA,
        SC_FULL_NAME_ENG
    FROM dbo.vw_Ev_TeacherSchedule
    WHERE INSTITUTION = @Institution
      AND STRM = @Year";

		var result = await con.QueryAsync<TeacherScheduleDto>(sql, new { Institution = institutionId, Year = year });
		return result.ToList();
	}
	// vw_Student_Daily_Attendance_By_Month
	public async Task<List<StudentDailyAttendanceByMonthDto>> GetStudentDailyAttendanceByMonthByInstitutionAndYearAsync(string institutionId, int year)
	{
		using var con = new SqlConnection(ClsAppSetting.QNEDSConnection);
		const string sql = @"
    SELECT
        STRM,
        Month,
        SchoolID,
        Program,
        Grade,
        PlanSection,
        Students,
        AttendanceDescription
    FROM dbo.vw_Student_Daily_Attendance_By_Month
    WHERE SchoolID = @Institution
      AND STRM = @Year";

		var result = await con.QueryAsync<StudentDailyAttendanceByMonthDto>(sql, new { Institution = institutionId, Year = year });
		return result.ToList();
	}

	// ─── Sync ─────────────────────────────────────────────────────────────────

	public async Task<bool> SyncQnedsIntegrationAsync(int academicYear)
	{
		var schools = await uow.GetRepository<OrgTree>()
			.GetAllActiveNonDeleted()
			.Where(x => x.NSISCode != null)
			.Select(x => new { x.Id, x.NSISCode })
			.ToListAsync();

		foreach (var school in schools)
		{
			var alreadySynced = await uow.GetRepository<QnedsIntegration>()
				.GetAllActiveNonDeleted()
				.AnyAsync(x => x.OrgTreeId == school.Id && x.AcademicYear == academicYear);

			if (!alreadySynced)
				await SyncSchoolYear(school.Id, academicYear);
		}

		return true;
	}

	public async Task<bool> SyncQnedsIntegrationBySchoolAsync(Guid schoolId, int academicYear)
	{
		await SyncSchoolYear(schoolId, academicYear);
		await SyncSchoolYear(schoolId, academicYear - 1);
		return true;
	}

	private async Task SyncSchoolYear(Guid schoolId, int academicYear)
	{
		var school = await uow.GetRepository<OrgTree>()
			.GetAllActiveNonDeleted(x => x.Id == schoolId)
			.Select(x => new { x.Id, x.NSISCode })
			.FirstOrDefaultAsync();

		if (school == null || string.IsNullOrEmpty(school.NSISCode))
			return;

		var data = await FetchAllQnedsDataAsync(school.NSISCode, academicYear);

		if (!HasAnyData(data))
			return;

		var json = JsonSerializer.Serialize(data);

		var existing = await uow.GetRepository<QnedsIntegration>()
			.GetAllActiveNonDeleted()
			.FirstOrDefaultAsync(x => x.OrgTreeId == school.Id && x.AcademicYear == academicYear);

		if (existing != null)
		{
			existing.JsonValue = json;
			existing.UpdateDate = DateTime.Now;
			existing.IsActive = true;
			existing.IsDeleted = false;
			uow.GetRepository<QnedsIntegration>().Update(existing);
		}
		else
		{
			await uow.GetRepository<QnedsIntegration>().InsertAsync(new QnedsIntegration
			{
				Id = Guid.NewGuid(),
				AcademicYear = academicYear,
				OrgTreeId = school.Id,
				JsonValue = json,
				QnedsConfig = null,
				IsActive = true,
				CreateDate = DateTime.Now,
				IsDeleted = false
			});
		}

		await uow.CommitAsync();
	}

	private async Task<QnedsIntegrationJsonDto> FetchAllQnedsDataAsync(string institutionId, int year) => new()
	{
		Achievement = await GetAchievementByInstitutionAndYearAsync(institutionId, year),
		AchievementSev3 = await GetAchievementSev3ByInstitutionAndYearAsync(institutionId, year),
		AchievementG12 = await GetAchievementG12ByInstitutionAndYearAsync(institutionId, year),
		AchievementTrackSubjectG11G12 = await GetAchievementTrackSubjectG11G12ByInstitutionAndYearAsync(institutionId, year),
		SuccessRateByGrade = await GetSuccessRateByGradeByInstitutionAndYearAsync(institutionId, year),
		YearlyStudentBelow70Track = await GetYearlyStudentBelow70TrackByInstitutionAndYearAsync(institutionId, year),
		YearlyStudentBelow70Grade = await GetYearlyStudentBelow70GradeByInstitutionAndYearAsync(institutionId, year),
		SuccessRateByTrack = await GetSuccessRateByTrackByInstitutionAndYearAsync(institutionId, year),
		AchievementTrackNoSubjectG11G12 = await GetAchievementTrackNoSubjectG11G12ByInstitutionAndYearAsync(institutionId, year),
		AchievementG1G11 = await GetAchievementG1G11ByInstitutionAndYearAsync(institutionId, year),
		TeacherSchedule = await GetTeacherScheduleByInstitutionAndYearAsync(institutionId, year),
		StudentDailyAttendance = await GetStudentDailyAttendanceByMonthByInstitutionAndYearAsync(institutionId, year)
	};

	private static bool HasAnyData(QnedsIntegrationJsonDto data) =>
		(data.Achievement?.Any() ?? false) ||
		(data.AchievementSev3?.Any() ?? false) ||
		(data.AchievementG12?.Any() ?? false) ||
		(data.AchievementTrackSubjectG11G12?.Any() ?? false) ||
		(data.SuccessRateByGrade?.Any() ?? false) ||
		(data.YearlyStudentBelow70Track?.Any() ?? false) ||
		(data.YearlyStudentBelow70Grade?.Any() ?? false) ||
		(data.SuccessRateByTrack?.Any() ?? false) ||
		(data.AchievementTrackNoSubjectG11G12?.Any() ?? false) ||
		(data.AchievementG1G11?.Any() ?? false) ||
		(data.TeacherSchedule?.Any() ?? false) ||
		(data.StudentDailyAttendance?.Any() ?? false);

	// ─── Output Analysis Generation ───────────────────────────────────────────

	public async Task<bool> GenerateOutputAnalysisFromQnedsAsync(Guid evaluationRequestId, int academicYear)
	{
		var evaluationRequest = await uow.GetRepository<EvaluationRequest>()
			.GetAllActiveNonDeleted(x => x.Id == evaluationRequestId)
			.Include(x => x.OrgTree)
			.FirstOrDefaultAsync();

		if (evaluationRequest?.OrgTreeId == null)
			return false;

		var orgTreeId = evaluationRequest.OrgTreeId;
		var previousYear = academicYear - 1;

		var integrations = await uow.GetRepository<QnedsIntegration>()
			.GetAllActiveNonDeleted(x =>
				x.OrgTreeId == orgTreeId &&
				(x.AcademicYear == academicYear || x.AcademicYear == previousYear))
			.ToListAsync();

		var lastJson = Deserialize(integrations.FirstOrDefault(x => x.AcademicYear == academicYear)?.JsonValue);
		if (lastJson == null) return false;

		var previousJson = Deserialize(integrations.FirstOrDefault(x => x.AcademicYear == previousYear)?.JsonValue);

		var analysisTypes = await uow.GetRepository<AnalysisType>()
			.GetAllActiveNonDeleted()
			.Where(x => AnalysisBackendNames.Contains(x.BackendName))
			.Include(x => x.DataFormEvalMatrix).ThenInclude(x => x.FormEvalMatrixValues)
			.Include(x => x.ResultFormEvalMatrix).ThenInclude(x => x.FormEvalMatrixValues)
			.OrderBy(x => x.OrderNo)
			.ToListAsync();

		foreach (var analysisType in analysisTypes)
		{
			var details = BuildOutputAnalysisDetails(
				analysisType, evaluationRequestId,
				academicYear, previousYear,
				lastJson, previousJson);

			if (!details.Any()) continue;

			foreach (var detail in details)
			{
				FormEvalMatrixValue? dataMatrixValue;

				if (analysisType.BackendName == "StudentsWithDisabilities")
				{
					dataMatrixValue = GetSupportPreparatoryMatrixValue(
						analysisType.DataFormEvalMatrix?.FormEvalMatrixValues,
						detail.ActualValue,
						detail.Difference);
				}
				else
				{
					dataMatrixValue = GetMatrixValue(
						analysisType.DataFormEvalMatrix?.FormEvalMatrixValues,
						detail.ActualValue);
				}

				detail.FormEvalMatrixValueId = dataMatrixValue?.Id;
				detail.Note = dataMatrixValue?.NameAr;
				detail.MartixTextValue = dataMatrixValue?.ReportTextAr;
			}

			var totalStudents = details.Sum(x => x.LastYearStudentCount + x.PreviousYearStudentCount);

			decimal avgActualValue;

			if (totalStudents > 0)
			{
				avgActualValue = details.Sum(x =>
					x.LastYearValue * x.LastYearStudentCount +
					x.PreviousYearValue * x.PreviousYearStudentCount
				) / totalStudents;
			}
			else
			{
				avgActualValue = details.Average(x => x.ActualValue);
			}

			var resultMatrixValue = GetMatrixValue(
				analysisType.ResultFormEvalMatrix?.FormEvalMatrixValues,
				avgActualValue);

			var finalResult = new OutputAnalysisFinalResult
			{
				Id = Guid.NewGuid(),
				AnalysisTypeId = analysisType.Id,
				EvaluationRequestId = evaluationRequestId,
				ActualValue = avgActualValue,
				FormEvalMatrixValueId = resultMatrixValue?.Id,
				Note = resultMatrixValue?.NameAr,
				IsActive = true,
				CreateDate = DateTime.Now,
				IsDeleted = false
			};
			await uow.GetRepository<OutputAnalysisFinalResult>().InsertAsync(finalResult);

			foreach (var detail in details)
			{
				detail.OutputAnalysisFinalResultId = finalResult.Id;
				detail.IsActive = true;
				detail.CreateDate = DateTime.Now;
				detail.IsDeleted = false;
				await uow.GetRepository<OutputAnalysisData>().InsertAsync(detail);
			}
		}

		await uow.CommitAsync();
		return true;
	}
	private static FormEvalMatrixValue? GetMatrixValue(
	IEnumerable<FormEvalMatrixValue>? values,
	decimal actualValue)
	{
		return values?
			.Where(x =>
				x.IsActive &&
				!x.IsDeleted &&
				actualValue >= x.MinValue &&
				actualValue <= x.MaxValue)
			.OrderBy(x => x.OrderNo)
			.FirstOrDefault();
	}
	private static readonly List<string> AnalysisBackendNames =
[
	"AcademicAchievement",
	"StudentsWithDisabilities",
	"LowPerformanceStudents",
	"FailedStudents",
	"StudentLevelsAnalysis",
	"AchievementTests",
	"GeneralSecondaryAchievement",
	"GeneralSecondarySuccess"
];

	private static int GetSupportPreparatoryMatrixCode(decimal actualValue, decimal difference)
	{
		if (actualValue == 0)
			return 0; // No Data

		if (actualValue < 50)
			return 1; // Low Achievement Level

		if (actualValue < 70)
		{
			if (difference < -5)
				return 5; // Regression

			if (difference <= 5)
				return 3; // Stable Below Level

			return 4; // Improvement Below Level
		}

		if (actualValue < 80)
		{
			if (difference < -5)
				return 2; // Regression Below Level

			if (difference <= 5)
				return 3; // Stable Below Level

			return 7; // Improvement
		}

		if (actualValue < 90)
		{
			if (difference < -5)
				return 8; // Regression at High Level

			return 9; // High
		}

		return 10; // Very High Achievement Level
	}

	private static FormEvalMatrixValue? GetSupportPreparatoryMatrixValue(
	IEnumerable<FormEvalMatrixValue>? values,
	decimal actualValue,
	decimal difference)
	{
		if (values == null)
			return null;

		var matrixCode = GetSupportPreparatoryMatrixCode(actualValue, difference);

		return values
			.Where(x => x.IsActive && !x.IsDeleted)
			.OrderBy(x => x.OrderNo)
			.FirstOrDefault(x => x.ActualMatrixValue == matrixCode);
	}
	private static QnedsIntegrationJsonDto? Deserialize(string? json) =>
		string.IsNullOrWhiteSpace(json)
			? null
			: JsonSerializer.Deserialize<QnedsIntegrationJsonDto>(json);

	// ─── Detail Builders ──────────────────────────────────────────────────────

	private List<OutputAnalysisData> BuildOutputAnalysisDetails(
		AnalysisType analysisType,
		Guid evaluationRequestId,
		int lastYear,
		int previousYear,
		QnedsIntegrationJsonDto lastJson,
		QnedsIntegrationJsonDto? previousJson)
	{
		var allowedGrades = ParseGrades(analysisType.Grades);
		var allowedSubjects = ParseSubjectCodes(analysisType.SubjectCode);
		var config = GetAnalysisConfig(analysisType.AnalysisConfig);

		var allowedTracks = ToFilterSet(config.Tracks);
		var allowedTerms = ToFilterSet(config.Terms);

		var lastAttendance = BuildAttendanceStudentCountLookup(lastJson.StudentDailyAttendance, allowedGrades);
		var previousAttendance = BuildAttendanceStudentCountLookup(previousJson?.StudentDailyAttendance, allowedGrades);

		var ctx = new BuilderContext(analysisType.Id, evaluationRequestId, lastYear, previousYear,
									 allowedGrades, allowedSubjects, allowedTracks,
	allowedTerms, lastAttendance, previousAttendance);

		return analysisType.BackendName switch
		{
			"AcademicAchievement" =>
			[
				..BuildAchievement(ctx, lastJson.Achievement, previousJson?.Achievement),
		..BuildAchievementTrackSubjectG11G12(ctx, lastJson.AchievementTrackSubjectG11G12, previousJson?.AchievementTrackSubjectG11G12)
			],

			"StudentsWithDisabilities" =>
				BuildAchievementSev3(ctx, lastJson.AchievementSev3, previousJson?.AchievementSev3),

			"LowPerformanceStudents" =>
				BuildBelow70Grade(ctx, lastJson.YearlyStudentBelow70Grade, previousJson?.YearlyStudentBelow70Grade),

			"FailedStudents" =>
			[
				..BuildSuccessByGrade(ctx, lastJson.SuccessRateByGrade, previousJson?.SuccessRateByGrade),
		..BuildSuccessByTrack(ctx, lastJson.SuccessRateByTrack, previousJson?.SuccessRateByTrack)
			],

			"StudentLevelsAnalysis" =>
				BuildAchievement(ctx, lastJson.Achievement, previousJson?.Achievement),

			"AchievementTests" =>
				BuildAchievement(ctx, lastJson.Achievement, previousJson?.Achievement),

			"GeneralSecondaryAchievement" =>
				BuildAchievementTrackNoSubject(ctx, lastJson.AchievementTrackNoSubjectG11G12, previousJson?.AchievementTrackNoSubjectG11G12),

			"GeneralSecondarySuccess" =>
				BuildSuccessByTrack(ctx, lastJson.SuccessRateByTrack, previousJson?.SuccessRateByTrack),

			_ => []
		};
	}

	private record BuilderContext(
		Guid AnalysisTypeId,
		Guid EvaluationRequestId,
		int LastYear,
		int PreviousYear,
		List<int> AllowedGrades,
		List<string> AllowedSubjects,
		List<string> AllowedTracks,
	    List<string> AllowedTerms,
		Dictionary<string, int> LastAttendance,
		Dictionary<string, int> PreviousAttendance);

	private OutputAnalysisData MakeDetail(BuilderContext ctx, int grade, string? subjectCode,
	string? track, string? termCode, decimal lastValue, decimal previousValue, string? note,
	int lastStudentCount = 0, int previousStudentCount = 0) => new()
	{
			Id = Guid.NewGuid(),
			AnalysisTypeId = ctx.AnalysisTypeId,
			EvaluationRequestId = ctx.EvaluationRequestId,
			Grade = grade,
			SubjectCode = subjectCode,
			Track = track,
			TermCode = termCode,
			LastYear = ctx.LastYear,
			PreviousYear = ctx.PreviousYear,
			LastYearValue = lastValue,
			PreviousYearValue = previousValue,
			Difference = lastValue - previousValue,
			ActualValue = lastValue,
		LastYearStudentCount = lastStudentCount > 0 ? lastStudentCount : GetStudentCount(ctx.LastAttendance, grade, termCode),
		PreviousYearStudentCount = previousStudentCount > 0 ? previousStudentCount : GetStudentCount(ctx.PreviousAttendance, grade, termCode),
		Note = note
		};

	private List<OutputAnalysisData> BuildAchievement(
		BuilderContext ctx,
		List<AchievementDto>? lastRows,
		List<AchievementDto>? previousRows)
	{
		var lookup = BuildLookup(
			previousRows?
				.Where(x =>
					ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
					IsAllowedSubject(x.CourseCode, ctx.AllowedSubjects) &&
					MatchFilter(x.Timespan, ctx.AllowedTerms)),
			x => $"{ToInt(x.Grade)}|{x.CourseCode?.Trim()}|{x.Timespan?.Trim()}");

		return lastRows?
			.Where(x =>
				ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
				IsAllowedSubject(x.CourseCode, ctx.AllowedSubjects) &&
				MatchFilter(x.Timespan, ctx.AllowedTerms))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var subjectCode = x.CourseCode?.Trim();
				var termCode = x.Timespan?.Trim();

				lookup.TryGetValue(
					$"{grade}|{subjectCode}|{termCode}",
					out var prev);

				return MakeDetail(
					ctx,
					grade,
					subjectCode,
					null,
					termCode,
					ToDecimal(x.Grade_Achvment_NoSev3_And_Present_NoActivity),
					ToDecimal(prev?.Grade_Achvment_NoSev3_And_Present_NoActivity),
					x.CourseTitle,
					ToInt(x.No_of_Student_Grade_Without_Activity), 
					ToInt(prev?.No_of_Student_Grade_Without_Activity));
			})
			.ToList() ?? [];
	}
	private List<OutputAnalysisData> BuildAchievementTrackSubjectG11G12(
	BuilderContext ctx,
	List<AchievementTrackSubjectG11G12Dto>? lastRows,
	List<AchievementTrackSubjectG11G12Dto>? previousRows)
	{
		var lookup = BuildLookup(
			previousRows?
				.Where(x =>
					ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
					IsAllowedSubject(x.CourseCode, ctx.AllowedSubjects) &&
					MatchFilter(x.Track, ctx.AllowedTracks) &&
					MatchFilter(x.Timespan, ctx.AllowedTerms)),
			x => $"{ToInt(x.Grade)}|{x.Track?.Trim()}|{x.CourseCode?.Trim()}|{x.Timespan?.Trim()}");

		return lastRows?
			.Where(x =>
				ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
				IsAllowedSubject(x.CourseCode, ctx.AllowedSubjects) &&
				MatchFilter(x.Track, ctx.AllowedTracks) &&
				MatchFilter(x.Timespan, ctx.AllowedTerms))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var track = x.Track?.Trim();
				var subjectCode = x.CourseCode?.Trim();
				var termCode = x.Timespan?.Trim();

				lookup.TryGetValue(
					$"{grade}|{track}|{subjectCode}|{termCode}",
					out var prev);

				return MakeDetail(
							ctx, grade, subjectCode, track, termCode,
							ToDecimal(x.Grade_Achvment_NoSev3_And_Present),
							ToDecimal(prev?.Grade_Achvment_NoSev3_And_Present),
							x.CourseTitle,
							ToInt(x.Grade_NumberOf_Students_NoSev3_And_Present),
							ToInt(prev?.Grade_NumberOf_Students_NoSev3_And_Present));
									})
			.ToList() ?? [];
	}
	private List<OutputAnalysisData> BuildAchievementTrackNoSubject(
	BuilderContext ctx,
	List<AchievementTrackNoSubjectG11G12Dto>? lastRows,
	List<AchievementTrackNoSubjectG11G12Dto>? previousRows)
	{
		var lookup = BuildLookup(
			previousRows?
				.Where(x =>
					ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
					MatchFilter(x.Track, ctx.AllowedTracks) &&
					MatchFilter(x.Timespan, ctx.AllowedTerms)),
			x => $"{ToInt(x.Grade)}|{x.Track?.Trim()}|{x.Timespan?.Trim()}");

		return lastRows?
			.Where(x =>
				ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
				MatchFilter(x.Track, ctx.AllowedTracks) &&
				MatchFilter(x.Timespan, ctx.AllowedTerms))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var track = x.Track?.Trim();
				var termCode = x.Timespan?.Trim();

				lookup.TryGetValue($"{grade}|{track}|{termCode}", out var prev);

				return MakeDetail(ctx, grade, null, track, termCode,
					ToDecimal(x.Track_Achvment_NoSev3_And_Present),
					ToDecimal(prev?.Track_Achvment_NoSev3_And_Present),
					$"Students: {x.Grade_NumberOf_Students_NoSev3_And_Present}");
			})
			.ToList() ?? [];
	}

	private List<OutputAnalysisData> BuildAchievementSev3(
		BuilderContext ctx,
		List<AchievementSev3Dto>? lastRows,
		List<AchievementSev3Dto>? previousRows)
	{
		var lookup = BuildLookup(
			previousRows?
				.Where(x =>
					ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
					IsAllowedSubject(x.CourseCode, ctx.AllowedSubjects) &&
					MatchFilter(x.Timespan, ctx.AllowedTerms)),
			x => $"{ToInt(x.Grade)}|{x.CourseCode?.Trim()}|{x.Timespan?.Trim()}");

		return lastRows?
			.Where(x =>
				ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
				IsAllowedSubject(x.CourseCode, ctx.AllowedSubjects) &&
				MatchFilter(x.Timespan, ctx.AllowedTerms))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var subjectCode = x.CourseCode?.Trim();
				var termCode = x.Timespan?.Trim();

				lookup.TryGetValue(
					$"{grade}|{subjectCode}|{termCode}",
					out var prev);

				return MakeDetail(
						ctx,
						grade,
						subjectCode,
						null,
						termCode,
						ToDecimal(x.Grade_Achvment_Sev3_And_Present_NoActivity),
						ToDecimal(prev?.Grade_Achvment_Sev3_And_Present_NoActivity),
						x.CourseTitle,

						GetStudentCount(ctx.LastAttendance, grade),
						GetStudentCount(ctx.PreviousAttendance, grade)
					);
			})
			.ToList() ?? [];
	}

	private List<OutputAnalysisData> BuildBelow70Grade(
	BuilderContext ctx,
	List<YearlyStudentBelow70GradeDto>? lastRows,
	List<YearlyStudentBelow70GradeDto>? previousRows)
	{
		var lookup = BuildLookup(
			previousRows?
				.Where(x =>
					ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
					IsAllowedSubject(x.CourseCode, ctx.AllowedSubjects) &&
					MatchFilter(x.TermCode, ctx.AllowedTerms)),
			x => $"{ToInt(x.Grade)}|{x.CourseCode?.Trim()}|{x.TermCode?.Trim()}");

		return lastRows?
			.Where(x =>
				ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
				IsAllowedSubject(x.CourseCode, ctx.AllowedSubjects) &&
				MatchFilter(x.TermCode, ctx.AllowedTerms))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);

				lookup.TryGetValue(
					$"{grade}|{x.CourseCode?.Trim()}|{x.TermCode?.Trim()}",
					out var prev);

				return MakeDetail(
								ctx, grade, x.CourseCode?.Trim(), null, x.TermCode?.Trim(),
								ToDecimal(x.No_of_Student_Grading_Assignment_Below_70),
								ToDecimal(prev?.No_of_Student_Grading_Assignment_Below_70),
								$"Term: {x.TermCode?.Trim()}",
								ToInt(x.No_of_Student_Grading_Assignment),
								ToInt(prev?.No_of_Student_Grading_Assignment));
										})
			.ToList() ?? [];
	}

	private List<OutputAnalysisData> BuildSuccessByGrade(
	BuilderContext ctx,
	List<SuccessRateByGradeDto>? lastRows,
	List<SuccessRateByGradeDto>? previousRows)
	{
		var lookup = BuildLookup(
			previousRows?
				.Where(x =>
					ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
					MatchFilter(x.Timespan, ctx.AllowedTerms)),
			x => $"{ToInt(x.Grade)}|{x.Timespan?.Trim()}");

		return lastRows?
			.Where(x =>
				ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
				MatchFilter(x.Timespan, ctx.AllowedTerms))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var termCode = x.Timespan?.Trim();

				lookup.TryGetValue($"{grade}|{termCode}", out var prev);

				return MakeDetail(ctx, grade, null, null, termCode,
					ToDecimal(x.NotSucceededPercentOfStudent),
					ToDecimal(prev?.NotSucceededPercentOfStudent),
					$"Succeeded: {x.SucceededPercentOfStudent}");
			})
			.ToList() ?? [];
	}

	private List<OutputAnalysisData> BuildSuccessByTrack(
		BuilderContext ctx,
		List<SuccessRateByTrackDto>? lastRows,
		List<SuccessRateByTrackDto>? previousRows)
	{
		var lookup = BuildLookup(
			previousRows?
				.Where(x =>
					ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
					MatchFilter(x.Track, ctx.AllowedTracks) &&
					MatchFilter(x.Timespan, ctx.AllowedTerms)),
			x => $"{ToInt(x.Grade)}|{x.Track?.Trim()}|{x.Timespan?.Trim()}");

		return lastRows?
			.Where(x =>
				ctx.AllowedGrades.Contains(ToInt(x.Grade)) &&
				MatchFilter(x.Track, ctx.AllowedTracks) &&
				MatchFilter(x.Timespan, ctx.AllowedTerms))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var track = x.Track?.Trim();
				var termCode = x.Timespan?.Trim();

				lookup.TryGetValue($"{grade}|{track}|{termCode}", out var prev);

				return MakeDetail(ctx, grade, null, track, termCode,
					ToDecimal(x.NotSucceededPercentOfStudent),
					ToDecimal(prev?.NotSucceededPercentOfStudent),
					$"Succeeded: {x.SucceededPercentOfStudent}");
			})
			.ToList() ?? [];
	}

	// ─── Query / Get Output Analysis ─────────────────────────────────────────

	public async Task<OutputAnalysisResponseDto> GetOutputAnalysisAsync(Guid evaluationRequestId)
	{
		//evaluationRequestId = Guid.Parse("82DE8371-49D4-402F-96BE-3B53B8E92B00"); // TODO: remove hardcoded override

		await EnsureOutputAnalysisExistsAsync(evaluationRequestId);

		var analysisTypes = await uow.GetRepository<AnalysisType>()
			.GetAllActiveNonDeleted()
			.Include(x => x.ResultFormEvalMatrix).ThenInclude(x => x.FormEvalMatrixValues)
			.Include(x => x.DataFormEvalMatrix).ThenInclude(x => x.FormEvalMatrixValues)
			.OrderBy(x => x.OrderNo)
			.ToListAsync();

		var finals = await uow.GetRepository<OutputAnalysisFinalResult>()
			.GetAllActiveNonDeleted(x => x.EvaluationRequestId == evaluationRequestId)
			.ToListAsync();

		var details = await uow.GetRepository<OutputAnalysisData>()
			.GetAllActiveNonDeleted(x => x.EvaluationRequestId == evaluationRequestId)
			.ToListAsync();

		var subjectCodes = details
			.Where(x => !string.IsNullOrWhiteSpace(x.SubjectCode))
			.Select(x => x.SubjectCode!.Trim())
			.Distinct().ToList();

		var courses = await uow.GetRepository<SchoolCourse>()
			.GetAllActiveNonDeleted(x => x.IntegrationCode != null && subjectCodes.Contains(x.IntegrationCode))
			.ToListAsync();

		var gradeCodes = details.Select(x => x.Grade.ToString()).Distinct().ToList();

		var gradeLevels = await uow.GetRepository<GradeLevel>()
			.GetAllActiveNonDeleted(x => gradeCodes.Contains(x.Grade))
			.Include(x => x.EducationLevel)
			.ToListAsync();

		var analysisTypeIds = analysisTypes.Select(x => x.Id).ToList();

		var formItems = await uow.GetRepository<FormItem>()
			.GetAllActiveNonDeleted(x => x.AnalysisTypeId != null && analysisTypeIds.Contains(x.AnalysisTypeId.Value))
			.Include(x => x.Scope)
			.OrderBy(x => x.Scope!.OrderNo).ThenBy(x => x.OrderNo)
			.ToListAsync();

		var evaluationRequest = await uow.GetRepository<EvaluationRequest>()
								.GetAllActiveNonDeleted(x => x.Id == evaluationRequestId)
								.Include(x => x.OrgTree)
								.FirstOrDefaultAsync();

		return new OutputAnalysisResponseDto
		{
			SchoolNameAr = evaluationRequest?.OrgTree?.NameAr,
			SchoolNameEn = evaluationRequest?.OrgTree?.NameEn,
			AnalysisTypes = analysisTypes.Select(type =>
			{
				var final = finals.FirstOrDefault(x => x.AnalysisTypeId == type.Id);

				var resultMatrixValue = final == null ? null
					: type.ResultFormEvalMatrix?.FormEvalMatrixValues?
						.Where(v => final.ActualValue >= v.MinValue && final.ActualValue <= v.MaxValue)
						.OrderBy(v => v.OrderNo)
						.FirstOrDefault();

				return new OutputAnalysisTypeDto
				{
					AnalysisTypeId = type.Id,
					AnalysisTypeNameAr = type.NameAr,
					AnalysisTypeNameEn = type.NameEn,
					BackendName = type.BackendName,

					OutputAnalysisFinalResult = final == null ? null : new OutputAnalysisFinalResultDto
					{
						Id = final.Id,
						ActualValue = final.ActualValue,
						Note = final.Note,
						FormEvalMatrixValueId = final.FormEvalMatrixValueId,
						MatrixValue = MapMatrixValue(resultMatrixValue)
					},

					OutputAnalysisData = details
						.Where(x => x.AnalysisTypeId == type.Id)
						.OrderBy(x => x.Grade).ThenBy(x => x.SubjectCode)
						.Select(x =>
						{
							var course = !string.IsNullOrWhiteSpace(x.SubjectCode)
								? courses.FirstOrDefault(c => c.IntegrationCode == x.SubjectCode)
								: null;

							var grade = gradeLevels.FirstOrDefault(g => g.Grade == x.Grade.ToString());

							var dataMatrixValue = type.DataFormEvalMatrix?.FormEvalMatrixValues?
								.Where(v => x.ActualValue >= v.MinValue && x.ActualValue <= v.MaxValue)
								.OrderBy(v => v.OrderNo)
								.FirstOrDefault();

							return new OutputAnalysisDataDto
							{
								Id = x.Id,
								Grade = x.Grade,

								GradeNameAr = grade?.NameAr,
								GradeNameEn = grade?.NameEn,

								EducationLevelId = grade?.EducationLevelId,
								EducationLevelNameAr = grade?.EducationLevel?.NameAr,
								EducationLevelNameEn = grade?.EducationLevel?.NameEn,
								EducationLevelBackendName = grade?.EducationLevel?.BackendName,

								SubjectCode = x.SubjectCode,
								SubjectNameAr = course?.NameAr,
								SubjectNameEn = course?.NameEn,

								Track = x.Track,
								LastYear = x.LastYear,
								PreviousYear = x.PreviousYear,
								LastYearValue = x.LastYearValue,
								PreviousYearValue = x.PreviousYearValue,
								Difference = x.Difference,
								ActualValue = x.ActualValue,
								MartixTextValue = x.MartixTextValue,
								Note = x.Note,
								LastYearStudentCount = x.LastYearStudentCount,
								PreviousYearStudentCount = x.PreviousYearStudentCount,
								TermCode = x.TermCode,
								DataConfig = x.DataConfig,

								FormEvalMatrixValueId = dataMatrixValue?.Id,
								MatrixValue = MapMatrixValue(dataMatrixValue)
							};
						}).ToList(),

					FormItems = formItems
						.Where(i => i.AnalysisTypeId == type.Id)
						.OrderBy(i => i.Scope?.OrderNo).ThenBy(i => i.OrderNo)
						.Select(i => new OutputAnalysisFormItemDto
						{
							Id = i.Id,
							ItemNumber = i.ItemNumber,
							NameAr = i.NameAr,
							NameEn = i.NameEn,
							OrderNo = i.OrderNo,
							ScopeId = i.ScopeId,
							ScopeNameAr = i.Scope?.NameAr,
							ScopeNameEn = i.Scope?.NameEn
						}).ToList()
				};
			}).ToList()
		};
	}

	private async Task EnsureOutputAnalysisExistsAsync(Guid evaluationRequestId)
	{
		var analysisTypes = await uow.GetRepository<AnalysisType>()
			.GetAllActiveNonDeleted()
			.ToListAsync();

		var finals = await uow.GetRepository<OutputAnalysisFinalResult>()
			.GetAllActiveNonDeleted(x => x.EvaluationRequestId == evaluationRequestId)
			.ToListAsync();

		var details = await uow.GetRepository<OutputAnalysisData>()
			.GetAllActiveNonDeleted(x => x.EvaluationRequestId == evaluationRequestId)
			.ToListAsync();

		bool hasFinals = finals.Any();
		bool hasDetails = details.Any();

		bool countsAreMissing = hasDetails && details.All(x =>
			x.LastYearStudentCount == 0 && x.PreviousYearStudentCount == 0);

		if (hasFinals && hasDetails && !countsAreMissing)
			return;


		var evaluationRequest = await uow.GetRepository<EvaluationRequest>()
			.GetAllActiveNonDeleted(x => x.Id == evaluationRequestId)
			.FirstOrDefaultAsync();

		if (evaluationRequest == null)
			return;

		await GenerateOutputAnalysisFromQnedsAsync(evaluationRequestId, 2025);
	}

	// ─── Helpers ─────────────────────────────────────────────────────────────

	private static Dictionary<string, T> BuildLookup<T>(IEnumerable<T>? source, Func<T, string> keySelector) =>
		source?
			.GroupBy(keySelector)
			.ToDictionary(g => g.Key, g => g.First())
		?? [];
	private static Dictionary<string, int> BuildAttendanceStudentCountLookup(
		List<StudentDailyAttendanceByMonthDto>? rows,
		List<int> allowedGrades) =>
		rows?
			.Where(x =>
				allowedGrades.Contains(ToInt(x.Grade)) &&
				string.Equals(
					x.AttendanceDescription?.Trim(),
					"Present",
					StringComparison.OrdinalIgnoreCase))
			.GroupBy(x => ToInt(x.Grade).ToString())
			.ToDictionary(
				g => g.Key,
				g => g.Max(r => ToInt(r.Students)))
		?? [];

	private static int GetStudentCount(
		Dictionary<string, int> lookup,
		int grade,
		string? termCode = null)
	{
		return lookup.TryGetValue(grade.ToString(), out var count)
			? count
			: 0;
	}

	private static List<int> ParseGrades(string? grades) =>
		(grades ?? "")
			.Split(',', StringSplitOptions.RemoveEmptyEntries)
			.Select(x => int.TryParse(x.Trim(), out var g) ? g : 0)
			.Where(x => x > 0)
			.ToList();

	private static List<string> ParseSubjectCodes(string? subjectCodes)
	{
		if (string.IsNullOrWhiteSpace(subjectCodes) ||
			subjectCodes.Trim().Equals("NULL", StringComparison.OrdinalIgnoreCase))
			return [];

		return subjectCodes
			.Split(',', StringSplitOptions.RemoveEmptyEntries)
			.Select(x => x.Trim())
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.ToList();
	}

	private static bool IsAllowedSubject(string? subjectCode, List<string> allowed) =>
		!allowed.Any() || (!string.IsNullOrWhiteSpace(subjectCode) && allowed.Contains(subjectCode.Trim()));

	private static AnalysisConfigDto GetAnalysisConfig(string? json)
	{
		if (string.IsNullOrWhiteSpace(json)) return new();
		try { return JsonSerializer.Deserialize<AnalysisConfigDto>(json) ?? new(); }
		catch { return new(); }
	}
	private static List<string> ToFilterSet(IEnumerable<string>? values)
	{
		return values?
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.Select(x => x.Trim())
			.ToList()
			?? [];
	}

	private static bool MatchFilter(string? value, List<string> filter)
	{
		if (!filter.Any())
			return true;

		return !string.IsNullOrWhiteSpace(value)
			&& filter.Contains(value.Trim());
	}

	private static MatrixValueDto? MapMatrixValue(FormEvalMatrixValue? value) =>
		value == null ? null : new MatrixValueDto
		{
			Id = value.Id,
			NameAr = value.NameAr,
			NameEn = value.NameEn,
			ReportTextAr = value.ReportTextAr,
			ReportTextEn = value.ReportTextEn
		};

	private static int ToInt(object? value) =>
		int.TryParse(value?.ToString(), out var r) ? r : 0;

	private static decimal ToDecimal(object? value) =>
		decimal.TryParse(value?.ToString(), out var r) ? r : 0;
}