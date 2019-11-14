using System;

class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool IsGasto { get; set; }
    public int UsuarioId { get; set; }
    public virtual Usuario Usuario { get; set; }
}