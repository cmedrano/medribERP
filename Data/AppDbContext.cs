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
        public DbSet<Localidad> Localidades { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<PriceList> PriceList { get; set; }
        public DbSet<Provider> Provider { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<ProductCategory> Product_Category { get; set; }
        public DbSet<AreasPerUser> AreasPerUser { get; set; }
        public DbSet<ArticulosPrecios> ArticulosPrecios { get; set; }
        //public DbSet<Periodo> Periodos { get; set; }
        public DbSet<Year> Year { get; set; }
        public DbSet<Month> Months { get; set; }
        public DbSet<PeriodoResumen> PeriodoResumenes { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Company> Companies { get; set; }


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

            modelBuilder.Entity<PriceList>(entity =>
            {
                entity.ToTable("listas_precios");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Activo).HasColumnName("activo");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<PeriodoResumen>(entity =>
            {
                entity.ToTable("periods");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Fecha).HasColumnName("fecha");
                entity.Property(e => e.ValorPresupuestado).HasColumnName("valor_presupuestado");
                entity.Property(e => e.TotalGastos)
                        .HasColumnName("total_gastos")
                        .HasColumnType("numeric(18,2)")
                        .HasDefaultValue(0);
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.ToTable("provincias");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id");

                entity.Property(x => x.Nombre)
                    .HasColumnName("nombre")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.DateInserted)
                    .HasColumnName("date_inserted")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Configuración de la relación con Localidad
                entity.HasMany(x => x.Localidades)
                    .WithOne(x => x.Provincia)
                    .HasForeignKey(x => x.ProvinciaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Localidad>(entity =>
            {
                entity.ToTable("localidades");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("id");

                entity.Property(x => x.Nombre)
                    .HasColumnName("nombre")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.CodigoPostal)
                    .HasColumnName("codigo_postal")
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(x => x.ProvinciaId)
                    .HasColumnName("provincia_id");

                entity.Property(x => x.DateInserted)
                    .HasColumnName("date_inserted")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("sales");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ClientId).HasColumnName("client_id");
                entity.Property(e => e.NameClient).HasColumnName("name_client");
                entity.Property(e => e.DNI).HasColumnName("dni");
                entity.Property(e => e.PriceListId).HasColumnName("price_list_id");
                entity.Property(e => e.Subtotal)
                        .HasColumnName("subtotal")
                        .HasColumnType("numeric(18,2)")
                        .HasDefaultValue(0);
                entity.Property(e => e.Descuento)
                        .HasColumnName("descuento")
                        .HasColumnType("numeric(18,2)")
                        .HasDefaultValue(0);
                entity.Property(e => e.Total)
                        .HasColumnName("total")
                        .HasColumnType("numeric(18,2)")
                        .HasDefaultValue(0);
                entity.Property(e => e.DateInserted)
                        .HasColumnName("date_inserted")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<SaleDetail>(entity =>
            {
                entity.ToTable("sales_details");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.SaleId).HasColumnName("sale_id");
                entity.Property(e => e.ItemId).HasColumnName("item_id");
                entity.Property(e => e.CodeItem).HasColumnName("code_item");
                entity.Property(e => e.NameItem).HasColumnName("name_item");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.PrecioUnitario)
                        .HasColumnName("precio_unitario")
                        .HasColumnType("numeric(18,2)")
                        .HasDefaultValue(0);
                entity.Property(e => e.Total)
                        .HasColumnName("total")
                        .HasColumnType("numeric(18,2)")
                        .HasDefaultValue(0);

                // Configuración de la relación con Sale
                entity.HasOne(sd => sd.Sale)
                        .WithMany(s => s.Detail)
                        .HasForeignKey(sd => sd.SaleId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CompanyName)
                    .HasColumnName("CompanyName")
                    .HasMaxLength(200);

                entity.Property(e => e.Street)
                    .HasColumnName("Street")
                    .HasMaxLength(200);

                entity.Property(e => e.StreetNumber)
                    .HasColumnName("StreetNumber");

                entity.Property(e => e.FloorOrApartment)
                    .HasColumnName("FloorOrApartment")
                    .HasMaxLength(100);

                entity.Property(e => e.Locality)
                    .HasColumnName("Locality")
                    .HasMaxLength(150);

                entity.Property(e => e.Province)
                    .HasColumnName("Province")
                    .HasMaxLength(150);

                entity.Property(e => e.Country)
                    .HasColumnName("Country")
                    .HasMaxLength(150);

                entity.Property(e => e.CP)
                    .HasColumnName("CP");

                entity.Property(e => e.Phone)
                    .HasColumnName("Phone")
                    .HasMaxLength(50);

                entity.Property(e => e.CUIT)
                    .HasColumnName("CUIT")
                    .HasMaxLength(20);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("CreateDate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });




        }

    }
}
