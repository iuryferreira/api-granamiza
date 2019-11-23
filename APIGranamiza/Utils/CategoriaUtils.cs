using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGranamiza.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGranamiza.Utils
{
    public static class CategoriaUtils
    {
        private static readonly Contexto contexto;

        public static int AdicionarCategoria(Categoria categoria)
        {
            contexto.Categoria.Add(categoria);
            try
            {
                _ = contexto.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

                throw;
            }

            return categoria.Id;
        }
    }
}
