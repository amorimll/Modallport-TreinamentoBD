using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace APIModallportV5.Dao
{
    public class DaoItens
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        DateTime dataAtual = DateTime.Now;

        public DaoItens(LogService logService, OracleConnection connection)
        {
            _logService = logService;
            _connection = connection;
        }

        public class ItemWithIdModel
        {
            public int IdItem { get; set; }
            public List<ItemModel> Itens { get; set; }
        }

        public List<ItemModel> ListaItens()
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

                return itens;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public List<ItemModel> ListaItensById(int idVistoria)
        {
            try
            {
                _connection.Open();

                var itens = new List<ItemModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $"SELECT IdItem, Descricao, Ordem, Tipo, Ativo, IdVistoria, DataDeCadastro, DhAlteracao FROM Itens WHERE Ativo = 'S' AND IdVistoria = {idVistoria} ORDER BY Ordem ASC";

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

                return itens;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public ItemModel PostItem(int idVistoria, ItemModel itemModel)
        {
            try
            {
                _connection.Open();

                int idItem;

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Itens (Descricao, Ordem, Tipo, IdVistoria, DataDeCadastro, DhAlteracao) VALUES (:Descricao, :Ordem, :Tipo, :IdVistoria, :DataDeCadastro, :DhAlteracao)  RETURNING IdItem INTO :IdItem";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = itemModel.Descricao;
                    command.Parameters.Add("Ordem", OracleDbType.Int32).Value = Convert.ToInt32(itemModel.Ordem);
                    command.Parameters.Add("Tipo", OracleDbType.Char).Value = itemModel.Tipo;
                    command.Parameters.Add("IdVistoria", OracleDbType.Int32).Value = idVistoria;
                    command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("IdItem", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    var idItemOracleDecimal = (OracleDecimal)command.Parameters["IdItem"].Value;
                    idItem = idItemOracleDecimal.ToInt32();
                }

                _connection.Close();
                _logService.PerformOperation("POST", "Dados de ITENS inseridos.");

                itemModel.IdItem = idItem;

                return itemModel;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("POST", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public ItemModel AlteraItem(int idItem, ItemModel itemModel)
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

                void OrdenaItens(int idVistoria)
                {
                    var selectCommand = _connection.CreateCommand();
                    selectCommand.CommandText = "SELECT * FROM Itens WHERE IdVistoria = :IdVistoria ORDER BY Ordem ASC";
                    selectCommand.Parameters.Add("IdVistoria", OracleDbType.Int32).Value = idVistoria;

                    var updateCommand = _connection.CreateCommand();
                    updateCommand.CommandText = "UPDATE Itens SET Ordem = :Ordem WHERE IdItem = :IdItem";
                    updateCommand.Parameters.Add("Ordem", OracleDbType.Int32);
                    updateCommand.Parameters.Add("IdItem", OracleDbType.Int32);

                    var reader = selectCommand.ExecuteReader();

                    int newOrder = 1;

                    while (reader.Read())
                    {
                        int itemId = Convert.ToInt32(reader["IdItem"]);
                        updateCommand.Parameters["Ordem"].Value = newOrder;
                        updateCommand.Parameters["IdItem"].Value = itemId;
                        updateCommand.ExecuteNonQuery();
                        newOrder++;
                    }

                    reader.Close();
                }

                OrdenaItens(itemModel.IdVistoria);

                _connection.Close();
                _logService.PerformOperation("PUT", "Dados de ITENS alterados.");

                return itemModel;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("PUT", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }



        public bool DeletaItem(int idItem)
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

                return true;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("DELETE", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
