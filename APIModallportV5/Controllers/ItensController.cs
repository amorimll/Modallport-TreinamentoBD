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
    public class ItensController : Controller
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        DateTime dataAtual = DateTime.Now;

        public ItensController(LogService logService, OracleConnection connection)
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

                var itens = new List<ItemModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdItem, Descricao, Ordem, Tipo, Ativo, IdVistoria, DataDeCadastro, DhAlteracao FROM Itens WHERE Ativo = 'S'";

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

                _connection.Close();
                _logService.PerformOperation("GET", "Dados de ITENS retornados.");

                return new JsonResult(itens);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }


        [HttpPost]
        public JsonResult Post(int idVistoria, [FromBody] List<ItemModel> itemModels)
        {
            try
            {
                _connection.Open();

                //var opcoesController = new OpcoesController(_connection);
                //var respostasController = new RespostasController(_connection);
                //List<int> numerosOpcoes = new List<int>();
                //List<int> numerosRespostas = new List<int>();
                //var itemOpcoes = new OpcoesListModel
                //{
                //    IdItemList = new List<int>(numerosOpcoes),
                //    Opcoes = new List<OpcaoModel>(),
                //};

                //var itemRespostas = new RespostasListModel
                //{
                //    IdItemList = new List<int>(numerosRespostas),
                //    Respostas = new List<RespostaModel>(),
                //};

                foreach (var itemModel in itemModels)
                {
                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Itens (Descricao, Ordem, Tipo, IdVistoria, DataDeCadastro, DhAlteracao) VALUES (:Descricao, :Ordem, :Tipo, :IdVistoria, :DataDeCadastro, :DhAlteracao)  /*RETURNING IdItem INTO :IdItem*/";
                        command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = itemModel.Descricao;
                        command.Parameters.Add("Ordem", OracleDbType.Int32).Value = Convert.ToInt32(itemModel.Ordem);
                        command.Parameters.Add("Tipo", OracleDbType.Char).Value = itemModel.Tipo;
                        command.Parameters.Add("IdVistoria", OracleDbType.Int32).Value = idVistoria;
                        command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = dataAtual;
                        command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                        //command.Parameters.Add("IdItem", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();

                        //var idItemOracleDecimal = (OracleDecimal)command.Parameters["IdItem"].Value;
                        //itemOpcoes.IdItemList.Add(idItemOracleDecimal.ToInt32());
                        //itemRespostas.IdItemList.Add(idItemOracleDecimal.ToInt32());
                    }

                    //foreach (var opcaoModel in itemModel.opcaoModels)
                    //{
                    //    itemOpcoes.Opcoes.AddRange(opcaoModel.Opcoes);
                    //}

                    //foreach (var respostaModel in itemModel.respostaModels)
                    //{
                    //    itemRespostas.Respostas.AddRange(respostaModel.Respostas);
                    //}

                    //itemOpcoes.Opcoes.Clear();
                    //itemRespostas.Respostas.Clear();
                }

                _connection.Close();
                _logService.PerformOperation("POST", "Dados de ITENS inseridos.");


                //foreach (var idItem in itemOpcoes.IdItemList)
                //{
                //    Console.WriteLine(idItem);
                //    opcoesController.Post(idItem, itemOpcoes.Opcoes);
                //}

                //foreach (var idItem in itemRespostas.IdItemList)
                //{
                //    Console.WriteLine(idItem);
                //    respostasController.Post(idItem, itemRespostas.Respostas);
                //}
                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("POST", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("{idItem}")]
        public JsonResult Put(int idItem, [FromBody] ItemModel itemModel)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Itens SET Descricao = :Descricao, Ordem = :Ordem, Tipo = :Tipo, DhAlteracao = :DhAlteracao WHERE IdItem = :IdItem";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = itemModel.Descricao;
                    command.Parameters.Add("Ordem", OracleDbType.Int32).Value = itemModel.Ordem;
                    command.Parameters.Add("Tipo", OracleDbType.Int32).Value = itemModel.Tipo;
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("IdItem", OracleDbType.Int32).Value = idItem;
                    command.ExecuteNonQuery();
                }

                _connection.Close();
                _logService.PerformOperation("PUT", "Dados de ITENS alterados.");

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("PUT", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }

        //[HttpPut("{idItem}")]
        //public JsonResult Put(int idItem, [FromBody] ItemModel itemModel)
        //{

        //    try
        //    {
        //        _connection.Open();

        //        using (var command = _connection.CreateCommand())
        //        {
        //            command.CommandText = "UPDATE Itens SET Descricao = :Descricao, Tipo = :Tipo, Ordem = :Ordem, DhAlteracao = :DhAlteracao WHERE IdItem = :IdItem";
        //            command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = itemModel.Descricao;
        //            command.Parameters.Add("Ordem", OracleDbType.Int32).Value = itemModel.Ordem;
        //            command.Parameters.Add("Tipo", OracleDbType.Int32).Value = itemModel.Tipo;
        //            command.Parameters.Add("IdItem", OracleDbType.Int32).Value = idItem;
        //            command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
        //            command.ExecuteNonQuery();
        //        }

        //        _connection.Close();

        //        return new JsonResult(Ok());
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult(ex.Message);
        //    }
        //}

        [HttpDelete("{idItem}")]
        public JsonResult Delete(int idItem)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Itens SET Ativo = 'N', DhAlteracao = :DhAlteracao WHERE IdItem = :IdItem";
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("IdItem", OracleDbType.Int32).Value = idItem;
                    command.ExecuteNonQuery();
                }

                _connection.Close();
                _logService.PerformOperation("DELETE", "Dados de ITENS removidos.");

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
