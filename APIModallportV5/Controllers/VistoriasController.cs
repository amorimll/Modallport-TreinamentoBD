using APIModallportV5.Dao;
using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [HttpGet("itens/{idVistoria}")]
        public JsonResult GetByVistoriaId(int idVistoria)
        {
            try
            {
                _connection.Open();

                var vistorias = new List<VistoriaModelAndItems>();
                var itens = new List<ItemModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdItem, Descricao, Ordem, Tipo, Ativo, IdVistoria, DataDeCadastro, DhAlteracao FROM Itens WHERE Ativo = 'S' AND idVistoria = :idVistoria";
                    command.Parameters.Add(":IdVistoria", idVistoria);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new ItemModel
                            {
                                IdItem = reader.GetInt32(reader.GetOrdinal("IdItem")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Ordem = reader.GetInt32(reader.GetOrdinal("Ordem")),
                                Tipo = reader.GetInt32(reader.GetOrdinal("Tipo")),
                                Ativo = reader.GetString(reader.GetOrdinal("Ativo")),
                                IdVistoria = reader.GetInt32(reader.GetOrdinal("IdVistoria")),
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro")),
                                DhAlteracao = reader.GetDateTime(reader.GetOrdinal("DhAlteracao"))
                            };

                            itens.Add(item);
                        }
                    }
                }

                using (var command = _connection.CreateCommand())
                {
                    command.Parameters.Add(":IdVistoria", idVistoria);
                    command.CommandText = "SELECT IdVistoria, Descricao, Processo, Ativo, DataDeCadastro, DhAlteracao FROM Vistorias WHERE Ativo = 'S' AND idVistoria = :idVistoria";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var vistoria = new VistoriaModelAndItems
                            {
                                IdVistoria = reader.GetInt32(reader.GetOrdinal("IdVistoria")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Processo = reader.GetString(reader.GetOrdinal("Processo")),
                                Ativo = reader.GetString(reader.GetOrdinal("Ativo")),
                                Items = itens,
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro")),
                                DhAlteracao = reader.GetDateTime(reader.GetOrdinal("DhAlteracao"))
                            };

                            vistorias.Add(vistoria);
                        }
                    }
                }

                _connection.Close();
                _logService.PerformOperation("GET", "Dados de VISTORIAS com o mesmo id de vistoria retornados.");

                return new JsonResult(vistorias);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }


        [HttpGet("{idProcesso}")]
        public JsonResult GetByrocessoId(string idProcesso)
        {
            try
            {
                _connection.Open();

                var vistorias = new List<VistoriaModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdVistoria,  Descricao, Processo, Ativo, DataDeCadastro, DhAlteracao FROM Vistorias WHERE Ativo = 'S' AND Processo = :IdProcesso";
                    // Bind the value to the parameter
                    command.Parameters.Add(":IdProcesso", idProcesso);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var vistoria = new VistoriaModel
                            {
                                IdVistoria = reader.GetInt32(reader.GetOrdinal("IdVistoria")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Processo = reader.GetInt32(reader.GetOrdinal("Processo")),
                                Ativo = reader.GetString(reader.GetOrdinal("Ativo")),
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro")),
                                DhAlteracao = reader.GetDateTime(reader.GetOrdinal("DhAlteracao"))
                            };

                            vistorias.Add(vistoria);
                        }
                    }
                }

                _connection.Close();
                _logService.PerformOperation("GET", "Dados de VISTORIAS com o mesmo id de vistoria retornados.");

                return new JsonResult(vistorias);
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
