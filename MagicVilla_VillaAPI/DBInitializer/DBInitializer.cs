using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.DBInitializer;

public class DBInitializer(ApplicationDbContext db) : IDBInitializer
{
    public void Initialize()
    {
        //add migrations if they are not applied
        try
        {
            if (db.Database.GetPendingMigrations().Count() > 0)
            {
                db.Database.Migrate();
            }
        }
        catch (Exception ex) { }

    }
}
