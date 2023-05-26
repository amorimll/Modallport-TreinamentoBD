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

        public OpcoesController(OracleConnection connection)
        {
            _connection = connection;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            try
            {
                _connection.Open();

                var opcoes = new List<OpcaoModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdOpcao, Opcao, IdItem FROM OpcoesItens";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var opcao = new OpcaoModel
                            {
                                IdOpcao = reader.GetInt32(reader.GetOrdinal("IdOpcao")),
                                Opcao = reader.GetString(reader.GetOrdinal("Opcao")),
                                IdItem = reader.GetInt32(reader.GetOrdinal("IdItem"))
                            };

                            opcoes.Add(opcao);
                        }
                    }
                }

                _connection.Close();

                return new JsonResult(opcoes);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }


        [HttpPost]
        public JsonResult Post(int idItem, [FromBody] List<OpcaoModel> opcoesModels)
        {
            try
            {
                _connection.Open();

                DateTime dataDeCadastro = DateTime.Now;

                foreach (var opcoesModel in opcoesModels)
                {
                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO OpcoesItens (Opcao, IdItem) VALUES (:Opcao, :IdItem)";
                        command.Parameters.Add("Opcao", OracleDbType.Varchar2).Value = opcoesModel.Opcao;
                        command.Parameters.Add("IdItem", OracleDbType.Int32).Value = idItem;
                        command.ExecuteNonQuery();
                    }
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
