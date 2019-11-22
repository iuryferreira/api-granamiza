using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGranamiza.Models;
using APIGranamiza.Utils;
using CryptSharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIGranamiza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly Contexto contexto;

        public UsuarioController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        {
            return await contexto.Usuario.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {

            var usuario = await contexto.Usuario.FindAsync(id);

            if (usuario != null)
            {
                return usuario;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> Adicionar(Usuario usuario)
        {
            AdicionarCategoriasPadrao(usuario);
            usuario.Senha = Crypter.Sha256.Crypt(usuario.Senha);
            usuario.DataCriacao = DateTime.Now;
            contexto.Usuario.Add(usuario);
            await contexto.SaveChangesAsync();
            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioAutenticado>> Autenticar(Login login)
        {
            var usuario = await contexto.Usuario.Where(u => u.Email == login.Email).FirstOrDefaultAsync();
            if (usuario != null)
            {
                if (Crypter.CheckPassword(login.Senha, usuario.Senha))
                {
                    return new UsuarioAutenticado
                    {
                        Id = usuario.Id,
                        Email = usuario.Email,
                        Nome = usuario.Nome,
                        Token = Token.GerarToken(usuario)
                    };
                }
                return BadRequest("Usuário ou senha inválidos");
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Usuario>> Atualizar(int id, Usuario usuario)
        {
            if (id == usuario.Id)
            {
                contexto.Entry(usuario).State = EntityState.Modified;
                try
                {
                    await contexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExiste(id))
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
        public async Task<ActionResult<Usuario>> Remover(int id)
        {
            Usuario usuario = await contexto.Usuario.FindAsync(id);
            if (usuario != null)
            {
                await contexto.SaveChangesAsync();
                return usuario;
            }
            return NotFound();
        }
        private bool UsuarioExiste(int id)
        {
            return contexto.Usuario.Any(u => u.Id == id);
        }
        private void AdicionarCategoriasPadrao(Usuario usuario)
        {
            var categoriaReceita = new Categoria { Nome = "Geral", IsGasto = false, UsuarioId = usuario.Id, Usuario = usuario };
            var categoriaDespesa = new Categoria { Nome = "Geral", IsGasto = true, UsuarioId = usuario.Id, Usuario = usuario };
            contexto.Categoria.Add(categoriaDespesa);
            contexto.Categoria.Add(categoriaReceita);
        }
    }

}