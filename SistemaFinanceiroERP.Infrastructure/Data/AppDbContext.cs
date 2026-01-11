using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ITenantProvider? _tenantProvider;

        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            _tenantProvider = null;
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<LocalEstoque> LocaisEstoque { get; set; }
        public DbSet<ProdutoLocalEstoque> ProdutosLocaisEstoque { get; set; }
        public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<ProdutoLocalEstoque>()
                .HasOne(pl => pl.Produto)
                .WithMany(p => p.ProdutosLocaisEstoque)
                .HasForeignKey(pl => pl.ProdutoId);

            modelBuilder.Entity<ProdutoLocalEstoque>()
                .HasOne(pl => pl.LocalEstoque)
                .WithMany(l => l.ProdutosLocaisEstoque)
                .HasForeignKey(pl => pl.LocalEstoqueId);

            modelBuilder.Entity<ProdutoLocalEstoque>()
                .HasOne(pl => pl.Empresa)
                .WithMany()
                .HasForeignKey(pl => pl.EmpresaId);

            modelBuilder.Entity<LocalEstoque>()
                .HasOne(l => l.Empresa)
                .WithMany()
                .HasForeignKey(l => l.EmpresaId);

            if (_tenantProvider != null)
            {
                modelBuilder.Entity<Produto>()
                    .HasQueryFilter(p => p.EmpresaId == _tenantProvider.GetEmpresaId());

                modelBuilder.Entity<Usuario>()
                    .HasQueryFilter(u => u.EmpresaId == _tenantProvider.GetEmpresaId());

                modelBuilder.Entity<Empresa>()
                    .HasQueryFilter(e => e.Id == _tenantProvider.GetEmpresaId());

                modelBuilder.Entity<LocalEstoque>()
                    .HasQueryFilter(l => l.EmpresaId == _tenantProvider.GetEmpresaId());

                modelBuilder.Entity<ProdutoLocalEstoque>()
                    .HasQueryFilter(pl => pl.EmpresaId == _tenantProvider.GetEmpresaId());



            }
        }
    }
}