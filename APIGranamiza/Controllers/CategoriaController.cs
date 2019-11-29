using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGranamiza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

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
        [HttpPost("buscar")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAll([FromBody]UsuarioId usuarioId)
        {
            
            return await contexto.Categoria.Where(c => c.UsuarioId == usuarioId.Id).ToListAsync();
        }

        [Authorize]
        [HttpGet("buscar/receita")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllReceita([FromBody]UsuarioId usuarioId)
        {
            return await contexto.Categoria.Where(c => c.IsGasto == false && c.UsuarioId == usuarioId.Id).ToListAsync();
        }
        
        [Authorize]
        [HttpGet("buscar/despesa")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllDespesa([FromBody]UsuarioId usuarioId)
        {
            return await contexto.Categoria.Where(c => c.IsGasto == true && c.UsuarioId == usuarioId.Id).ToListAsync();
        }
    }
}