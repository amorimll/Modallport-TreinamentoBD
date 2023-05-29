using System;

namespace APIModallportV5.Model
{
    public class OpcaoModel
    {
        public int IdOpcao { get; set; }
        public string Opcao { get; set; }
        public int IdItem { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public DateTime DhAlteracao { get; set; }
    }
}
