using System;
using Microsoft.EntityFrameworkCore;


namespace APIGranamiza.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions options) : base(options){}
        DbSet<Usuario> Usuario { get; set; }
        DbSet<Receita> Receita { get; set; }
        DbSet<Despesa> Despesa { get; set; }
        DbSet<Categoria> Categoria { get; set; }

    }
}