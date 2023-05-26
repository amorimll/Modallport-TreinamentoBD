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
    public class VistoriasController : Controller
    {
        private readonly OracleConnection _connection;

        public VistoriasController(OracleConnection connection)
        {
            _connection = connection;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            try
            {
                _connection.Open();

                var vistorias = new List<VistoriaModel>();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT IdVistoria, CodVistoria, Descricao, Processo, DataDeCadastro FROM Vistorias WHERE Ativo = 'S'";

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
                                DataDeCadastro = reader.GetDateTime(reader.GetOrdinal("DataDeCadastro"))
                            };

                            vistorias.Add(vistoria);
                        }
                    }
                }

                _connection.Close();

                return new JsonResult(vistorias);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }


        [HttpPost]
        public JsonResult Post([FromBody] VistoriaModel vistoriaModel)
        {
            try
            {
                _connection.Open();

                var itensController = new ItensController(_connection);
                DateTime dataDeCadastro = DateTime.Now;
                int idVistoria;

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Vistorias (CodVistoria, Descricao, Processo, DataDeCadastro) VALUES (:CodVistoria, :Descricao, :Processo, :DataDeCadastro) RETURNING IdVistoria INTO :IdVistoria";
                    command.Parameters.Add("CodVistoria", OracleDbType.Varchar2).Value = vistoriaModel.CodVistoria;
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = vistoriaModel.Descricao;
                    command.Parameters.Add("Processo", OracleDbType.Varchar2).Value = vistoriaModel.Processo;
                    command.Parameters.Add("DataDeCadastro", OracleDbType.Date).Value = vistoriaModel.DataDeCadastro;
                    command.Parameters.Add("IdVistoria", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    var idVistoriaOracleDecimal = (OracleDecimal)command.Parameters["IdVistoria"].Value;
                    idVistoria = idVistoriaOracleDecimal.ToInt32();
                }

                _connection.Close();

                var vistoriaItem = new VistoriaItemListModel
                {
                    IdVistoria = idVistoria,
                    Itens = new List<ItemModel>()
                };

                vistoriaItem.Itens.AddRange(vistoriaModel.Itens);

                itensController.Post(vistoriaItem.IdVistoria, vistoriaItem.Itens);

                return new JsonResult(vistoriaItem);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("{idVistoria}")]
        public JsonResult Put(int idVistoria, [FromBody] VistoriaModel vistoriaModel)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Vistorias SET Descricao = :Descricao, Processo = :Processo WHERE IdVistoria = :IdVistoria";
                    command.Parameters.Add("Descricao", OracleDbType.Varchar2).Value = vistoriaModel.Descricao;
                    command.Parameters.Add("Processo", OracleDbType.Varchar2).Value = vistoriaModel.Processo;
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
