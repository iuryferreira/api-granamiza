using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGranamiza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIGranamiza.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DespesaController : ControllerBase
    {

        private readonly Contexto contexto;

        public DespesaController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Despesa>>> GetAll(int usuarioId)
        {
            return await contexto.Despesa.
            Where(d => d.DataRemocao == null && d.UsuarioId == usuarioId)
            .ToListAsync();
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Despesa>> GetDespesa(int id)
        {
            var despesa = await contexto.Despesa.
            Where(d => d.DataRemocao == null && d.Id == id).FirstOrDefaultAsync();

            if (despesa != null)
            {
                return despesa;
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Despesa>> Adicionar(Despesa despesa)
        {
            despesa.DataCriacao = DateTime.Now;
            contexto.Despesa.Add(despesa);
            await contexto.SaveChangesAsync().ConfigureAwait(false);
            return CreatedAtAction("GetDespesa", new { usuarioId = despesa.UsuarioId, id = despesa.Id }, despesa);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Usuario>> Atualizar(int id, Despesa despesa)
        {
            if (id == despesa.Id)
            {
                contexto.Entry(despesa).State = EntityState.Modified;
                try
                {
                    await contexto.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DespesaExiste(id))
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
        public async Task<ActionResult<Despesa>> Remover(int id)
        {
            Despesa despesa = await contexto.Despesa.FindAsync(id);
            if (despesa != null)
            {
                despesa.DataRemocao = DateTime.Now;
                await contexto.SaveChangesAsync().ConfigureAwait(false);
                return despesa;
            }
            return NotFound();
        }
        private bool DespesaExiste(int id)
        {
            return contexto.Despesa.Any(d => d.Id == id);
        }
    }
}