namespace Actualizador_Precios.Data
{
    using Actualizador_Precios.Models;
    using Microsoft.EntityFrameworkCore;
    public class AppDBContext : DbContext
    {
        public string ConnectionString { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        //DBSETS
        public DbSet<AlertaCambioCosto> AlertaCambioCosto { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PrecioProducto>()
                .HasKey(p => new { p.CVE_ART, p.CVE_PRECIO });

            modelBuilder.Entity<PrecioProducto>()
                .HasOne(p => p.ListaPrecio)
                .WithMany()
                .HasForeignKey(p => p.CVE_PRECIO);
        }

    }
}
