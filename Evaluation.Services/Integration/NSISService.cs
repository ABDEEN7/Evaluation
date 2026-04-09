using AutoMapper;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.NsisIntegrationDto;
using Evaluation.SharedHelper.Helper;
using System.Text;
using System.Text.Json;

namespace Evaluation.Services.Integration;

public class NSISService
{
    private readonly HttpClient _httpClient;
    private readonly LoggingServices _loggingServices;
    private readonly IMapper _mapper;

    public NSISService(HttpClient httpClient, LoggingServices loggingServices, IMapper mapper)
    {
        _httpClient = httpClient;
        _loggingServices = loggingServices;
        _mapper = mapper;
    }


    public async Task<AuthenticationResponse> GetTokenAsync()
    {
        try
        {
            var authToken = Encoding.ASCII.GetBytes($"{ClsAppSetting.NsisUsername}:{ClsAppSetting.NsisPassword}");
            var auth = "Basic " + Convert.ToBase64String(authToken);

            var request = new HttpRequestMessage(HttpMethod.Post, ClsAppSetting.NsisAuthenticationURL);

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", auth);

            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("grant_type", ClsAppSetting.NsisGrantType));
            var bodycontent = new FormUrlEncodedContent(collection);
            request.Content = bodycontent;

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<AuthenticationResponse>(content, options);
        }
        catch (Exception)
        {
            _loggingServices.SaveExceptionLog(new Exception("Error occurred while calling NSIS API."));
            return default;
        }
    }

    private async Task<T?> SendRequestAsync<T>(string endpoint)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ClsAppSetting.NsisBaseURL}/{endpoint}");

            var JWTToken = GetTokenAsync().Result.Access_token;
            var auth = "Bearer " + JWTToken;

            request.Headers.Add("Authorization", auth);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(content, options);
        }
        catch (Exception)
        {
            _loggingServices.SaveExceptionLog(new Exception("Error occurred while calling NSIS API."));
            return default;
        }
    }

    public async Task<List<SchoolDto>?> GetSchoolsAsync()
    {
        var endpoint = $"{ClsAppSetting.NsisSchoolsApi}?limit={ClsAppSetting.NsisLimit}&filter=status='{ClsAppSetting.NsisStatus}'";
        var result = await SendRequestAsync<NSISSchoolsResponse>(endpoint);
        return result.Orgs;
    }
    public async Task<NSISSchool> GetSchoolbyIdAsync(Guid Id)
    {
        var endpoint = $"{ClsAppSetting.NsisSchoolsApi}/{Id}";
        var classesEndpoint = string.Format(ClsAppSetting.NsisClassesApi, Id);
        var teachersEndpoint = string.Format(ClsAppSetting.NsisTeachersApi, Id);
        var staffEndpoint = string.Format(ClsAppSetting.NsisStaffApi, Id);
        var enrollmentEndpoint = string.Format(ClsAppSetting.NsisEnrollmentApi, Id);
        var result = await SendRequestAsync<NSISSchoolResponse>(endpoint);
        var classesResult = await SendRequestAsync<NSISClassResponse>(classesEndpoint);
        var teachersResult = await SendRequestAsync<NSISTeacherResponse>(teachersEndpoint);
        var staffResult = await SendRequestAsync<NSISStaffResponse>(staffEndpoint);
        var enrollmentResult = await SendRequestAsync<NSISEnrollmentResponse>(enrollmentEndpoint);
        result.Org.Classes = classesResult.Classes;
        result.Org.Teachers = teachersResult.Users;
        result.Org.Staff = staffResult.Users;
        result.Org.Enrollments = enrollmentResult.Enrollments;

        var mappedData = _mapper.Map<NSISSchool>(result.Org);

        return mappedData;
    }

}