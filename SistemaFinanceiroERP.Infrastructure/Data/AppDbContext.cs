using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Application.Interfaces;
using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;
        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider) : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>()
                .HasQueryFilter(p => p.EmpresaId == _tenantProvider.GetEmpresaId());

            modelBuilder.Entity<Usuario>()
                .HasQueryFilter(u => u.EmpresaId == _tenantProvider.GetEmpresaId());

            modelBuilder.Entity<Empresa>()
                .HasQueryFilter(e => e.Id == _tenantProvider.GetEmpresaId());
        }
    }

}
