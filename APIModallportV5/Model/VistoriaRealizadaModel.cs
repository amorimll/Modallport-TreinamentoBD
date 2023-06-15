using Microsoft.AspNetCore.Http.Json;
using System;
using System.Collections.Generic;

namespace APIModallportV5.Model
{
    public class VistoriaRealizadaModel
    {
        public int IdVistoriaRealizada { get; set; }
        public string Descricao { get; set; }
        public object Itens { get; set; }
        public int Processo { get; set; }
        public string Ativo { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public DateTime DhAlteracao { get; set; }
    }

    public class CreateVistoriaRealizadaModel
    {
        public int IdVistoriaRealizada { get; set; }
        public string Descricao { get; set; }
        public object Itens { get; set; }
        public int Processo { get; set; }
        public string Ativo { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public DateTime DhAlteracao { get; set; }
    }
}