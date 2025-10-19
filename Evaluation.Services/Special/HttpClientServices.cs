using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Evaluation.Services.Special
{
    public class HttpClientServices
    {
        private readonly HttpClient _client;

        public HttpClientServices(HttpClient httpClient)
        {
            _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        #region === Public HTTP Methods ===

        public Task<(T response, HttpResponseMessage httpResponse)> GetResponse<T>(
            string url,
            AuthenticationHeaderValue? authHeader = null,
            List<KeyValuePair<string, string>>? headers = null,
            CancellationToken cancellationToken = default)
            => SendAsync<T>(HttpMethod.Get, url, null, authHeader, headers, cancellationToken);

        public Task<(T response, HttpResponseMessage httpResponse)> DeleteResponse<T>(
            string url,
            AuthenticationHeaderValue? authHeader = null,
            List<KeyValuePair<string, string>>? headers = null,
            CancellationToken cancellationToken = default)
            => SendAsync<T>(HttpMethod.Delete, url, null, authHeader, headers, cancellationToken);

        public Task<(T response, HttpResponseMessage httpResponse)> PostResponse<T>(
            string url,
            string jsonBody,
            AuthenticationHeaderValue? authHeader = null,
            List<KeyValuePair<string, string>>? headers = null,
            CancellationToken cancellationToken = default)
            => SendAsync<T>(HttpMethod.Post, url, new StringContent(jsonBody, Encoding.UTF8, "application/json"), authHeader, headers, cancellationToken);

        public Task<(T response, HttpResponseMessage httpResponse)> PostResponse<T>(
            string url,
            FormUrlEncodedContent formContent,
            AuthenticationHeaderValue? authHeader = null,
            List<KeyValuePair<string, string>>? headers = null,
            CancellationToken cancellationToken = default)
            => SendAsync<T>(HttpMethod.Post, url, formContent, authHeader, headers, cancellationToken);

        public Task<(T response, HttpResponseMessage httpResponse)> PutResponse<T>(
            string url,
            string jsonBody,
            AuthenticationHeaderValue? authHeader = null,
            List<KeyValuePair<string, string>>? headers = null,
            CancellationToken cancellationToken = default)
            => SendAsync<T>(HttpMethod.Put, url, new StringContent(jsonBody, Encoding.UTF8, "application/json"), authHeader, headers, cancellationToken);

        #endregion

        #region === Core Request Pipeline ===

        private async Task<(T response, HttpResponseMessage httpResponse)> SendAsync<T>(
            HttpMethod method,
            string url,
            HttpContent? content = null,
            AuthenticationHeaderValue? authHeader = null,
            List<KeyValuePair<string, string>>? headers = null,
            CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(method, url);

            if (authHeader != null)
                request.Headers.Authorization = authHeader;

            if (headers != null)
            {
                foreach (var h in headers)
                    request.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }

            if (content != null)
                request.Content = content;

            HttpResponseMessage response;
            try
            {
                response = await _client.SendAsync(request, cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                throw new HttpRequestException($"Request to {url} timed out or was canceled.", ex);
            }

            string body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                string shortBody = body?.Length > 500 ? body.Substring(0, 500) + "..." : body;
                throw new HttpRequestException($"HTTP {response.StatusCode} for {url}: {shortBody}");
            }

            try
            {
                if (typeof(T) == typeof(string))
                    return ((T)(object)body, response);

                var result = JsonConvert.DeserializeObject<T>(body);
                return (result!, response);
            }
            catch (System.Text.Json.JsonException ex)
            {
                throw new InvalidOperationException($"Failed to deserialize response from {url}: {body}", ex);
            }
        }

        #endregion
    }
}
