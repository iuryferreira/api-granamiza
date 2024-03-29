using System;

namespace APIGranamiza.Models
{
    public class Despesa
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataRemocao { get; set; }
        public bool Debitada { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
    }
}
