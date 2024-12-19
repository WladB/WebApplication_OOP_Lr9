using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using WebApp_OOP_Lr9.DataBase;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApp_OOP_Lr9.Servises
{
    public interface IPropertiesService
    {
        Task<List<Property>> GetAllPropertyAsync(int buildId);
        Task<Property> GetPropertyAsync(int id);
        Task<bool> CreatePropertyAsync(int countRooms, int newBuildingId, float area, byte floor);
        Task<bool> UpdatePropertyAsync(int id, int countRooms, int newBuildingId, float area, byte floor);
        Task<bool> DeletePropertyAsync(int id);
    }
    public class PropertiesService : IPropertiesService
    {
        private readonly ApplicationContext db;
        public PropertiesService(ApplicationContext context)
        {
            db = context;
        }
        public async Task<List<Property>> GetAllPropertyAsync(int buildId)
        {
            var existingBuilding = await db.Properties.Where(p => p.NewBuildingId == buildId).ToListAsync();
            if (existingBuilding != null)
            {
                return existingBuilding;
            }
            throw new InvalidOperationException("Квартири з таким id не існує");
        }

        public async Task<Property> GetPropertyAsync(int id)
        {
            var existingBuilding = await db.Properties.FirstOrDefaultAsync(c => c.Id == id);
            if (existingBuilding != null)
            {
                return existingBuilding;
            }
            throw new InvalidOperationException("Квартири з таким id не існує");
        }
       
        public async Task<bool> CreatePropertyAsync(int countRooms, int newBuildingId, float area, byte floor)
        {
            var property = new Property()
            {
                CountRooms = countRooms,
                NewBuildingId = newBuildingId,
                Area = area,
                Floor = floor
            };

            db.Properties.Add(property);
            await db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdatePropertyAsync(int id, int countRooms, int newBuildingId, float area, byte floor)
        {
            var existingProperty = await db.Properties.FirstOrDefaultAsync(b => b.Id == id);
            if (existingProperty != null)
            {
                db.Entry(existingProperty).CurrentValues.SetValues(new Property() { Id = id, CountRooms = countRooms, NewBuildingId = newBuildingId, Area = area, Floor = floor });
                await db.SaveChangesAsync();
                return true;

            }
            throw new InvalidOperationException("Квартири з таким id не знайдено");
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {

            var existingProperty = await db.Properties.FindAsync(id);
            if (existingProperty != null)
            {
                db.Properties.Remove(existingProperty);
                await db.SaveChangesAsync();

                if (db.Database.IsSqlServer())
                {
                    var maxId = db.Properties.Max(c => (int?)c.Id) ?? 0;
                    db.Database.ExecuteSql($"DBCC CHECKIDENT ('Properties', RESEED, {maxId})");
                }
                return true;
            }
            throw new InvalidOperationException("Квартири з таким id не знайдено");
        }
    }
}