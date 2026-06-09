using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Extensions;
using Evaluation.Services.Models.NSISSchool;
using Evaluation.Services.Special;
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


		public async Task<bool> SyncAllSchoolsAsync()
		{
			var nsisSchools = await _integrationLogger.ExecuteAsync(
				async () => await GetAllNSISSchoolsAsync());

			if (nsisSchools == null || nsisSchools.Count == 0)
				return false;

			using var uow = serviceScopeFactory.CreateScopedUow();

			var schoolRepo = uow.GetRepository<School>();

			var existingSchools = await schoolRepo.GetAllNonDeleted()
									.Where(s => s.NSISCode != null)
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
				}
				else
				{
					var newSchool = new School
					{
						NSISCode = NSISCode,
						HrCode = "2"+NSISCode,
						Code = NSISCode
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

					existingSchools[NSISCode] = newSchool;
				}
			}

			await uow.CommitAsync();

			await SyncSchoolLevelsAsync(nsisSchools, existingSchools);

			return true;
		}
		public async Task<List<NSISSchoolDto>> GetAllNSISSchoolsAsync()
		{
			var result = new List<NSISSchoolDto>();

			using var con = new OracleConnection(ClsAppSetting.OracleDBConnection);

			try
			{
				await con.OpenAsync();

				using var cmd = con.CreateCommand();

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
									FROM NSIS.NSIS_SCHOOL";

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

		public async Task<List<NSISHomeroomDto>> GetHomeroomsBySchoolAsync(string institution)
		{
			var result = new List<NSISHomeroomDto>();
			using var con = new OracleConnection(ClsAppSetting.OracleDBConnection);
			try
			{
				await con.OpenAsync();
				using var cmd = con.CreateCommand();
				cmd.BindByName = true;
				cmd.CommandText = @"
                SELECT STRM, INSTITUTION, LOCATION, ACAD_LEVEL, ACAD_PLAN, PLAN_SECTION
                FROM NSIS.NSIS_HOMEROOM
                WHERE INSTITUTION = :Institution";
				cmd.Parameters.Add(new OracleParameter("Institution", institution));
				using var reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
					result.Add(reader.ToNSISHomeroomDto());
			}
			catch (Exception ex) { Console.WriteLine($"NSIS_HOMEROOM error: {ex.Message}"); }
			finally { await con.CloseAsync(); }
			return result;
		}

		public async Task<List<NSISScheduleDto>> GetSchedulesBySchoolAsync(string institution)
		{
			var result = new List<NSISScheduleDto>();
			using var con = new OracleConnection(ClsAppSetting.OracleDBConnection);
			try
			{
				await con.OpenAsync();
				using var cmd = con.CreateCommand();
				cmd.BindByName = true;
				cmd.CommandText = @"
                SELECT INSTITUTION, EMPL_ID, SUBJECT, ACAD_PLAN, PLAN_SECTION,
                       GROUP_ID, PERIODS, DAYCD, SC_PRD_BGN_TM, SC_PRD_END_TM, LOCATION
                FROM NSIS.NSIS_SCHEDULE
                WHERE INSTITUTION = :Institution";
				cmd.Parameters.Add(new OracleParameter("Institution", institution));
				using var reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
					result.Add(reader.ToNSISScheduleDto());
			}
			catch (Exception ex) { Console.WriteLine($"NSIS_SCHEDULE error: {ex.Message}"); }
			finally { await con.CloseAsync(); }
			return result;
		}

		public async Task<List<NSISAcadPlanDto>> GetAcadPlansByLevelAsync(string acadLevel)
		{
			var result = new List<NSISAcadPlanDto>();
			using var con = new OracleConnection(ClsAppSetting.OracleDBConnection);
			try
			{
				await con.OpenAsync();
				using var cmd = con.CreateCommand();
				cmd.BindByName = true;
				cmd.CommandText = @"
                SELECT ACAD_PLAN, ACAD_PLAN_ARA, ACAD_PLAN_ENG,
                       ACAD_LEVEL, ACAD_PROG, ACAD_PROG_ARA, ACAD_PROG_ENG
                FROM NSIS.ACAD_PLAN
                WHERE ACAD_LEVEL = :AcadLevel";
				cmd.Parameters.Add(new OracleParameter("AcadLevel", acadLevel));
				using var reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
					result.Add(reader.ToNSISAcadPlanDto());
			}
			catch (Exception ex) { Console.WriteLine($"ACAD_PLAN error: {ex.Message}"); }
			finally { await con.CloseAsync(); }
			return result;
		}
		private async Task SyncSchoolLevelsAsync(List<NSISSchoolDto> nsisSchools,Dictionary<string, School> schoolMap)
		{
			using var uow = serviceScopeFactory.CreateScopedUow();

			var levelRepo = uow.GetRepository<SchoolLevel>();
			var eduLevelRepo = uow.GetRepository<EducationLevel>();

			var acadPlans = await GetAllAcadPlansAsync();

			var eduLevels = await eduLevelRepo.GetAllNonDeleted()
				.Where(x => x.IntegrationCode != null)
				.ToDictionaryAsync(x => x.IntegrationCode!, x => x);

			var existingLevels = await levelRepo.GetAllNonDeleted()
				.ToListAsync();

			var existingKeys = existingLevels
				.Select(x => $"{x.SchoolId}_{x.EducationLevelId}_{x.AcademicYear}")
				.ToHashSet();

			int currentYear = DateTime.Now.Year;

			foreach (var dto in nsisSchools)
			{
				var nsisCode = dto.Institution?.Trim();

				if (string.IsNullOrWhiteSpace(nsisCode))
					continue;

				if (!schoolMap.TryGetValue(nsisCode, out var school))
					continue;

				var homerooms = await GetHomeroomsBySchoolAsync(nsisCode);

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

					var key = $"{school.Id}_{eduLevel.Id}_{currentYear}";

					if (existingKeys.Contains(key))
						continue;

					await levelRepo.InsertAsync(new SchoolLevel
					{
						SchoolId = school.Id,
						EducationLevelId = eduLevel.Id,
						AcademicYear = currentYear
					});

					existingKeys.Add(key);
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
		public async Task SyncGradeSectionsAsync(Guid schoolLevelId, string institution,
			string location, string acadLevel)
		{
			var homerooms = await GetHomeroomsBySchoolAsync(institution);
			var filtered = homerooms.Where(h => h.AcadLevel == acadLevel).ToList();

			if (!filtered.Any()) return;

			using var uow = serviceScopeFactory.CreateScopedUow();
			var sectionRepo = uow.GetRepository<SchoolGradeSction>();

			var existing = await sectionRepo.GetAllNonDeleted()
				.Where(s => s.SchoolLevelId == schoolLevelId)
				.ToListAsync();

			foreach (var homeroom in filtered)
			{
				bool exists = existing.Any(s => s.IntegrationCode == $"{homeroom.AcadPlan}_{homeroom.PlanSection}");
				if (exists) continue;

				// Fetch grade label from ACAD_PLAN
				var acadPlans = await GetAcadPlansByLevelAsync(homeroom.AcadLevel);
				var plan = acadPlans.FirstOrDefault(p => p.AcadPlan == homeroom.AcadPlan);

				await sectionRepo.InsertAsync(new SchoolGradeSction
				{
					SchoolLevelId = schoolLevelId,
					NameAr = plan?.AcadPlanAr ?? homeroom.AcadPlan,
					NameEn = plan?.AcadPlanEn ?? homeroom.AcadPlan,
					Grade = homeroom.AcadLevel,
					Type = plan?.AcadProgEn,
					IntegrationCode = $"{homeroom.AcadPlan}_{homeroom.PlanSection}"
				});
			}

			await uow.CommitAsync();
		}


		private static void MapNSISToSchool(
	NSISSchoolDto dto,
	School school,
	DateOnly today,
	Dictionary<string, Guid> schoolTypeMap,
	Dictionary<string, Guid> genderMap,
	Dictionary<string, Guid> empGenderMap,
	Dictionary<string, Guid> modelMap,
	Dictionary<string, Guid> programMap)
		{
			school.NameAr = dto.NameAr;
			school.NameEn = dto.NameEn;

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


		private async Task<Dictionary<string, Guid>> GetOrCreateLookupMapAsync<T>(
	UnitOfWork uow,
	IEnumerable<LookupDto> items,
	Func<T, string?> integrationCodeSelector,
	Func<LookupDto, T> factory) where T : EntityBase
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
