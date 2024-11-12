using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MagicVilla_Web.Services;

public class BaseService(IHttpClientFactory httpClient) : IBaseService
{
    public APIResponse responseModel { get; set; } = new();

    public IHttpClientFactory httpClient { get; set; } = httpClient;

    public async Task<T> SendAsync<T>(APIRequest apiRequest)
    {
        try
        {
            var client = httpClient.CreateClient("MagicAPI");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new(apiRequest.Url);
            if (apiRequest.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                   Encoding.UTF8, "application/json");
            }
            message.Method = apiRequest.ApiType switch
            {
                SD.ApiType.POST => HttpMethod.Post,
                SD.ApiType.PUT => HttpMethod.Put,
                SD.ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get,
            };

            HttpResponseMessage apiResponse;

            apiResponse = await client.SendAsync(message);

            var apiContent = await apiResponse.Content.ReadAsStringAsync();

            try
            {
                APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                if (apiResponse.StatusCode == HttpStatusCode.BadRequest || apiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    ApiResponse.StatusCode = apiResponse.StatusCode switch
                    {
                        HttpStatusCode.BadRequest => HttpStatusCode.BadRequest,
                        _ => HttpStatusCode.NotFound,
                    };

                    ApiResponse.IsSuccess = false;
                    var res = JsonConvert.SerializeObject(ApiResponse);
                    var responseObj = JsonConvert.DeserializeObject<T>(res);

                    return responseObj;
                }
            }
            catch (Exception ex)
            {
                var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return exceptionResponse;
            }
            var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
            return APIResponse;
        }
        catch (Exception ex)
        {
            var dto = new APIResponse
            {
                ErrorMessages = [Convert.ToString(ex.Message)],
                IsSuccess = false
            };

            var res = JsonConvert.SerializeObject(dto);
            var APIResponse = JsonConvert.DeserializeObject<T>(res);

            return APIResponse;
        }
    }
}
