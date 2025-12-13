using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; } 
        public DbSet<Produto> Produtos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       
    }

}
