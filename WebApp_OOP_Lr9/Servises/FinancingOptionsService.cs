using Microsoft.EntityFrameworkCore;
using WebApp_OOP_Lr9.DataBase;

namespace WebApp_OOP_Lr9.Servises
{
    public interface IFinancingOptionsService
    {
        Task<List<FinancingOption>> GetAllFinancingOptionsAsync(int buildId);
        Task<FinancingOption> GetFinancingOptionAsync(int id);
        Task<bool> CreateFinancingOptionAsync(string сaption, string description, int newBuildingId);
        Task<bool> UpdateFinancingOptionAsync(int id, string сaption, string description, int newBuildingId);
        Task<bool> DeleteFinancingOptionAsync(int id);
    }
    public class FinancingOptionsService : IFinancingOptionsService
    {
        private readonly ApplicationContext db;
        public FinancingOptionsService(ApplicationContext context)
        {
            db = context;
        }
        public async Task<List<FinancingOption>> GetAllFinancingOptionsAsync(int buildId)
        {
            var existingBuilding = await db.FinancingOptions.Where(p => p.NewBuildingId == buildId).ToListAsync();
            if (existingBuilding != null)
            {
                return existingBuilding;
            }
            throw new InvalidOperationException("Квартири з таким id не існує");
        }

        public async Task<FinancingOption> GetFinancingOptionAsync(int id)
        {
            var existingBuilding = await db.FinancingOptions.FirstOrDefaultAsync(c => c.Id == id);
            if (existingBuilding != null)
            {
                return existingBuilding;
            }
            throw new InvalidOperationException("Квартири з таким id не існує");
        }

        public async Task<bool> CreateFinancingOptionAsync(string сaption, string description, int newBuildingId)
        {
            var financingOption = new FinancingOption(){
                Caption = сaption,
                Description = description,
                NewBuildingId = newBuildingId
            };

            db.FinancingOptions.Add(financingOption);
            await db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateFinancingOptionAsync(int id, string сaption, string description, int newBuildingId)
        {
            var existingFinancingOption = await db.FinancingOptions.FirstOrDefaultAsync(b => b.Id == id);
            if (existingFinancingOption != null)
            {
                db.Entry(existingFinancingOption).CurrentValues.SetValues(new FinancingOption() { 
                    Id = id,
                    Caption = сaption,
                    Description = description,
                    NewBuildingId = newBuildingId 
                });
                await db.SaveChangesAsync();
                return true;

            }
            throw new InvalidOperationException("Квартири з таким id не знайдено");
        }

        public async Task<bool> DeleteFinancingOptionAsync(int id)
        {

            var existingFinancingOption = await db.FinancingOptions.FindAsync(id);
            if (existingFinancingOption != null)
            {
                db.FinancingOptions.Remove(existingFinancingOption);
                await db.SaveChangesAsync();

                if (db.Database.IsSqlServer())
                {
                    var maxId = db.FinancingOptions.Max(c => (int?)c.Id) ?? 0;
                    db.Database.ExecuteSql($"DBCC CHECKIDENT ('FinancingOptions', RESEED, {maxId})");
                }
                return true;
            }
            throw new InvalidOperationException("Квартири з таким id не знайдено");
        }
    }
}
