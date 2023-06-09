using APIModallportV5.Dao;
using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIModallportV5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RespostasController : Controller
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        public RespostasController(LogService logService, OracleConnection connection)
        {
            _logService = logService;
            _connection = connection;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            try
            {
                var Dao = new DaoRespostas(_logService, _connection);
                var respostas = Dao.ListaRespostas();

                return new JsonResult(respostas);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Post(int idItem, [FromBody] RespostaModel respostaModel)
        {
            try
            {
                if (idItem <= 0)
                {
                    return new JsonResult("ID do item inválido");
                }

                var validationContext = new ValidationContext(respostaModel, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(respostaModel, validationContext, validationResults, validateAllProperties: true);

                if (!isValid)
                {
                    return new JsonResult("Dados inválidos. Verifique os campos fornecidos.");
                }

                var Dao = new DaoRespostas(_logService, _connection);
                var resposta = Dao.PostResposta(idItem, respostaModel);

                return new JsonResult(resposta);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("POST", "Dados de RESPOSTAS inseridos.");
                return new JsonResult(ex.Message);
            }
        }
    }
}
