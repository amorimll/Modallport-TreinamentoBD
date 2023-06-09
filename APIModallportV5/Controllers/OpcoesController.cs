﻿using APIModallportV5.Dao;
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
        public JsonResult Post(int idItem, [FromBody] OpcaoModel opcaoModel)
        {
            try
            {
                if (idItem <= 0)
                {
                    return new JsonResult("ID do item inválido");
                }

                var validationContext = new ValidationContext(opcaoModel, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(opcaoModel, validationContext, validationResults, validateAllProperties: true);

                if (!isValid)
                {
                    return new JsonResult("Dados inválidos. Verifique os campos fornecidos.");
                }

                var Dao = new DaoOpcoes(_logService, _connection);
                var retorno = Dao.PostOpcao(idItem, opcaoModel);

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
