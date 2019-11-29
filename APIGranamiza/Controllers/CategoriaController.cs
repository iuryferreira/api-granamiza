using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGranamiza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIGranamiza.Controllers{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly Contexto contexto;
        public CategoriaController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAll([FromBody] int usuarioId){
            
            return await contexto.Categoria.Where(c => c.UsuarioId == usuarioId).ToListAsync();
        }

        [Authorize]
        [HttpGet("receita")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllReceita([FromBody] int usuarioId)
        {
            return await contexto.Categoria.Where(c => c.IsGasto == false && c.UsuarioId == usuarioId).ToListAsync();
        }
        
        [Authorize]
        [HttpGet("despesa")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllDespesa([FromBody] int usuarioId)
        {
            return await contexto.Categoria.Where(c => c.IsGasto == true && c.UsuarioId == usuarioId).ToListAsync();
        }
    }
}