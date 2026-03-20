using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaFinanceiroERP.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // 1. Cria o builder de opções
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // 2. Define a connection string
            var connectionString = "Server=localhost;Port=3306;Database=SistemaFinanceiroERP;User=root;Password=Trakinas123.;";

            // 3. Configura o MySQL
            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );

            // 4. Retorna o AppDbContext usando o construtor SIMPLES (sem TenantProvider)
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}