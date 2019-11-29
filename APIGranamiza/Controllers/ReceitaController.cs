using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGranamiza.Models;
using APIGranamiza.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIGranamiza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceitaController : ControllerBase
    {
        private readonly Contexto context;

        public ReceitaController(Contexto context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receita>>> GetAll([FromBody]int usuarioId)
        {
            return await context.Receita.
                Include(r => r.Categoria).
                Where(r => r.DataRemocao == null && r.UsuarioId == usuarioId).
                ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Receita>> GetReceita(int id)
        {
            Receita receita = await context.Receita.
                Include(r => r.Categoria).
                Where(r => r.DataRemocao == null && r.Id == id).
                FirstOrDefaultAsync();

            if (receita != null)
            {
                return receita;
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("total-receitas")]
        public async Task<ActionResult<decimal>> GetReceitaTotal(int usuarioId)
        {
            return await context.Receita.
                Where(r => r.DataRemocao == null && r.UsuarioId == usuarioId).
                SumAsync(r => r.Valor);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Receita>> Adicionar(Receita receita)
        {
            if (receita.CategoriaId == null)
            {
                int categoriaId = CategoriaUtils.AdicionarCategoria(receita.Categoria);
                receita.CategoriaId = categoriaId;
            }
            receita.DataCriacao = DateTime.Now;
            context.Receita.Add(receita);
            await context.SaveChangesAsync();
            return CreatedAtAction("GetReceita", new { id = receita.Id }, receita);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Receita>> Atualizar(int id, Receita receita)
        {
            if (id == receita.Id)
            {
                context.Entry(receita).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!ExisteReceita(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }

            return BadRequest();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Receita>> Remover(int id)
        {
            Receita receita = await context.Receita.FindAsync(id);
            if (receita != null)
            {
                receita.DataRemocao = DateTime.Now;
                await context.SaveChangesAsync();
                return receita;
            }
            return NotFound();
        }

        private bool ExisteReceita(int id)
        {
            return context.Receita.Any(d => d.Id == id);
        }
    }
}