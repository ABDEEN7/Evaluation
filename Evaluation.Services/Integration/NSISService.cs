using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.NsisIntegrationDto;
using System.Text;
using System.Text.Json;

namespace Evaluation.Services.Integration;

public class NSISService
{
    private readonly HttpClient _httpClient;
    private readonly LoggingServices _loggingServices;
    private readonly string baseURL = "https://nsis-test.edu.gov.qa/INTCore.Web";
    private readonly string authenticationURL = "https://nsis-test.edu.gov.qa/GWCore.Web/connect/token";
    private readonly string username = "svc_eval-nsis_api";
    private readonly string password = "UgBNL%Pxukl4g8f";
    private readonly string grant_type = "client_credentials";
    private readonly string nsis_schools_api = "ims/oneroster/v1p1/schools";
    private readonly string nsis_classes_api = "ims/oneroster/v1p1/schools/{0}/classes?offset=0&limit=100&filter=status='active'";
    private readonly int limit = 1000;
    private readonly string? status = "active";

    public NSISService(HttpClient httpClient, LoggingServices loggingServices)
    {
        _httpClient = httpClient;
        _loggingServices = loggingServices;
    }


    public async Task<AuthenticationResponse> GetTokenAsync()
    {
        try
        {   var authToken = Encoding.ASCII.GetBytes($"{username}:{password}");
            var auth = "Basic "+ Convert.ToBase64String(authToken);

            var request = new HttpRequestMessage(HttpMethod.Post, authenticationURL);

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", auth);

            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("grant_type", grant_type));
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
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseURL}/{endpoint}");
            
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
        var endpoint = $"{nsis_schools_api}?limit={limit}&filter=status='{status}'";
        var result = await SendRequestAsync<NSISSchoolsResponse>(endpoint);
        return result.Orgs;
    }
    public async Task<SchoolDto> GetSchoolbyIdAsync(Guid Id)
    {
        var endpoint = $"{nsis_schools_api}/{Id}";
        var classesEndpoint = string.Format(nsis_classes_api,Id);
        var result = await SendRequestAsync<NSISSchoolResponse>(endpoint);
        var classesResult = await SendRequestAsync<NSISClassResponse>(classesEndpoint);
        result.Org.Classes = classesResult.Classes;
        return result.Org;
    }

}