using System;
using System.Collections.Generic;

namespace APIGranamiza.Models
{
    public class UsuarioAutenticado
    {
        
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
