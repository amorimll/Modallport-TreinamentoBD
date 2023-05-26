using APIModallportV5.Model;
using System.Collections.Generic;

public class VistoriaItemListModel
{
    public int IdVistoria { get; set; }
    public List<ItemModel> Itens { get; internal set; }
}