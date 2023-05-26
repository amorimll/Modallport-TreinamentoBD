﻿using System;
using System.Collections.Generic;

namespace APIModallportV5.Model
{
    public class VistoriaModel
    {
        public int IdVistoria { get; set; }
        public string CodVistoria { get; set; }
        public string Descricao { get; set; }
        public string Processo { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public List<ItemModel> Itens { get; set; }
    }
}
