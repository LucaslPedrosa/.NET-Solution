using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Aqui você pode usar a string de conexão que está no appsettings.json ou definir diretamente
            optionsBuilder.UseSqlServer("Server=DESKTOP-4NLCO5M;Database=MyDatabase;Trusted_Connection=True;TrustServerCertificate=True;");


            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}