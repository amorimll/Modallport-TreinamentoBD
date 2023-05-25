﻿using APIModallportV5.Model;
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
                    command.CommandText = "INSERT INTO Vistorias (CodVistoria, Descricao, Processo, DataDeCadastro) VALUES (:CodVistoria, :Descricao, :Processo, :DataDeCadastro)";
                    command.Parameters.Add("CodVistoria", OracleDbType.Varchar2).Value = model.CodVistoria;
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = model.Descricao;
                    command.Parameters.Add("Processo", OracleDbType.Varchar2).Value = model.Processo; // Adicionado o parâmetro Processo
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


        [HttpPut("{idVistoria}")]
        public JsonResult Put(int idVistoria, [FromBody] VistoriaModel VistoriaModel)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Vistorias SET Descricao = :Descricao, Processo = :Processo WHERE IdVistoria = :IdVistoria";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = VistoriaModel.Descricao;
                    command.Parameters.Add("Processo", OracleDbType.Varchar2).Value = VistoriaModel.Processo;
                    command.Parameters.Add("IdVistoria", OracleDbType.Int32).Value = idVistoria;
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


        [HttpDelete("{idVistoria}")]
        public JsonResult Delete(int idVistoria)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Vistorias SET Ativo = 'N' WHERE IdVistoria = :IdVistoria";
                    command.Parameters.Add("IdVistoria", OracleDbType.Int32).Value = idVistoria;
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
