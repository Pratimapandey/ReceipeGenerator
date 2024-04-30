using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReceipeGenerator.Model;
using Microsoft.AspNetCore.Identity;

namespace ReceipeGenerator.Data
{
    public class ReceipeDbContext : IdentityDbContext<ApplicationUser>
    {
        public ReceipeDbContext(DbContextOptions<ReceipeDbContext> options) : base(options)
        {

        }
        public DbSet<Ingredient> Ingredients { get; set; }
       
        public DbSet<Receipe> Receipes { get; set; }
        // Modify ReceipeDbContext to include DbSet for Festival
        public DbSet<Festival> Festivals { get; set; }
        public DbSet<ReceipeFestival> ReceipeFestivals { get; set; }
        //public DbSet<CreateRecipeRequest> CreateRecipeRequests { get; set; }
       public DbSet<RecipeCategory> RecipeCategories { get; set; }





        //public DbSet<IngredientWithMeasurement> IngredientWithMeasurements { get; set; }

    }
}
