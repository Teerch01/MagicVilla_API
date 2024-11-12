using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : BaseService(clientFactory), IVillaService
{
    private string? villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

    public Task<T> CreateAsync<T>(VillaCreateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = dto,
            Url = $"{villaUrl}/api/villaAPI",
        });
    }

    public Task<T> DeleteAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = $"{villaUrl}/api/villaAPI/{id}",
        });
    }

    public Task<T> GetAllAsync<T>()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = $"{villaUrl}/api/villaAPI",
        });
    }

    public Task<T> GetAsync<T>(int id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = $"{villaUrl}/api/villaAPI/{id}",
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = dto,
            Url = $"{villaUrl}/api/villaAPI/{dto.Id}",
        });
    }
}
