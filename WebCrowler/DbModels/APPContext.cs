using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrowler.DbModels;

namespace WebCrawler.DbModels
{
    class APPContext: DbContext
    {
        public APPContext() : base("PPGastronomyAssistance")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<APPContext, WebCrowler.Migrations.Configuration>());
        }


        public DbSet<Restaurants> Restaurant { get; set; }
        public DbSet<Ingredients> Ingredient { get; set; }
        public DbSet<Keys> Key { get; set; }
        public DbSet<GoogleResults>  GoogleResult { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Restaurants>()
            .HasMany<Ingredients>(s => s.Ingredients)
            .WithMany(c => c.Restaurants)
            .Map(cs =>
            {
                cs.MapLeftKey("RestaurantRefId");
                cs.MapRightKey("IngredientRefId");
                cs.ToTable("RestaurantsIngredients");
            });

        }
    }
}
