using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository;

public class UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration) : IUserRepository
{

    private readonly string secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    public async Task<bool> IsUniqueUser(string username)
    {
        var user = await db.LocalUsers.FirstOrDefaultAsync(x => x.UserName == username);
        if (user == null)
        {
            return true;
        }

        return false;
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginUser)
    {
        var user = await db.LocalUsers.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginUser.UserName.ToLower() && x.Password == loginUser.Password);

        if (user == null)
        {
            return new LoginResponseDTO()
            {
                User = null,
                Token = ""
            };
        }

        //Generate JWT Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            ]),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        LoginResponseDTO loginDTO = new()
        {
            User = user,
            Token = tokenHandler.WriteToken(token)
        };
        return loginDTO;

    }

    public async Task<LocalUser> Register(RegistrationRequestDTO createUser)
    {
        var user = mapper.Map<LocalUser>(createUser);
        await db.LocalUsers.AddAsync(user);
        await db.SaveChangesAsync();
        user.Password = "";
        return user;
    }
}
