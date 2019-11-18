using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGranamiza.Models;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receita>>> GetAll()
        {
            return await context.Receita.
                // [TAG - add referência de usuário] Receitas do usuario
                 Where(r => r.DataRemocao == null /* && r.UsuarioId == 1*/).
                 ToListAsync().ConfigureAwait(false);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Receita>> GetReceita(int id)
        {
            Receita receita = await context.Receita.
                // [TAG - add referência de usuário] Receita do usuario
                Include(r => r.Usuario).
                Where(r => r.Id == id).
                FirstOrDefaultAsync().
                ConfigureAwait(false);

            if (receita != null)
            {
                return receita;
            }

            return NotFound();
        }

        [HttpGet("total")]
        public async Task<ActionResult<decimal>> GetSaldoAnual(int ano)
        {
            return await context.Receita.
                // [TAG - add referência de usuário] Receitas do usuario
                Where(r => r.UsuarioId == 1).
                SumAsync(r => r.Valor).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ActionResult<Receita>> Adicionar(Receita receita)
        {
            receita.DataCriacao = DateTime.Now;
            context.Receita.Add(receita);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return CreatedAtAction("GetReceita", new { id = receita.Id }, receita);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Receita>> Atualizar(int id, Receita receita)
        {
            if (id == receita.Id)
            {
                //receita.AtualizadoEm = DateTime.Now;
                context.Entry(receita).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync().ConfigureAwait(false);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<Receita>> Remover(int id)
        {
            Receita receita = await context.Receita.FindAsync(id);
            if (receita != null)
            {
                receita.DataRemocao = DateTime.Now;
                await context.SaveChangesAsync().ConfigureAwait(false);
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