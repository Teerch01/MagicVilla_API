﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration) : BaseService(clientFactory), IVillaNumberService
{
    private readonly string? villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

    public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = dto,
            Url = $"{villaUrl}/villaNumberAPI",
            Token = token
        });
    }

    public Task<T> DeleteAsync<T>(int id, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = $"{villaUrl}/villaNumberAPI/{id}",
            Token = token
        });
    }

    public Task<T> GetAllAsync<T>(string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = $"{villaUrl}/villaNumberAPI",
            Token = token
        });
    }

    public Task<T> GetAsync<T>(int id, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = $"{villaUrl}/villaNumberAPI/{id}",
            Token = token
        });
    }

    public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = dto,
            Url = $"{villaUrl}/villaNumberAPI/{dto.VillaNo}",
            Token = token
        });
    }
}
