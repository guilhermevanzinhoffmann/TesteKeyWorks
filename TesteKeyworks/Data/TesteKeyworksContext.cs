using Microsoft.EntityFrameworkCore;
using TesteKeyworks.Models;

namespace TesteKeyworks.Data
{
    public class TesteKeyworksContext : DbContext
    {
        public TesteKeyworksContext (DbContextOptions<TesteKeyworksContext> options)
            : base(options)
        {
        }

        public DbSet<TesteKeyworks.Models.Filme> Filme { get; set; } = default!;
        public DbSet<TesteKeyworks.Models.Streaming> Streaming { get; set; } = default!;
        public DbSet<FilmesStreamings> FilmesStreamings { get; set; }
        public DbSet<Avaliacao> Avaliacoess { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<FilmesStreamings>()
                .HasKey(c => new { c.StreamingId, c.FilmeId });

            modelBuilder.Entity<Filme>()
                .HasIndex(x => x.Titulo)
                .IsUnique();

            modelBuilder.Entity<Filme>()
                .HasMany(x => x.Streamings)
                .WithMany(x => x.Filmes)
                .UsingEntity<FilmesStreamings>();

            modelBuilder.Entity<Streaming>()
                .HasIndex(x => x.Nome)
                .IsUnique();
        }
    }
}
