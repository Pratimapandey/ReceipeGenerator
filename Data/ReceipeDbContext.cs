using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReceipeGenerator.Model;

namespace ReceipeGenerator.Data
{
    public class ReceipeDbContext : IdentityDbContext<ApplicationUser>
    {
        public ReceipeDbContext(DbContextOptions<ReceipeDbContext> options) : base(options)
        {

        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Receipe> Receipes { get; set; }
        public DbSet<Festival> Festivals { get; set; }
        public DbSet<ReceipeFestival> ReceipeFestivals { get; set; }
        public DbSet<RecipeCategory> RecipeCategories { get; set; }
    }
}
