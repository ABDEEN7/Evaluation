using AutoMapper;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Dtos.NsisIntegrationDto;
using Evaluation.SharedHelper.Helper;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;

namespace Evaluation.Services.Integration;

public class NSISService
{
    private readonly HttpClient _httpClient;
    private readonly LoggingServices _loggingServices;
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory serviceScopeFactory;

    public NSISService(HttpClient httpClient, LoggingServices loggingServices, IMapper mapper, IServiceScopeFactory serviceScopeFactory )
    {
        _httpClient = httpClient;
        _loggingServices = loggingServices;
        _mapper = mapper;
		this.serviceScopeFactory = serviceScopeFactory;
	}


	public async Task<AuthenticationResponse?> GetTokenAsync()
	{
		try
		{
			var authToken = Encoding.ASCII.GetBytes(
				$"{ClsAppSetting.NsisUsername}:{ClsAppSetting.NsisPassword}"
			);

			var request = new HttpRequestMessage(
				HttpMethod.Post,
				ClsAppSetting.NsisAuthenticationURL
			);

			request.Headers.Add("Accept", "application/json");
			request.Headers.Authorization =
				new System.Net.Http.Headers.AuthenticationHeaderValue(
					"Basic",
					Convert.ToBase64String(authToken)
				);

			request.Content = new FormUrlEncodedContent(new[]
			{
			new KeyValuePair<string, string>("grant_type", ClsAppSetting.NsisGrantType)
		});

			var response = await _httpClient.SendAsync(request);
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				_loggingServices.SaveExceptionLog(
					new Exception($"NSIS Token Error: {response.StatusCode} - {content}")
				);
				return null;
			}

			return JsonSerializer.Deserialize<AuthenticationResponse>(
				content,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			);
		}
		catch (Exception ex)
		{
			_loggingServices.SaveExceptionLog(ex);
			return null;
		}
	}
	private async Task<T?> SendRequestAsync<T>(string endpoint)
	{
		try
		{
			var tokenResponse = await GetTokenAsync();

			if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.Access_token))
			{
				_loggingServices.SaveExceptionLog(
					new Exception("NSIS token response is null or access token is empty.")
				);
				return default;
			}

			var request = new HttpRequestMessage(
				HttpMethod.Get,
				$"{ClsAppSetting.NsisBaseURL}/{endpoint}"
			);

			request.Headers.Authorization =
				new System.Net.Http.Headers.AuthenticationHeaderValue(
					"Bearer",
					tokenResponse.Access_token
				);

			var response = await _httpClient.SendAsync(request);

			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				_loggingServices.SaveExceptionLog(
					new Exception($"NSIS API Error: {response.StatusCode} - {content}")
				);
				return default;
			}

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};

			return JsonSerializer.Deserialize<T>(content, options);
		}
		catch (Exception ex)
		{
			_loggingServices.SaveExceptionLog(ex);
			return default;
		}
	}
	public async Task<List<SchoolDto>?> GetSchoolsAsync()
    {
        var endpoint = $"{ClsAppSetting.NsisSchoolsApi}?limit={ClsAppSetting.NsisLimit}&filter=status='{ClsAppSetting.NsisStatus}'";
        var result = await SendRequestAsync<NSISSchoolsResponse>(endpoint);
        return result.Orgs;
    }
	public async Task<NSISSchool?> GetSchoolbyIdAsync(Guid id)
	{
		try
		{
			var endpoint = $"{ClsAppSetting.NsisSchoolsApi}/{id}";
			var classesEndpoint = string.Format(ClsAppSetting.NsisClassesApi, id);
			var teachersEndpoint = string.Format(ClsAppSetting.NsisTeachersApi, id);
			var staffEndpoint = string.Format(ClsAppSetting.NsisStaffApi, id);
			var enrollmentEndpoint = string.Format(ClsAppSetting.NsisEnrollmentApi, id);

			var result = await SendRequestAsync<NSISSchoolResponse>(endpoint);

			if (result?.Org == null)
			{
				_loggingServices.SaveExceptionLog(
					new Exception($"School not found for id: {id}")
				);

				return null;
			}

			var classesResult = await SendRequestAsync<NSISClassResponse>(classesEndpoint);
			var teachersResult = await SendRequestAsync<NSISTeacherResponse>(teachersEndpoint);
			var staffResult = await SendRequestAsync<NSISStaffResponse>(staffEndpoint);
			var enrollmentResult = await SendRequestAsync<NSISEnrollmentResponse>(enrollmentEndpoint);

			result.Org.Classes = classesResult?.Classes ?? new List<Class>();
			result.Org.Teachers = teachersResult?.Users ?? new List<Teacher>();
			result.Org.Staff = staffResult?.Users ?? new List<StaffDto>();
			result.Org.Enrollments = enrollmentResult?.Enrollments ?? new List<EnrollmentDto>();

			var mappedData = _mapper.Map<NSISSchool>(result.Org);

			return mappedData;
		}
		catch (Exception ex)
		{
			_loggingServices.SaveExceptionLog(ex);
			return null;
		}
	}

	//public async Task SyncAllSchoolClassesAsync()
	//{
	//	using var uow = serviceScopeFactory.CreateScopedUow();

	//	var nsisSchools = await GetSchoolsAsync();

	//	if (nsisSchools == null || !nsisSchools.Any())
	//		return;

	//	foreach (var nsisSchool in nsisSchools)
	//	{
	//		if (!Guid.TryParse(nsisSchool.Id, out Guid nsisSchoolId))
	//			continue;

	//		var school = await uow.GetRepository<OrgTree>()
	//			.FirstOrDefaultAsync(x => x.NSISCode == nsisSchoolId);

	//		if (school == null)
	//			continue;

	//		var schoolDetails = await GetSchoolbyIdAsync(nsisSchoolId);

	//		if (schoolDetails?.Classes == null || !schoolDetails.Classes.Any())
	//			continue;

	//		foreach (var nsisClass in schoolDetails.Classes)
	//		{
				

	//			var classCode = nsisClass.ClassCode ;

	//			if (string.IsNullOrWhiteSpace(classCode))
	//				continue;

	//			var exists = await uow.GetRepository<SchoolClass>()
	//				.AnyAsync(x =>
	//					x.SchoolId == school.Id &&
	//					x.Code == classCode);

	//			if (exists)
	//				continue;

	//			var schoolClass = new SchoolClass
	//			{
	//				Id = Guid.NewGuid(),

	//				SchoolId = school.Id,

	//				NameAr = nsisClass.NameAr ?? string.Empty,
	//				NameEn = nsisClass.NameEn ?? string.Empty,

	//				Grade = nsisClass.Grade ?? string.Empty,
	//				Duration = 45,

	//				Code = classCode,

	//			};

	//			await uow.GetRepository<SchoolClass>().AddAsync(schoolClass);
	//		}

	//		await uow.SaveChangesAsync();
	//	}
	//}

}