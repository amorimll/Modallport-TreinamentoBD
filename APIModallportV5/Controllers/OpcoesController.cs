using APIModallportV5.Dao;
using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace APIModallportV5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpcoesController : Controller
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        public OpcoesController(LogService logService, OracleConnection connection)
        {
            _logService = logService;
            _connection = connection;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            try
            {
                var Dao = new DaoOpcoes(_logService, _connection);
                var opcoes = Dao.ListaOpcoes();

                return new JsonResult(opcoes);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }


        [HttpPost]
        public JsonResult Post(int idItem, [FromBody] List<OpcaoModel> opcaoModels)
        {
            try
            {
                var Dao = new DaoOpcoes(_logService, _connection);
                var retorno = Dao.PostOpcao(idItem, opcaoModels);

                return new JsonResult(retorno);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("POST", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }
    }
}
