using Asp.Versioning;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers.V1;

[ApiVersion(1)]
[Route("api/v{version:apiVersion}/UsersAuth")]
[ApiController]
public class UsersController(IUnitOfWork unit) : ControllerBase
{
    protected APIResponse response = new();

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
    {
        var login = await unit.UserRepository.Login(model);
        if (login.User == null || string.IsNullOrEmpty(login.Token))
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.IsSuccess = false;
            response.ErrorMessages =
            [
                "Username or password is incorrect"
            ];
            return BadRequest(response);
        }

        response.StatusCode = HttpStatusCode.OK;
        response.IsSuccess = true;
        response.Result = login;

        return Ok(response);

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
    {
        var isUsernameUnique = await unit.UserRepository.IsUniqueUser(model.UserName);
        if (!isUsernameUnique)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.IsSuccess = false;
            response.ErrorMessages = [
                "Username already exists"
                ];
            return BadRequest(response);
        }

        var user = await unit.UserRepository.Register(model);

        if (user == null)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.IsSuccess = false;
            response.ErrorMessages = [
                "Error while registering"];
            return BadRequest(response);
        }

        response.StatusCode = HttpStatusCode.OK;
        response.IsSuccess = true;
        return Ok(response);
    }
}
