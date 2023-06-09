using APIModallportV5.Dao;
using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace APIModallportV5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItensController : Controller
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        DateTime dataAtual = DateTime.Now;

        public ItensController(LogService logService, OracleConnection connection)
        {
            _logService = logService;
            _connection = connection;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            try
            {
                var Dao = new DaoItens(_logService, _connection);
                var itens = Dao.ListaItens();

                return new JsonResult(itens);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet("{idVistoria}")]
        public JsonResult GetById(int idVistoria)
        {
            try
            {
                if (idVistoria <= 0)
                {
                    return new JsonResult("ID da vistoria inválido");
                }

                var Dao = new DaoItens(_logService, _connection);
                var itens = Dao.ListaItensById(idVistoria);

                return new JsonResult(itens);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Post(int idVistoria, [FromBody] ItemModel itemModel)
        {
            try
            {
                if (idVistoria <= 0)
                {
                    return new JsonResult("ID da vistoria inválido");
                }

                var validationContext = new ValidationContext(itemModel, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(itemModel, validationContext, validationResults, validateAllProperties: true);

                if (!isValid)
                {
                    return new JsonResult("Dados inválidos. Verifique os campos fornecidos.");
                }

                var Dao = new DaoItens(_logService, _connection);
                var retorno = Dao.PostItem(idVistoria, itemModel);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("{idItem}")]
        public JsonResult Put(int idItem, [FromBody] ItemModel itemModel)
        {
            try
            {
                if (idItem <= 0)
                {
                    return new JsonResult("ID do item inválido");
                }

                var validationContext = new ValidationContext(itemModel, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(itemModel, validationContext, validationResults, validateAllProperties: true);

                if (!isValid)
                {
                    return new JsonResult("Dados inválidos. Verifique os campos fornecidos.");
                }

                var Dao = new DaoItens(_logService, _connection);
                var retorno = Dao.AlteraItem(idItem, itemModel);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpDelete("{idItem}")]
        public JsonResult Delete(int idItem)
        {
            try
            {
                if (idItem <= 0)
                {
                    return new JsonResult("ID do item inválido");
                }

                var Dao = new DaoItens(_logService, _connection);
                var retorno = Dao.DeletaItem(idItem);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
