using Microsoft.EntityFrameworkCore;
using WeatherPredictor.Models.DbModels;
namespace WeatherPredictor.Context
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<ForecastHistory> ForecastHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
}
