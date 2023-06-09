using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using APIModallportV5.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using APIModallportV5.Dao;
using APIModallportV5;
using System.ComponentModel;

namespace APIModallPortV5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessosController : ControllerBase
    {

        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        public ProcessosController(LogService logService, OracleConnection connection)
        {
            _logService = logService;
            _connection = connection;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            try
            {
                var Dao = new DaoProcessos(_logService, _connection);
                var processos = Dao.ListaProcessos();

                return new JsonResult(processos);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Post([FromBody] ProcessoModel processoModel)
        {
            try
            {
                var validationContext = new ValidationContext(processoModel, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(processoModel, validationContext, validationResults, validateAllProperties: true);

                if (!isValid)
                {
                    return new JsonResult("Dados inválidos. Verifique os campos fornecidos.");
                }

                var Dao = new DaoProcessos(_logService, _connection);
                var retorno = Dao.PostProcesso(processoModel);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("{idProcesso}")]
        public JsonResult Put(int idProcesso, [FromBody] ProcessoModel processoModel)
        {
            try
            {
                if (idProcesso <= 0)
                {
                    return new JsonResult("ID do processo inválido");
                }

                var validationContext = new ValidationContext(processoModel, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(processoModel, validationContext, validationResults, validateAllProperties: true);

                if (!isValid)
                {
                    return new JsonResult("Dados inválidos. Verifique os campos fornecidos.");
                }

                var Dao = new DaoProcessos(_logService, _connection);
                var retorno = Dao.AlteraProcesso(idProcesso, processoModel);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpDelete("{idProcesso}")]
        public JsonResult Delete(int idProcesso)
        {
            try
            {
                if (idProcesso <= 0)
                {
                    return new JsonResult("ID do processo inválido");
                }

                var Dao = new DaoProcessos(_logService, _connection);
                var retorno = Dao.DeletaProcesso(idProcesso);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("DELETE", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }
    }
}
