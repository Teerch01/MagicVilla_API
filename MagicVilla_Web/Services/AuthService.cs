using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : BaseService(clientFactory), IAuthService
{
    private readonly string? villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
    public Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = loginRequestDTO,
            Url = $"{villaUrl}/api/UsersAuth/login"
        });
    }

    public Task<T> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDTO)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = registrationRequestDTO,
            Url = $"{villaUrl}/api/UsersAuth/register"
        });
    }
}
