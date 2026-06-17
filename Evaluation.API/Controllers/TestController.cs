using Evaluation.DAL.DTOs;
using Evaluation.Services.Integration;
using Evaluation.Services.Models.SMTP;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.QNEDsDto;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[ApiController]
[Route("api/[controller]/{depRouting}/[action]")]
public class TestController : ControllerBase
{
    private readonly HRService _hrService;
    private readonly IEmailServices EmailServices;
    private readonly QNEDSService _qnedsService;
    public TestController(
       HRService hrService,
       IEmailServices EmailServices, QNEDSService qnedsService)
    {
        _hrService = hrService;
        this.EmailServices = EmailServices;
        _qnedsService = qnedsService;
    }
    [HttpGet]
    public async Task<List<HREmployeeInfoDto>> GetHREmployeesDetails(int page)
    {
        return await _hrService.GetAllHRUsersAsync(page);
    }

    [HttpGet]
    public async Task<List<HREmployeeInfoDto>> GetHREmployees(long? qID = null, string email = null, string orgno = null)
    {
        return await _hrService.GetHRUsersAsync(qID, email, orgno);
    }

    [HttpGet]
    public async Task<bool> AddUpdateOrgTree(string? hrCode = null, long? qID = null)
    {
        return await _hrService.AddUpdateOrgTree(hrCode, qID);
    }

    [HttpPost]
    public async Task<bool> AddUpdateAllSchools()
    {
        return await _hrService.AddUpdateAllSchools();
    }


    [HttpGet]
    public async Task<List<HROrganizationInfoDto>> GetHROrgDetailsAsync(int page)
    {
        return await _hrService.GetAllHROrgAsync(page);
    }

    [HttpGet]
    public async Task<List<HROrganizationInfoDto>> GetAllHRSchoolsAsync()
    {
        return await _hrService.GetAllHRSchoolsAsync();
    }

    [HttpGet]
    public async Task<bool> SendTestEmail(string email)
    {

        return await EmailServices.SendTestEmail(email);
    }
    [HttpGet]
    public async Task<List<AchievementDto>> GetAchievement(string institutionId, int year)
    {
        return await _qnedsService.GetAchievementByInstitutionAndYearAsync(institutionId, year);
    }

    [HttpGet]
    public async Task<List<AchievementSev3Dto>> GetAchievementSev3(string institutionId, int year)
    {
        return await _qnedsService.GetAchievementSev3ByInstitutionAndYearAsync(institutionId, year);
    }

    [HttpGet]
    public async Task<List<AchievementG12NoSubjectDto>> GetAchievementG12(string institutionId, int year)
    {
        return await _qnedsService.GetAchievementG12ByInstitutionAndYearAsync(institutionId, year);
    }

    [HttpGet]
    public async Task<List<AchievementTrackSubjectG11G12Dto>> GetAchievementTrackSubjectG11G12(string institutionId, int year)
    {
        return await _qnedsService.GetAchievementTrackSubjectG11G12ByInstitutionAndYearAsync(institutionId, year);
    }

    [HttpGet]
    public async Task<List<SuccessRateByGradeDto>> GetSuccessRateByGrade(string institutionId, int year)
    {
        return await _qnedsService.GetSuccessRateByGradeByInstitutionAndYearAsync(institutionId, year);
    }
    [HttpGet]
    public async Task<List<YearlyStudentBelow70TrackDto>> GetYearlyStudentBelow70Track(string institutionId, int year)
    {
        return await _qnedsService.GetYearlyStudentBelow70TrackByInstitutionAndYearAsync(institutionId, year);
    }
    [HttpGet]
    public async Task<List<YearlyStudentBelow70GradeDto>> GetYearlyStudentBelow70Grade(string institutionId, int year)
    {
        return await _qnedsService.GetYearlyStudentBelow70GradeByInstitutionAndYearAsync(institutionId, year);
    }
    [HttpGet]
    public async Task<List<SuccessRateByTrackDto>> GetSuccessRateByTrack(string institutionId, int year)
    {
        return await _qnedsService.GetSuccessRateByTrackByInstitutionAndYearAsync(institutionId, year);
    }
    [HttpGet]
    public async Task<List<AchievementTrackNoSubjectG11G12Dto>> GetAchievementTrackNoSubjectG11G12(string institutionId, int year)
    {
        return await _qnedsService.GetAchievementTrackNoSubjectG11G12ByInstitutionAndYearAsync(institutionId, year);
    }
    [HttpGet]
    public async Task<List<AchievementG1G11Dto>> GetAchievementG1G11(string institutionId, int year)
    {
        return await _qnedsService.GetAchievementG1G11ByInstitutionAndYearAsync(institutionId, year);
    }
    [HttpGet]
    public async Task<List<TeacherScheduleDto>> GetTeacherSchedule(string institutionId, int year)
    {
        return await _qnedsService.GetTeacherScheduleByInstitutionAndYearAsync(institutionId, year);
    }
    [HttpGet]
    public async Task<List<StudentDailyAttendanceByMonthDto>> GetStudentDailyAttendance(string institutionId, int year)
    {
        return await _qnedsService.GetStudentDailyAttendanceByMonthByInstitutionAndYearAsync(institutionId, year);
    }
	[HttpPost]
	public async Task<bool> SyncQnedsIntegration(int year)
	{
		return await _qnedsService.SyncQnedsIntegrationAsync(year);
	}

	[HttpPost]
	public async Task<bool> GenerateOutputAnalysisFromQneds(Guid evaluationRequestId, int academicYear)
	{
		return await _qnedsService.GenerateOutputAnalysisFromQnedsAsync(evaluationRequestId, academicYear);
	}
}