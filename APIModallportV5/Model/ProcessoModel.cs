using System;

namespace APIModallportV5.Model
{
    public class ProcessoModel
    {
        public int IdProcesso { get; set; }
        public string Descricao { get; set; }
        public string Ativo { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public DateTime DhAlteracao { get; set; }
    }
}
