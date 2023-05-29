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
}
