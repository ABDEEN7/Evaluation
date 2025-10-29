using Evaluation.Services.Models.SMS;
using Evaluation.Services.Models.SMTP;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Evaluation.Services.Special;
public class SmsServices : ISmsServices
{
    private readonly LoggingServices loggingServices;

    public SmsServices(LoggingServices loggingServices)
    {
        this.loggingServices = loggingServices;
    }
    public async Task<bool> _SendMessage(SMSMessageModel model)
    {
        var today = DateTime.Now;
        loggingServices.WriteLine($"SendMessage :{model.message}, Date : {today.ToString("dd/MM/yyyy HH:mm:ss")}");
        return true;
    }

    public async Task<bool> SendMessage(SMSMessageModel model)
    {
        using (var client = new HttpClient())
        {

            // Prepare query parameters with encoding
            var queryParams = new Dictionary<string, string>()
                {
                    { "MobileNo", Uri.EscapeDataString(model.mobile) },
                    { "Message", Uri.EscapeDataString(model.message) }
                };

            var requestUri = new Uri(QueryHelpers.AddQueryString(model.BaseUrl, queryParams));

            // Set headers
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Username", model.UserName);
            client.DefaultRequestHeaders.Add("Password", model.Password);

            // Send the request
            var response = await client.PostAsync(requestUri, null); // Sending empty body

            // Check the response status code
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SMSMessageResponse>(responseContent);

                if (result != null && result.IsSuccess)
                    return true;
            }
            // Log the status code and content of the response
            var statusCode = (int)response.StatusCode;
            var errorContent = await response.Content.ReadAsStringAsync();

            try
            {
                throw new Exception(errorContent);
            }
            catch (Exception ex)
            {
                loggingServices.SaveExceptionLog(ex);
            }

        }

        return false;
    }
}