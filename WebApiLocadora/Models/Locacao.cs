using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiLocadora.Models
{
    public class Locacao
    {
        
        public int id { get; set; }
        public int idFilme { get; set; }
        public int idCliente { get; set; }
        public DateTime PrevisaoDevolucao { get; set; }
    }
}