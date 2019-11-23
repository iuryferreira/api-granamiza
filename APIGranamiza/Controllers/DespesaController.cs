using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGranamiza.Models;
using APIGranamiza.Utils;
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
            Include(d => d.Categoria).
            Where(d => d.DataRemocao == null && d.UsuarioId == usuarioId)
            .ToListAsync();
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Despesa>> GetDespesa(int id)
        {
            var despesa = await contexto.Despesa.
            Include(d => d.Categoria).
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
        [HttpGet("total-despesas")]
        public async Task<ActionResult<decimal>> GetTotalDespesas(int usuarioId)
        {
            return await contexto.Despesa.
                Where(d => d.DataRemocao == null &&
                d.Debitada == false &&
                d.UsuarioId == usuarioId).
                SumAsync(d => d.Valor);
        }
        [Authorize]
        [HttpGet("total-despesas-pagas")]
        public async Task<ActionResult<decimal>> GetTotalDespesasPagas(int usuarioId)
        {
            return await contexto.Despesa.
                Where(d => d.DataRemocao == null &&
                d.Debitada == true &&
                d.UsuarioId == usuarioId).
                SumAsync(d => d.Valor);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Despesa>> Adicionar(Despesa despesa)
        {
            if (despesa.CategoriaId == null)
            {
                int categoriaId = CategoriaUtils.AdicionarCategoria(despesa.Categoria);
                despesa.CategoriaId = categoriaId;
            }
            despesa.DataCriacao = DateTime.Now;
            contexto.Despesa.Add(despesa);
            await contexto.SaveChangesAsync();
            return CreatedAtAction("GetDespesa", new { id = despesa.Id }, despesa);
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
                    await contexto.SaveChangesAsync();
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
                await contexto.SaveChangesAsync();
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