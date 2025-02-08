using Api_BD.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_BD.Context
{
    public class UserContext : DbContext
    {


        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<UserModels> Users { get; set; }
        // Configuración adicional del modelo al crear la base de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configura un índice único en la propiedad 'Id' de TransactionsModel
            modelBuilder.Entity<UserModels>().HasIndex(c => c.Id).IsUnique();
        }
    }
}
