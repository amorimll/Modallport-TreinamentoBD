using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

namespace APIModallportV5.Dao
{
    public class DaoVistoriasRealizadas
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        DateTime dataAtual = DateTime.Now;

        public DaoVistoriasRealizadas(LogService logService, OracleConnection connection)
        {
            _logService = logService;
            _connection = connection;
        }

        public List<VistoriaRealizadaModel> ListaVistoriasRealizadas()
        {
            try
            {
                _connection.Open();

                var vistoriasRealizadas = new List<VistoriaRealizadaModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM VistoriasRealizadas WHERE Ativo = 'S'";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var vistoria = new VistoriaRealizadaModel
                            {
                                IdVistoriaRealizada = reader.GetInt32(reader.GetOrdinal("IdVistoriaRealizada")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Processo = reader.GetInt32(reader.GetOrdinal("Processo")),
                                Itens = reader.GetValue(reader.GetOrdinal("Itens")),
                                Ativo = reader.GetString(reader.GetOrdinal("Ativo")),
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro")),
                                DhAlteracao = reader.GetDateTime(reader.GetOrdinal("DhAlteracao"))
                            };

                            vistoriasRealizadas.Add(vistoria);
                        }
                    }
                }

                _connection.Close();
                _logService.PerformOperation("GET", "Dados de VISTORIAS retornados.");

                return vistoriasRealizadas;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public List<VistoriaModel> ListaVistoriasProcesso(int idProcesso)
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
                _logService.PerformOperation("GET", "Dados de VISTORIAS apartir do id do processo retornados.");

                return vistorias;

            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public List<VistoriaModelAndItems> ListaVistoriasItens(int idVistoria)
        {
            try
            {
                _connection.Open();

                var vistorias = new List<VistoriaModelAndItems>();
                var itens = new List<ItemModel>();
                var itensAndOptions = new List<ItemModelAndOption>();

                // Itens list
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

                // var idVistoriaArray = itens.Split(',').Select(int.Parse).ToArray();
                var idVistoriaArray = itens.ToArray().Select(item => item.IdItem).ToArray();
                string joinedString = string.Join(",", idVistoriaArray);

                // Itens and Options list
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM OpcoesItens JOIN itens ON Itens.iditem = OpcoesItens.Iditem WHERE OpcoesItens.Iditem IN ({joinedString})";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var itemOptions = new ItemModelAndOption
                            {
                                IdItem = reader.GetInt32(reader.GetOrdinal("IdItem")),
                                IdOpcao = reader.GetInt32(reader.GetOrdinal("IdOpcao")),
                                Opcao = reader.GetString(reader.GetOrdinal("Opcao")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Ordem = reader.GetInt32(reader.GetOrdinal("Ordem")),
                                Tipo = reader.GetInt32(reader.GetOrdinal("Tipo")),
                                Ativo = reader.GetString(reader.GetOrdinal("Ativo")),
                                IdVistoria = reader.GetInt32(reader.GetOrdinal("IdVistoria")),
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro")),
                                DhAlteracao = reader.GetDateTime(reader.GetOrdinal("DhAlteracao"))
                            };

                            itensAndOptions.Add(itemOptions);
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
                                Items = itensAndOptions,
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro")),
                                DhAlteracao = reader.GetDateTime(reader.GetOrdinal("DhAlteracao"))
                            };

                            vistorias.Add(vistoria);
                        }
                    }
                }

                _connection.Close();
                _logService.PerformOperation("GET", "Dados de VISTORIAS com o mesmo id de vistoria retornados.");

                return vistorias;
            }

            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }


        public CreateVistoriaRealizadaModel PostVistoriaRealizada(CreateVistoriaRealizadaModel vistoriaModelRealizada)
        {
            try
            {
                _connection.Open();
                int idVistoriaRealizada;
                string jsonItens = vistoriaModelRealizada.Itens.ToString();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO VistoriasRealizadas (Descricao, Processo,Itens, DataDeCadastro, DhAlteracao) VALUES (:Descricao, :Processo,:Itens, :DataDeCadastro, :DhAlteracao) RETURNING IdVistoriaRealizada INTO :IdVistoriaRealizada";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = vistoriaModelRealizada.Descricao;
                    command.Parameters.Add("Processo", OracleDbType.Varchar2).Value = vistoriaModelRealizada.Processo;
                    command.Parameters.Add("Itens", OracleDbType.Json).Value = jsonItens;
                    command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("IdVistoria", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();
                }

                _connection.Close();
                _logService.PerformOperation("POST", "Vistoria realizada");


                return vistoriaModelRealizada;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("POST", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public VistoriaModel AlteraVistoria(int idVistoria, VistoriaModel vistoriaModel)
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

                return vistoriaModel;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("PUT", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public bool DeletaVistoria(int idVistoria)
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
