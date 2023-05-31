﻿using APIModallportV5.Model;
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
        private readonly LogService _logService;

        DateTime dataAtual = DateTime.Now;

        public OpcoesController(LogService logService, OracleConnection connection)
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

                var opcoes = new List<OpcaoModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdOpcao, Opcao, IdItem, DataDeCadastro, DhAlteracao FROM OpcoesItens";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var opcao = new OpcaoModel
                            {
                                IdOpcao = reader.GetInt32(reader.GetOrdinal("IdOpcao")),
                                Opcao = reader.GetString(reader.GetOrdinal("Opcao")),
                                IdItem = reader.GetInt32(reader.GetOrdinal("IdItem")),
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro")),
                                DhAlteracao = reader.GetDateTime(reader.GetOrdinal("DhAlteracao")),
                            };

                            opcoes.Add(opcao);
                        }
                    }
                }

                _connection.Close();
                _logService.PerformOperation("GET", "Dados de OPÇÕES retornados.");

                return new JsonResult(opcoes);
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("GET", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }


        [HttpPost]
        public JsonResult Post(int idItem, [FromBody] List<OpcaoModel> opcaoModels)
        {
            try
            {
                _connection.Open();

                DateTime dataDeCadastro = DateTime.Now;

                foreach (var opcaoModel in opcaoModels)
                {
                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO OpcoesItens (Opcao, DataDeCadastro, DhAlteracao, IdItem) VALUES (:Opcao, :DataDeCadastro, :DhAlteracao, :IdItem)";
                        command.Parameters.Add("Opcao", OracleDbType.Varchar2).Value = opcaoModel.Opcao;
                        command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = dataAtual;
                        command.Parameters.Add("DhAlteracao", OracleDbType.Date).Value = dataAtual;
                        command.Parameters.Add("IdItem", OracleDbType.Int32).Value = idItem;
                        command.ExecuteNonQuery();
                    }
                }

                _connection.Close();
                _logService.PerformOperation("POST", "Dados de OPÇÕES inseridos.");

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                _logService.PerformOperation("POST", $"{ex.Message}");
                return new JsonResult(ex.Message);
            }
        }
    }
}
