using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Budget> Budget { get; set; }
        public DbSet<RubroType> RubroType { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Diary> Diary { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Income> Income { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<LocalidadPostal> localidades_postales { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<PriceList> PriceList { get; set; }
        public DbSet<Provider> Provider { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<ProductCategory> Product_Category { get; set; }
        public DbSet<AreasPerUser> AreasPerUser { get; set; }
        public DbSet<ArticulosPrecios> ArticulosPrecios { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<Year> Year { get; set; }
        public DbSet<Month> Months { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<RubroType>()
                .HasOne(r => r.RubroPadre)
                .WithMany(r => r.SubRubros)
                .HasForeignKey(r => r.RubroPadreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Income>()
                .HasOne(t => t.FromAccount)
                .WithMany()
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Income>()
                .HasOne(t => t.ToAccount)
                .WithMany()
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración para LocalidadPostal
            //modelBuilder.Entity<LocalidadPostal>()
            //    //.HasOne(l => l.Provincia)
            //    .WithMany()
            //    .HasForeignKey(l => l.IdProvincia)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PriceList>(entity =>
            {
                entity.ToTable("listas_precios"); 

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Activo).HasColumnName("activo");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });
        }

    }
}
