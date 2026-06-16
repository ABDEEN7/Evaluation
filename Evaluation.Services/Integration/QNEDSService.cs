using AutoMapper;
using Dapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.QNEDsDto;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

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
}