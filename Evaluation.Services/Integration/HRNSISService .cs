using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Extensions;
using Evaluation.Services.Models.NSISSchool;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Integration
{
	public class HRNSISService : ApiBase
	{
		private readonly IntegrationLogger _integrationLogger;

		public HRNSISService(
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


		public async Task<bool> SyncAllSchoolsAsync(string schoolCategory,string schoolOrgTypeBackendName)
		{
			var syncedSchools = new List<School>();

			var nsisSchools = await _integrationLogger.ExecuteAsync(
				async () => await GetAllNSISSchoolsAsync(schoolCategory));

			if (nsisSchools == null || nsisSchools.Count == 0)
				return false;

			var allHomeroomsTask =  GetAllHomeroomsAsync();
			var allCoursesTask =  GetAllCoursesAsync();
			var allAcadPlansTask =  GetAllAcadPlansAsync();

			using var uow = serviceScopeFactory.CreateScopedUow();

			var schoolRepo = uow.GetRepository<School>();

			var existingSchools = await schoolRepo.GetAllNonDeleted()
									.Where(s => s.NSISCode != null)
									.AsNoTracking()
									.ToDictionaryAsync(s => s.NSISCode!, s => s);

			var schoolTypeMap = await GetOrCreateLookupMapAsync<SchoolType>(
								uow,
								nsisSchools.Select(s => new LookupDto
								{
									Code = s.SchoolCategoryCode,
									NameAr = s.SchoolCategoryAr,
									NameEn = s.SchoolCategoryEn
								}),
								x => x.IntegrationCode,
								item => new SchoolType
								{
									IntegrationCode = item.Code,
									NameAr = item.NameAr,
									NameEn = item.NameEn,
									BackendName = item.Code
								});

			var genderMap = await GetOrCreateLookupMapAsync<SchoolGender>(
									uow,
									nsisSchools.Select(s => new LookupDto
									{
										Code = s.GenderCode,
										NameAr = s.GenderName,
										NameEn = s.GenderName
									}),
									x => x.IntegrationCode,
									item => new SchoolGender
									{
										IntegrationCode = item.Code,
										NameAr = item.NameAr,
										NameEn = item.NameEn,
										BackendName = item.Code
									});

			var empGenderMap = await GetOrCreateLookupMapAsync<SchoolGender>(
								uow,
								nsisSchools.Select(s => new LookupDto
								{
									Code = s.EmpGenderCode,
									NameAr = s.EmpGenderName,
									NameEn = s.EmpGenderName
								}),
								x => x.IntegrationCode,
								item => new SchoolGender
								{
									IntegrationCode = item.Code,
									NameAr = item.NameAr,
									NameEn = item.NameEn,
									BackendName = item.Code
								});

			var modelMap = await GetOrCreateLookupMapAsync<SchoolModel>(
								uow,
								nsisSchools.Select(s => new LookupDto
								{
									Code = s.ModelCode,
									NameAr = s.ModelName,
									NameEn = s.ModelName
								}),
								x => x.IntegrationCode,
								item => new SchoolModel
								{
									IntegrationCode = item.Code,
									NameAr = item.NameAr,
									NameEn = item.NameEn,
									BackendName = item.Code
								});

			var programMap = await GetOrCreateLookupMapAsync<SchoolProgram>(
							uow,
							nsisSchools.Select(s => new LookupDto
							{
								Code = s.ProgramCode,
								NameAr = s.ProgramName,
								NameEn = s.ProgramName
							}),
							x => x.IntegrationCode,
							item => new SchoolProgram
							{
								IntegrationCode = item.Code,
								NameAr = item.NameAr,
								NameEn = item.NameEn,
								BackendName = item.Code
							});

			await uow.CommitAsync();

			var today = DateOnly.FromDateTime(DateTime.Now);


			var orgTypeRepo = uow.GetRepository<OrgType>();
			var orgClassRepo = uow.GetRepository<OrgClass>();
			var orgTreeRepo = uow.GetRepository<OrgTree>();

			var schoolOrgType = await orgTypeRepo.GetAllNonDeleted()
				.FirstOrDefaultAsync(x => x.BackendName == "2");

			var schoolOrgClass = await orgClassRepo.GetAllNonDeleted()
				.FirstOrDefaultAsync(x => x.BackendName == schoolOrgTypeBackendName);

			if (schoolOrgType == null)
				throw new BusinessException("School OrgType not found. BackendName = 11");

			if (schoolOrgClass == null)
				throw new BusinessException("School OrgClass not found. BackendName = 2");

			var parentOrgTree = await orgTreeRepo.GetAllNonDeleted()
				.FirstOrDefaultAsync(x =>
					x.OrgParentId == null &&
					x.OrgClassId == schoolOrgClass.Id);

			if (parentOrgTree == null)
				throw new BusinessException("Parent OrgTree not found for school OrgType.");

			foreach (var dto in nsisSchools)
			{
				var NSISCode = dto.Institution?.Trim();

				if (string.IsNullOrWhiteSpace(NSISCode))
					continue;

				if (existingSchools.TryGetValue(NSISCode, out var existingSchool))
				{
					MapNSISToSchool(
						dto,
						existingSchool,
						today,
						schoolTypeMap,
						genderMap,
						empGenderMap,
						modelMap,
						programMap);

					schoolRepo.Update(existingSchool);
					syncedSchools.Add(existingSchool);
				}
				else
				{
					
					var newSchool = new School
					{
						NameAr = dto.NameAr ?? NSISCode,
						NameEn = dto.NameEn ?? NSISCode,

						NSISCode = NSISCode,
						HrCode = "2" + NSISCode,
						Code = NSISCode,

						OrgParentId = parentOrgTree.Id,
						OrgTypeId = schoolOrgType.Id,
						OrgClassId = schoolOrgClass.Id,

						EstablishmentDate = today
					};

					MapNSISToSchool(
						dto,
						newSchool,
						today,
						schoolTypeMap,
						genderMap,
						empGenderMap,
						modelMap,
						programMap);

					await schoolRepo.InsertAsync(newSchool);

					syncedSchools.Add(newSchool);

					existingSchools[NSISCode] = newSchool;
				}
			}

			await uow.CommitAsync();

			var allHomerooms = await allHomeroomsTask;

			var academicYear = allHomerooms
								.Where(x => !string.IsNullOrWhiteSpace(x.Strm))
								.Select(x => int.TryParse(x.Strm, out var y) ? y : (int?)null)
								.FirstOrDefault(x => x.HasValue);

			var homeroomsByInstitution = allHomerooms
										.Where(x => !string.IsNullOrWhiteSpace(x.Institution))
										.GroupBy(x => x.Institution!.Trim(), StringComparer.OrdinalIgnoreCase)
										.ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

			var allCourses = await allCoursesTask;

			var coursesByInstitution = allCourses
										.Where(x => !string.IsNullOrWhiteSpace(x.Institution))
										.GroupBy(x => x.Institution!.Trim(), StringComparer.OrdinalIgnoreCase)
										.ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);


			var allAcadPlans = await allAcadPlansTask;

			if (!academicYear.HasValue)
				throw new BusinessException("Academic year not found from NSIS_HOMEROOM.");
			if (academicYear.HasValue)
			{
				await SyncOrgAcademicYearsAsync(academicYear.Value, syncedSchools);
				await SyncSchoolLevelsAsync(nsisSchools,existingSchools,academicYear.Value,homeroomsByInstitution, allAcadPlans);

				await SyncSchoolGradesAndSectionsAsync(nsisSchools,existingSchools,academicYear.Value,homeroomsByInstitution, allAcadPlans);

				await SyncSchoolCoursesAsync(nsisSchools,coursesByInstitution);
			}
			return true;
		}
		public async Task<List<NSISSchoolDto>> GetAllNSISSchoolsAsync(string schoolCategory)
		{
			var result = new List<NSISSchoolDto>();

			using var con = new OracleConnection(ClsAppSetting.OracleDBConnection);

			try
			{
				await con.OpenAsync();

				using var cmd = con.CreateCommand();
				cmd.BindByName = true;

				cmd.CommandText = @"
									SELECT 
										INSTITUTION,
										LOCATION,
										SCHOOL_NAME_ARA,
										SCHOOL_NAME_ENG,
										CAMPUS,
										AREA,
										AREA_NAME_ARA,
										AREA_NAME_ENG,
										SUB_AREA,
										SUB_AREA_NAME_ARA,
										SUB_AREA_NAME_ENG,
										ADDRESS_LINE1,
										PHONE,
										FAX,
										EMAIL_ADDR,
										SCHOOL_CATEGORY,
										SCHOOL_CATEGORY_ARA,
										SCHOOL_CATEGORY_ENG,
										SCHL_GENDER_CD,
										SCHL_GENDER_DN,
										KG,
										PRIMARY,
										PREPARATORY,
										SECONDARY,
										SCHL_MODEL_CD,
										SCHL_MODEL_DN,
										SCHL_COHORT_CD,
										SCHL_COHORT_DN,
										SCHL_EMP_GEN_CD,
										SCHL_EMP_GEN_DN,
										SCHL_TYPE_CD,
										SCHL_TYPE_DN,
										LATITUDE,
										LONGITUDE,
										URL,
										SCHL_OUTS_CD,
										SCHL_OUTS_DN,
										SCHL_PROGRAM_CD,
										SCHL_PROGRAM_DN,
										SCHOOL_CAPACITY,
										SCHL_PIN_NUMBER
									FROM NSIS.NSIS_SCHOOL
									WHERE SCHOOL_CATEGORY = :SchoolCategory";

				cmd.Parameters.Add(new OracleParameter("SchoolCategory", schoolCategory));

				using var reader = await cmd.ExecuteReaderAsync();

				while (await reader.ReadAsync())
				{
					result.Add(reader.ToNSISSchoolDto());
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"NSIS_SCHOOL error: {ex.Message}");
			}

			return result;
		}
		private async Task SyncSchoolLevelsAsync(List<NSISSchoolDto> nsisSchools,Dictionary<string, School> schoolMap,int academicYear, Dictionary<string, List<NSISHomeroomDto>> homeroomsByInstitution, List<NSISAcadPlanDto> acadPlans)
		{
			using var uow = serviceScopeFactory.CreateScopedUow();

			var levelRepo = uow.GetRepository<SchoolLevel>();
			var eduLevelRepo = uow.GetRepository<EducationLevel>();

			//var acadPlans = await GetAllAcadPlansAsync();

			var eduLevels = await eduLevelRepo.GetAllNonDeleted()
				.Where(x => x.IntegrationCode != null)
				.ToDictionaryAsync(x => x.IntegrationCode!, x => x);

			var existingLevels = await levelRepo.GetAllNonDeleted()
				.ToListAsync();

			var existingKeys = existingLevels
				.Select(x => $"{x.SchoolId}_{x.EducationLevelId}_{x.AcademicYear}")
				.ToHashSet();

			//int currentYear = DateTime.Now.Year;

			foreach (var dto in nsisSchools)
			{
				var nsisCode = dto.Institution?.Trim();

				if (string.IsNullOrWhiteSpace(nsisCode))
					continue;

				if (!schoolMap.TryGetValue(nsisCode, out var school))
					continue;

				if (!homeroomsByInstitution.TryGetValue(nsisCode, out var homerooms))
					continue;
				//var homerooms = await GetHomeroomsBySchoolAsync(nsisCode);

				foreach (var homeroom in homerooms)
				{
					var plan = acadPlans.FirstOrDefault(x => x.AcadPlan == homeroom.AcadPlan);

					if (plan == null || string.IsNullOrWhiteSpace(plan.AcadProg))
						continue;

					if (!eduLevels.TryGetValue(plan.AcadProg, out var eduLevel))
					{
						eduLevel = new EducationLevel
						{
							IntegrationCode = plan.AcadProg,
							NameAr = plan.AcadProgAr ?? plan.AcadProg,
							NameEn = plan.AcadProgEn ?? plan.AcadProg,
							BackendName = plan.AcadProg,
							OrderNo = int.TryParse(plan.AcadProg, out var orderNo) ? orderNo : 0
						};

						await eduLevelRepo.InsertAsync(eduLevel);
						eduLevels[plan.AcadProg] = eduLevel;
					}

					var key = $"{school.Id}_{eduLevel.Id}_{academicYear}";

					if (existingKeys.Contains(key))
						continue;

					await levelRepo.InsertAsync(new SchoolLevel
					{
						SchoolId = school.Id,
						EducationLevelId = eduLevel.Id,
						AcademicYear = academicYear
					});

					existingKeys.Add(key);
				}
			}

			await uow.CommitAsync();
		}
		private async Task SyncOrgAcademicYearsAsync(int year,List<School> nsisSchools)
		{
			if (nsisSchools == null || nsisSchools.Count == 0)
				return;

			using var uow = serviceScopeFactory.CreateScopedUow();

			var orgAcademicYearRepo = uow.GetRepository<OrgAcademicYear>();

			var schoolIds = nsisSchools
				.Select(x => x.Id)
				.Distinct()
				.ToList();

			var existingSchoolIds = await orgAcademicYearRepo.GetAllNonDeleted()
				.Where(x => x.Year == year && schoolIds.Contains(x.OrgTreeId))
				.Select(x => x.OrgTreeId)
				.ToListAsync();

			var existingSet = existingSchoolIds.ToHashSet();

			foreach (var school in nsisSchools)
			{
				if (existingSet.Contains(school.Id))
					continue;

				await orgAcademicYearRepo.InsertAsync(new OrgAcademicYear
				{
					OrgTreeId = school.Id,
					ParentOrgTreeId = school.OrgParentId,

					JobTitleAr = "مدرسة",
					JobTitleEn = "School",

					Year = year,
					IsActive = true,

					SchoolGenderId = school.SchoolGenderId,
					SchoolModelId = school.SchoolModelId
				});

				existingSet.Add(school.Id);
			}

			await uow.CommitAsync();
		}
		private async Task SyncSchoolCoursesAsync(List<NSISSchoolDto> nsisSchools,Dictionary<string, List<NSISCourseDto>> coursesByInstitution)
		{
			using var uow = serviceScopeFactory.CreateScopedUow();

			var courseRepo = uow.GetRepository<SchoolCourse>();

			var existingCourses = await courseRepo.GetAllNonDeleted()
				.Where(x => x.IntegrationCode != null)
				.ToListAsync();

			var courseMap = existingCourses
				.Where(x => !string.IsNullOrWhiteSpace(x.IntegrationCode))
				.GroupBy(x => x.IntegrationCode!.Trim(), StringComparer.OrdinalIgnoreCase)
				.ToDictionary(
					g => g.Key,
					g => g.First(),
					StringComparer.OrdinalIgnoreCase);

			foreach (var schoolDto in nsisSchools)
			{
				var nsisCode = schoolDto.Institution?.Trim();

				if (string.IsNullOrWhiteSpace(nsisCode))
					continue;

				if (!coursesByInstitution.TryGetValue(nsisCode, out var nsisCourses))
					continue;
				foreach (var item in nsisCourses)
				{
					var subjectCode = item.SubjectCode?.Trim();

					if (string.IsNullOrWhiteSpace(subjectCode))
						continue;

					if (courseMap.ContainsKey(subjectCode))
						continue;

					var course = new SchoolCourse
					{
						IntegrationCode = subjectCode,
						NameAr = item.SubjectNameAr ?? subjectCode,
						NameEn = item.SubjectNameEn ?? subjectCode,
						OrderNo = 0
					};

					await courseRepo.InsertAsync(course);

					courseMap[subjectCode] = course;
				}
			}

			await uow.CommitAsync();
		}
		private async Task SyncSchoolGradesAndSectionsAsync(
		List<NSISSchoolDto> nsisSchools,
		Dictionary<string, School> schoolMap,
		int academicYear,
		Dictionary<string, List<NSISHomeroomDto>> homeroomsByInstitution, List<NSISAcadPlanDto> acadPlans)
		{
			using var uow = serviceScopeFactory.CreateScopedUow();

			var schoolLevelRepo = uow.GetRepository<SchoolLevel>();
			var gradeLevelRepo = uow.GetRepository<GradeLevel>();
			var schoolGradeRepo = uow.GetRepository<SchoolGrade>();
			var sectionRepo = uow.GetRepository<SchoolGradeSection>();

			var acadPlanMap = acadPlans
				.Where(x => !string.IsNullOrWhiteSpace(x.AcadPlan))
				.GroupBy(x => x.AcadPlan!.Trim(), StringComparer.OrdinalIgnoreCase)
				.ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

			var schoolLevels = await schoolLevelRepo.GetAllNonDeleted()
				.Include(x => x.EducationLevel)
				.Where(x => x.AcademicYear == academicYear)
				.ToListAsync();

			var gradeLevels = await gradeLevelRepo.GetAllNonDeleted()
				.Where(x => x.IntegrationCode != null)
				.ToListAsync();

			var gradeLevelMap = gradeLevels
				.GroupBy(x => x.IntegrationCode!.Trim(), StringComparer.OrdinalIgnoreCase)
				.ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

			var existingSchoolGrades = await schoolGradeRepo.GetAllNonDeleted()
				.ToListAsync();

			var existingSchoolGradeKeys = existingSchoolGrades
				.Select(x => $"{x.SchoolLevelId}_{x.GradeLevelId}")
				.ToHashSet();

			var existingSections = await sectionRepo.GetAllNonDeleted()
				.ToListAsync();

			var existingSectionKeys = existingSections
				.Where(x => x.IntegrationCode != null)
				.Select(x => $"{x.SchoolGradeId}_{x.IntegrationCode}")
				.ToHashSet();

			foreach (var schoolDto in nsisSchools)
			{
				var nsisCode = schoolDto.Institution?.Trim();

				if (string.IsNullOrWhiteSpace(nsisCode))
					continue;

				if (!schoolMap.TryGetValue(nsisCode, out var school))
					continue;

				if (!homeroomsByInstitution.TryGetValue(nsisCode, out var homerooms))
					continue;
				foreach (var homeroom in homerooms)
				{
					var acadPlanCode = homeroom.AcadPlan?.Trim();
					var sectionCode = homeroom.PlanSection?.Trim();

					if (string.IsNullOrWhiteSpace(acadPlanCode))
						continue;

					if (!acadPlanMap.TryGetValue(acadPlanCode, out var plan))
						continue;

					var acadProg = plan.AcadProg?.Trim();

					if (string.IsNullOrWhiteSpace(acadProg))
						continue;

					var schoolLevel = schoolLevels.FirstOrDefault(x =>
						x.SchoolId == school.Id &&
						x.EducationLevel != null &&
						x.EducationLevel.IntegrationCode == acadProg);

					if (schoolLevel == null)
						continue;

					if (!gradeLevelMap.TryGetValue(acadPlanCode, out var gradeLevel))
					{
						gradeLevel = new GradeLevel
						{
							IntegrationCode = acadPlanCode,
							NameAr = plan.AcadPlanAr ?? acadPlanCode,
							NameEn = plan.AcadPlanEn ?? acadPlanCode,
							//BackendName = acadPlanCode,
							//OrderNo = 0
						};

						await gradeLevelRepo.InsertAsync(gradeLevel);
						gradeLevelMap[acadPlanCode] = gradeLevel;
					}

					var schoolGradeKey = $"{schoolLevel.Id}_{gradeLevel.Id}";

					var schoolGrade = existingSchoolGrades.FirstOrDefault(x =>
						x.SchoolLevelId == schoolLevel.Id &&
						x.GradeLevelId == gradeLevel.Id);

					if (schoolGrade == null)
					{
						schoolGrade = new SchoolGrade
						{
							SchoolLevelId = schoolLevel.Id,
							GradeLevelId = gradeLevel.Id,
							Type = plan.AcadProgEn
						};

						await schoolGradeRepo.InsertAsync(schoolGrade);
						existingSchoolGrades.Add(schoolGrade);
						existingSchoolGradeKeys.Add(schoolGradeKey);
					}

					if (string.IsNullOrWhiteSpace(sectionCode))
						continue;

					var sectionKey = $"{schoolGrade.Id}_{sectionCode}";

					if (existingSectionKeys.Contains(sectionKey))
						continue;

					await sectionRepo.InsertAsync(new SchoolGradeSection
					{
						SchoolGradeId = schoolGrade.Id,
						SectionAr = sectionCode,
						SectionEn = sectionCode,
						IntegrationCode = sectionCode
					});

					existingSectionKeys.Add(sectionKey);
				}
			}

			await uow.CommitAsync();
		}
		public async Task<List<NSISAcadPlanDto>> GetAllAcadPlansAsync()
		{
			var result = new List<NSISAcadPlanDto>();

			using var con = new OracleConnection(ClsAppSetting.OracleDBConnection);

			try
			{
				await con.OpenAsync();

				using var cmd = con.CreateCommand();

				cmd.CommandText = @"
								SELECT 
									ACAD_PLAN,
									ACAD_PLAN_ARA,
									ACAD_PLAN_ENG,
									ACAD_LEVEL,
									ACAD_PROG,
									ACAD_PROG_ARA,
									ACAD_PROG_ENG
								FROM NSIS.ACAD_PLAN";

				using var reader = await cmd.ExecuteReaderAsync();

				while (await reader.ReadAsync())
				{
					result.Add(reader.ToNSISAcadPlanDto());
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ACAD_PLAN error: {ex.Message}");
			}

			return result;
		}
		public async Task<List<NSISHomeroomDto>> GetAllHomeroomsAsync()
		{
			var result = new List<NSISHomeroomDto>();

			using var con = new OracleConnection(ClsAppSetting.OracleDBConnection);

			try
			{
				await con.OpenAsync();

				using var cmd = con.CreateCommand();

				cmd.CommandText = @"
									SELECT STRM, INSTITUTION, LOCATION, ACAD_LEVEL, ACAD_PLAN, PLAN_SECTION
									FROM NSIS.NSIS_HOMEROOM";

				using var reader = await cmd.ExecuteReaderAsync();

				while (await reader.ReadAsync())
					result.Add(reader.ToNSISHomeroomDto());
			}
			catch (Exception ex)
			{
				Console.WriteLine($"NSIS_HOMEROOM error: {ex.Message}");
			}

			return result;
		}
		public async Task<List<NSISCourseDto>> GetAllCoursesAsync()
		{
			var result = new List<NSISCourseDto>();

			using var con = new OracleConnection(ClsAppSetting.OracleDBConnection);

			try
			{
				await con.OpenAsync();

				using var cmd = con.CreateCommand();

				cmd.CommandText = @"
									SELECT DISTINCT
										STRM,
										INSTITUTION,
										ACAD_PLAN,
										SUBJECT,
										SHORT_SBJ_NAM_ENG,
										SUBJECT_NAME_ENG,
										SUBJECT_NAME_ARA
									FROM NSIS.NSIS_COURSE";

				using var reader = await cmd.ExecuteReaderAsync();

				while (await reader.ReadAsync())
					result.Add(reader.ToNSISCourseDto());
			}
			catch (Exception ex)
			{
				Console.WriteLine($"NSIS_COURSE error: {ex.Message}");
			}

			return result;
		}
		private static void MapNSISToSchool(NSISSchoolDto dto,
											School school,
											DateOnly today,
											Dictionary<string, Guid> schoolTypeMap,
											Dictionary<string, Guid> genderMap,
											Dictionary<string, Guid> empGenderMap,
											Dictionary<string, Guid> modelMap,
											Dictionary<string, Guid> programMap)
		{
			school.NameAr = string.IsNullOrWhiteSpace(dto.NameAr)? school.NameAr: dto.NameAr;

			school.NameEn = string.IsNullOrWhiteSpace(dto.NameEn)? school.NameEn: dto.NameEn;

			school.Address = dto.Address;
			school.Phone = dto.Phone;
			school.OrgEmail = dto.Email;

			school.LATITUDE = dto.Latitude;
			school.LONGITUDE = dto.Longitude;
			school.URL = dto.Url;

			school.SchoolCapacity = dto.SchoolCapacity;

			if (school.EstablishmentDate == default)
				school.EstablishmentDate = today;

			school.IsActive = true;
			school.IsAccredited = false;
			school.SupportIdentity = false;

			if (!school.AcceditedDate.HasValue)
				school.AcceditedDate = today;

			if (!school.SupportIdentityDate.HasValue)
				school.SupportIdentityDate = today;

			if (!string.IsNullOrWhiteSpace(dto.SchoolCategoryCode) && schoolTypeMap.TryGetValue(dto.SchoolCategoryCode.Trim(), out var typeId))
			{
				school.TypeId = typeId;
			}

			if (!string.IsNullOrWhiteSpace(dto.GenderCode) &&
				genderMap.TryGetValue(dto.GenderCode.Trim(), out var genderId))
			{
				school.SchoolGenderId = genderId;
			}

			if (!string.IsNullOrWhiteSpace(dto.EmpGenderCode) &&
				empGenderMap.TryGetValue(dto.EmpGenderCode.Trim(), out var empGenderId))
			{
				school.SchoolEmpGenderId = empGenderId;
			}

			if (!string.IsNullOrWhiteSpace(dto.ModelCode) &&
				modelMap.TryGetValue(dto.ModelCode.Trim(), out var modelId))
			{
				school.SchoolModelId = modelId;
			}

			if (!string.IsNullOrWhiteSpace(dto.ProgramCode) &&
				programMap.TryGetValue(dto.ProgramCode.Trim(), out var programId))
			{
				school.SchoolProgramId = programId;
			}
		}

		private async Task<Dictionary<string, Guid>> GetOrCreateLookupMapAsync<T>(UnitOfWork uow,IEnumerable<LookupDto> items,Func<T, string?> integrationCodeSelector,Func<LookupDto, T> factory) where T : EntityBase
		{
			var repo = uow.GetRepository<T>();

			var requiredItems = items
				.Where(x => !string.IsNullOrWhiteSpace(x.Code))
				.GroupBy(x => x.Code!.Trim(), StringComparer.OrdinalIgnoreCase)
				.Select(g => g.First())
				.ToList();

			var existingItems = await repo.GetAllNonDeleted().ToListAsync();

			var map = existingItems
				.Where(x => !string.IsNullOrWhiteSpace(integrationCodeSelector(x)))
				.GroupBy(x => integrationCodeSelector(x)!.Trim(), StringComparer.OrdinalIgnoreCase)
				.ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase);

			foreach (var item in requiredItems)
			{
				var code = item.Code!.Trim();

				if (map.ContainsKey(code))
					continue;

				var newItem = factory(item);
				await repo.InsertAsync(newItem);

				map[code] = newItem.Id;
			}

			return map;
		}
	}
}
