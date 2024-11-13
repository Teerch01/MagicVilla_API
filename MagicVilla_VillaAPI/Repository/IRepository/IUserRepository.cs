using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Repository.IRepository;

public interface IUserRepository
{
    Task<bool> IsUniqueUser(string username);
    Task<LoginResponseDTO> Login(LoginRequestDTO user);
    Task<LocalUser> Register(RegistrationRequestDTO createUser);
}
