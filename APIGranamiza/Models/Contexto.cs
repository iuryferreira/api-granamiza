using System;
using Microsoft.EntityFrameworkCore;


namespace APIGranamiza.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions options) : base(options){}
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Receita> Receita { get; set; }
        public DbSet<Despesa> Despesa { get; set; }
        public DbSet<Categoria> Categoria { get; set; }

    }
}