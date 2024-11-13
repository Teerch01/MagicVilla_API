using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository;

public class UnitOfWork(ApplicationDbContext db, IMapper mapper, IConfiguration configuration) : IUnitOfWork
{
    public IVillaRepository VillaRepository { get; private set; } = new VillaRepository(db);

    public IVillaNumberRepository VillaNumberRepository { get; private set; } = new VillaNumberRepository(db);

    public IUserRepository UserRepository { get; private set; } = new UserRepository(db, mapper, configuration);

}
