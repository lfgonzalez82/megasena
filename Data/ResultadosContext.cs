using Microsoft.EntityFrameworkCore;

public class ResultadoContext : DbContext {

    public ResultadoContext(DbContextOptions<ResultadoContext> option) : base(option) {

    }

    public DbSet<Resultado> Resultados {get;set;}

    protected override void OnModelCreating(ModelBuilder builder) {

        builder.Entity<Resultado>().HasKey(r => r.NumeroSorteio);

        base.OnModelCreating(builder);
    }
}