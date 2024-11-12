using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository;

public class UnitOfWork(ApplicationDbContext db) : IUnitOfWork
{
    public IVillaRepository VillaRepository { get; private set; } =  new VillaRepository(db);

    public IVillaNumberRepository VillaNumberRepository { get; private set; } = new VillaNumberRepository(db);

}
