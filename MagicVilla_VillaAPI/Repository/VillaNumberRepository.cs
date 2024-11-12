using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository;

public class VillaNumberRepository(ApplicationDbContext db) : Repository<VillaNumber>(db), IVillaNumberRepository
{
    public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
    {
        entity.UpdatedDate = DateTime.Now;
        db.VillaNumbers.Update(entity);
        await db.SaveChangesAsync();
        return entity;
    }

}
