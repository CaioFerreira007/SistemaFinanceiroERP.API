using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;

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

        // Design-time / migrations
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
        public DbSet<AjusteEstoque> AjustesEstoque { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // RELACIONAMENTOS
            // =========================
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

            // =========================
            // ✅ FAIL-SAFE TENANT ID
            // Se não tem usuário/token (login/register), empresaId = 0
            // Assim NÃO EXPLODE o model building.
            // =========================
            var empresaId = GetEmpresaIdSafe();

            // =========================
            // QUERY FILTERS (Multi-tenant + Soft Delete)
            // =========================
            modelBuilder.Entity<Produto>()
                .HasQueryFilter(p => p.EmpresaId == empresaId && p.Ativo);

            modelBuilder.Entity<Usuario>()
                .HasQueryFilter(u => u.EmpresaId == empresaId && u.Ativo);

            modelBuilder.Entity<Empresa>()
                .HasQueryFilter(e => e.Id == empresaId && e.Ativo);

            modelBuilder.Entity<LocalEstoque>()
                .HasQueryFilter(l => l.EmpresaId == empresaId && l.Ativo);

            modelBuilder.Entity<ProdutoLocalEstoque>()
                .HasQueryFilter(pl => pl.EmpresaId == empresaId && pl.Ativo);

            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasQueryFilter(m => m.EmpresaId == empresaId && m.Ativo);

            modelBuilder.Entity<AjusteEstoque>()
                .HasQueryFilter(a => a.EmpresaId == empresaId && a.Ativo);
        }

        private int GetEmpresaIdSafe()
        {
            try
            {
                if (_tenantProvider == null) return 0;
                return _tenantProvider.GetEmpresaId();
            }
            catch
            {
                // Sem token / sem usuário autenticado
                return 0;
            }
        }

        // =========================
        // AUDITORIA CENTRALIZADA UTC
        // =========================
        public override int SaveChanges()
        {
            ApplyAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAudit()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DataCriacao = now;
                    entry.Entity.DataAtualizacao = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(e => e.DataCriacao).IsModified = false;
                    entry.Entity.DataAtualizacao = now;
                }
            }
        }
    }
}
