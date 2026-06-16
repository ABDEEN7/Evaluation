using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Dtos.QNEDsDto
{
	public class QnedsIntegrationJsonDto
	{
		public List<AchievementDto> Achievement { get; set; } = new();
		public List<AchievementSev3Dto> AchievementSev3 { get; set; } = new();
		public List<AchievementG12NoSubjectDto> AchievementG12 { get; set; } = new();
		public List<AchievementTrackSubjectG11G12Dto> AchievementTrackSubjectG11G12 { get; set; } = new();
		public List<SuccessRateByGradeDto> SuccessRateByGrade { get; set; } = new();
		public List<YearlyStudentBelow70TrackDto> YearlyStudentBelow70Track { get; set; } = new();
		public List<YearlyStudentBelow70GradeDto> YearlyStudentBelow70Grade { get; set; } = new();
		public List<SuccessRateByTrackDto> SuccessRateByTrack { get; set; } = new();
		public List<AchievementTrackNoSubjectG11G12Dto> AchievementTrackNoSubjectG11G12 { get; set; } = new();
		public List<AchievementG1G11Dto> AchievementG1G11 { get; set; } = new();
		public List<TeacherScheduleDto> TeacherSchedule { get; set; } = new();
		public List<StudentDailyAttendanceByMonthDto> StudentDailyAttendance { get; set; } = new();
	}
}
