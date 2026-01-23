using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;

namespace SistemaFinanceiroERP.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ITenantProvider? _tenantProvider;

        public int CurrentEmpresaId { get; private set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
            CurrentEmpresaId = TryGetEmpresaIdOrZero();
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            _tenantProvider = null;
            CurrentEmpresaId = 0;
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<LocalEstoque> LocaisEstoque { get; set; }
        public DbSet<ProdutoLocalEstoque> ProdutosLocaisEstoque { get; set; }
        public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; }
        public DbSet<AjusteEstoque> AjusteEstoque { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<ItemTransacao> ItensTransacao { get; set; }

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

            modelBuilder.Entity<Transacao>()
            .HasOne(t => t.EmpresaVendedora)
            .WithMany()
            .HasForeignKey(t => t.EmpresaVendedoraId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.EmpresaCompradora)
                .WithMany()
                .HasForeignKey(t => t.EmpresaCompradoraId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.Usuario)
                .WithMany()
                .HasForeignKey(t => t.UsuarioId);

            modelBuilder.Entity<ItemTransacao>()
            .HasOne(it => it.Transacao)
            .WithMany(t => t.ItemsTransacao)
            .HasForeignKey(it => it.TransacaoId);

            modelBuilder.Entity<ItemTransacao>()
                .HasOne(it => it.Produto)
                .WithMany()
                .HasForeignKey(it => it.ProdutoId);

            modelBuilder.Entity<Produto>()
                .HasQueryFilter(p => (CurrentEmpresaId == 0 || p.EmpresaId == CurrentEmpresaId) && p.Ativo);

            modelBuilder.Entity<Usuario>()
                .HasQueryFilter(u => (CurrentEmpresaId == 0 || u.EmpresaId == CurrentEmpresaId) && u.Ativo);

            modelBuilder.Entity<Empresa>()
                .HasQueryFilter(e => (CurrentEmpresaId == 0 || e.Id == CurrentEmpresaId) && e.Ativo);

            modelBuilder.Entity<LocalEstoque>()
                .HasQueryFilter(l => (CurrentEmpresaId == 0 || l.EmpresaId == CurrentEmpresaId) && l.Ativo);

            modelBuilder.Entity<ProdutoLocalEstoque>()
                .HasQueryFilter(pl => (CurrentEmpresaId == 0 || pl.EmpresaId == CurrentEmpresaId) && pl.Ativo);

            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasQueryFilter(m => (CurrentEmpresaId == 0 || m.EmpresaId == CurrentEmpresaId) && m.Ativo);

            modelBuilder.Entity<AjusteEstoque>()
                .HasQueryFilter(a => (CurrentEmpresaId == 0 || a.EmpresaId == CurrentEmpresaId) && a.Ativo);

            modelBuilder.Entity<Transacao>()
                .HasQueryFilter(t => (CurrentEmpresaId == 0 || t.EmpresaCompradoraId == CurrentEmpresaId || t.EmpresaVendedoraId == CurrentEmpresaId) && t.Ativo);
            modelBuilder.Entity<ItemTransacao>()
                .HasQueryFilter(it => (CurrentEmpresaId == 0 || it.Transacao.EmpresaCompradoraId == CurrentEmpresaId || it.Transacao.EmpresaVendedoraId == CurrentEmpresaId) && it.Ativo);
        }

        public override int SaveChanges()
        {
            ApplyAuditUtc();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditUtc();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditUtc()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DataCriacao = now;
                    entry.Entity.DataAtualizacao = now;
                    entry.Entity.Ativo = true;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.DataAtualizacao = now;
                }
            }
        }

        private int TryGetEmpresaIdOrZero()
        {
            try
            {
                return _tenantProvider?.GetEmpresaId() ?? 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}