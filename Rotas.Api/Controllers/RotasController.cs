using Microsoft.AspNetCore.Mvc;
using Rotas.Api.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rotas.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RotasController : ControllerBase
    {

        private readonly AppDbContext _context;

        public RotasController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<List<entities.Rota>>> GetRotas() 
        {
            return biz.Rota.GetAll();
        }

        [HttpGet("Find")]
        public async Task<ActionResult<entities.Rota>> Find(string origem, string destino)
        {
            return biz.Rota.Find(origem, destino);
        }

        [HttpGet("FindAll")]
        public async Task<ActionResult<List<entities.Rota>>> FindAll(string origem, string destino)
        {
            return biz.Rota.FindAll(origem, destino);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<string>> Create(entities.Rota rota)
        {
            string response = string.Empty;
            var result = biz.Rota.Save(rota);

            if (result)
                response = "Viagem cadastrada com sucesso!";
            else
                response = "Ocoreu um erro ao cadastrar a viagem, por favor entre em contato com o administrador!";
            return response;
        }

        [HttpPut("Update")]
        public async Task<ActionResult<string>> Update(entities.Rota rota)
        {
            string response = string.Empty;
            var result = biz.Rota.Update(rota);

            if (result)
                response = "Viagem atualizada com sucesso!";
            else
                response = "Ocoreu um erro ao atualizar a viagem, por favor entre em contato com o administrador!";
            return response;
        }


        [HttpDelete("Delete")]
        public async Task<ActionResult<string>> Delete(int Id)
        {
            string response = string.Empty;
            var result = biz.Rota.Delete(Id);

            if (result)
                response = "Cadastro da viagem deleta com sucesso!";
            else
                response = "Ocoreu um erro ao deletar a viagem, por favor entre em contato com o administrador!";
            return response;
        }

    }
}
