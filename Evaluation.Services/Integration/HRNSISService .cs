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

		// ───────────────────────────────────────────────
		// ORACLE FETCH METHODS
		// ───────────────────────────────────────────────

		public async Task<List<NSISSchoolDto>> GetAllNSISSchoolsAsync()
		{
			var result = new List<NSISSchoolDto>();
			using var con = new OracleConnection(ClsAppSetting.OracleDBConnection);
			try
			{
				await con.OpenAsync();
				using var cmd = con.CreateCommand();
				cmd.CommandText = "SELECT * FROM NSIS.NSIS_SCHOOL";
				using var reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
					result.Add(reader.ToNSISSchoolDto());
			}
			catch (Exception ex) { Console.WriteLine($"NSIS_SCHOOL error: {ex.Message}"); }
			finally { await con.CloseAsync(); }
			return result;
		}

		public async Task<List<NSISHomeroomDto>> GetHomeroomsBySchoolAsync(string institution, string location)
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
                WHERE INSTITUTION = :Institution AND LOCATION = :Location";
				cmd.Parameters.Add(new OracleParameter("Institution", institution));
				cmd.Parameters.Add(new OracleParameter("Location", location));
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

		// ───────────────────────────────────────────────
		// MAIN SYNC: SCHOOLS (UPSERT BY HrCode/LOCATION)
		// ───────────────────────────────────────────────

		public async Task<bool> SyncAllSchoolsAsync()
		{
			var nsisSchools = await _integrationLogger.ExecuteAsync(
				async () => await GetAllNSISSchoolsAsync());

			if (nsisSchools.Count == 0)
				return false;

			using var uow = serviceScopeFactory.CreateScopedUow();
			var schoolRepo = uow.GetRepository<School>();

			// Load all lookup tables into memory to avoid N+1 queries
			var existingSchools = await schoolRepo.GetAllNonDeleted()
				.ToDictionaryAsync(s => s.HrCode, s => s);

			var schoolTypeMap = await GetOrCreateLookupMapAsync<SchoolType>(
				uow, nsisSchools.Select(s => s.TypeCode).Distinct(),
				(repo, code) => repo.GetAllNonDeleted().Where(x => x.IntegrationCode == code).FirstOrDefaultAsync(),
				code => new SchoolType { IntegrationCode = code, NameAr = code, NameEn = code, BackendName = code });

			var genderMap = await GetOrCreateLookupMapAsync<SchoolGender>(
				uow, nsisSchools.Select(s => s.GenderCode).Distinct(),
				(repo, code) => repo.GetAllNonDeleted().Where(x => x.IntegrationCode == code).FirstOrDefaultAsync(),
				code => new SchoolGender { IntegrationCode = code, NameAr = code, NameEn = code, BackendName = code });

			var empGenderMap = await GetOrCreateLookupMapAsync<SchoolGender>(
				uow, nsisSchools.Select(s => s.EmpGenderCode).Distinct(),
				(repo, code) => repo.GetAllNonDeleted().Where(x => x.IntegrationCode == code).FirstOrDefaultAsync(),
				code => new SchoolGender { IntegrationCode = code, NameAr = code, NameEn = code, BackendName = code });

			var modelMap = await GetOrCreateLookupMapAsync<SchoolModel>(
				uow, nsisSchools.Select(s => s.ModelCode).Distinct(),
				(repo, code) => repo.GetAllNonDeleted().Where(x => x.IntegrationCode == code).FirstOrDefaultAsync(),
				code => new SchoolModel { IntegrationCode = code, NameAr = code, NameEn = code, BackendName = code });

			var programMap = await GetOrCreateLookupMapAsync<SchoolProgram>(
				uow, nsisSchools.Select(s => s.ProgramCode).Distinct(),
				(repo, code) => repo.GetAllNonDeleted().Where(x => x.IntegrationCode == code).FirstOrDefaultAsync(),
				code => new SchoolProgram { IntegrationCode = code, NameAr = code, NameEn = code, BackendName = code });

			await uow.CommitAsync(); // commit any new lookup inserts

			DateOnly today = DateOnly.FromDateTime(DateTime.Now);

			foreach (var dto in nsisSchools)
			{
				var hrCode = dto.Location; // LOCATION is the unique school key

				if (existingSchools.TryGetValue(hrCode, out School existing))
				{
					// UPDATE
					MapNSISToSchool(dto, existing, today, schoolTypeMap, genderMap, empGenderMap, modelMap, programMap);
					schoolRepo.Update(existing);
				}
				else
				{
					// INSERT
					var newSchool = new School { HrCode = hrCode, Code = hrCode };
					MapNSISToSchool(dto, newSchool, today, schoolTypeMap, genderMap, empGenderMap, modelMap, programMap);
					await schoolRepo.InsertAsync(newSchool);
					existingSchools[hrCode] = newSchool; // track for SchoolLevel sync below
				}
			}

			await uow.CommitAsync();

			// Sync school levels (homeroom → SchoolLevel rows)
			await SyncSchoolLevelsAsync(nsisSchools, existingSchools);

			return true;
		}

		// ───────────────────────────────────────────────
		// SYNC: SCHOOL LEVELS (HOMEROOM → SchoolLevel)
		// ───────────────────────────────────────────────

		private async Task SyncSchoolLevelsAsync(
			List<NSISSchoolDto> nsisSchools,
			Dictionary<string, School> schoolMap)
		{
			using var uow = serviceScopeFactory.CreateScopedUow();
			var levelRepo = uow.GetRepository<SchoolLevel>();
			var eduLevelRepo = uow.GetRepository<EducationLevel>();

			// Load all EducationLevels keyed by IntegrationCode (ACAD_LEVEL value)
			var eduLevels = await eduLevelRepo.GetAllNonDeleted()
				.Where(e => e.IntegrationCode != null)
				.ToDictionaryAsync(e => e.IntegrationCode, e => e);

			// Load existing SchoolLevel rows to avoid duplicates
			var existingLevels = await levelRepo.GetAllNonDeleted()
				.ToListAsync();

			int currentYear = DateTime.Now.Year;

			foreach (var dto in nsisSchools)
			{
				if (!schoolMap.TryGetValue(dto.Location, out var school))
					continue;

				var homerooms = await GetHomeroomsBySchoolAsync(dto.Institution, dto.Location);

				foreach (var homeroom in homerooms)
				{
					// Resolve or create EducationLevel
					if (!eduLevels.TryGetValue(homeroom.AcadLevel, out var eduLevel))
					{
						eduLevel = new EducationLevel
						{
							IntegrationCode = homeroom.AcadLevel,
							NameAr = homeroom.AcadLevel,
							NameEn = homeroom.AcadLevel,
							BackendName = homeroom.AcadLevel,
							OrderNo = 0
						};
						await eduLevelRepo.InsertAsync(eduLevel);
						await uow.CommitAsync();
						eduLevels[homeroom.AcadLevel] = eduLevel;
					}

					// Upsert SchoolLevel
					bool alreadyExists = existingLevels.Any(l =>
						l.SchoolId == school.Id &&
						l.EducationLevelId == eduLevel.Id &&
						l.AcademicYear == currentYear);

					if (!alreadyExists)
					{
						await levelRepo.InsertAsync(new SchoolLevel
						{
							SchoolId = school.Id,
							EducationLevelId = eduLevel.Id,
							AcademicYear = currentYear
						});
					}
				}
			}

			await uow.CommitAsync();
		}

		// ───────────────────────────────────────────────
		// SYNC: GRADE SECTIONS (HOMEROOM → SchoolGradeSection)
		// ───────────────────────────────────────────────

		public async Task SyncGradeSectionsAsync(Guid schoolLevelId, string institution,
			string location, string acadLevel)
		{
			var homerooms = await GetHomeroomsBySchoolAsync(institution, location);
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

		// ───────────────────────────────────────────────
		// HELPERS
		// ───────────────────────────────────────────────

		private static void MapNSISToSchool(
			NSISSchoolDto dto, School school, DateOnly today,
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
			school.EstablishmentDate = today;
			school.IsActive = true;
			school.IsAccredited = false;
			school.SupportIdentity = false;
			school.AcceditedDate = today;
			school.SupportIdentityDate = today;

			if (!string.IsNullOrEmpty(dto.TypeCode) && schoolTypeMap.TryGetValue(dto.TypeCode, out var typeId))
				school.TypeId = typeId;
			if (!string.IsNullOrEmpty(dto.GenderCode) && genderMap.TryGetValue(dto.GenderCode, out var gId))
				school.SchoolGenderId = gId;
			if (!string.IsNullOrEmpty(dto.EmpGenderCode) && empGenderMap.TryGetValue(dto.EmpGenderCode, out var egId))
				school.SchoolEmpGenderId = egId;
			if (!string.IsNullOrEmpty(dto.ModelCode) && modelMap.TryGetValue(dto.ModelCode, out var mId))
				school.SchoolModelId = mId;
			if (!string.IsNullOrEmpty(dto.ProgramCode) && programMap.TryGetValue(dto.ProgramCode, out var pId))
				school.SchoolProgramId = pId;
		}

	
		private async Task<Dictionary<string, Guid>> GetOrCreateLookupMapAsync<T>(
		UnitOfWork uow,
		IEnumerable<string> codes,
		Func<Repository<T>, string, Task<T>> finder,  
		Func<string, T> factory) where T : EntityBase
		{
			var map = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
			var repo = uow.GetRepository<T>();

			foreach (var code in codes.Where(c => !string.IsNullOrEmpty(c)))
			{
				if (map.ContainsKey(code)) continue;

				var entity = await finder(repo, code);
				if (entity == null)
				{
					entity = factory(code);
					await repo.InsertAsync(entity);
				}
				map[code] = entity.Id;
			}

			return map;
		}
	}
}
