using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using APIModallportV5.Model;

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

        [HttpPost]
        public JsonResult Post([FromBody] ProcessoModel model)
        {
            try
            {
                _connection.Open();

                DateTime dataDeCadastro = DateTime.Now;

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Processos (CodProcesso, Descricao, DataDeCadastro) VALUES (:CodProcesso, :Descricao, :DataDeCadastro)";
                    command.Parameters.Add("CodProcesso", OracleDbType.Varchar2).Value = model.CodProcesso;
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = model.Descricao;
                    command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = model.DataDeCadastro;
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
        public JsonResult Put(int idProcesso, [FromBody] ProcessoModel model)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Processos SET Descricao = :Descricao WHERE IdProcesso = :IdProcesso";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = model.Descricao;
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
