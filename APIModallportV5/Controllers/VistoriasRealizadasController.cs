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
    public class VistoriasRealizadasController : Controller
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        public VistoriasRealizadasController(LogService logService, OracleConnection connection)
        {
            _logService = logService;
            _connection = connection;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            try
            {
                var Dao = new DaoVistoriasRealizadas(_logService, _connection);
                var vistorias = Dao.ListaVistoriasRealizadas();

                return new JsonResult(vistorias);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Post([FromBody] CreateVistoriaRealizadaModel vistoriaRealizadaModel)
        {
            try
            {
                // Validar o modelo de vistoria usando anotações de validação
                var validationContext = new ValidationContext(vistoriaRealizadaModel, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(vistoriaRealizadaModel, validationContext, validationResults, validateAllProperties: true);

                if (!isValid)
                {
                    return new JsonResult("Dados inválidos. Verifique os campos fornecidos.");
                }

                var Dao = new DaoVistoriasRealizadas(_logService, _connection);
                var retorno = Dao.PostVistoriaRealizada(vistoriaRealizadaModel);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

    }
}
