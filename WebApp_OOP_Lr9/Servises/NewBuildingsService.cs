using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApp_OOP_Lr9.DataBase;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApp_OOP_Lr9.Servises
{
    public interface INewBuildingsService
    {
       Task<List<NewBuilding>> GetAllNewBuildingAsync();
       Task<NewBuilding> GetNewBuildingAsync(int id);
       Task<bool> CreateNewBuildingAsync(string caption, string adress);
       Task<bool> UpdateNewBuildingAsync(int id, string caption, string adress);
       Task<bool> DeleteNewBuildingAsync(int id);
    }
    public class NewBuildingsService : INewBuildingsService
    {
        private readonly ApplicationContext db;
        public NewBuildingsService(ApplicationContext context)
        {
            db = context;
        }
        public async Task<List<NewBuilding>> GetAllNewBuildingAsync()
        {
            var existingBuilding = await db.NewBuildings.ToListAsync();
            if (existingBuilding != null)
            {
                return existingBuilding;
            }
            throw new InvalidOperationException("Новобудови з таким id не існує");
        }

        public async Task<NewBuilding> GetNewBuildingAsync(int id)
        {
            var existingBuilding = await db.NewBuildings.FirstOrDefaultAsync(c => c.Id == id);
            if (existingBuilding != null)
            {
                return existingBuilding;
            }
            throw new InvalidOperationException("Новобудови з таким id не існує");
        }
        public async Task<bool> CreateNewBuildingAsync(string caption, string address)
        {
            var newBuilding = new NewBuilding()
            {
                Caption = caption,
                Address = address
            };

            db.NewBuildings.Add(newBuilding);
            await db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateNewBuildingAsync(int id, string caption, string address)
        {
            var existingNewBuilding = await db.NewBuildings.FirstOrDefaultAsync(b => b.Id == id);
            if (existingNewBuilding != null)
            {
                db.Entry(existingNewBuilding).CurrentValues.SetValues(new NewBuilding() { Id = id, Caption = caption, Address = address});
                await db.SaveChangesAsync();
                return true;

            }
            throw new InvalidOperationException("Новобудови з таким id не знайдено");
        }

        public async Task<bool> DeleteNewBuildingAsync(int id)
        {

            var existingNewBuilding = await db.NewBuildings.FindAsync(id);
            if (existingNewBuilding != null)
            {
                db.NewBuildings.Remove(existingNewBuilding);
                await db.SaveChangesAsync();

                if (db.Database.IsSqlServer())
                {
                    var maxId = db.NewBuildings.Max(c => (int?)c.Id) ?? 0;
                    db.Database.ExecuteSql($"DBCC CHECKIDENT ('NewBuildings', RESEED, {maxId})");
                }
                return true;
            }
            throw new InvalidOperationException("Новобудови з таким id не знайдено");
        }
    }
}

