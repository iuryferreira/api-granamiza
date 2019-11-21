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

    }
}