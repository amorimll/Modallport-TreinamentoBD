using APIModallportV5.Dao;
using APIModallportV5.Model;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;

namespace APIModallportV5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VistoriasController : Controller
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        public VistoriasController(LogService logService, OracleConnection connection)
        {
            _logService = logService;
            _connection = connection;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            try
            {
                var Dao = new DaoVistorias(_logService, _connection);
                var vistorias = Dao.ListaVistorias();

                return new JsonResult(vistorias);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet("/api/Vistorias/id/{idVistoria}")]
        public JsonResult GetOneVistoria(int idVistoria)
        {
            try
            {

                if (idVistoria <= 0)
                {
                    return new JsonResult("ID do processo inválido");
                }


                var Dao = new DaoVistorias(_logService, _connection);
                var retorno = Dao.ListaVistoriasById(idVistoria);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet("itens/{idVistoria}")]
        public JsonResult GetByVistoriaId(int idVistoria)
        {
            try
            {

                if (idVistoria <= 0)
                {
                    return new JsonResult("ID do processo inválido");
                }


                var Dao = new DaoVistorias(_logService, _connection);
                var retorno = Dao.ListaVistoriasItens(idVistoria);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }


        [HttpGet("{idProcesso}")]
        public JsonResult GetByrocessoId(int idProcesso)
        {
            try
            {

                if (idProcesso <= 0)
                {
                    return new JsonResult("ID do processo inválido");
                }


                var Dao = new DaoVistorias(_logService, _connection);
                var retorno = Dao.ListaVistoriasProcesso(idProcesso);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Post([FromBody] VistoriaModel vistoriaModel)
        {
            try
            {
                // Validar o modelo de vistoria usando anotações de validação
                var validationContext = new ValidationContext(vistoriaModel, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(vistoriaModel, validationContext, validationResults, validateAllProperties: true);

                if (!isValid)
                {
                    return new JsonResult("Dados inválidos. Verifique os campos fornecidos.");
                }

                var Dao = new DaoVistorias(_logService, _connection);
                var retorno = Dao.PostVistoria(vistoriaModel);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("{idVistoria}")]
        public JsonResult Put(int idVistoria, [FromBody] VistoriaModel vistoriaModel)
        {
            try
            {
                if (idVistoria <= 0)
                {
                    return new JsonResult("ID da vistoria inválido");
                }

                // Validar o modelo de vistoria usando anotações de validação
                var validationContext = new ValidationContext(vistoriaModel, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(vistoriaModel, validationContext, validationResults, validateAllProperties: true);

                if (!isValid)
                {
                    return new JsonResult("Dados inválidos. Verifique os campos fornecidos.");
                }

                var Dao = new DaoVistorias(_logService, _connection);
                var retorno = Dao.AlteraVistoria(idVistoria, vistoriaModel);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpDelete("{idVistoria}")]
        public JsonResult Delete(int idVistoria)
        {
            try
            {
                if (idVistoria <= 0)
                {
                    return new JsonResult("ID da vistoria inválido");
                }

                var Dao = new DaoVistorias(_logService, _connection);
                var retorno = Dao.DeletaVistoria(idVistoria);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
