using System;
using System.Collections.Generic;

namespace APIModallportV5.Model
{
    public class ItemModel
    {
        public int IdItem { get; set; }
        public string Descricao { get; set; }
        public int Ordem { get; set; }
        public int Tipo { get; set; }
        public int IdVistoria { get; set; }
        public string Ativo { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public DateTime DhAlteracao { get; set; }
        //public List<OpcoesListModel> opcaoModels { get; set; }
        //public List<RespostasListModel> respostaModels { get; set; }
    }

    public class ItemRespondidoModel
    {
        public string Descricao { get; set; }
        public int Ordem { get; set; }
        public int Tipo { get; set; }
        public string[] Opcoes { get; set; }
        public string[] Resposta { get; set; }
    }

    public class ItemModelAndOption
    {
        public int IdOpcao { get; set; }
        public string Opcao { get; set; }
        public int IdItem { get; set; }
        public int IdItem_1 { get; set; }
        public string Descricao { get; set; }
        public int Ordem { get; set; }
        public int Tipo { get; set; }
        public string Ativo { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public DateTime DhAlteracao { get; set; }
        public DateTime DataDeCadastro_1 { get; set; }
        public DateTime DhAlteracao_1 { get; set; }
        public int IdVistoria { get; set; }
        //public List<OpcoesListModel> opcaoModels { get; set; }
        //public List<RespostasListModel> respostaModels { get; set; }
    }
}
