using Microsoft.EntityFrameworkCore;

public class ResultadoContext : DbContext {

    public ResultadoContext(DbContextOptions<ResultadoContext> option) : base(option) {

    }

    public DbSet<Resultado> Resultados {get;set;}
    public DbSet<DezenaSorteio> DezenasSorteio {get;set;}

    protected override void OnModelCreating(ModelBuilder builder) {

        builder.Entity<Resultado>().HasKey(r => r.NumeroSorteio);
        builder.Entity<DezenaSorteio>().HasKey(d => d.Id);
        
        builder.Entity<DezenaSorteio>().HasIndex(d => d.NumeroSorteio);

        base.OnModelCreating(builder);
    }
}