using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;

namespace APIModallportV5.Dao
{
    public class DaoVistorias
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        DateTime dataAtual = DateTime.Now;

        public DaoVistorias(LogService logService, OracleConnection connection)
        {
            _logService = logService;
            _connection = connection;
        }

        public List<VistoriaModel> ListaVistorias()
        {
            try
            {
                _connection.Open();

                var vistorias = new List<VistoriaModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdVistoria, CodVistoria, Descricao, Processo, Ativo, DataDeCadastro, DhAlteracao FROM Vistorias WHERE Ativo = 'S'";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var vistoria = new VistoriaModel
                            {
                                IdVistoria = reader.GetInt32(reader.GetOrdinal("IdVistoria")),
                                CodVistoria = reader.GetString(reader.GetOrdinal("CodVistoria")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Processo = reader.GetString(reader.GetOrdinal("Processo")),
                                Ativo = reader.GetString(reader.GetOrdinal("Ativo")),
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro")),
                                DhAlteracao = reader.GetDateTime(reader.GetOrdinal("DhAlteracao"))
                            };

                            vistorias.Add(vistoria);
                        }
                    }
                }

                _connection.Close();
                _logService.PerformOperation("GET", "Dados de VISTORIAS retornados.");

                return vistorias;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public VistoriaModel PostVistoria(VistoriaModel vistoriaModel)
        {
            try
            {
                _connection.Open();
                int idVistoria;

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Vistorias (CodVistoria, Descricao, Processo, DataDeCadastro, DhAlteracao) VALUES (:CodVistoria, :Descricao, :Processo, :DataDeCadastro, :DhAlteracao) RETURNING IdVistoria INTO :IdVistoria";
                    command.Parameters.Add("CodVistoria", OracleDbType.Varchar2).Value = vistoriaModel.CodVistoria;
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = vistoriaModel.Descricao;
                    command.Parameters.Add("Processo", OracleDbType.Varchar2).Value = vistoriaModel.Processo;
                    command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("IdVistoria", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    var idVistoriaOracleDecimal = (OracleDecimal)command.Parameters["IdVistoria"].Value;
                    idVistoria = idVistoriaOracleDecimal.ToInt32();
                }

                _connection.Close();
                _logService.PerformOperation("POST", "Dados de VISTORIAS inseridos.");

                vistoriaModel.IdVistoria = idVistoria;

                return vistoriaModel;
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
