using APIModallportV5.Dao;
using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

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


        [HttpPost]
        public JsonResult Post([FromBody] VistoriaModel vistoriaModel)
        {
            try
            {
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
