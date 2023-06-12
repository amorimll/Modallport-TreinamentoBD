using APIModallportV5.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;

namespace APIModallportV5.Dao
{
    public class DaoRespostas
    {
        private readonly OracleConnection _connection;
        private readonly LogService _logService;

        DateTime dataAtual = DateTime.Now;

        public DaoRespostas(LogService logService, OracleConnection connection)
        {
            _logService = logService;
            _connection = connection;
        }
        public List<RespostaModel> ListaRespostas()
        {
            try
            {
                _connection.Open();

                var respostas = new List<RespostaModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdResposta, Resposta, IdItem, DataDeCadastro, DhAlteracao FROM RespostasItens";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var resposta = new RespostaModel
                            {
                                IdResposta = reader.GetInt32(reader.GetOrdinal("IdResposta")),
                                Resposta = reader.GetString(reader.GetOrdinal("Resposta")),
                                IdItem = reader.GetInt32(reader.GetOrdinal("IdItem")),
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro")),
                                DhAlteracao = reader.GetDateTime(reader.GetOrdinal("DhAlteracao")),
                            };

                            respostas.Add(resposta);
                        }
                    }
                }

                _connection.Close();
                _logService.PerformOperation("GET", "Dados de RESPOSTAS retornados.");

                return respostas;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public RespostaModel PostResposta(int idItem, [FromBody] RespostaModel respostasModel)
        {
            try
            {
                _connection.Open();

                DateTime dataDeCadastro = DateTime.Now;

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO RespostasItens (Resposta, DataDeCadastro, DhAlteracao, IdItem) VALUES (:Resposta, :DataDeCadastro, :DhAlteracao, :IdItem)";
                    command.Parameters.Add("Resposta", OracleDbType.Varchar2).Value = respostasModel.Resposta;
                    command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                    command.Parameters.Add("IdItem", OracleDbType.Int32).Value = idItem;
                    command.ExecuteNonQuery();
                }

                _connection.Close();
                _logService.PerformOperation("POST", "Dados de RESPOSTAS inseridos.");

                return respostasModel;
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("POST", "Dados de RESPOSTAS inseridos.");
                throw new Exception(ex.Message);
            }
        }
    }
}
