using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository;

public class UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IUserRepository
{

    private readonly string secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    public async Task<bool> IsUniqueUser(string username)
    {
        var user = await db.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName == username);
        if (user == null)
        {
            return true;
        }

        return false;
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginUser)
    {
        var user = await db.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginUser.UserName.ToLower());

        bool isValid = await userManager.CheckPasswordAsync(user, loginUser.Password);
        if (user == null || isValid == false)
        {
            return new LoginResponseDTO()
            {
                User = null,
                Token = ""
            };
        }

        //Generate JWT Token
        var roles = await userManager.GetRolesAsync(user);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
            ]),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        LoginResponseDTO loginDTO = new()
        {
            User = mapper.Map<UserDTO>(user),
            //Role = roles.FirstOrDefault(),
            Token = tokenHandler.WriteToken(token)
        };
        return loginDTO;

    }

    public async Task<UserDTO> Register(RegistrationRequestDTO createUser)
    {
        ApplicationUser user = new()
        {
            UserName = createUser.UserName,
            Email = createUser.UserName,
            NormalizedEmail = createUser.UserName.ToUpper(),
            Name = createUser.Name
        };

        try
        {
            var result = await userManager.CreateAsync(user, createUser.Password);
            if (result.Succeeded)
            {
                if (!roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await roleManager.CreateAsync(new IdentityRole("admin"));
                    await roleManager.CreateAsync(new IdentityRole("customer"));
                }
                await userManager.AddToRoleAsync(user, "admin");
                var userDTO = await db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == createUser.UserName);
                return mapper.Map<UserDTO>(userDTO);
            }
        }
        catch
        {

        }

        return new UserDTO();
    }
}
