using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using APIModallportV5.Model;
using System.Collections.Generic;

namespace APIModallPortV5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessosController : ControllerBase
    {
        private readonly OracleConnection _connection;

        public ProcessosController(OracleConnection connection)
        {
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
                    command.CommandText = "SELECT IdProcesso, CodProcesso, Descricao, DataDeCadastro FROM Processos WHERE Ativo = 'S'";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var processo = new ProcessoModel
                            {
                                IdProcesso = reader.GetInt32(reader.GetOrdinal("IdProcesso")),
                                CodProcesso = reader.GetString(reader.GetOrdinal("CodProcesso")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro"))
                            };

                            processos.Add(processo);
                        }
                    }
                }

                _connection.Close();

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
                _connection.Open();

                DateTime dataDeCadastro = DateTime.Now;

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Processos (CodProcesso, Descricao, DataDeCadastro) VALUES (:CodProcesso, :Descricao, :DataDeCadastro)";
                    command.Parameters.Add("CodProcesso", OracleDbType.Varchar2).Value = processoModel.CodProcesso;
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = processoModel.Descricao;
                    command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = processoModel.DataDeCadastro;
                    command.ExecuteNonQuery();
                }

                _connection.Close();

                return new JsonResult(Ok());
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
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Processos SET Descricao = :Descricao WHERE IdProcesso = :IdProcesso";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = processoModel.Descricao;
                    command.Parameters.Add("IdProcesso", OracleDbType.Int32).Value = idProcesso;
                    command.ExecuteNonQuery();
                }

                _connection.Close();

                return new JsonResult(Ok());
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
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Processos SET Ativo = 'N' WHERE IdProcesso = :IdProcesso";
                    command.Parameters.Add("IdProcesso", OracleDbType.Int32).Value = idProcesso;
                    command.ExecuteNonQuery();
                }

                _connection.Close();

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

    }
}
