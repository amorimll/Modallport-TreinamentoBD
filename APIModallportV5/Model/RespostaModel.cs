using System;

namespace APIModallportV5.Model
{
    public class RespostaModel
    {
        public int IdResposta { get; set; }
        public string Resposta { get; set; }
        public int IdItem { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public DateTime DhAlteracao { get; set; }
    }
}
