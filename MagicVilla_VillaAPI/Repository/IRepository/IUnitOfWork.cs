namespace MagicVilla_VillaAPI.Repository.IRepository;

public interface IUnitOfWork
{
    IVillaRepository VillaRepository { get; }
    IVillaNumberRepository VillaNumberRepository { get; }
}
