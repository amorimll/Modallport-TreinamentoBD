using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using APIModallportV5.Model;
using System.Collections.Generic;
using APIModallportV5;

namespace APIModallPortV5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessosController : ControllerBase
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        DateTime dataAtual = DateTime.Now;

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
                _connection.Open();

                var processos = new List<ProcessoModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdProcesso, CodProcesso, Descricao, Ativo, DataDeCadastro, DhAlteracao FROM Processos WHERE Ativo = 'S'";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var processo = new ProcessoModel
                            {
                                IdProcesso = reader.GetInt32(reader.GetOrdinal("IdProcesso")),
                                CodProcesso = reader.GetString(reader.GetOrdinal("CodProcesso")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Ativo = reader.GetString(reader.GetOrdinal("Ativo")),
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro")),
                                DhAlteracao = reader.GetDateTime(reader.GetOrdinal("DhAlteracao"))
                            };

                            processos.Add(processo);
                        }
                    }
                }

                _connection.Close();
                _logService.PerformOperation("GET", "Dados de PROCESSOS retornados.");

                return new JsonResult(processos);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }


        [HttpPost]
        public JsonResult Post([FromBody] ProcessoModel processoModel)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Processos (CodProcesso, Descricao, DataDeCadastro, DhAlteracao) VALUES (:CodProcesso, :Descricao, :DataDeCadastro, :DhAlteracao)";
                    command.Parameters.Add("CodProcesso", OracleDbType.Varchar2).Value = processoModel.CodProcesso;
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = processoModel.Descricao;
                    command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.ExecuteNonQuery();
                }

                _connection.Close();
                _logService.PerformOperation("POST", "Dados de PROCESSOS inseridos.");

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("POST", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("{idProcesso}")]
        public JsonResult Put(int idProcesso, [FromBody] ProcessoModel processoModel)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Processos SET Descricao = :Descricao, DhAlteracao = :DhAlteracao WHERE IdProcesso = :IdProcesso";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = processoModel.Descricao;
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("IdProcesso", OracleDbType.Int32).Value = idProcesso;
                    command.ExecuteNonQuery();
                }

                _connection.Close();
                _logService.PerformOperation("PUT", "Dados de PROCESSOS alterados.");

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("PUT", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }

        [HttpDelete("{idProcesso}")]
        public JsonResult Delete(int idProcesso)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Processos SET Ativo = 'N', DhAlteracao = :DhAlteracao WHERE IdProcesso = :IdProcesso";
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("IdProcesso", OracleDbType.Int32).Value = idProcesso;
                    command.ExecuteNonQuery();
                }

                _connection.Close();
                _logService.PerformOperation("DELETE", "Dados de PROCESSOS removidos.");

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
