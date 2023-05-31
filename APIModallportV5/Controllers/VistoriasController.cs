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

        DateTime dataAtual = DateTime.Now;

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
                _connection.Open();

                var vistorias = new List<VistoriaModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdVistoria, CodVistoria, Descricao, Processo, Ativo, DataDeCadastro, DhAlteracao FROM Vistorias WHERE Ativo = 'S'";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var vistoria = new VistoriaModel
                            {
                                IdVistoria = reader.GetInt32(reader.GetOrdinal("IdVistoria")),
                                CodVistoria = reader.GetString(reader.GetOrdinal("CodVistoria")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Processo = reader.GetString(reader.GetOrdinal("Processo")),
                                Ativo = reader.GetString(reader.GetOrdinal("Ativo")),
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro")),
                                DhAlteracao = reader.GetDateTime(reader.GetOrdinal("DhAlteracao"))
                            };

                            vistorias.Add(vistoria);
                        }
                    }
                }

                _connection.Close();
                _logService.PerformOperation("GET", "Dados de VISTORIAS retornados.");

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
                _connection.Open();

                //var itensController = new ItensController(_connection);
                //int idVistoria;

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Vistorias (CodVistoria, Descricao, Processo, DataDeCadastro, DhAlteracao) VALUES (:CodVistoria, :Descricao, :Processo, :DataDeCadastro, :DhAlteracao) /*RETURNING IdVistoria INTO :IdVistoria*/";
                    command.Parameters.Add("CodVistoria", OracleDbType.Varchar2).Value = vistoriaModel.CodVistoria;
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = vistoriaModel.Descricao;
                    command.Parameters.Add("Processo", OracleDbType.Varchar2).Value = vistoriaModel.Processo;
                    command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    //command.Parameters.Add("IdVistoria", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    //var idVistoriaOracleDecimal = (OracleDecimal)command.Parameters["IdVistoria"].Value;
                    //idVistoria = idVistoriaOracleDecimal.ToInt32();
                }

                _connection.Close();
                _logService.PerformOperation("POST", "Dados de VISTORIAS inseridos.");

                //var vistoriaItem = new VistoriaItemListModel
                //{
                //    IdVistoria = idVistoria,
                //    Itens = new List<ItemModel>()
                //};

                //vistoriaItem.Itens.AddRange(vistoriaModel.Itens);

                //itensController.Post(vistoriaItem.IdVistoria, vistoriaItem.Itens);

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("POST", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("{idVistoria}")]
        public JsonResult Put(int idVistoria, [FromBody] VistoriaModel vistoriaModel)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Vistorias SET Descricao = :Descricao, Processo = :Processo, DhAlteracao = :DhAlteracao WHERE IdVistoria = :IdVistoria";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = vistoriaModel.Descricao;
                    command.Parameters.Add("Processo", OracleDbType.Varchar2).Value = vistoriaModel.Processo;
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("IdVistoria", OracleDbType.Int32).Value = idVistoria;
                    command.ExecuteNonQuery();
                }

                _connection.Close();
                _logService.PerformOperation("PUT", "Dados de VISTORIAS alterados.");

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("PUT", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }


        [HttpDelete("{idVistoria}")]
        public JsonResult Delete(int idVistoria)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Vistorias SET Ativo = 'N', DhAlteracao = :DhAlteracao WHERE IdVistoria = :IdVistoria";
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("IdVistoria", OracleDbType.Int32).Value = idVistoria;
                    command.ExecuteNonQuery();
                }

                _connection.Close();
                _logService.PerformOperation("DELETE", "Dados de VISTORIAS removidos.");

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("DELETE", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }
    }
}
