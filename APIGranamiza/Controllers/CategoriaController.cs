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
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAll(){
            return await contexto.Categoria.ToListAsync();
        }

        [Authorize]
        [HttpGet("receita")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllReceita(){
            return await contexto.Categoria.Where(c => c.IsGasto == false).ToListAsync();
        }
        
        [Authorize]
        [HttpGet("despesa")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllDespesa(){
            return await contexto.Categoria.Where(c => c.IsGasto == true).ToListAsync();
        }
    }
}