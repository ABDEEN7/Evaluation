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

	public async Task<bool> SyncQnedsIntegrationAsync(int academicYear)
	{
		var schools = await uow.GetRepository<OrgTree>()
			.GetAllActiveNonDeleted()
			.Where(x => x.NSISCode != null)
			.Select(x => new
			{
				x.Id,
				x.NSISCode
			})
			.ToListAsync();

		foreach (var school in schools)
		{
			var exists = await uow.GetRepository<QnedsIntegration>()
				.GetAllActiveNonDeleted()
				.AnyAsync(x =>
					x.OrgTreeId == school.Id &&
					x.AcademicYear == academicYear);

			if (exists)
				continue;

			var institutionId = school.NSISCode.ToString();

			var qnedsData = new QnedsIntegrationJsonDto
			{
				Achievement = await GetAchievementByInstitutionAndYearAsync(institutionId, academicYear),
				AchievementSev3 = await GetAchievementSev3ByInstitutionAndYearAsync(institutionId, academicYear),
				AchievementG12 = await GetAchievementG12ByInstitutionAndYearAsync(institutionId, academicYear),
				AchievementTrackSubjectG11G12 = await GetAchievementTrackSubjectG11G12ByInstitutionAndYearAsync(institutionId, academicYear),
				SuccessRateByGrade = await GetSuccessRateByGradeByInstitutionAndYearAsync(institutionId, academicYear),
				YearlyStudentBelow70Track = await GetYearlyStudentBelow70TrackByInstitutionAndYearAsync(institutionId, academicYear),
				YearlyStudentBelow70Grade = await GetYearlyStudentBelow70GradeByInstitutionAndYearAsync(institutionId, academicYear),
				SuccessRateByTrack = await GetSuccessRateByTrackByInstitutionAndYearAsync(institutionId, academicYear),
				AchievementTrackNoSubjectG11G12 = await GetAchievementTrackNoSubjectG11G12ByInstitutionAndYearAsync(institutionId, academicYear),
				AchievementG1G11 = await GetAchievementG1G11ByInstitutionAndYearAsync(institutionId, academicYear),
				TeacherSchedule = await GetTeacherScheduleByInstitutionAndYearAsync(institutionId, academicYear),
				StudentDailyAttendance = await GetStudentDailyAttendanceByMonthByInstitutionAndYearAsync(institutionId, academicYear)
			};

			var entity = new QnedsIntegration
			{
				Id = Guid.NewGuid(),
				AcademicYear = academicYear,
				OrgTreeId = school.Id,
				JsonValue = System.Text.Json.JsonSerializer.Serialize(qnedsData),
				QnedsConfig = null,
				IsActive = true,
				CreateDate = DateTime.Now,
				IsDeleted = false
			};

			await uow.GetRepository<QnedsIntegration>().InsertAsync(entity);
			await uow.CommitAsync();

		}

		await uow.CommitAsync();

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
			.Select(x => new
			{
				x.Id,
				x.NSISCode
			})
			.FirstOrDefaultAsync();

		if (school == null || string.IsNullOrEmpty(school.NSISCode))
			return;

		var institutionId = school.NSISCode;

		var qnedsData = new QnedsIntegrationJsonDto
		{
			Achievement = await GetAchievementByInstitutionAndYearAsync(institutionId, academicYear),
			AchievementSev3 = await GetAchievementSev3ByInstitutionAndYearAsync(institutionId, academicYear),
			AchievementG12 = await GetAchievementG12ByInstitutionAndYearAsync(institutionId, academicYear),
			AchievementTrackSubjectG11G12 = await GetAchievementTrackSubjectG11G12ByInstitutionAndYearAsync(institutionId, academicYear),
			SuccessRateByGrade = await GetSuccessRateByGradeByInstitutionAndYearAsync(institutionId, academicYear),
			YearlyStudentBelow70Track = await GetYearlyStudentBelow70TrackByInstitutionAndYearAsync(institutionId, academicYear),
			YearlyStudentBelow70Grade = await GetYearlyStudentBelow70GradeByInstitutionAndYearAsync(institutionId, academicYear),
			SuccessRateByTrack = await GetSuccessRateByTrackByInstitutionAndYearAsync(institutionId, academicYear),
			AchievementTrackNoSubjectG11G12 = await GetAchievementTrackNoSubjectG11G12ByInstitutionAndYearAsync(institutionId, academicYear),
			AchievementG1G11 = await GetAchievementG1G11ByInstitutionAndYearAsync(institutionId, academicYear),
			TeacherSchedule = await GetTeacherScheduleByInstitutionAndYearAsync(institutionId, academicYear),
			StudentDailyAttendance = await GetStudentDailyAttendanceByMonthByInstitutionAndYearAsync(institutionId, academicYear)
		};

		var hasAnyData =
			(qnedsData.Achievement?.Any() ?? false) ||
			(qnedsData.AchievementSev3?.Any() ?? false) ||
			(qnedsData.AchievementG12?.Any() ?? false) ||
			(qnedsData.AchievementTrackSubjectG11G12?.Any() ?? false) ||
			(qnedsData.SuccessRateByGrade?.Any() ?? false) ||
			(qnedsData.YearlyStudentBelow70Track?.Any() ?? false) ||
			(qnedsData.YearlyStudentBelow70Grade?.Any() ?? false) ||
			(qnedsData.SuccessRateByTrack?.Any() ?? false) ||
			(qnedsData.AchievementTrackNoSubjectG11G12?.Any() ?? false) ||
			(qnedsData.AchievementG1G11?.Any() ?? false) ||
			(qnedsData.TeacherSchedule?.Any() ?? false) ||
			(qnedsData.StudentDailyAttendance?.Any() ?? false);

		if (!hasAnyData)
			return;

		var jsonValue = JsonSerializer.Serialize(qnedsData);

		var existing = await uow.GetRepository<QnedsIntegration>()
			.GetAllActiveNonDeleted()
			.FirstOrDefaultAsync(x =>
				x.OrgTreeId == school.Id &&
				x.AcademicYear == academicYear);

		if (existing != null)
		{
			existing.JsonValue = jsonValue;
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
				JsonValue = jsonValue,
				QnedsConfig = null,
				IsActive = true,
				CreateDate = DateTime.Now,
				IsDeleted = false
			});
		}

		await uow.CommitAsync();
	}
	public async Task<bool> GenerateOutputAnalysisFromQnedsAsync(Guid evaluationRequestId, int academicYear)
	{
		var lastYear = academicYear;
		var previousYear = academicYear - 1;

		var evaluationRequest = await uow.GetRepository<EvaluationRequest>()
			.GetAllActiveNonDeleted(x => x.Id == evaluationRequestId)
			.Include(x => x.OrgTree)
			.FirstOrDefaultAsync();

		if (evaluationRequest == null || evaluationRequest.OrgTreeId == null)
			return false;

		var orgTreeId = evaluationRequest.OrgTreeId;

		var integrations = await uow.GetRepository<QnedsIntegration>()
			.GetAllActiveNonDeleted(x =>
				x.OrgTreeId == orgTreeId &&
				(x.AcademicYear == lastYear || x.AcademicYear == previousYear))
			.ToListAsync();

		var lastIntegration = integrations.FirstOrDefault(x => x.AcademicYear == lastYear);
		var previousIntegration = integrations.FirstOrDefault(x => x.AcademicYear == previousYear);

		if (lastIntegration == null)
			return false;

		var lastJson = JsonSerializer.Deserialize<QnedsIntegrationJsonDto>(lastIntegration.JsonValue);
		var previousJson = previousIntegration == null
			? null
			: JsonSerializer.Deserialize<QnedsIntegrationJsonDto>(previousIntegration.JsonValue);

		if (lastJson == null)
			return false;

		var analysisTypes = await uow.GetRepository<AnalysisType>()
			.GetAllActiveNonDeleted()
			.Where(x =>
				x.BackendName == "AcademicAchievement" ||
				x.BackendName == "StudentsWithDisabilities" ||
				x.BackendName == "LowPerformanceStudents" ||
				x.BackendName == "FailedStudents")
			.Include(x => x.DataFormEvalMatrix)
			.ThenInclude(x => x.FormEvalMatrixValues)
			.Include(x => x.ResultFormEvalMatrix)
			.ThenInclude(x => x.FormEvalMatrixValues)
			.OrderBy(x => x.OrderNo)
			.ToListAsync();

		foreach (var analysisType in analysisTypes)
		{
			var details = BuildOutputAnalysisDetails(
				analysisType,
				evaluationRequestId,
				lastYear,
				previousYear,
				lastJson,
				previousJson);

			if (!details.Any())
				continue;

			foreach (var detail in details)
			{
				detail.FormEvalMatrixValueId = GetMatrixValueId(
					analysisType.DataFormEvalMatrix?.FormEvalMatrixValues,
					detail.ActualValue);

				detail.Note = GetMatrixNameAr(
					analysisType.DataFormEvalMatrix?.FormEvalMatrixValues,
					detail.ActualValue);
			}

			var finalActualValue = details.Average(x => x.ActualValue);

			var finalResult = new OutputAnalysisFinalResult
			{
				Id = Guid.NewGuid(),
				AnalysisTypeId = analysisType.Id,
				EvaluationRequestId = evaluationRequestId,
				ActualValue = finalActualValue,
				FormEvalMatrixValueId = GetMatrixValueId(
					analysisType.ResultFormEvalMatrix?.FormEvalMatrixValues,
					finalActualValue),
				Note = GetMatrixNameAr(
					analysisType.ResultFormEvalMatrix?.FormEvalMatrixValues,
					finalActualValue),
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

	private List<OutputAnalysisData> BuildOutputAnalysisDetails(
		AnalysisType analysisType,
		Guid evaluationRequestId,
		int lastYear,
		int previousYear,
		QnedsIntegrationJsonDto lastJson,
		QnedsIntegrationJsonDto? previousJson)
	{
		var result = new List<OutputAnalysisData>();
		var allowedGrades = ParseGrades(analysisType.Grades);

		var lastAttendanceLookup = BuildAttendanceStudentCountLookup(
			lastJson.StudentDailyAttendance,
			allowedGrades);

		var previousAttendanceLookup = BuildAttendanceStudentCountLookup(
			previousJson?.StudentDailyAttendance,
			allowedGrades);

		switch (analysisType.BackendName)
		{
			case "AcademicAchievement":
				result.AddRange(BuildAchievement(
					analysisType.Id,
					evaluationRequestId,
					lastYear,
					previousYear,
					lastJson.Achievement,
					previousJson?.Achievement,
					allowedGrades,
					lastAttendanceLookup,
					previousAttendanceLookup));

				result.AddRange(BuildAchievementTrackNoSubject(
					analysisType.Id,
					evaluationRequestId,
					lastYear,
					previousYear,
					lastJson.AchievementTrackNoSubjectG11G12,
					previousJson?.AchievementTrackNoSubjectG11G12,
					allowedGrades,
					lastAttendanceLookup,
					previousAttendanceLookup));
				break;

			case "StudentsWithDisabilities":
				result.AddRange(BuildAchievementSev3(
					analysisType.Id,
					evaluationRequestId,
					lastYear,
					previousYear,
					lastJson.AchievementSev3,
					previousJson?.AchievementSev3,
					allowedGrades,
					lastAttendanceLookup,
					previousAttendanceLookup));
				break;

			case "LowPerformanceStudents":
				result.AddRange(BuildBelow70Grade(
					analysisType.Id,
					evaluationRequestId,
					lastYear,
					previousYear,
					lastJson.YearlyStudentBelow70Grade,
					previousJson?.YearlyStudentBelow70Grade,
					allowedGrades,
					lastAttendanceLookup,
					previousAttendanceLookup));
				break;

			case "FailedStudents":
				result.AddRange(BuildSuccessByGrade(
					analysisType.Id,
					evaluationRequestId,
					lastYear,
					previousYear,
					lastJson.SuccessRateByGrade,
					previousJson?.SuccessRateByGrade,
					allowedGrades,
					lastAttendanceLookup,
					previousAttendanceLookup));

				result.AddRange(BuildSuccessByTrack(
					analysisType.Id,
					evaluationRequestId,
					lastYear,
					previousYear,
					lastJson.SuccessRateByTrack,
					previousJson?.SuccessRateByTrack,
					allowedGrades,
					lastAttendanceLookup,
					previousAttendanceLookup));
				break;
		}

		return result;
	}

	private Dictionary<string, int> BuildAttendanceStudentCountLookup(
		List<StudentDailyAttendanceByMonthDto>? rows,
		HashSet<int> allowedGrades)
	{
		return rows?
			.Where(x =>
				allowedGrades.Contains(ToInt(x.Grade)) &&
				string.Equals(x.AttendanceDescription?.Trim(), "Present", StringComparison.OrdinalIgnoreCase))
			.GroupBy(x => $"{ToInt(x.Grade)}|{ToInt(x.Month)}")
			.ToDictionary(
				x => x.Key,
				x => x.Sum(r => ToInt(r.Students)))
			?? new Dictionary<string, int>();
	}

	private static int GetStudentCount(
		Dictionary<string, int> lookup,
		int grade,
		string? termCode)
	{
		var key = $"{grade}|{ToInt(termCode)}";
		return lookup.TryGetValue(key, out var count) ? count : 0;
	}

	private List<OutputAnalysisData> BuildAchievement(
		Guid analysisTypeId,
		Guid evaluationRequestId,
		int lastYear,
		int previousYear,
		List<AchievementDto>? lastRows,
		List<AchievementDto>? previousRows,
		HashSet<int> allowedGrades,
		Dictionary<string, int> lastAttendanceLookup,
		Dictionary<string, int> previousAttendanceLookup)
	{
		var previousLookup = previousRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.GroupBy(x => $"{ToInt(x.Grade)}|{x.CourseCode?.Trim()}|{x.Timespan?.Trim()}")
			.ToDictionary(x => x.Key, x => x.First())
			?? new Dictionary<string, AchievementDto>();

		return lastRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var subjectCode = x.CourseCode?.Trim();
				var termCode = x.Timespan?.Trim();

				var key = $"{grade}|{subjectCode}|{termCode}";
				previousLookup.TryGetValue(key, out var prev);

				var lastValue = ToDecimal(x.Grade_Achvment_NoSev3_And_Present_NoActivity);
				var previousValue = ToDecimal(prev?.Grade_Achvment_NoSev3_And_Present_NoActivity);

				return new OutputAnalysisData
				{
					Id = Guid.NewGuid(),
					AnalysisTypeId = analysisTypeId,
					EvaluationRequestId = evaluationRequestId,
					Grade = grade,
					SubjectCode = subjectCode,
					Track = null,
					TermCode = termCode,
					LastYear = lastYear,
					PreviousYear = previousYear,
					LastYearValue = lastValue,
					PreviousYearValue = previousValue,
					Difference = lastValue - previousValue,
					ActualValue = lastValue,
					LastYearStudentCount = GetStudentCount(lastAttendanceLookup, grade, termCode),
					PreviousYearStudentCount = GetStudentCount(previousAttendanceLookup, grade, termCode),
					Note = x.CourseTitle
				};
			})
			.ToList()
			?? new List<OutputAnalysisData>();
	}

	private List<OutputAnalysisData> BuildAchievementTrackNoSubject(
		Guid analysisTypeId,
		Guid evaluationRequestId,
		int lastYear,
		int previousYear,
		List<AchievementTrackNoSubjectG11G12Dto>? lastRows,
		List<AchievementTrackNoSubjectG11G12Dto>? previousRows,
		HashSet<int> allowedGrades,
		Dictionary<string, int> lastAttendanceLookup,
		Dictionary<string, int> previousAttendanceLookup)
	{
		var previousLookup = previousRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.GroupBy(x => $"{ToInt(x.Grade)}|{x.Track?.Trim()}|{x.Timespan?.Trim()}")
			.ToDictionary(x => x.Key, x => x.First())
			?? new Dictionary<string, AchievementTrackNoSubjectG11G12Dto>();

		return lastRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var track = x.Track?.Trim();
				var termCode = x.Timespan?.Trim();

				var key = $"{grade}|{track}|{termCode}";
				previousLookup.TryGetValue(key, out var prev);

				var lastValue = ToDecimal(x.Track_Achvment_NoSev3_And_Present);
				var previousValue = ToDecimal(prev?.Track_Achvment_NoSev3_And_Present);

				return new OutputAnalysisData
				{
					Id = Guid.NewGuid(),
					AnalysisTypeId = analysisTypeId,
					EvaluationRequestId = evaluationRequestId,
					Grade = grade,
					SubjectCode = null,
					Track = track,
					TermCode = termCode,
					LastYear = lastYear,
					PreviousYear = previousYear,
					LastYearValue = lastValue,
					PreviousYearValue = previousValue,
					Difference = lastValue - previousValue,
					ActualValue = lastValue,
					LastYearStudentCount = GetStudentCount(lastAttendanceLookup, grade, termCode),
					PreviousYearStudentCount = GetStudentCount(previousAttendanceLookup, grade, termCode),
					Note = $"Students: {x.Grade_NumberOf_Students_NoSev3_And_Present}"
				};
			})
			.ToList()
			?? new List<OutputAnalysisData>();
	}

	private List<OutputAnalysisData> BuildAchievementSev3(
		Guid analysisTypeId,
		Guid evaluationRequestId,
		int lastYear,
		int previousYear,
		List<AchievementSev3Dto>? lastRows,
		List<AchievementSev3Dto>? previousRows,
		HashSet<int> allowedGrades,
		Dictionary<string, int> lastAttendanceLookup,
		Dictionary<string, int> previousAttendanceLookup)
	{
		var previousLookup = previousRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.GroupBy(x => $"{ToInt(x.Grade)}|{x.CourseCode?.Trim()}|{x.Timespan?.Trim()}")
			.ToDictionary(x => x.Key, x => x.First())
			?? new Dictionary<string, AchievementSev3Dto>();

		return lastRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var subjectCode = x.CourseCode?.Trim();
				var termCode = x.Timespan?.Trim();

				var key = $"{grade}|{subjectCode}|{termCode}";
				previousLookup.TryGetValue(key, out var prev);

				var lastValue = ToDecimal(x.Grade_Achvment_Sev3_And_Present_NoActivity);
				var previousValue = ToDecimal(prev?.Grade_Achvment_Sev3_And_Present_NoActivity);

				return new OutputAnalysisData
				{
					Id = Guid.NewGuid(),
					AnalysisTypeId = analysisTypeId,
					EvaluationRequestId = evaluationRequestId,
					Grade = grade,
					SubjectCode = subjectCode,
					Track = null,
					TermCode = termCode,
					LastYear = lastYear,
					PreviousYear = previousYear,
					LastYearValue = lastValue,
					PreviousYearValue = previousValue,
					Difference = lastValue - previousValue,
					ActualValue = lastValue,
					LastYearStudentCount = GetStudentCount(lastAttendanceLookup, grade, termCode),
					PreviousYearStudentCount = GetStudentCount(previousAttendanceLookup, grade, termCode),
					Note = x.CourseTitle
				};
			})
			.ToList()
			?? new List<OutputAnalysisData>();
	}

	private List<OutputAnalysisData> BuildBelow70Grade(
		Guid analysisTypeId,
		Guid evaluationRequestId,
		int lastYear,
		int previousYear,
		List<YearlyStudentBelow70GradeDto>? lastRows,
		List<YearlyStudentBelow70GradeDto>? previousRows,
		HashSet<int> allowedGrades,
		Dictionary<string, int> lastAttendanceLookup,
		Dictionary<string, int> previousAttendanceLookup)
	{
		var previousLookup = previousRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.GroupBy(x => $"{ToInt(x.Grade)}|{x.CourseCode?.Trim()}|{x.TermCode?.Trim()}")
			.ToDictionary(x => x.Key, x => x.First())
			?? new Dictionary<string, YearlyStudentBelow70GradeDto>();

		return lastRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var subjectCode = x.CourseCode?.Trim();
				var termCode = x.TermCode?.Trim();

				var key = $"{grade}|{subjectCode}|{termCode}";
				previousLookup.TryGetValue(key, out var prev);

				var lastValue = ToDecimal(x.No_of_Student_Grading_Assignment_Below_70);
				var previousValue = ToDecimal(prev?.No_of_Student_Grading_Assignment_Below_70);

				return new OutputAnalysisData
				{
					Id = Guid.NewGuid(),
					AnalysisTypeId = analysisTypeId,
					EvaluationRequestId = evaluationRequestId,
					Grade = grade,
					SubjectCode = subjectCode,
					Track = null,
					TermCode = termCode,
					LastYear = lastYear,
					PreviousYear = previousYear,
					LastYearValue = lastValue,
					PreviousYearValue = previousValue,
					Difference = lastValue - previousValue,
					ActualValue = lastValue,
					LastYearStudentCount = GetStudentCount(lastAttendanceLookup, grade, termCode),
					PreviousYearStudentCount = GetStudentCount(previousAttendanceLookup, grade, termCode),
					Note = $"Term: {termCode}"
				};
			})
			.ToList()
			?? new List<OutputAnalysisData>();
	}

	private List<OutputAnalysisData> BuildSuccessByGrade(
		Guid analysisTypeId,
		Guid evaluationRequestId,
		int lastYear,
		int previousYear,
		List<SuccessRateByGradeDto>? lastRows,
		List<SuccessRateByGradeDto>? previousRows,
		HashSet<int> allowedGrades,
		Dictionary<string, int> lastAttendanceLookup,
		Dictionary<string, int> previousAttendanceLookup)
	{
		var previousLookup = previousRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.GroupBy(x => $"{ToInt(x.Grade)}|{x.Timespan?.Trim()}")
			.ToDictionary(x => x.Key, x => x.First())
			?? new Dictionary<string, SuccessRateByGradeDto>();

		return lastRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var termCode = x.Timespan?.Trim();

				var key = $"{grade}|{termCode}";
				previousLookup.TryGetValue(key, out var prev);

				var lastValue = ToDecimal(x.NotSucceededPercentOfStudent);
				var previousValue = ToDecimal(prev?.NotSucceededPercentOfStudent);

				return new OutputAnalysisData
				{
					Id = Guid.NewGuid(),
					AnalysisTypeId = analysisTypeId,
					EvaluationRequestId = evaluationRequestId,
					Grade = grade,
					SubjectCode = null,
					Track = null,
					TermCode = termCode,
					LastYear = lastYear,
					PreviousYear = previousYear,
					LastYearValue = lastValue,
					PreviousYearValue = previousValue,
					Difference = lastValue - previousValue,
					ActualValue = lastValue,
					LastYearStudentCount = GetStudentCount(lastAttendanceLookup, grade, termCode),
					PreviousYearStudentCount = GetStudentCount(previousAttendanceLookup, grade, termCode),
					Note = $"Succeeded: {x.SucceededPercentOfStudent}"
				};
			})
			.ToList()
			?? new List<OutputAnalysisData>();
	}

	private List<OutputAnalysisData> BuildSuccessByTrack(
		Guid analysisTypeId,
		Guid evaluationRequestId,
		int lastYear,
		int previousYear,
		List<SuccessRateByTrackDto>? lastRows,
		List<SuccessRateByTrackDto>? previousRows,
		HashSet<int> allowedGrades,
		Dictionary<string, int> lastAttendanceLookup,
		Dictionary<string, int> previousAttendanceLookup)
	{
		var previousLookup = previousRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.GroupBy(x => $"{ToInt(x.Grade)}|{x.Track?.Trim()}|{x.Timespan?.Trim()}")
			.ToDictionary(x => x.Key, x => x.First())
			?? new Dictionary<string, SuccessRateByTrackDto>();

		return lastRows?
			.Where(x => allowedGrades.Contains(ToInt(x.Grade)))
			.Select(x =>
			{
				var grade = ToInt(x.Grade);
				var track = x.Track?.Trim();
				var termCode = x.Timespan?.Trim();

				var key = $"{grade}|{track}|{termCode}";
				previousLookup.TryGetValue(key, out var prev);

				var lastValue = ToDecimal(x.NotSucceededPercentOfStudent);
				var previousValue = ToDecimal(prev?.NotSucceededPercentOfStudent);

				return new OutputAnalysisData
				{
					Id = Guid.NewGuid(),
					AnalysisTypeId = analysisTypeId,
					EvaluationRequestId = evaluationRequestId,
					Grade = grade,
					SubjectCode = null,
					Track = track,
					TermCode = termCode,
					LastYear = lastYear,
					PreviousYear = previousYear,
					LastYearValue = lastValue,
					PreviousYearValue = previousValue,
					Difference = lastValue - previousValue,
					ActualValue = lastValue,
					LastYearStudentCount = GetStudentCount(lastAttendanceLookup, grade, termCode),
					PreviousYearStudentCount = GetStudentCount(previousAttendanceLookup, grade, termCode),
					Note = $"Succeeded: {x.SucceededPercentOfStudent}"
				};
			})
			.ToList()
			?? new List<OutputAnalysisData>();
	}

	private static HashSet<int> ParseGrades(string? grades)
	{
		return (grades ?? "")
			.Split(',', StringSplitOptions.RemoveEmptyEntries)
			.Select(x => int.TryParse(x.Trim(), out var grade) ? grade : 0)
			.Where(x => x > 0)
			.ToHashSet();
	}

	private static Guid? GetMatrixValueId(IEnumerable<FormEvalMatrixValue>? values, decimal actualValue)
	{
		return values?
			.Where(x =>
				x.IsActive &&
				!x.IsDeleted &&
				actualValue >= x.MinValue &&
				actualValue <= x.MaxValue)
			.OrderBy(x => x.OrderNo)
			.Select(x => (Guid?)x.Id)
			.FirstOrDefault();
	}

	private static string? GetMatrixNameAr(IEnumerable<FormEvalMatrixValue>? values, decimal actualValue)
	{
		return values?
			.Where(x =>
				x.IsActive &&
				!x.IsDeleted &&
				actualValue >= x.MinValue &&
				actualValue <= x.MaxValue)
			.OrderBy(x => x.OrderNo)
			.Select(x => x.NameAr)
			.FirstOrDefault();
	}

	private static int ToInt(object? value)
	{
		return int.TryParse(value?.ToString(), out var result)
			? result
			: 0;
	}

	private static decimal ToDecimal(object? value)
	{
		return decimal.TryParse(value?.ToString(), out var result)
			? result
			: 0;
	}
	public async Task<OutputAnalysisResponseDto> GetOutputAnalysisAsync(Guid evaluationRequestId)
	{
		evaluationRequestId = Guid.Parse("82DE8371-49D4-402F-96BE-3B53B8E92B00");

		await EnsureOutputAnalysisExistsAsync(evaluationRequestId);

		var analysisTypes = await uow.GetRepository<AnalysisType>()
			.GetAllActiveNonDeleted()
			.OrderBy(x => x.OrderNo)
			.ToListAsync();

		var finals = await uow.GetRepository<OutputAnalysisFinalResult>()
			.GetAllActiveNonDeleted(x => x.EvaluationRequestId == evaluationRequestId)
			.ToListAsync();

		var details = await uow.GetRepository<OutputAnalysisData>()
			.GetAllActiveNonDeleted(x => x.EvaluationRequestId == evaluationRequestId)
			.ToListAsync();

		return new OutputAnalysisResponseDto
		{
			AnalysisTypes = analysisTypes.Select(type =>
			{
				var final = finals.FirstOrDefault(x => x.AnalysisTypeId == type.Id);

				return new OutputAnalysisTypeDto
				{
					AnalysisTypeId = type.Id,
					AnalysisTypeNameAr = type.NameAr,
					AnalysisTypeNameEn = type.NameEn,
					BackendName = type.BackendName,

					OutputAnalysisFinalResult = final == null
						? null
						: new OutputAnalysisFinalResultDto
						{
							Id = final.Id,
							ActualValue = final.ActualValue,
							Note = final.Note,
							FormEvalMatrixValueId = final.FormEvalMatrixValueId
						},

					OutputAnalysisData = details
						.Where(x => x.AnalysisTypeId == type.Id)
						.OrderBy(x => x.Grade)
						.ThenBy(x => x.SubjectCode)
						.Select(x => new OutputAnalysisDataDto
						{
							Id = x.Id,
							Grade = x.Grade,
							LastYear = x.LastYear,
							PreviousYear = x.PreviousYear,
							LastYearValue = x.LastYearValue,
							PreviousYearValue = x.PreviousYearValue,
							Difference = x.Difference,
							SubjectCode = x.SubjectCode,
							Track = x.Track,
							ActualValue = x.ActualValue,
							MartixTextValue = x.MartixTextValue,
							Note = x.Note,
							LastYearStudentCount = x.LastYearStudentCount,
							PreviousYearStudentCount = x.PreviousYearStudentCount,
							TermCode = x.TermCode,
							DataConfig = x.DataConfig
						})
						.ToList()
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
			.GetAllActiveNonDeleted(x =>
				x.EvaluationRequestId == evaluationRequestId)
			.ToListAsync();

		var details = await uow.GetRepository<OutputAnalysisData>()
			.GetAllActiveNonDeleted(x =>
				x.EvaluationRequestId == evaluationRequestId)
			.ToListAsync();

		bool needGenerate = false;

		if (!finals.Any())
		{
			needGenerate = true;
		}
		else if (!details.Any())
		{
			needGenerate = true;
		}
		else
		{
			foreach (var analysisType in analysisTypes)
			{
				var finalExists = finals.Any(x =>
					x.AnalysisTypeId == analysisType.Id);

				if (!finalExists)
				{
					needGenerate = true;
					break;
				}

				var detailExists = details.Any(x =>
					x.AnalysisTypeId == analysisType.Id);

				if (!detailExists)
				{
					needGenerate = true;
					break;
				}
			}
		}

		if (!needGenerate)
			return;

		var evaluationRequest = await uow.GetRepository<EvaluationRequest>()
			.GetAllActiveNonDeleted(x => x.Id == evaluationRequestId)
			//.Include(x => x.OrgAcademicYear)
			.FirstOrDefaultAsync();

		if (evaluationRequest == null)
			return;

		var academicYear =
			//evaluationRequest.OrgAcademicYear?.AcademicYear
			//?? 
			DateTime.Now.Year;

		await GenerateOutputAnalysisFromQnedsAsync(
			evaluationRequestId,
			academicYear);
	}
}


