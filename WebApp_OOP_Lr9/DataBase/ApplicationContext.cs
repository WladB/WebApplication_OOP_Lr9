using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Proxies;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApp_OOP_Lr9.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<NewBuilding> NewBuildings => Set<NewBuilding>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<FinancingOption> FinancingOptions => Set<FinancingOption>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
    }
}
