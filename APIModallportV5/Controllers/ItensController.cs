using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;

namespace APIModallportV5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItensController : Controller
    {
        private readonly OracleConnection _connection;

        public ItensController(OracleConnection connection)
        {
            _connection = connection;
        }

        [HttpPost]
        public JsonResult Post([FromBody] ItemModel ItemModel)
        {
            try
            {
                _connection.Open();

                DateTime dataDeCadastro = DateTime.Now;

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Itens (Descricao, Ordem, Tipo, IdVistoria) VALUES (:Descricao, :Ordem, :Tipo, :IdVistoria)";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = ItemModel.Descricao;
                    command.Parameters.Add("Ordem", OracleDbType.Varchar2).Value = ItemModel.Ordem;
                    command.Parameters.Add("Processo", OracleDbType.Varchar2).Value = ItemModel.Tipo;
                    command.Parameters.Add("IdVistoria", OracleDbType.Varchar2).Value = ItemModel.IdVistoria;
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

        [HttpPut("{idItem}")]
        public JsonResult Put(int idItem, [FromBody] ItemModel ItemModel)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Itens SET Descricao = :Descricao, Ordem = :Ordem WHERE IdVistoria = :IdVistoria AND IdItem = :IdItem";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = ItemModel.Descricao;
                    command.Parameters.Add("Ordem", OracleDbType.Int32).Value = ItemModel.Ordem;
                    command.Parameters.Add("Tipo", OracleDbType.Char).Value = ItemModel.Tipo;
                    command.Parameters.Add("IdVistoria", OracleDbType.Int32).Value = ItemModel.IdVistoria;
                    command.Parameters.Add("IdItem", OracleDbType.Int32).Value = idItem;
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

        [HttpDelete("{idItem}")]
        public JsonResult Delete(int idItem)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Itens SET Ativo = 'N' WHERE IdItem = :IdItem";
                    command.Parameters.Add("IdItem", OracleDbType.Int32).Value = idItem;
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
