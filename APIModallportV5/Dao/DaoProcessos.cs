using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace APIModallportV5.Dao
{
    public class DaoProcessos
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        DateTime dataAtual = DateTime.Now;

        public DaoProcessos(LogService logService, OracleConnection connection)
        {
            _logService = logService;
            _connection = connection;
        }
        public List<ProcessoModel> ListaProcessos()
        {
            try
            {
                _connection.Open();

                var processos = new List<ProcessoModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdProcesso, Descricao, Ativo, DataDeCadastro, DhAlteracao FROM Processos WHERE Ativo = 'S'";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var processo = new ProcessoModel
                            {
                                IdProcesso = reader.GetInt32(reader.GetOrdinal("IdProcesso")),
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
                return processos;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public ProcessoModel PostProcesso(ProcessoModel processoModel)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Processos (Descricao, DataDeCadastro, DhAlteracao) VALUES (:Descricao, :DataDeCadastro, :DhAlteracao)";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = processoModel.Descricao;
                    command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.ExecuteNonQuery();
                }

                _connection.Close();
                _logService.PerformOperation("POST", "Dados de PROCESSOS inseridos.");

                return processoModel;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("POST", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public ProcessoModel AlteraProcesso(int idProcesso, ProcessoModel processoModel)
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

                return processoModel;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("PUT", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public bool DeletaProcesso(int idProcesso)
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
