using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

namespace MagicVilla_VillaAPI.Repository;

public class UnitOfWork(ApplicationDbContext db, IMapper mapper, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IUnitOfWork
{
    public IVillaRepository VillaRepository { get; private set; } = new VillaRepository(db);

    public IVillaNumberRepository VillaNumberRepository { get; private set; } = new VillaNumberRepository(db);

    public IUserRepository UserRepository { get; private set; } = new UserRepository(db, mapper, configuration, userManager, roleManager);

}
