using Microsoft.EntityFrameworkCore;
using SistemaDeGerenciamentoDePacientes.Models;

namespace SistemaDeGerenciamentoDePacientes
{
    public class ConfigDbContext : DbContext
    {
        public ConfigDbContext(DbContextOptions<ConfigDbContext> options) : base(options) { }

        DbSet<Produtos> Produtos { get; set; }
        DbSet<Usuario> Usuario { get; set; }
    }
}
