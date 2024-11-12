using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository;

public class VillaRepository(ApplicationDbContext db) : Repository<Villa>(db), IVillaRepository
{
    public async Task<Villa> UpdateAsync(Villa entity)
    {
        entity.UpdatedDate = DateTime.Now;
        db.Villas.Update(entity);
        await db.SaveChangesAsync();
        return entity;
    }

}
