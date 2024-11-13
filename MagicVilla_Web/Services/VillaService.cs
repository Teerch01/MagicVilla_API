using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : BaseService(clientFactory), IVillaService
{
    private readonly string? villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

    public Task<T> CreateAsync<T>(VillaCreateDTO dto, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = dto,
            Url = $"{villaUrl}/villaAPI",
            Token = token
        });
    }

    public Task<T> DeleteAsync<T>(int id, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = $"{villaUrl}/villaAPI/{id}",
            Token = token
        });
    }

    public Task<T> GetAllAsync<T>(string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = $"{villaUrl}/villaAPI",
            Token = token
        });
    }

    public Task<T> GetAsync<T>(int id, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = $"{villaUrl}/villaAPI/{id}",
            Token = token
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDTO dto, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = dto,
            Url = $"{villaUrl}/villaAPI/{dto.Id}",
            Token = token
        });
    }
}
