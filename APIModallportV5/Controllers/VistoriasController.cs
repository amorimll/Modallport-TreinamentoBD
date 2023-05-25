using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;

namespace APIModallportV5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VistoriasController : Controller
    {
        private readonly OracleConnection _connection;

        public VistoriasController(OracleConnection connection)
        {
            _connection = connection;
        }

        [HttpPost]
        public JsonResult Post([FromBody] VistoriaModel model)
        {
            try
            {
                _connection.Open();

                DateTime dataDeCadastro = DateTime.Now;

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Vistorias (CodVistoria, Descricao, DataDeCadastro) VALUES (:CodVistoria, :Descricao, :DataDeCadastro)";
                    command.Parameters.Add("CodVistoria", OracleDbType.Int32).Value = model.CodVistoria;
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = model.Descricao;
                    command.Parameters.Add("Processo", OracleDbType.Varchar2).Value = model.Processo;
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
    }
}
